//
//  ENOJSProcess.m
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import "ENOJSProcess.h"


@implementation ENOJSProcess

@synthesize platform;

- (id)init
{
    self = [super init];
    
    self.platform = @"darwin";
    
    return self;
}

@end
