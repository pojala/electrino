;//
//  ENOJSBrowserWindow.h
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>
#import "ENOBrowserWindowController.h"


@protocol ENOJSBrowserWindowExports <JSExport>

- (instancetype)initWithOptions:(NSDictionary *)opts;

- (void)loadURL:(NSString *)url;

- (BOOL)isVisible;
- (void)show;
- (void)hide;
- (void)focus;

- (NSDictionary *)getBounds;

JSExportAs(setPosition,
- (void)setPositionX:(double)x y:(double)y
);

JSExportAs(on,
- (void)on:(NSString *)event withCallback:(JSValue *)cb
);

@end


@interface ENOJSBrowserWindow : NSObject <ENOJSBrowserWindowExports>

@property (nonatomic, strong) ENOBrowserWindowController *windowController;

@end
