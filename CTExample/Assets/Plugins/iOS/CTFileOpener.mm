#import <UIKit/UIKit.h>
#import "UnityInterface.h"

@interface CTFileOpener : NSObject <UIDocumentInteractionControllerDelegate>
@property (nonatomic, strong) UIDocumentInteractionController *controller;
+ (instancetype)shared;
@end

@implementation CTFileOpener

+ (instancetype)shared {
    static CTFileOpener *instance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{ instance = [[CTFileOpener alloc] init]; });
    return instance;
}

- (void)openFile:(NSString *)path {
    NSURL *url = [NSURL fileURLWithPath:path];
    self.controller = [UIDocumentInteractionController interactionControllerWithURL:url];
    self.controller.delegate = self;
    BOOL previewed = [self.controller presentPreviewAnimated:YES];
    if (!previewed) {
        BOOL opened = [self.controller presentOpenInMenuFromRect:CGRectZero
                                                         inView:UnityGetGLViewController().view
                                                       animated:YES];
        if (!opened) {
            [UIPasteboard generalPasteboard].string = path;
        }
    }
}

- (UIViewController *)documentInteractionControllerViewControllerForPreview:(UIDocumentInteractionController *)controller {
    return UnityGetGLViewController();
}

@end

extern "C" {
    void _CTOpenFile(const char *filePath) {
        NSString *path = [NSString stringWithUTF8String:filePath];
        dispatch_async(dispatch_get_main_queue(), ^{
            [[CTFileOpener shared] openFile:path];
        });
    }
}
