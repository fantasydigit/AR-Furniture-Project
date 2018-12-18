//
//  NatCamRenderDispatch.m
//  NatCam Render Pipeline
//
//  Created by Yusuf Olokoba on 2/26/17.
//  Copyright © 2017 Yusuf Olokoba. All rights reserved.
//

#import "NatCamRenderDispatch.h"
#import "IUnityInterface.h"
#import "IUnityGraphics.h"

#define NATCAM_ID 0x6723872

static RenderDispatch* sharedInstance;

@interface RenderDispatch ()
@property NSMutableArray *queue;
@end

@implementation RenderDispatch

@synthesize queue;

+ (void) initialize {
    sharedInstance = [RenderDispatch new];
    sharedInstance.queue = [NSMutableArray new];
}

+ (void) dispatch:(RenderDelegate) delegate {
    @synchronized (sharedInstance) {
        [sharedInstance.queue addObject:delegate];
    }
}

- (void) invoke {
    NSArray* executing;
    @synchronized (self) {
        executing = [NSArray arrayWithArray:queue];
        [queue removeAllObjects];
    }
    for (int i = 0; i < executing.count; i++) ((RenderDelegate)executing[i])();
}
@end


#pragma mark --Unity Interop--

static void UNITY_INTERFACE_API OnRenderDispatch (int eventID) {
    if (eventID == NATCAM_ID) [sharedInstance invoke];
}

UnityRenderingEvent UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API NatCamRenderDelegate () {
    return OnRenderDispatch;
}
