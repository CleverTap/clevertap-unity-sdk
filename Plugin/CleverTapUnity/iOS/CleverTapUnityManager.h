#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

#import <CleverTapSDK/CleverTap.h>
#import <CleverTapSDK/CleverTapEventDetail.h>
#import <CleverTapSDK/CleverTapUTMDetail.h>

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

- (void)setOffline:(BOOL)enabled;
- (void)setOptOut:(BOOL)enabled;
- (void)enableDeviceNetworkInfoReporting:(BOOL)enabled;
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
- (NSString*)profileGetCleverTapID;
- (NSString*)profileGetCleverTapAttributionIdentifier;

- (void)recordScreenView:(NSString *)screenName;
- (void)recordEvent:(NSString *)event;
- (void)recordEvent:(NSString *)event withProps:(NSDictionary *)properties;
- (void)recordChargedEventWithDetails:(NSDictionary *)chargeDetails andItems:(NSArray *)items;

- (NSTimeInterval)eventGetFirstTime:(NSString *)event;
- (NSTimeInterval)eventGetLastTime:(NSString *)event;
- (int)eventGetOccurrences:(NSString *)event;
- (NSDictionary *)userGetEventHistory;
- (CleverTapEventDetail *)eventGetDetail:(NSString *)event;

- (NSTimeInterval)sessionGetTimeElapsed;
- (CleverTapUTMDetail *)sessionGetUTMDetails;
- (int)userGetTotalVisits;
- (int)userGetScreenCount;
- (NSTimeInterval)userGetPreviousVisitTime;

- (void)setPushToken:(NSData *)pushToken;
- (void)setPushTokenAsString:(NSString *)pushTokenString;
- (void)registerApplication:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)notification;

- (void)handleOpenURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication;
- (void)pushInstallReferrerSource:(NSString *)source
                           medium:(NSString *)medium
                         campaign:(NSString *)campaign;

- (void)initializeInbox;
- (void)showAppInbox:(NSDictionary *)styleConfig;
- (int)getInboxMessageUnreadCount;
- (int)getInboxMessageCount;

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

- (NSString *)get


@end
