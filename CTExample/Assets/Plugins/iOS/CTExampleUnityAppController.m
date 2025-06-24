#import "CTExampleUnityAppController.h"
#import <UserNotifications/UserNotifications.h>
#import <CleverTapGeofence/CleverTapGeofence-Swift.h>
#import <CoreLocation/CLLocationManager.h>

@implementation CTExampleUnityAppController

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary<UIApplicationLaunchOptionsKey,id> *)launchOptions {
    // Register category with actions
    [self setCTNotificationCategory];
    
    // Call super to init CleverTap
    BOOL result = [super application:application didFinishLaunchingWithOptions:launchOptions];
    
    // Request location permission
    [self requestLocationAlwaysPermission];
    
    // Ensure that Geofence SDK init is done after CleverTap SDK init
    [[CleverTapGeofence monitor] startWithDidFinishLaunchingWithOptions:launchOptions];

    return result;
}

- (void)setCTNotificationCategory {
    UNUserNotificationCenter *center = [UNUserNotificationCenter currentNotificationCenter];
    UNNotificationAction *action1 = [UNNotificationAction actionWithIdentifier:@"action_1" title:@"Back" options:UNNotificationActionOptionNone];
    UNNotificationAction *action2 = [UNNotificationAction actionWithIdentifier:@"action_2" title:@"Next" options:UNNotificationActionOptionNone];
    UNNotificationAction *action3 = [UNNotificationAction actionWithIdentifier:@"action_3" title:@"View In App" options:UNNotificationActionOptionNone];
    
    UNNotificationCategory *category = [UNNotificationCategory categoryWithIdentifier:@"CTNotification" actions:@[action1, action2, action3] intentIdentifiers:@[] options:UNNotificationCategoryOptionNone];
    
    [center setNotificationCategories:[NSSet setWithObjects:category, nil]];
}

- (void)requestLocationAlwaysPermission {
    CLLocationManager *locationManager = [[CLLocationManager alloc] init];
    CLAuthorizationStatus status;
    if (@available(iOS 14.0, *)) {
        status = locationManager.authorizationStatus;
    } else {
        status = [CLLocationManager authorizationStatus];
    }
    
    if (status == kCLAuthorizationStatusNotDetermined) {
        if ([locationManager respondsToSelector:@selector(requestWhenInUseAuthorization)]) {
            [locationManager requestWhenInUseAuthorization];
        }
    } else if (status == kCLAuthorizationStatusAuthorizedWhenInUse) {
        if ([locationManager respondsToSelector:@selector(requestAlwaysAuthorization)]) {
            [locationManager requestAlwaysAuthorization];
        }
    }
}

@end
