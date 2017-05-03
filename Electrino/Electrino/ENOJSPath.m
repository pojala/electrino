//
//  ENOJSPath.m
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
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
