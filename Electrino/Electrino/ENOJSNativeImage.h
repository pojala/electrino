//
//  ENOJSNativeImage.h
//  Electrino
//
//  Created by Pauli Ojala on 17/05/2017.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
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
