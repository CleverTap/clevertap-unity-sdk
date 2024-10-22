//
//  CleverTapTemplatePresenter.h
//
//  Created by Nikola Zagorchev on 22.10.24.
//

#import <Foundation/Foundation.h>
#import "CTTemplatePresenter.h"

NS_ASSUME_NONNULL_BEGIN

static NSString * kCleverTapCustomTemplatePresent = @"CleverTapCustomTemplatePresent";
static NSString * kCleverTapCustomTemplateClose = @"CleverTapCustomTemplateClose";

@interface CleverTapTemplatePresenter : NSObject <CTTemplatePresenter>

@end

NS_ASSUME_NONNULL_END
