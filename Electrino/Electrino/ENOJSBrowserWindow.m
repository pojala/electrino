//
//  ENOJSBrowserWindow.m
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import "ENOJSBrowserWindow.h"
#import "ENOBrowserWindowController.h"



@implementation ENOJSBrowserWindow

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
    
    return self;
}

- (void)loadURL:(NSString *)urlStr
{
    NSURL *url = [NSURL URLWithString:urlStr];
    
    NSLog(@"%s: %@", __func__, url);
}

@end
