//
//  ENOJSIPCMain.h
//  Electrino
//
//  Created by Pauli Ojala on 17/05/2017.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <JavaScriptCore/JavaScriptCore.h>


@protocol ENOJSIPCMainExports <JSExport>

JSExportAs(on,
- (void)on:(NSString *)event withCallback:(JSValue *)cb
);

@end


@interface ENOJSIPCMain : NSObject <ENOJSIPCMainExports>

@end
