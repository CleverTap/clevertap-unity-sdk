#import "CleverTapUnityManager.h"
#import <CleverTapSDK/CleverTap.h>
#import <CleverTapSDK/CleverTapSyncDelegate.h>
#import <CleverTapSDK/CleverTapInAppNotificationDelegate.h>

static CleverTap *clevertap;

static NSString * kCleverTapGameObjectName = @"CleverTapUnity";
static NSString * kCleverTapGameObjectProfileInitializedCallback = @"CleverTapProfileInitializedCallback";
static NSString * kCleverTapGameObjectProfileUpdatesCallback = @"CleverTapProfileUpdatesCallback";
static NSString * kCleverTapDeepLinkCallback = @"CleverTapDeepLinkCallback";
static NSString * kCleverTapPushReceivedCallback = @"CleverTapPushReceivedCallback";
static NSString * kCleverTapPushOpenedCallback = @"CleverTapPushOpenedCallback";
static NSString * kCleverTapInAppNotificationDismissedCallback = @"CleverTapInAppNotificationDismissedCallback";

@interface CleverTapUnityManager () <CleverTapInAppNotificationDelegate> {
}

@end

@implementation CleverTapUnityManager

+ (CleverTapUnityManager*)sharedInstance {
    static CleverTapUnityManager *sharedInstance = nil;
    
    if(!sharedInstance) {
        sharedInstance = [[CleverTapUnityManager alloc] init];
        [sharedInstance registerListeners];
        
        clevertap = [CleverTap sharedInstance];
        
        [clevertap setInAppNotificationDelegate:sharedInstance];
    }
    
    return sharedInstance;
}

#pragma mark Profile/Event/Session APIs

#pragma mark Profile API

- (void)onUserLogin:(NSDictionary *)properties {
    [clevertap onUserLogin:properties];
}

- (void)profilePush:(NSDictionary *)properties {
    [clevertap profilePush:properties];
}

- (void)profilePushGraphUser:(NSDictionary *)fbGraphUser {
    [clevertap profilePushGraphUser:fbGraphUser];
}

- (void)profilePushGooglePlusUser:(NSDictionary *)googleUser {
    [clevertap profilePushGooglePlusUser:googleUser];
}

- (id)profileGet:(NSString *)propertyName {
    return [clevertap profileGet:propertyName];
}

- (void)profileRemoveValueForKey:(NSString *)key {
    [clevertap profileRemoveValueForKey:key];
}

- (void)profileSetMultiValues:(NSArray<NSString *> *)values forKey:(NSString *)key {
    [clevertap profileSetMultiValues:values forKey:key];
}

- (void)profileAddMultiValue:(NSString *)value forKey:(NSString *)key {
    [clevertap profileAddMultiValue:value forKey:key];
}

- (void)profileAddMultiValues:(NSArray<NSString *> *)values forKey:(NSString *)key {
    [clevertap profileAddMultiValues:values forKey:key];
}

- (void)profileRemoveMultiValue:(NSString *)value forKey:(NSString *)key {
    [clevertap profileRemoveMultiValue:value forKey:key];
}

- (void)profileRemoveMultiValues:(NSArray<NSString *> *)values forKey:(NSString *)key {
    [clevertap profileRemoveMultiValues:values forKey:key];
}

- (NSString *)profileGetCleverTapID {
    return [clevertap profileGetCleverTapID];
}

- (NSString*)profileGetCleverTapAttributionIdentifier {
    return [clevertap profileGetCleverTapAttributionIdentifier];
}

#pragma mark User Action Events API

- (void)recordEvent:(NSString *)event {
    [clevertap recordEvent:event];
}

- (void)recordEvent:(NSString *)event withProps:(NSDictionary *)properties {
    [clevertap recordEvent:event withProps:properties];
}

- (void)recordChargedEventWithDetails:(NSDictionary *)chargeDetails andItems:(NSArray *)items {
    [clevertap recordChargedEventWithDetails:chargeDetails andItems:items];
}

- (void)recordErrorWithMessage:(NSString *)message andErrorCode:(int)code {
    [clevertap recordErrorWithMessage:message andErrorCode:code];
}

- (NSTimeInterval)eventGetFirstTime:(NSString *)event {
    return [clevertap eventGetFirstTime:event];
}

- (NSTimeInterval)eventGetLastTime:(NSString *)event {
    return [clevertap eventGetLastTime:event];
}

- (int)eventGetOccurrences:(NSString *)event {
    return [clevertap eventGetOccurrences:event];
}

- (NSDictionary *)userGetEventHistory {
    return [clevertap userGetEventHistory];
}

- (CleverTapEventDetail *)eventGetDetail:(NSString *)event {
    return [clevertap eventGetDetail:event];
}


#pragma mark Session API

- (NSTimeInterval)sessionGetTimeElapsed {
    return [clevertap sessionGetTimeElapsed];
}

- (CleverTapUTMDetail *)sessionGetUTMDetails {
    return [clevertap sessionGetUTMDetails];
}

- (int)userGetTotalVisits {
    return [clevertap userGetTotalVisits];
}

- (int)userGetScreenCount {
    return [clevertap userGetScreenCount];
}

- (NSTimeInterval)userGetPreviousVisitTime {
    return [clevertap userGetPreviousVisitTime];
}

# pragma mark Notifications

+ (void)registerPush {
    UIApplication *application = [UIApplication sharedApplication];
    if ([application respondsToSelector:@selector(isRegisteredForRemoteNotifications)]) {
        UIUserNotificationSettings *settings = [UIUserNotificationSettings
                                                settingsForTypes:UIUserNotificationTypeAlert | UIUserNotificationTypeBadge | UIUserNotificationTypeSound
                                                categories:nil];
        
        
        [application registerUserNotificationSettings:settings];
        [application registerForRemoteNotifications];
    }
    else {
#if __IPHONE_OS_VERSION_MAX_ALLOWED < 80000
        [application registerForRemoteNotificationTypes:
         (UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeSound)];
#endif
    }
}

- (void)setPushToken:(NSData *)pushToken {
    [clevertap setPushToken:pushToken];
}

- (void)setPushTokenAsString:(NSString *)pushTokenString {
    [clevertap setPushTokenAsString:pushTokenString];
}

- (void)handleNotificationWithData:(id)data {
    [clevertap handleNotificationWithData:data];
}

- (void)showInAppNotificationIfAny {
    [clevertap showInAppNotificationIfAny];
}

- (void)registerApplication:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)notification {
    [self handleNotificationWithData:notification];
    
    // generate a new dictionary that rearrange the notification elements
    NSMutableDictionary *aps = [NSMutableDictionary dictionaryWithDictionary:[notification objectForKey:@"aps"]];
    
    // check if the object for key alert is a string; if it is, then convert it to a dictionary
    id alert = [aps objectForKey:@"alert"];
    if ([alert isKindOfClass:[NSString class]]) {
        NSDictionary *alertDictionary = [NSDictionary dictionaryWithObject:alert forKey:@"body"];
        [aps setObject:alertDictionary forKey:@"alert"];
    }
    
    // move all other dictionarys other than aps in payload to key extra in aps dictionary
    NSMutableDictionary *extraDictionary = [NSMutableDictionary dictionaryWithDictionary:notification];
    [extraDictionary removeObjectForKey:@"aps"];
    if ([extraDictionary count] > 0) {
        [aps setObject:extraDictionary forKey:@"extra"];
    }
    
    if ([NSJSONSerialization isValidJSONObject:aps]) {
        NSError *pushParsingError = nil;
        NSData *data = [NSJSONSerialization dataWithJSONObject:aps options:0 error:&pushParsingError];
        
        if (pushParsingError == nil) {
            NSString *dataString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
            
            NSString *methodName = (application.applicationState == UIApplicationStateActive) ? kCleverTapPushReceivedCallback : kCleverTapPushOpenedCallback;
            
            [self callUnityObject:kCleverTapGameObjectName forMethod:methodName withMessage:dataString];
        }
    }
}


#pragma mark DeepLink handling

- (void)handleOpenURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication {
    
    [self callUnityObject:kCleverTapGameObjectName forMethod:kCleverTapDeepLinkCallback withMessage:[url absoluteString]];
}

#pragma mark Referrer Tracking

- (void)pushInstallReferrerSource:(NSString *)source
                           medium:(NSString *)medium
                         campaign:(NSString *)campaign {
    
    [clevertap pushInstallReferrerSource:source medium:medium campaign:campaign];
}

#pragma mark Admin

+ (void)launchWithAccountID:(NSString*)accountID andToken:(NSString *)token {
    [CleverTap changeCredentialsWithAccountID:accountID andToken:token];
    [[CleverTap sharedInstance] notifyApplicationLaunchedWithOptions:nil];
}

+ (void)setApplicationIconBadgeNumber:(int)num {
    [UIApplication sharedApplication].applicationIconBadgeNumber = num;
}

+ (void)setDebugLevel:(int)level {
    [CleverTap setDebugLevel:level];
}

- (void)setSyncDelegate:(id <CleverTapSyncDelegate>)delegate {
    [clevertap setSyncDelegate:delegate];
}

+ (void)enablePersonalization {
    [CleverTap enablePersonalization];
}

+ (void)disablePersonalization {
    [CleverTap disablePersonalization];
}

+ (void)setLocation:(CLLocationCoordinate2D)location {
    [CleverTap setLocation:location];
}

# pragma mark CleverTapInAppNotificationDelegate

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
        [self callUnityObject:kCleverTapGameObjectName forMethod:kCleverTapInAppNotificationDismissedCallback withMessage:jsonString];
    }
}

# pragma mark CleverTapSyncDelegate/Listener

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
        [self callUnityObject:kCleverTapGameObjectName forMethod:kCleverTapGameObjectProfileInitializedCallback withMessage:jsonString];
    }
}


- (void)didReceiveCleverTapProfileDidChangeNotification:(NSNotification*)notification {
    NSString *jsonString = [self dictToJson:notification.userInfo];
    if (jsonString != nil) {
        [self callUnityObject:kCleverTapGameObjectName forMethod:kCleverTapGameObjectProfileUpdatesCallback withMessage:jsonString];
    }
}

#pragma mark private helpers

-(void)callUnityObject:(NSString *)objectName forMethod:(NSString *)method withMessage:(NSString *)message {
    UnitySendMessage([objectName UTF8String], [method UTF8String], [message UTF8String]);
}

-(NSString *)dictToJson:(NSDictionary *)dict {
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&err];
    
    if(err != nil) {
        return nil;
    }
    
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

@end
