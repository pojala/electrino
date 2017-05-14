//
//  ENOJSConsole.h
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>


@protocol ENOJSConsoleExports <JSExport>

@property (nonatomic, copy) void (^log)(NSString *str);

@end


@interface ENOJSConsole : NSObject <ENOJSConsoleExports>

@end
