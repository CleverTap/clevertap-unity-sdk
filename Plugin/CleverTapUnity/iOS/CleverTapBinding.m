#import "CleverTapUnityManager.h"

NSString* clevertap_stringToNSString(const char* str) {
    return str != NULL ? [NSString stringWithUTF8String:str] : [NSString stringWithUTF8String:""];
}

NSString* clevertap_toJsonString(id val) {
    NSString *jsonString;
    
    if (val == nil) {
        return nil;
    }
    
    if ([val isKindOfClass:[NSArray class]] || [val isKindOfClass:[NSDictionary class]]) {
        NSError *error;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:val options:NSJSONWritingPrettyPrinted error:&error];
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        
        if (error != nil) {
            jsonString = nil;
        }
    } else {
        jsonString = [NSString stringWithFormat:@"%@", val];
    }
    
    return jsonString;
}

NSMutableArray* clevertap_NSArrayFromArray(const char* array[], int size) {
    
    NSMutableArray *values = [NSMutableArray arrayWithCapacity:size];
    for (int i = 0; i < size; i ++) {
        NSString *value = clevertap_stringToNSString(array[i]);
        [values addObject:value];
    }
    
    return values;
}

NSMutableDictionary* clevertap_dictFromJsonString(const char* jsonString) {
    
    NSMutableDictionary *dict = [NSMutableDictionary dictionaryWithCapacity:1];
    
    if (jsonString != NULL && jsonString != nil) {
        NSError *jsonError;
        NSData *objectData = [clevertap_stringToNSString(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        dict = [NSJSONSerialization JSONObjectWithData:objectData
                                               options:NSJSONReadingMutableContainers
                                                 error:&jsonError];
    }
    
    return dict;
}

NSMutableArray* clevertap_NSArrayFromJsonString(const char* jsonString) {
    NSMutableArray *arr = [NSMutableArray arrayWithCapacity:1];
    
    if (jsonString != NULL && jsonString != nil) {
        NSError *jsonError;
        NSData *objectData = [clevertap_stringToNSString(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        arr = [NSJSONSerialization JSONObjectWithData:objectData
                                              options:NSJSONReadingMutableContainers
                                                error:&jsonError];
    }
    
    return arr;
}

NSMutableDictionary* clevertap_eventDetailToDict(CleverTapEventDetail* detail) {
    
    NSMutableDictionary *_dict = [NSMutableDictionary new];
    
    if(detail) {
        if(detail.eventName) {
            [_dict setObject:detail.eventName forKey:@"eventName"];
        }
        
        if(detail.firstTime){
            [_dict setObject:@(detail.firstTime) forKey:@"firstTime"];
        }
        
        if(detail.lastTime){
            [_dict setObject:@(detail.lastTime) forKey:@"lastTime"];
        }
        
        if(detail.count){
            [_dict setObject:@(detail.count) forKey:@"count"];
        }
    }
    
    return _dict;
}

NSMutableDictionary* clevertap_utmDetailToDict(CleverTapUTMDetail* detail) {
    
    NSMutableDictionary *_dict = [NSMutableDictionary new];
    
    if(detail) {
        if(detail.source) {
            [_dict setObject:detail.source forKey:@"source"];
        }
        
        if(detail.medium) {
            [_dict setObject:detail.medium forKey:@"medium"];
        }
        
        if(detail.campaign) {
            [_dict setObject:detail.campaign forKey:@"campaign"];
        }
    }
    
    return _dict;
}

char* clevertap_cStringCopy(const char* string) {
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}

void CleverTap_launchWithCredentials(const char* accountID, const char* token) {
    [CleverTapUnityManager launchWithAccountID:clevertap_stringToNSString(accountID) andToken:clevertap_stringToNSString(token)];
}

void CleverTap_setApplicationIconBadgeNumber(int num) {
    [CleverTapUnityManager setApplicationIconBadgeNumber:num];
}

void CleverTap_registerPush() {
    [CleverTapUnityManager registerPush];
}

void CleverTap_setDebugLevel(int level) {
    [CleverTapUnityManager setDebugLevel:level];
}

void CleverTap_enablePersonalization() {
    [CleverTapUnityManager enablePersonalization];
}

void CleverTap_disablePersonalization() {
    [CleverTapUnityManager disablePersonalization];
}

void CleverTap_setLocation(double lat, double lon) {
    CLLocationCoordinate2D coord = CLLocationCoordinate2DMake(lat, lon);
    [CleverTapUnityManager setLocation:coord];
}

void CleverTap_recordEvent(const char* eventName, const char* properties) {
    NSMutableDictionary *eventProperties = clevertap_dictFromJsonString(properties);
    [[CleverTapUnityManager sharedInstance] recordEvent:clevertap_stringToNSString(eventName) withProps:eventProperties];
}

void CleverTap_recordChargedEventWithDetailsAndItems(const char* chargeDetails, const char* items) {
    NSDictionary *details = clevertap_dictFromJsonString(chargeDetails);
    NSArray *_items = clevertap_NSArrayFromJsonString(items);
    [[CleverTapUnityManager sharedInstance] recordChargedEventWithDetails:details andItems:_items];
}

void CleverTap_onUserLogin(const char* properties) {
    NSMutableDictionary *profileProperties = clevertap_dictFromJsonString(properties);
    [[CleverTapUnityManager sharedInstance] onUserLogin:profileProperties];
}

void CleverTap_profilePush(const char* properties) {
    NSMutableDictionary *profileProperties = clevertap_dictFromJsonString(properties);
    [[CleverTapUnityManager sharedInstance] profilePush:profileProperties];
}

void CleverTap_profilePushGraphUser(const char* fbGraphUser) {
    NSMutableDictionary *user = clevertap_dictFromJsonString(fbGraphUser);
    [[CleverTapUnityManager sharedInstance] profilePush:user];
}

void CleverTap_profilePushGooglePlusUser(const char* googleUser) {
    NSMutableDictionary *user = clevertap_dictFromJsonString(googleUser);
    [[CleverTapUnityManager sharedInstance] profilePush:user];
}

char* CleverTap_profileGet(const char* key) {
    id ret = [[CleverTapUnityManager sharedInstance] profileGet:clevertap_stringToNSString(key)];
    
    NSString *jsonString = clevertap_toJsonString(ret);
    
    if (jsonString == nil) {
        return NULL;
    }
    
    return clevertap_cStringCopy([jsonString UTF8String]);
}

char* CleverTap_profileGetCleverTapID() {
    NSString *ret = [[CleverTapUnityManager sharedInstance] profileGetCleverTapID];
    
    if (ret == nil) {
        return NULL;
    }
    
    return clevertap_cStringCopy([ret UTF8String]);
}

char* CleverTap_profileGetCleverTapAttributionIdentifier() {
    NSString *ret = [[CleverTapUnityManager sharedInstance] profileGetCleverTapAttributionIdentifier];
    
    if (ret == nil) {
        return NULL;
    }
    
    return clevertap_cStringCopy([ret UTF8String]);
}

void CleverTap_profileRemoveValueForKey(const char* key) {
    [[CleverTapUnityManager sharedInstance] profileRemoveValueForKey:clevertap_stringToNSString(key)];
}

void CleverTap_profileSetMultiValuesForKey(const char* key, const char* array[], int size) {
    
    if (array == NULL || array == nil || size == 0) {
        return;
    }
    
    NSArray *values = clevertap_NSArrayFromArray(array, size);
    
    [[CleverTapUnityManager sharedInstance] profileSetMultiValues:values forKey:clevertap_stringToNSString(key)];
}

void CleverTap_profileAddMultiValuesForKey(const char* key, const char* array[], int size) {
    
    if (array == NULL || array == nil || size == 0) {
        return;
    }
    
    NSArray *values = clevertap_NSArrayFromArray(array, size);
    
    [[CleverTapUnityManager sharedInstance] profileAddMultiValues:values forKey:clevertap_stringToNSString(key)];
    
}

void CleverTap_profileRemoveMultiValuesForKey(const char* key, const char* array[], int size) {
    
    if (array == NULL || array == nil || size == 0) {
        return;
    }
    
    NSArray *values = clevertap_NSArrayFromArray(array, size);
    
    [[CleverTapUnityManager sharedInstance] profileRemoveMultiValues:values forKey:clevertap_stringToNSString(key)];
}

void CleverTap_profileAddMultiValueForKey(const char* key, const char* value) {
    [[CleverTapUnityManager sharedInstance] profileAddMultiValue:clevertap_stringToNSString(value) forKey:clevertap_stringToNSString(key)];
}

void CleverTap_profileRemoveMultiValueForKey(const char* key, const char* value) {
    [[CleverTapUnityManager sharedInstance] profileRemoveMultiValue:clevertap_stringToNSString(value) forKey:clevertap_stringToNSString(key)];
}

int CleverTap_eventGetFirstTime(const char* eventName) {
    return [[CleverTapUnityManager sharedInstance] eventGetFirstTime:clevertap_stringToNSString(eventName)];
}

int CleverTap_eventGetLastTime(const char* eventName) {
    return [[CleverTapUnityManager sharedInstance] eventGetLastTime:clevertap_stringToNSString(eventName)];
}

int CleverTap_eventGetOccurrences(const char* eventName) {
    return [[CleverTapUnityManager sharedInstance] eventGetOccurrences:clevertap_stringToNSString(eventName)];
}

char* CleverTap_userGetEventHistory() {
    NSDictionary *history = [[CleverTapUnityManager sharedInstance] userGetEventHistory];
    
    NSMutableDictionary *_history = [NSMutableDictionary new];
    
    for (NSString *key in history.allKeys) {
        _history[key] = clevertap_eventDetailToDict(history[key]);
    }
    
    NSString *jsonString = clevertap_toJsonString(_history);
    
    if (jsonString == nil) {
        return NULL;
    }
    
    return clevertap_cStringCopy([jsonString UTF8String]);
}

char* CleverTap_sessionGetUTMDetails() {
    CleverTapUTMDetail *detail = [[CleverTapUnityManager sharedInstance] sessionGetUTMDetails];
    
    NSMutableDictionary *_detail = clevertap_utmDetailToDict(detail);
    
    NSString *jsonString = clevertap_toJsonString(_detail);
    
    if (jsonString == nil) {
        return NULL;
    }
    
    return clevertap_cStringCopy([jsonString UTF8String]);
}

int CleverTap_sessionGetTimeElapsed() {
    return [[CleverTapUnityManager sharedInstance] sessionGetTimeElapsed];
}

char* CleverTap_eventGetDetail(const char* eventName) {
    CleverTapEventDetail *detail = [[CleverTapUnityManager sharedInstance] eventGetDetail:clevertap_stringToNSString(eventName)];
    
    NSMutableDictionary *_detail = clevertap_eventDetailToDict(detail);
    
    NSString *jsonString = clevertap_toJsonString(_detail);
    
    if (jsonString == nil) {
        return NULL;
    }
    
    return clevertap_cStringCopy([jsonString UTF8String]);
}

int CleverTap_userGetTotalVisits() {
    return [[CleverTapUnityManager sharedInstance] userGetTotalVisits];
}

int CleverTap_userGetScreenCount() {
    return [[CleverTapUnityManager sharedInstance] userGetScreenCount];
}

int CleverTap_userGetPreviousVisitTime() {
    return [[CleverTapUnityManager sharedInstance] userGetPreviousVisitTime];
}
