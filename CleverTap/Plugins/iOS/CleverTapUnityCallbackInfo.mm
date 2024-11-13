#import "CleverTapUnityCallbackInfo.h"
#import <Foundation/Foundation.h>

@implementation CleverTapUnityCallbackInfo

- (instancetype)initWithName:(NSString *)callbackName bufferable:(BOOL)bufferable {
    self = [super init];
    if (self) {
        _callbackName = callbackName;
        _isBufferable = bufferable;
    }
    return self;
}

- (BOOL)isEqual:(id)object {
    if (self == object) return YES;
    if (![object isKindOfClass:[CleverTapUnityCallbackInfo class]]) return NO;
    
    CleverTapUnityCallbackInfo *other = (CleverTapUnityCallbackInfo *)object;
    return [self.callbackName isEqualToString:other.callbackName] && self.isBufferable == other.isBufferable;
}

- (NSUInteger)hash {
    return self.callbackName.hash ^ @(self.isBufferable).hash;
}

- (id)copyWithZone:(NSZone *)zone {
    // Return a new instance with the same property values
    return [[CleverTapUnityCallbackInfo allocWithZone:zone] initWithName:self.callbackName bufferable:self.isBufferable];
}

+ (NSArray<CleverTapUnityCallbackInfo *> *)callbackInfos {
    static NSArray<CleverTapUnityCallbackInfo *> *callbacks = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        callbacks = @[
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapProfileInitializedCallback" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapProfileUpdatesCallback" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapDeepLinkCallback" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapPushOpenedCallback" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInAppNotificationDismissedCallback" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInAppNotificationButtonTapped" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapOnPushPermissionResponseCallback" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInboxDidInitializeCallback" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInboxMessagesDidUpdateCallback" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInboxCustomExtrasButtonSelect" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInboxItemClicked" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInAppNotificationButtonTapped" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapNativeDisplayUnitsUpdated" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapFeatureFlagsUpdated" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapProductConfigInitialized" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapProductConfigFetched" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapProductConfigActivated" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInitCleverTapIdCallback" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapVariablesChanged" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapVariableValueChanged" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapVariablesFetched" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapInAppsFetched" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapVariablesChangedAndNoDownloadsPending" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapVariableFileIsReady" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapCustomTemplatePresent" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapCustomFunctionPresent" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapCustomTemplateClose" bufferable:NO],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapPushReceivedCallback" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapPushPermissionResponseReceived" bufferable:YES],
            [[CleverTapUnityCallbackInfo alloc] initWithName:@"CleverTapPushNotificationPermissionStatus" bufferable:NO]
        ];
    });
    return callbacks;
}

+ (nullable CleverTapUnityCallbackInfo *)infoForCallback:(CleverTapUnityCallback)callback {
    NSArray<CleverTapUnityCallbackInfo *> *callbacks = [self callbackInfos];
    if (callback < callbacks.count) {
        return callbacks[callback];
    }
    return nil;
}

+ (nullable CleverTapUnityCallbackInfo *)callbackFromName:(NSString *)callbackName {
    NSArray<CleverTapUnityCallbackInfo *> *callbacks = [self callbackInfos];
    for (CleverTapUnityCallbackInfo *callback in callbacks) {
        if ([callback.callbackName isEqualToString:callbackName]) {
            return callback;
        }
    }
    return nil;
}

@end