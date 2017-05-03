//
//  ENOJSConsole.h
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>


@protocol ENOJSConsoleExports <JSExport>

@property (nonatomic, copy) void (^log)(NSString *str);

@end


@interface ENOJSConsole : NSObject <ENOJSConsoleExports>

@end
