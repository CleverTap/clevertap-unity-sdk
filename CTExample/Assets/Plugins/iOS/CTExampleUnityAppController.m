#import "CTExampleUnityAppController.h"
#import <UserNotifications/UserNotifications.h>
#import <CleverTapGeofence/CleverTapGeofence-Swift.h>
#import <CoreLocation/CLLocationManager.h>

@interface CTExampleUnityAppController ()
@property (nonatomic, strong) CLLocationManager *locationManager;
@end

@implementation CTExampleUnityAppController

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary<UIApplicationLaunchOptionsKey,id> *)launchOptions {
    // Register category with actions
    [self setCTNotificationCategory];
    
    // Call super to init CleverTap
    BOOL result = [super application:application didFinishLaunchingWithOptions:launchOptions];
    
    // Request location permission
    self.locationManager = [[CLLocationManager alloc] init];
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
    if (!self.locationManager) {
        NSLog(@"LocationManager is not initialized");
        return;
    }
    
    CLAuthorizationStatus status;
    if (@available(iOS 14.0, *)) {
        status = self.locationManager.authorizationStatus;
    } else {
        status = [CLLocationManager authorizationStatus];
    }
    
    if (status == kCLAuthorizationStatusNotDetermined) {
        if ([self.locationManager respondsToSelector:@selector(requestWhenInUseAuthorization)]) {
            [self.locationManager requestWhenInUseAuthorization];
        }
    } else if (status == kCLAuthorizationStatusAuthorizedWhenInUse) {
        if ([self.locationManager respondsToSelector:@selector(requestAlwaysAuthorization)]) {
            [self.locationManager requestAlwaysAuthorization];
        }
    }
}

@end
