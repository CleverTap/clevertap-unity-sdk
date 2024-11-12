//
//  CleverTapUnityCallbackHandler.m
//
//  Created by Nikola Zagorchev on 11.11.24.
//

#import "CleverTapUnityCallbackHandler.h"
#import "CleverTapUnityCallbackInfo.h"
#import "CleverTapMessageSender.h"
#import <CleverTapSDK/CleverTap.h>
#import <CleverTapSDK/CleverTapSyncDelegate.h>
#import <CleverTapSDK/CleverTap+DisplayUnit.h>
#import <CleverTapSDK/CleverTap+FeatureFlags.h>
#import <CleverTapSDK/CleverTap+ProductConfig.h>
#import <CleverTapSDK/CleverTapInAppNotificationDelegate.h>
#import <CleverTapSDK/Clevertap+PushPermission.h>

@interface CleverTapUnityCallbackHandler () <CleverTapInAppNotificationDelegate, CleverTapDisplayUnitDelegate, CleverTapInboxViewControllerDelegate, CleverTapProductConfigDelegate, CleverTapFeatureFlagsDelegate, CleverTapPushPermissionDelegate>

@end

@implementation CleverTapUnityCallbackHandler

+ (instancetype)sharedInstance {
    static dispatch_once_t once = 0;
    static id _sharedObject = nil;
    dispatch_once(&once, ^{
        _sharedObject = [[self alloc] init];
    });
    return _sharedObject;
}

- (void)callUnityObject:(CleverTapUnityCallback)callback withMessage:(NSString *)message {
    [[CleverTapMessageSender sharedInstance] send:callback withMessage:message];
}

- (void)attachInstance:(CleverTap *)instance {
    [self registerListeners];
    
    [instance setInAppNotificationDelegate:self];
    [instance setDisplayUnitDelegate:self];
    [instance setPushPermissionDelegate:self];
    
    [instance onVariablesChanged:[self variablesChanged]];
    [instance onVariablesChangedAndNoDownloadsPending:[self variablesChangedAndNoDownloadsPending]];
    
    [[instance productConfig] setDelegate:self];
    [[instance featureFlags] setDelegate:self];
}

- (CleverTapVariablesChangedBlock)variablesChanged {
    return ^{
        [self callUnityObject:CleverTapUnityCallbackVariablesChanged withMessage:@"VariablesChanged"];
    };
}

- (CleverTapVariablesChangedBlock)variablesChangedAndNoDownloadsPending {
    return ^{
        [self callUnityObject:CleverTapUnityCallbackVariablesChangedAndNoDownloadsPending withMessage:@"VariablesChangedAndNoDownloadsPending"];
    };
}

- (CleverTapFetchVariablesBlock)fetchVariablesBlock:(int)callbackId {
    return ^(BOOL success) {
        NSDictionary* response = @{
            @"callbackId": @(callbackId),
            @"isSuccess": @(success)
        };
        
        NSString* json = [self dictToJson:response];
        [self callUnityObject:CleverTapUnityCallbackVariablesFetched withMessage:json];
    };
}

- (CleverTapFetchInAppsBlock)fetchInAppsBlock:(int)callbackId {
    return ^(BOOL success) {
        NSDictionary* response = @{
            @"callbackId": @(callbackId),
            @"isSuccess": @(success)
        };
        
        NSString* json = [self dictToJson:response];
        [self callUnityObject:CleverTapUnityCallbackInAppsFetched withMessage:json];
    };
}

- (void)pushPermissionCallback:(BOOL)isPushEnabled {
    [self callUnityObject:CleverTapUnityCallbackPushNotificationPermissionStatus withMessage:[NSString stringWithFormat:@"%@", isPushEnabled? @"True": @"False"]];
}

- (CleverTapVariablesChangedBlock)variableValueChanged:(NSString *)varName {
    return ^{
        [self callUnityObject:CleverTapUnityCallbackVariableValueChanged withMessage:varName];
    };
}

- (CleverTapVariablesChangedBlock)variableFileIsReady:(NSString *)varName {
    return ^{
        [self callUnityObject:CleverTapUnityCallbackVariableFileIsReady withMessage:varName];
    };
}

- (CleverTapInboxSuccessBlock)initializeInboxBlock {
    return ^(BOOL success) {
        NSLog(@"Inbox initialized %d", success);
        [self callUnityObject:CleverTapUnityCallbackInboxDidInitialize withMessage:[NSString stringWithFormat:@"%@", success? @"YES": @"NO"]];
    };
}

- (CleverTapInboxUpdatedBlock)inboxUpdatedBlock {
    return ^{
        NSLog(@"Inbox Messages updated");
        [self callUnityObject:CleverTapUnityCallbackInboxMessagesDidUpdate withMessage:@"inbox updated."];
    };
}

- (void)didReceiveRemoteNotification:(UIApplicationState)applicationState data:(NSData *)data {
    NSString *dataString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
    
    CleverTapUnityCallback callback = (applicationState == UIApplicationStateActive) ? CleverTapUnityCallbackPushReceived : CleverTapUnityCallbackPushOpened;
    
    [self callUnityObject:callback withMessage:dataString];
}

- (void)deepLinkCallback:(NSString *)url {
    [self callUnityObject:CleverTapUnityCallbackDeepLink withMessage:url];
}

- (NSString *)dictToJson:(NSDictionary *)dict {
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&err];
    
    if(err != nil) {
        return nil;
    }
    
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

- (void)dealloc {
    [[NSNotificationCenter defaultCenter] removeObserver:self];
}

#pragma mark - CleverTapSyncDelegate/Listener

- (void)registerListeners {
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(didReceiveCleverTapProfileDidChangeNotification:)
                                                 name:CleverTapProfileDidChangeNotification object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(didReceiveCleverTapProfileDidInitializeNotification:)
                                                 name:CleverTapProfileDidInitializeNotification object:nil];
}

- (void)didReceiveCleverTapProfileDidInitializeNotification:(NSNotification*)notification {
    NSString *jsonString = [self dictToJson:notification.userInfo];
    if (jsonString != nil) {
        [self callUnityObject:CleverTapUnityCallbackProfileInitialized withMessage:jsonString];
    }
}


- (void)didReceiveCleverTapProfileDidChangeNotification:(NSNotification*)notification {
    NSString *jsonString = [self dictToJson:notification.userInfo];
    if (jsonString != nil) {
        [self callUnityObject:CleverTapUnityCallbackProfileUpdates withMessage:jsonString];
    }
}

#pragma mark - InApp Notification Delegates

- (void)inAppNotificationDismissedWithExtras:(NSDictionary *)extras andActionExtras:(NSDictionary *)actionExtras {
    
    NSMutableDictionary *jsonDict = [NSMutableDictionary new];
    
    if (extras != nil) {
        jsonDict[@"extras"] = extras;
    }
    
    if (actionExtras != nil) {
        jsonDict[@"actionExtras"] = actionExtras;
    }
    
    NSString *jsonString = [self dictToJson:jsonDict];
    
    if (jsonString != nil) {
        [self callUnityObject:CleverTapUnityCallbackInAppNotificationDismissed withMessage:jsonString];
    }
}

- (void)inAppNotificationButtonTappedWithCustomExtras:(NSDictionary *)customExtras {
    
    NSMutableDictionary *jsonDict = [NSMutableDictionary new];
    
    if (customExtras != nil) {
        jsonDict[@"customExtras"] = customExtras;
    }
    
    NSString *jsonString = [self dictToJson:jsonDict];
    
    if (jsonString != nil) {
        [self callUnityObject:CleverTapUnityCallbackInAppNotificationButtonTapped withMessage:jsonString];
    }
}


#pragma mark - Native Display

- (void)displayUnitsUpdated:(NSArray<CleverTapDisplayUnit *>*)displayUnits {
    
    NSMutableArray *jsonArray = [NSMutableArray new];
    
    for (id object in displayUnits) {
        if ([object isKindOfClass:[CleverTapDisplayUnit class]]) {
            CleverTapDisplayUnit *unit = object;
            [jsonArray addObject:unit.json];
        }
    }
    
    NSMutableDictionary *jsonDict = [NSMutableDictionary new];
    
    if (jsonArray != nil) {
        jsonDict[@"displayUnits"] = jsonArray;
    }
    
    NSString *jsonString = [self dictToJson:jsonDict];
    
    if (jsonString != nil) {
        [self callUnityObject:CleverTapUnityCallbackNativeDisplayUnitsUpdated withMessage:jsonString];
    }
}

#pragma mark - App Inbox

- (void)messageButtonTappedWithCustomExtras:(NSDictionary *)customExtras {
    
    NSMutableDictionary *jsonDict = [NSMutableDictionary new];
    
    if (customExtras != nil) {
        jsonDict[@"customExtras"] = customExtras;
    }
    
    NSString *jsonString = [self dictToJson:jsonDict];
    
    if (jsonString != nil) {
        [self callUnityObject:CleverTapUnityCallbackInboxCustomExtrasButtonSelect withMessage:jsonString];
    }
}

- (void)messageDidSelect:(CleverTapInboxMessage *_Nonnull)message atIndex:(int)index withButtonIndex:(int)buttonIndex {
    NSMutableDictionary *body = [NSMutableDictionary new];
    if ([message json] != nil) {
        NSError *error;
        if ([message json] != nil) {
            body[@"CTInboxMessagePayload"] = [NSMutableDictionary dictionaryWithDictionary:[message json]];
        }
        body[@"ContentPageIndex"] = @(index);
        body[@"ButtonIndex"] = @(buttonIndex);
        NSString *jsonString = [self dictToJson:body];
        if (jsonString != nil) {
            [self callUnityObject:CleverTapUnityCallbackInboxItemClicked withMessage:jsonString];
        }
    }
}

#pragma mark - Push Permission Delegate

- (void)onPushPermissionResponse:(BOOL)accepted {
    NSMutableDictionary *jsonDict = [NSMutableDictionary new];
   
    jsonDict[@"accepted"] = [NSNumber numberWithBool:accepted];
    
    NSString *jsonString = [self dictToJson:jsonDict];

    if (jsonString != nil) {
        [self callUnityObject:CleverTapUnityCallbackPushPermissionResponseReceived withMessage:jsonString];
    }
}

#pragma mark - Product Config

- (void)ctProductConfigFetched {
    [self callUnityObject:CleverTapUnityCallbackProductConfigFetched withMessage:@"Product Config Fetched"];
}

- (void)ctProductConfigActivated {
    [self callUnityObject:CleverTapUnityCallbackProductConfigActivated withMessage:@"Product Config Activated"];
}

- (void)ctProductConfigInitialized {
    [self callUnityObject:CleverTapUnityCallbackProductConfigInitialized withMessage:@"Product Config Initialized"];
}

#pragma mark - Feature Flags

- (void)ctFeatureFlagsUpdated {
    [self callUnityObject:CleverTapUnityCallbackFeatureFlagsUpdated withMessage:@"Feature Flags updated"];
}

@end
