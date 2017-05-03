//
//  ENOBrowserWindowController.m
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOBrowserWindowController.h"
#import <WebKit/WebKit.h>


@interface ENOBrowserWindowController ()

@property (nonatomic, strong) WKWebView *webView;

@end



@implementation ENOBrowserWindowController

- (id)init
{
    NSWindowStyleMask styleMask = NSWindowStyleMaskTitled
    | NSWindowStyleMaskMiniaturizable
    | NSWindowStyleMaskResizable;
    
    NSWindow *window = [[NSWindow alloc] initWithContentRect:NSMakeRect(100, 100, 640, 480)
                                                   styleMask:styleMask
                                                     backing:NSBackingStoreBuffered
                                                       defer:NO];
    
    window.opaque = NO;
    window.hasShadow = YES;
    window.ignoresMouseEvents = NO;
    window.allowsConcurrentViewDrawing = YES;
    window.releasedWhenClosed = NO;
    
    WKWebViewConfiguration *wkConf = [[WKWebViewConfiguration alloc] init];
    
    WKWebView *webView = [[WKWebView alloc] initWithFrame:window.contentView.frame configuration:wkConf];
    window.contentView = webView;
    self.webView = webView;
    
    return [self initWithWindow:window];
}


- (void)loadURL:(NSURL *)url
{
    if (url.isFileURL) {
        NSString *dir = [url.path stringByDeletingLastPathComponent];
        
        NSLog(@"%s, %@", __func__, url);

        [self.webView loadFileURL:url allowingReadAccessToURL:[NSURL fileURLWithPath:dir isDirectory:YES]];
    }
    else {
        NSLog(@"** %s: only supports file urls", __func__);
    }
}

@end
