//
//  ENOJavaScriptApp.m
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import "ENOJavaScriptApp.h"
#import "ENOJSPath.h"
#import "ENOJSUrl.h"
#import "ENOJSBrowserWindow.h"
#import "ENOJSApp.h"
#import "ENOJSProcess.h"
#import "ENOJSConsole.h"


NSString * const kENOJavaScriptErrorDomain = @"ENOJavaScriptErrorDomain";


@interface ENOJavaScriptApp ()

@property (nonatomic, strong) JSContext *jsContext;
@property (nonatomic, strong) NSDictionary *jsModules;
@property (nonatomic, strong) ENOJSApp *jsAppGlobalObject;

@property (nonatomic, assign) BOOL inException;

@end


@implementation ENOJavaScriptApp

+ (instancetype)sharedApp
{
    static ENOJavaScriptApp *s_app = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        s_app = [[self alloc] init];
    });
    return s_app;
}

- (id)init
{
    self = [super init];
    
    self.jsContext = [[JSContext alloc] init];
    
    self.jsAppGlobalObject = [[ENOJSApp alloc] init];
    self.jsAppGlobalObject.jsApp = self;
    
    
    // initialize available modules
    
    NSMutableDictionary *modules = [NSMutableDictionary dictionary];
    
    modules[@"electrino"] = @{
                              @"app": self.jsAppGlobalObject,
                              @"BrowserWindow": [ENOJSBrowserWindow class],
                              };
    modules[@"path"] = [[ENOJSPath alloc] init];
    modules[@"url"] = [[ENOJSUrl alloc] init];
    
    self.jsModules = modules;
    
    
    // add exception handler and global functions
    
    __block __weak ENOJavaScriptApp *weakSelf = self;
    
    self.jsContext.exceptionHandler = ^(JSContext *context, JSValue *exception) {
        [weakSelf _jsException:exception];
    };
    
    self.jsContext[@"require"] = ^(NSString *arg) {
        id module = weakSelf.jsModules[arg];
        return module;
    };
    
    self.jsContext[@"process"] = [[ENOJSProcess alloc] init];
    self.jsContext[@"console"] = [[ENOJSConsole alloc] init];
    
    return self;
}

- (void)dealloc
{
    self.jsContext.exceptionHandler = NULL;
    self.jsContext[@"require"] = nil;
}

- (void)_jsException:(JSValue *)exception
{
    NSLog(@"%s, %@", __func__, exception);
    
    if (self.inException) {  // prevent recursion, just in case
        return; // --
    }
    
    self.inException = YES;
    
    self.lastException = exception.toString;
    self.lastExceptionLine = [exception valueForProperty:@"line"].toInt32;
    
    self.inException = NO;
}

- (BOOL)loadMainJS:(NSString *)js error:(NSError **)outError
{
    self.lastException = nil;
    
    NSLog(@"%s...", __func__);
    
    [self.jsContext evaluateScript:js];
    
    if (self.lastException) {
        if (outError) {
            *outError = [NSError errorWithDomain:kENOJavaScriptErrorDomain
                                           code:101
                                       userInfo:@{
                                                  NSLocalizedDescriptionKey: self.lastException,
                                                  @"SourceLineNumber": @(self.lastExceptionLine),
                                                  }];
        }
        return NO; // --
    }
    
    NSLog(@"%s done", __func__);
    
    return YES;
}

@end
