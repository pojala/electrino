//
//  ENOJSUrl.h
//  Electrino
//
//  Created by Pauli Ojala on 03/05/17.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>


@protocol ENOJSUrlExports <JSExport>

@property (nonatomic, copy) NSString *(^format)(NSDictionary *urlDict);

@end



@interface ENOJSUrl : NSObject <ENOJSUrlExports>

@end
