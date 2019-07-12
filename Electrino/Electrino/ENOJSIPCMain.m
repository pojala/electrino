//
//  ENOJSIPCMain.m
//  Electrino
//
//  Created by Pauli Ojala on 17/05/2017.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOJSIPCMain.h"


@interface ENOJSIPCMain ()

@property (nonatomic, strong) NSMutableDictionary *eventCallbacks;

@end


@implementation ENOJSIPCMain

- (id)init
{
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

@end
