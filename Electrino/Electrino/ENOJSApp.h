//
//  ENOJSApp.h
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
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
