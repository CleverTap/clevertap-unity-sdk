//
//  CleverTapMessageSender.h
//
//  Created by Nikola Zagorchev on 12.11.24.
//

#import <Foundation/Foundation.h>
#import "CleverTapUnityCallbackInfo.h"

NS_ASSUME_NONNULL_BEGIN

@interface CleverTapMessageSender : NSObject

+ (instancetype)sharedInstance;

- (void)send:(CleverTapUnityCallback)callback withMessage:(NSString *)message;
- (void)flushBuffer:(CleverTapUnityCallbackInfo *)callbackInfo;
- (void)disableBuffer:(CleverTapUnityCallbackInfo *)callbackInfo;
- (void)resetAllBuffers;

@end

NS_ASSUME_NONNULL_END
