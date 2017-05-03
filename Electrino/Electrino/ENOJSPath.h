//
//  ENOJSPath.h
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>


@protocol ENOJSPathExports <JSExport>

@property (nonatomic, copy) NSString *(^join)();

@end



@interface ENOJSPath : NSObject <ENOJSPathExports>

@end
