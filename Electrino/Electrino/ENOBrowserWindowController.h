//
//  ENOBrowserWindowController.h
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import <Cocoa/Cocoa.h>
#import <WebKit/WebKit.h>


@interface ENOBrowserWindowController : NSWindowController

@property (nonatomic, strong) IBOutlet WebView *testWebView;

- (instancetype)initAsResizable:(BOOL)resizable hasFrame:(BOOL)hasFrame;

- (void)loadURL:(NSURL *)url;

@end
