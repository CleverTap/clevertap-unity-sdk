#import "CleverTapMessageBuffer.h"

@implementation CleverTapMessageBuffer

- (instancetype)initWithEnabled:(BOOL)isEnabled {
    if (self = [super init]) {
        self.isEnabled = isEnabled;
        self.items = [NSMutableArray array];
    }
    return self;
}

- (void)addItem:(NSString *)item {
    if (item) {
        [self.items addObject:item];
    }
}

- (nullable NSString *)popItem {
    if (self.items.count > 0) {
        NSString *last = [self.items lastObject];
        [self.items removeLastObject];
        return last;
    }
    return nil;
}

- (NSUInteger)count {
    return self.items.count;
}

@end
