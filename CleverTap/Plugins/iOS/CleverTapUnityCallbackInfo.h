//
//  CleverTapUnityCallbackInfo.h
//
//  Created by Nikola Zagorchev on 11.11.24.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, CleverTapUnityCallback) {
    CleverTapUnityCallbackProfileInitialized = 0,
    CleverTapUnityCallbackProfileUpdates = 1,
    CleverTapUnityCallbackDeepLink = 2,
    CleverTapUnityCallbackPushOpened = 3,
    CleverTapUnityCallbackInAppNotificationDismissed = 4,
    CleverTapUnityCallbackInAppNotificationButtonTapped = 5,
    CleverTapUnityCallbackOnPushPermissionResponse = 6,
    CleverTapUnityCallbackInboxDidInitialize = 7,
    CleverTapUnityCallbackInboxMessagesDidUpdate = 8,
    CleverTapUnityCallbackInboxCustomExtrasButtonSelect = 9,
    CleverTapUnityCallbackInboxItemClicked = 10,
    CleverTapUnityCallbackInAppButtonClicked = 11,
    CleverTapUnityCallbackNativeDisplayUnitsUpdated = 12,
    CleverTapUnityCallbackFeatureFlagsUpdated = 13,
    CleverTapUnityCallbackProductConfigInitialized = 14,
    CleverTapUnityCallbackProductConfigFetched = 15,
    CleverTapUnityCallbackProductConfigActivated = 16,
    CleverTapUnityCallbackInitCleverTapId = 17,
    CleverTapUnityCallbackVariablesChanged = 18,
    CleverTapUnityCallbackVariableValueChanged = 19,
    CleverTapUnityCallbackVariablesFetched = 20,
    CleverTapUnityCallbackInAppsFetched = 21,
    CleverTapUnityCallbackVariablesChangedAndNoDownloadsPending = 22,
    CleverTapUnityCallbackVariableFileIsReady = 23,
    CleverTapUnityCallbackCustomTemplatePresent = 24,
    CleverTapUnityCallbackCustomFunctionPresent = 25,
    CleverTapUnityCallbackCustomTemplateClose = 26,
    CleverTapUnityCallbackPushReceived = 27,
    CleverTapUnityCallbackPushPermissionResponseReceived = 28,
    CleverTapUnityCallbackPushNotificationPermissionStatus = 29
};

@interface CleverTapUnityCallbackInfo : NSObject <NSCopying>

@property (nonatomic, strong, readonly) NSString *callbackName;
@property (nonatomic, assign, readonly) BOOL isBufferable;

+ (nullable CleverTapUnityCallbackInfo *)infoForCallback:(CleverTapUnityCallback)callback;
+ (nullable CleverTapUnityCallbackInfo *)callbackFromName:(NSString *)callbackName;
+ (NSArray<CleverTapUnityCallbackInfo *> *)callbackInfos;

@end

NS_ASSUME_NONNULL_END
