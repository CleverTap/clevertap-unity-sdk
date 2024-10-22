//
//  CleverTapTemplatePresenter.m
//
//  Created by Nikola Zagorchev on 22.10.24.
//

#import "CleverTapTemplatePresenter.h"
#import "CleverTapUnityManager.h"

@implementation CleverTapTemplatePresenter

- (void)onPresent:(nonnull CTTemplateContext *)context { 
    UnitySendMessage([kCleverTapGameObjectName UTF8String], [kCleverTapCustomTemplatePresent UTF8String], [context.templateName UTF8String]);
}

- (void)onCloseClicked:(nonnull CTTemplateContext *)context {
    UnitySendMessage([kCleverTapGameObjectName UTF8String], [kCleverTapCustomTemplateClose UTF8String], [context.templateName UTF8String]);
}

@end
