//
//  ENOJSPath.m
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import "ENOJSPath.h"


@implementation ENOJSPath

@synthesize join;

- (id)init
{
    self = [super init];
    
    self.join = ^NSString *(){
        NSArray *args = [JSContext currentArguments];
        NSString *pathSep = @"/";
        
        return [args componentsJoinedByString:pathSep];
    };
    
    return self;
}

@end
