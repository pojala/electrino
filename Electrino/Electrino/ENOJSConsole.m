//
//  ENOJSConsole.m
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
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
