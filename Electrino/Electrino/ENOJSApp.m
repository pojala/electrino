//
//  ENOJSApp.m
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import "ENOJSApp.h"
#import "ENOJavaScriptApp.h"


@interface ENOJSApp ()

@property (nonatomic, strong) NSMutableDictionary *eventCallbacks;

@end


@implementation ENOJSApp

@synthesize on;

- (id)init
{
    self = [super init];
    
    self.eventCallbacks = [NSMutableDictionary dictionary];
    
    __block __weak ENOJSApp *weakSelf = self;
    
    self.on = ^(NSString *event, JSValue *cb) {
        if (event.length > 0 && cb) {
            NSMutableArray *cbArr = weakSelf.eventCallbacks[event] ?: [NSMutableArray array];
            [cbArr addObject:cb];
            
            weakSelf.eventCallbacks[event] = cbArr;
        }
    };
    
    return self;
}

- (void)dealloc
{
    self.on = NULL;
}


- (BOOL)emitReady:(NSError **)outError
{
    self.jsApp.lastException = nil;
    
    for (JSValue *cb in self.eventCallbacks[@"ready"]) {
        //NSLog(@"%s, %@", __func__, cb);
        
        [cb callWithArguments:@[]];
        
        if (self.jsApp.lastException) {
            if (outError) {
                *outError = [NSError errorWithDomain:kENOJavaScriptErrorDomain
                                                code:102
                                            userInfo:@{
                                                       NSLocalizedDescriptionKey: self.jsApp.lastException,
                                                       @"SourceLineNumber": @(self.jsApp.lastExceptionLine),
                                                       }];
            }
            return NO;
        }
    }
    return YES;
}

@end
