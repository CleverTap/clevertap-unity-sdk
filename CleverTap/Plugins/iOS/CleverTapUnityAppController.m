#import "CleverTapUnityAppController.h"
#import "CleverTapUnityManager.h"
#import "CleverTapCustomTemplates.h"
#import <CleverTapSDK/CleverTap.h>

@implementation CleverTapUnityAppController

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    [CleverTapCustomTemplates registerCustomTemplates];
    [CleverTap autoIntegrate];
    [CleverTapUnityManager sharedInstance];
    return [super application:application didFinishLaunchingWithOptions:launchOptions];
}

@end
