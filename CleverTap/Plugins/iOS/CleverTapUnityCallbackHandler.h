#import <Foundation/Foundation.h>
#import <CleverTapSDK/CleverTap+Inbox.h>
#import <CleverTapSDK/CleverTap.h>
#import <CleverTapSDK/CleverTapSyncDelegate.h>
#import <CleverTapSDK/CleverTap+DisplayUnit.h>
#import <CleverTapSDK/CleverTap+FeatureFlags.h>
#import <CleverTapSDK/CleverTap+ProductConfig.h>
#import <CleverTapSDK/CleverTapInAppNotificationDelegate.h>
#import <CleverTapSDK/Clevertap+PushPermission.h>

NS_ASSUME_NONNULL_BEGIN

@interface CleverTapUnityCallbackHandler : NSObject 
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
<CleverTapInAppNotificationDelegate, CleverTapDisplayUnitDelegate, CleverTapInboxViewControllerDelegate, CleverTapProductConfigDelegate, CleverTapFeatureFlagsDelegate, CleverTapPushPermissionDelegate>

#pragma clang diagnostic pop

+ (instancetype)sharedInstance;

- (void)attachInstance:(CleverTap *)instance;

- (void)pushPermissionCallback:(BOOL)isPushEnabled;

- (CleverTapFetchVariablesBlock)fetchVariablesBlock:(int)callbackId;

- (CleverTapVariablesChangedBlock)variableValueChanged:(NSString *)varName;

- (CleverTapVariablesChangedBlock)variableFileIsReady:(NSString *)varName;

- (CleverTapFetchInAppsBlock)fetchInAppsBlock:(int)callbackId;
    
- (CleverTapInboxSuccessBlock)initializeInboxBlock;

- (CleverTapInboxUpdatedBlock)inboxUpdatedBlock;

- (void)didReceiveRemoteNotification:(UIApplicationState)applicationState data:(NSData *)data;
    
- (void)deepLinkCallback:(NSString *)url;

@end

NS_ASSUME_NONNULL_END
