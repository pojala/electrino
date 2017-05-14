//
//  ENOJSApp.h
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>
@class ENOJavaScriptApp;


@protocol ENOJSAppExports <JSExport>

@property (nonatomic, copy) void (^on)(NSString *eventName, JSValue *callback);

@end


@interface ENOJSApp : NSObject <ENOJSAppExports>

@property (nonatomic, weak) ENOJavaScriptApp *jsApp;

- (BOOL)emitReady:(NSError **)outError;

@end
