//
//  ENOJSBrowserWindow.m
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOJSBrowserWindow.h"
#import "ENOBrowserWindowController.h"


@interface ENOJSBrowserWindow ()

@property (nonatomic, strong) NSMutableDictionary *eventCallbacks;

@end


@implementation ENOJSBrowserWindow

@synthesize on;

- (instancetype)initWithDictionary:(NSDictionary *)dict
{
    self = [super init];
    
    NSLog(@"%s, %@", __func__, dict);

    self.windowController = [[ENOBrowserWindowController alloc] init];
    
    NSWindow *win = self.windowController.window;
    NSRect frame = win.frame;
    id val;
    if ((val = dict[@"width"]) && [val integerValue] > 0) {
        frame.size.width = [val doubleValue];
    }
    if ((val = dict[@"height"]) && [val integerValue] > 0) {
        frame.size.height = [val doubleValue];
    }
    
    [win setFrame:frame display:NO];
    [win center];
    
    [self.windowController showWindow:nil];

    
    __block __weak ENOJSBrowserWindow *weakSelf = self;
    
    self.on = ^(NSString *event, JSValue *cb) {
        if (event.length > 0 && cb) {
            NSMutableArray *cbArr = weakSelf.eventCallbacks[event] ?: [NSMutableArray array];
            [cbArr addObject:cb];
            
            weakSelf.eventCallbacks[event] = cbArr;
        }
    };
    
    return self;
}

- (void)loadURL:(NSString *)urlStr
{
    NSURL *url = [NSURL URLWithString:urlStr];
    
    [self.windowController loadURL:url];
}

@end
