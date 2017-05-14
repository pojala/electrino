//
//  ENOJSUrl.m
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOJSUrl.h"


@implementation ENOJSUrl

@synthesize format;

- (id)init
{
    self = [super init];
    
    self.format = ^NSString *(NSDictionary *urlDict){
        NSString *protocol = urlDict[@"protocol"];
        NSString *path = urlDict[@"pathname"];
        
        //NSLog(@"%s, %@", __func__, urlDict);
        
        return [NSString stringWithFormat:@"%@//%@", protocol, path];
    };
    
    return self;
}

@end
