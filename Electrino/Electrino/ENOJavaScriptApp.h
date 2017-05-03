//
//  ENOJavaScriptApp.h
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>
#import "ENOJSApp.h"


extern NSString * const kENOJavaScriptErrorDomain;


@interface ENOJavaScriptApp : NSObject

+ (instancetype)sharedApp;

@property (nonatomic, readonly) JSContext *jsContext;

@property (nonatomic, readonly) ENOJSApp *jsAppGlobalObject;

@property (nonatomic, strong) NSString *lastException;
@property (nonatomic, assign) NSInteger lastExceptionLine;


- (BOOL)loadMainJS:(NSString *)js error:(NSError **)outError;

@end
