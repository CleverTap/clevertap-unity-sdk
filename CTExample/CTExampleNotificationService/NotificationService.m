#import "NotificationService.h"
#if RECORD_PUSH_IMPRESSIONS
#import <CleverTapSDK/CleverTap.h>
#endif

@implementation NotificationService

- (void)didReceiveNotificationRequest:(UNNotificationRequest *)request withContentHandler:(void (^)(UNNotificationContent * _Nonnull))contentHandler {
#if RECORD_PUSH_IMPRESSIONS
    /// Dummy Profile Creation
    ///
    /// If the Notification viewed method is incorrectly called, it would create a new profile in CleverTap with only this event.
    /// This will create a dummy profile with no other event and will not be mapped to the profile from which the Push Impressions event was raised.
    /// To avoid the creation of a new user profile while calling the Notification Viewed method from your Application,
    /// you must pass the Identity/Email to CleverTap in the Notification Service Class,
    /// or you can call recordNotificationViewedEvent API from your Main Application.
    
    // Record the Notification viewed
    [[CleverTap sharedInstance] recordNotificationViewedEventWithData:request.content.userInfo];
#endif
    
    [super didReceiveNotificationRequest:request withContentHandler:contentHandler];
}

@end
