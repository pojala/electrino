//
//  ENOJSNativeImage.m
//  Electrino
//
//  Created by Pauli Ojala on 17/05/2017.
//  Copyright © 2017 Lacquer. All rights reserved.
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
