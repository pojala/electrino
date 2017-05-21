//
//  ENOJSApp.m
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOJSApp.h"
#import "ENOJavaScriptApp.h"


@interface ENOJSApp ()

@property (nonatomic, strong) NSMutableDictionary *eventCallbacks;

@end


@implementation ENOJSApp

- (id)init
{
    self = [super init];
    
    self.eventCallbacks = [NSMutableDictionary dictionary];
    
    return self;
}

- (void)on:(NSString *)event withCallback:(JSValue *)cb
{
    if (event.length > 0 && cb) {
        NSMutableArray *cbArr = self.eventCallbacks[event] ?: [NSMutableArray array];
        [cbArr addObject:cb];
        
        self.eventCallbacks[event] = cbArr;
    }
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
