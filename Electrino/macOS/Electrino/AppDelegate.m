//
//  AppDelegate.m
//  Electrino
//
//  Created by Pauli Olavi Ojala on 03/05/17.
//  Copyright © 2017 Pauli Olavi Ojala.
//
//  This software may be modified and distributed under the terms of the MIT license.  See the LICENSE file for details.
//

#import "AppDelegate.h"
#import "ENOJavaScriptApp.h"
#import "ENOBrowserWindowController.h"


@interface AppDelegate ()

@end




@implementation AppDelegate

- (void)applicationDidFinishLaunching:(NSNotification *)aNotification
{
    // enable WebKit devtools
    [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"WebKitDeveloperExtras"];

    
    NSString *appDir = [[[NSBundle mainBundle] resourcePath] stringByAppendingPathComponent:@"app"];
    NSString *mainJSPath = [appDir stringByAppendingPathComponent:@"main.js"];
    
    if ( ![[NSFileManager defaultManager] fileExistsAtPath:mainJSPath]) {
        NSLog(@"** no main.js found in dir: %@", appDir);
        [NSApp terminate:nil]; // --
    }
    
    NSString *mainJS = [NSString stringWithContentsOfFile:mainJSPath encoding:NSUTF8StringEncoding error:NULL];
    ENOJavaScriptApp *jsApp = [ENOJavaScriptApp sharedApp];
    NSError *error = nil;
    
    // app setup
    jsApp.jsContext[@"__dirname"] = appDir;
    
    // load code
    if ( ![jsApp loadMainJS:mainJS error:&error]) {
        NSLog(@"** could not load main.js: %@, path: %@", error, mainJSPath);
        
        [self _presentJSError:error message:@"Main program execution failed."];
        
        [NSApp terminate:nil]; // --
    }
    
    // for the old-style WebView, it seems that there needs to be an instance initialized early;
    // otherwise it can go into a weird state where scripts and default styles don't load at all.
    {
    ENOBrowserWindowController *windowController = [[ENOBrowserWindowController alloc] initWithWindowNibName:@"ENOBrowserWindowController"];
    NSWindow *win = windowController.window;
    [win setFrame:NSMakeRect(0, 0, 1, 1) display:NO];
    }
    
    // send 'ready' event to the main app
    if ( ![jsApp.jsAppGlobalObject emitReady:&error]) {
        NSLog(@"** app.on('ready'): %@", error);
        
        [self _presentJSError:error message:@"Exception in app.on('ready')"];
    }
}

- (void)_presentJSError:(NSError *)error message:(NSString *)msg
{
    NSAlert *alert = [[NSAlert alloc] init];
    alert.alertStyle = NSCriticalAlertStyle;
    alert.messageText = msg ?: @"JavaScript error";
    alert.informativeText = [NSString stringWithFormat:@"\n%@\n\n(On line %@)", error.localizedDescription, error.userInfo[@"SourceLineNumber"]];
    
    [alert runModal];
}

- (void)applicationWillTerminate:(NSNotification *)aNotification
{
    
}


@end
