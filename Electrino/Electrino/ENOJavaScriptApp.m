//
//  ENOJavaScriptApp.m
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOJavaScriptApp.h"
#import "ENOJSPath.h"
#import "ENOJSUrl.h"
#import "ENOJSBrowserWindow.h"
#import "ENOJSApp.h"
#import "ENOJSProcess.h"
#import "ENOJSConsole.h"
#import "ENOJSTray.h"
#import "ENOJSNativeImage.h"
#import "ENOJSIPCMain.h"


NSString * const kENOJavaScriptErrorDomain = @"ENOJavaScriptErrorDomain";


@interface ENOJavaScriptApp ()

@property (nonatomic, strong) JSVirtualMachine *jsVM;
@property (nonatomic, strong) JSContext *jsContext;
@property (nonatomic, strong) NSDictionary *jsModules;
@property (nonatomic, strong) ENOJSApp *jsAppGlobalObject;
@property (nonatomic, strong) ENOJSIPCMain *jsIPCMain;

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
    
    
    self.jsVM = [[JSVirtualMachine alloc] init];
    self.jsContext = [[JSContext alloc] initWithVirtualMachine:self.jsVM];
    
    self.jsAppGlobalObject = [[ENOJSApp alloc] init];
    self.jsAppGlobalObject.jsApp = self;
    
    self.jsIPCMain = [[ENOJSIPCMain alloc] init];
    
    // initialize available modules
    
    NSMutableDictionary *modules = [NSMutableDictionary dictionary];
    
    modules[@"electrino"] = @{
                              // singletons
                              @"app": self.jsAppGlobalObject,
                              @"ipcMain": self.jsIPCMain,
                              @"nativeImage": [[ENOJSNativeImageAPI alloc] init],
                              
                              // classes that can be constructed
                              @"BrowserWindow": [ENOJSBrowserWindow class],
                              @"Tray": [ENOJSTray class],
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
		
		NSString *appDir = [[[[NSBundle mainBundle] resourcePath] stringByAppendingPathComponent:@"app"] stringByAppendingString:@"/"];
		
		if ([arg hasSuffix:@".js"]) { // If a javascript file is being directly referenced
			JSContext *tmpContext = [weakSelf newContextForEvaluation];
			
			[tmpContext evaluateScript:[NSString stringWithContentsOfURL:[NSURL fileURLWithPath:[appDir stringByAppendingString:arg]] encoding:NSUTF8StringEncoding error:NULL]];
            JSValue *module = tmpContext[@"module"];
            return (id)[module valueForProperty:@"exports"]; // Casted to id as the compile doesn't like multiple types of return values when no return value is specified
		} else if (weakSelf.jsModules[arg] != nil) {
			id module = weakSelf.jsModules[arg];
			return module;
		}
		
		BOOL isDirectory;
		BOOL doesExist = [[NSFileManager defaultManager] fileExistsAtPath:[appDir stringByAppendingString:arg] isDirectory:&isDirectory];
		if (doesExist && isDirectory) {
			// Find where the starting point is within package.json
			NSData *packageJSON = [NSData dataWithContentsOfURL:[NSURL fileURLWithPath:[[appDir stringByAppendingString:arg] stringByAppendingString:@"/package.json"]]];
			if (packageJSON == nil) {
				return (id)nil;
			}
			NSDictionary *packageDictionary = [NSJSONSerialization JSONObjectWithData:packageJSON options:0 error:NULL];
			if (packageDictionary == nil || packageDictionary[@"main"] == nil) {
				return (id)nil;
			}
			NSString *mainJSFile = packageDictionary[@"main"];
			NSURL *fileURL = [NSURL fileURLWithPath:packageDictionary[@"main"] relativeToURL:[NSURL fileURLWithPath:[appDir stringByAppendingString:arg]]];
			mainJSFile = [@"/" stringByAppendingString:mainJSFile];
			NSString *jsFileContents = [NSString stringWithContentsOfURL:fileURL encoding:NSUTF8StringEncoding error:NULL];
			
			JSContext *tmpContext = [weakSelf newContextForEvaluation];
			
			[tmpContext evaluateScript:jsFileContents];
            JSValue *module = tmpContext[@"module"];
            return (id)[module valueForProperty:@"exports"]; // Casted to id as the compile doesn't like multiple types of return values when no return value is specified
		} else {
			// Module doesn't exist!
		}
		return (id)nil;
		
    };
    
    self.jsContext[@"process"] = [[ENOJSProcess alloc] init];
    self.jsContext[@"console"] = [[ENOJSConsole alloc] init];
    
    return self;
}

// Create a new context for just evaluating the file
// ISSUE: JSContext does not include -copyWithZone: method, so we have to manually copy the required methods.
-(JSContext*)newContextForEvaluation
{
	JSContext* newContext = [[JSContext alloc] initWithVirtualMachine:self.jsVM];
	newContext[@"require"] = self.jsContext[@"require"];
	newContext[@"process"] = self.jsContext[@"process"];
	newContext[@"console"] = self.jsContext[@"console"];
    [newContext evaluateScript:@"var exports = {}; var module = { exports }"]; // Evaluated so the developer doesnt have to
	return newContext;
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
