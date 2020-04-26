#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

#import <CleverTapSDK/CleverTap.h>
#import <CleverTapSDK/CleverTapUTMDetail.h>
#import <CleverTapSDK/CleverTapEventDetail.h>


@interface CleverTapUnityManager : NSObject

+ (CleverTapUnityManager *)sharedInstance;

+ (void)launchWithAccountID:(NSString*)accountID andToken:(NSString *)token;
+ (void)launchWithAccountID:(NSString*)accountID token:(NSString *)token region:(NSString *)region;
+ (void)setDebugLevel:(int)level;
+ (void)enablePersonalization;
+ (void)disablePersonalization;
+ (void)setLocation:(CLLocationCoordinate2D)location;
+ (void)registerPush;
+ (void)setApplicationIconBadgeNumber:(int)num;


#pragma mark - Offline API

- (void)setOffline:(BOOL)enabled;


#pragma mark - Opt-out API

- (void)setOptOut:(BOOL)enabled;
- (void)enableDeviceNetworkInfoReporting:(BOOL)enabled;


#pragma mark - User Profile

- (void)onUserLogin:(NSDictionary *)properties;
- (void)profilePush:(NSDictionary *)properties;
- (void)profileRemoveValueForKey:(NSString *)key;
- (void)profileSetMultiValues:(NSArray<NSString *> *)values forKey:(NSString*)key;
- (void)profileAddMultiValue:(NSString *)value forKey:(NSString *)key;
- (void)profileAddMultiValues:(NSArray<NSString *> *)values forKey:(NSString*)key;
- (void)profileRemoveMultiValue:(NSString *)value forKey:(NSString *)key;
- (void)profileRemoveMultiValues:(NSArray<NSString *> *)values forKey:(NSString*)key;
- (void)profilePushGraphUser:(NSDictionary *)fbGraphUser;
- (void)profilePushGooglePlusUser:(NSDictionary *)googleUser;
- (id)profileGet:(NSString *)propertyName;
- (NSString *)profileGetCleverTapID;
- (NSString *)profileGetCleverTapAttributionIdentifier;


#pragma mark - User Action Events

- (void)recordScreenView:(NSString *)screenName;
- (void)recordEvent:(NSString *)event;
- (void)recordEvent:(NSString *)event withProps:(NSDictionary *)properties;
- (void)recordChargedEventWithDetails:(NSDictionary *)chargeDetails andItems:(NSArray *)items;

- (NSTimeInterval)eventGetFirstTime:(NSString *)event;
- (NSTimeInterval)eventGetLastTime:(NSString *)event;
- (int)eventGetOccurrences:(NSString *)event;
- (NSDictionary *)userGetEventHistory;
- (CleverTapEventDetail *)eventGetDetail:(NSString *)event;


#pragma mark - User Session

- (NSTimeInterval)sessionGetTimeElapsed;
- (CleverTapUTMDetail *)sessionGetUTMDetails;
- (int)userGetTotalVisits;
- (int)userGetScreenCount;
- (NSTimeInterval)userGetPreviousVisitTime;


#pragma mark - Push Notifications

- (void)setPushToken:(NSData *)pushToken;
- (void)setPushTokenAsString:(NSString *)pushTokenString;
- (void)registerApplication:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)notification;

- (void)handleOpenURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication;
- (void)pushInstallReferrerSource:(NSString *)source
                           medium:(NSString *)medium
                         campaign:(NSString *)campaign;


#pragma mark - App Inbox

- (void)initializeInbox;
- (void)showAppInbox:(NSDictionary *)styleConfig;
- (int)getInboxMessageUnreadCount;
- (int)getInboxMessageCount;
- (void)recordInboxNotificationViewedEventForID:(NSString *)messageId;


#pragma mark - Native Display

- (NSArray *)getAllDisplayUnits;
- (void)recordDisplayUnitViewedEventForID:(NSString *)unitID;
- (void)recordDisplayUnitClickedEventForID:(NSString *)unitID;


#pragma mark - AB Testing

- (void)setUIEditorConnectionEnabled:(BOOL)enabled;
- (void)registerStringVariable:(NSString *)name;
- (void)registerIntegerVariable:(NSString *)name;
- (void)registerDoubleVariable:(NSString *)name;
- (void)registerBooleanVariable:(NSString *)name;
- (void)registerMapOfStringVariable:(NSString *)name;
- (void)registerMapOfIntegerVariable:(NSString *)name;
- (void)registerMapOfDoubleVariable:(NSString *)name;
- (void)registerMapOfBooleanVariable:(NSString *)name;
- (void)registerListOfBooleanVariable:(NSString *)name;
- (void)registerListOfDoubleVariable:(NSString *)name;
- (void)registerListOfStringVariable:(NSString *)name;
- (void)registerListOfIntegerVariable:(NSString *)name;

- (BOOL)getBooleanVariable:(NSString *)name defaultValue:(BOOL)defaultValue;
- (double)getDoubleVariable:(NSString *)name defaultValue:(double)defaultValue;
- (int)getIntegerVariable:(NSString *)name defaultValue:(int)defaultValue;
- (NSString *)getStringVariable:(NSString *)name defaultValue:(NSString *)defaultValue;

- (NSArray *)getListOfBooleanVariable:(NSString *)name defaultValue:(NSArray *)defaultValue;
- (NSArray *)getListOfDoubleVariable:(NSString *)name defaultValue:(NSArray *)defaultValue;
- (NSArray *)getListOfIntegerVariable:(NSString *)name defaultValue:(NSArray *)defaultValue;
- (NSArray *)getListOfStringVariable:(NSString *)name defaultValue:(NSArray *)defaultValue;

- (NSDictionary *)getMapOfBooleanVariable:(NSString *)name defaultValue:(NSDictionary *)defaultValue;
- (NSDictionary *)getMapOfDoubleVariable:(NSString *)name defaultValue:(NSDictionary *)defaultValue;
- (NSDictionary *)getMapOfIntegerVariable:(NSString *)name defaultValue:(NSDictionary *)defaultValue;
- (NSDictionary *)getMapOfStringVariable:(NSString *)name defaultValue:(NSDictionary *)defaultValue;

@end
