//
//  ENOJSNativeImage.m
//  Electrino
//
//  Created by Pauli Ojala on 17/05/2017.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOJSNativeImage.h"


@implementation ENOJSNativeImageAPI

- (id)createFromDataURL:(NSString *)dataURL
{
    NSString *pngPrefix = @"data:image/png;base64,";
    if ( ![dataURL hasPrefix:pngPrefix]) {
        NSLog(@"** %s: invalid data url", __func__);
        return nil;  // --
    }
    
    NSString *base64Str = [dataURL substringFromIndex:pngPrefix.length];
    
    NSData *data = [[NSData alloc] initWithBase64EncodedString:base64Str options:NSDataBase64DecodingIgnoreUnknownCharacters];
    NSImage *image = [[NSImage alloc] initWithData:data];
    
    ENOJSNativeImageInstance *jsImage = [[ENOJSNativeImageInstance alloc] init];
    jsImage.image = image;
    
    return jsImage;
}

@end



@implementation ENOJSNativeImageInstance
@end
