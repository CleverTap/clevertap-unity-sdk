//
//  CleverTapUnityCallbackHandler.h
//
//  Created by Nikola Zagorchev on 11.11.24.
//

#import <Foundation/Foundation.h>
#import <CleverTapSDK/CleverTap+Inbox.h>

NS_ASSUME_NONNULL_BEGIN

@interface CleverTapUnityCallbackHandler : NSObject

+ (instancetype)sharedInstance;

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
