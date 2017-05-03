//
//  ENOJSBrowserWindow.h
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>
#import "ENOBrowserWindowController.h"


@protocol ENOJSBrowserWindowExports <JSExport>

- (instancetype)initWithDictionary:(NSDictionary *)dict;

- (void)loadURL:(NSString *)url;

@end


@interface ENOJSBrowserWindow : NSObject <ENOJSBrowserWindowExports>

@property (nonatomic, strong) ENOBrowserWindowController *windowController;

@end
