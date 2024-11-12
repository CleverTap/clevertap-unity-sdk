//
//  CleverTapMessageBuffer.h
//
//  Created by Nikola Zagorchev on 12.11.24.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface CleverTapMessageBuffer : NSObject

@property (nonatomic, assign) BOOL isEnabled;
@property (nonatomic, strong) NSMutableArray *items;

- (instancetype)initWithEnabled:(BOOL)isEnabled;

- (void)add:(NSString *)item;
- (nullable NSString *)remove;
- (NSUInteger)count;

@end

NS_ASSUME_NONNULL_END
