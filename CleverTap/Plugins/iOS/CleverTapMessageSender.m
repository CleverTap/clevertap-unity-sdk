//
//  CleverTapMessageSender.m
//
//  Created by Nikola Zagorchev on 12.11.24.
//

#import "CleverTapMessageSender.h"
#import "CleverTapUnityCallbackInfo.h"
#import "CleverTapMessageBuffer.h"

static NSString * kCleverTapGameObjectName = @"IOSCallbackHandler";

@interface CleverTapMessageSender()

@property NSDictionary<CleverTapUnityCallbackInfo *, CleverTapMessageBuffer *> *messageBuffers;

@end

@implementation CleverTapMessageSender

+ (instancetype)sharedInstance {
    static dispatch_once_t once = 0;
    static id _sharedObject = nil;
    dispatch_once(&once, ^{
        _sharedObject = [[self alloc] init];
    });
    return _sharedObject;
}

- (instancetype)init {
    if (self = [super init]) {
        self.messageBuffers = [self createBuffers:YES];
    }
    return self;
}

- (NSDictionary<CleverTapUnityCallbackInfo *, CleverTapMessageBuffer *> *)createBuffers:(BOOL)enabled {
    NSMutableDictionary<CleverTapUnityCallbackInfo *, CleverTapMessageBuffer *> *buffers = [NSMutableDictionary dictionary];
    NSArray *callbackInfos = [CleverTapUnityCallbackInfo callbackInfos];
    for (CleverTapUnityCallbackInfo *info in callbackInfos) {
        if (info.isBufferable) {
            buffers[info] = [[CleverTapMessageBuffer alloc] initWithEnabled:enabled];
        }
    }
    return [NSDictionary dictionaryWithDictionary:buffers];
}

- (void)send:(CleverTapUnityCallback)callback withMessage:(NSString *)message {
    CleverTapUnityCallbackInfo *callbackInfo = [CleverTapUnityCallbackInfo infoForCallback:callback];
    if (callbackInfo.isBufferable) {
        CleverTapMessageBuffer *buffer = self.messageBuffers[callbackInfo];
        if (buffer.isEnabled) {
            [buffer add:message];
            return;
        }
    }
    [self sendToUnity:callbackInfo withMessage:message];
}

- (void)sendToUnity:(CleverTapUnityCallbackInfo *)callbackInfo withMessage:(NSString *)message {
    UnitySendMessage([kCleverTapGameObjectName UTF8String], [callbackInfo.callbackName UTF8String], [message UTF8String]);
}

- (void)flushBuffer:(CleverTapUnityCallbackInfo *)callbackInfo {
    CleverTapMessageBuffer *buffer = self.messageBuffers[callbackInfo];
    if (!buffer) {
        return;
    }
    
    while (buffer.count > 0) {
        NSString *message = [buffer remove];
        [self sendToUnity:callbackInfo withMessage:message];
    }
}

- (void)disableBuffer:(CleverTapUnityCallbackInfo *)callbackInfo {
    CleverTapMessageBuffer *buffer = self.messageBuffers[callbackInfo];
    if (buffer) {
        [buffer setIsEnabled:NO];
    }
}

- (void)resetAllBuffers {
    self.messageBuffers = [self createBuffers:NO];
}

@end
