//
//  ENOJSTray.h
//  Electrino
//
//  Created by Pauli Ojala on 17/05/2017.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>


@protocol ENOJSTrayExports <JSExport>

- (instancetype)initWithIcon:(id)icon;

JSExportAs(on,
- (void)on:(NSString *)event withCallback:(JSValue *)cb
);

- (NSDictionary *)getBounds;

@end


@interface ENOJSTray : NSObject <ENOJSTrayExports>

@end
