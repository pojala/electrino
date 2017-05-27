//
//  ENOJSTray.m
//  Electrino
//
//  Created by Pauli Ojala on 17/05/2017.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "ENOJSTray.h"
#import "ENOJSNativeImage.h"


@interface ENOJSTray ()

@property (nonatomic, strong) NSMutableDictionary *eventCallbacks;

@property (nonatomic, strong) NSStatusItem *statusItem;

@end


@implementation ENOJSTray

- (instancetype)initWithIcon:(id)icon
{
    self = [super init];
    
    NSLog(@"%s: %@", __func__, icon);
    
    self.eventCallbacks = [NSMutableDictionary dictionary];
    
    
    self.statusItem = [[NSStatusBar systemStatusBar] statusItemWithLength:NSSquareStatusItemLength];
    self.statusItem.highlightMode = YES;
    
    NSStatusBarButton *barButton = self.statusItem.button;
    barButton.action = @selector(statusBarButtonAction:);
    barButton.target = self;
    
    NSImage *image = nil;
    if ([icon respondsToSelector:@selector(image)]) {
        image = [icon image];
    }
    if (image) {
        image.template = YES;
        barButton.image = image;
    }
    else {
        barButton.title = @"E";
        barButton.font = [NSFont systemFontOfSize:17.0 weight:NSFontWeightBlack];
    }
    

    return self;
}

- (void)on:(NSString *)event withCallback:(JSValue *)cb
{
    if (event.length > 0 && cb) {
        NSMutableArray *cbArr = self.eventCallbacks[event] ?: [NSMutableArray array];
        [cbArr addObject:cb];
        
        self.eventCallbacks[event] = cbArr;
    }
}

- (NSDictionary *)getBounds
{
    NSView *view = self.statusItem.button;
    NSRect frameInWindow = [view convertRect:view.bounds toView:nil];
    NSRect frameOnScreen = [view.window convertRectToScreen:frameInWindow];
    
    //NSLog(@"frame %@ - window %@ - screen %@ / %p, %p", NSStringFromRect(view.frame), NSStringFromRect(frameInWindow), NSStringFromRect(frameOnScreen), view, view.window);
    
    return @{
             @"x": @(frameOnScreen.origin.x),
             @"y": @(frameOnScreen.origin.y),
             @"width": @(frameOnScreen.size.width),
             @"height": @(frameOnScreen.size.width),
             };
}


#pragma mark --- actions ---

- (IBAction)statusBarButtonAction:(id)sender
{
    //NSStatusBarButton *barButton = self.statusItem.button;
    
    for (JSValue *cb in self.eventCallbacks[@"click"]) {
        //NSLog(@"%s, %@", __func__, cb);
        
        [cb callWithArguments:@[]];
    }
    
    /*
     if (self.popover.shown) {
     [self.popover close];
     } else {
     [self.popover showRelativeToRect:barButton.bounds ofView:barButton preferredEdge:NSMinYEdge];
     
     if ( !self.popoverTransiencyMonitor) {
     self.popoverTransiencyMonitor = [NSEvent addGlobalMonitorForEventsMatchingMask:(NSLeftMouseDownMask | NSRightMouseDownMask | NSKeyUpMask) handler:^(NSEvent *event) {
     [NSEvent removeMonitor:self.popoverTransiencyMonitor];
     self.popoverTransiencyMonitor = nil;
     [self.popover close];
     }];
     }
     }*/
}


@end
