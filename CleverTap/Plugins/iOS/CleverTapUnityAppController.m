#import "CleverTapUnityAppController.h"
#import "CleverTapUnityManager.h"
#import <CleverTapSDK/CleverTap.h>

@implementation CleverTapUnityAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions {
    
    [CleverTap autoIntegrate];
    [CleverTapUnityManager sharedInstance];
    return [super application:application didFinishLaunchingWithOptions:launchOptions];
}

@end
