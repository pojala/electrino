//
//  ENOBrowserWindowController.m
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import "ENOBrowserWindowController.h"

@interface ENOBrowserWindowController ()

@end



@implementation ENOBrowserWindowController

- (id)init
{
    NSWindowStyleMask styleMask = NSWindowStyleMaskTitled
    | NSWindowStyleMaskMiniaturizable
    | NSWindowStyleMaskResizable;
    
    NSWindow *window = [[NSWindow alloc] initWithContentRect:NSMakeRect(200, 200, 640, 480)
                                                   styleMask:styleMask
                                                     backing:NSBackingStoreBuffered
                                                       defer:NO];
    
    window.opaque = NO;
    window.hasShadow = YES;
    window.ignoresMouseEvents = NO;
    window.allowsConcurrentViewDrawing = YES;
    window.releasedWhenClosed = NO;
    
    return [self initWithWindow:window];
}

- (void)windowDidLoad
{
    NSLog(@"%s", __func__);
}

@end
