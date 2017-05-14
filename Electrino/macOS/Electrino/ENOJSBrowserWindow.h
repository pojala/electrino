//
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

- (instancetype)initWithDictionary:(NSDictionary *)dict;

- (void)loadURL:(NSString *)url;

@property (nonatomic, copy) void (^on)(NSString *eventName, JSValue *callback);

@end


@interface ENOJSBrowserWindow : NSObject <ENOJSBrowserWindowExports>

@property (nonatomic, strong) ENOBrowserWindowController *windowController;

@end
