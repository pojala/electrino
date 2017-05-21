//
//  ENOJSNativeImage.h
//  Electrino
//
//  Created by Pauli Ojala on 17/05/2017.
//  Copyright © 2017 Lacquer. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import <JavaScriptCore/JavaScriptCore.h>


@protocol ENOJSNativeImageAPIExports <JSExport>

- (id)createFromDataURL:(NSString *)dataURL;

@end


@interface ENOJSNativeImageAPI : NSObject <ENOJSNativeImageAPIExports>

@end


@interface ENOJSNativeImageInstance : NSObject

@property (nonatomic, strong) NSImage *image;

@end
