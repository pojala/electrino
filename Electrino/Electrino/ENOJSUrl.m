//
//  ENOJSUrl.m
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
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
