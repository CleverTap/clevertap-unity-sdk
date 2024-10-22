//
//  CleverTapAppFunctionPresenter.m
//
//  Created by Nikola Zagorchev on 22.10.24.
//

#import "CleverTapAppFunctionPresenter.h"
#import "CleverTapUnityManager.h"

@implementation CleverTapAppFunctionPresenter

- (void)onPresent:(nonnull CTTemplateContext *)context {
    UnitySendMessage([kCleverTapGameObjectName UTF8String], [kCleverTapAppFunctionPresent UTF8String], [context.templateName UTF8String]);
}

- (void)onCloseClicked:(nonnull CTTemplateContext *)context {
    // NOOP - App Functions cannot have Action arguments.
}

@end
