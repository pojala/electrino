//
//  ENOJSConsole.m
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOJSConsole.h"


@implementation ENOJSConsole

@synthesize log;

- (id)init
{
    self = [super init];
    
    self.log = ^(NSString *str){
        NSArray *args = [JSContext currentArguments];
        
        if (args.count > 1) {
            NSString *argsStr = [[args subarrayWithRange:NSMakeRange(1, args.count - 1)] componentsJoinedByString:@", "];
            str = [str stringByAppendingFormat:@" %@", argsStr];
        }
        
        NSLog(@"JS:  %@", str);
    };
    
    return self;
}

@end
