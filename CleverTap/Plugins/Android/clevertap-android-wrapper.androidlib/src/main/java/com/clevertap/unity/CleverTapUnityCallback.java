package com.clevertap.unity;

public enum CleverTapUnityCallback {
    CLEVERTAP_PROFILE_INITIALIZED_CALLBACK("CleverTapProfileInitializedCallback"),
    CLEVERTAP_PROFILE_UPDATES_CALLBACK("CleverTapProfileUpdatesCallback"),
    CLEVERTAP_DEEP_LINK_CALLBACK("CleverTapDeepLinkCallback"),
    CLEVERTAP_PUSH_OPENED_CALLBACK("CleverTapPushOpenedCallback"),
    CLEVERTAP_INAPP_NOTIFICATION_DISMISSED_CALLBACK("CleverTapInAppNotificationDismissedCallback"),
    CLEVERTAP_INAPP_NOTIFICATION_SHOW_CALLBACK("CleverTapInAppNotificationShowCallback"),
    CLEVERTAP_ON_PUSH_PERMISSION_RESPONSE_CALLBACK("CleverTapOnPushPermissionResponseCallback"),
    CLEVERTAP_INBOX_DID_INITIALIZE("CleverTapInboxDidInitializeCallback"),
    CLEVERTAP_INBOX_MESSAGES_DID_UPDATE("CleverTapInboxMessagesDidUpdateCallback"),
    CLEVERTAP_ON_INBOX_BUTTON_CLICKED("CleverTapInboxCustomExtrasButtonSelect"),
    CLEVERTAP_ON_INBOX_ITEM_CLICKED("CleverTapInboxItemSelect"),
    CLEVERTAP_ON_INAPP_BUTTON_CLICKED("CleverTapInAppNotificationButtonTapped"),
    CLEVERTAP_DISPLAY_UNITS_UPDATED("CleverTapNativeDisplayUnitsUpdated"),
    CLEVERTAP_FEATURE_FLAG_UPDATED("CleverTapFeatureFlagsUpdated"),
    CLEVERTAP_PRODUCT_CONFIG_INITIALIZED("CleverTapProductConfigInitialized"),
    CLEVERTAP_PRODUCT_CONFIG_FETCHED("CleverTapProductConfigFetched"),
    CLEVERTAP_PRODUCT_CONFIG_ACTIVATED("CleverTapProductConfigActivated"),
    CLEVERTAP_INIT_CLEVERTAP_ID_CALLBACK("CleverTapInitCleverTapIdCallback"),
    CLEVERTAP_VARIABLES_CHANGED("CleverTapVariablesChanged"),
    CLEVERTAP_VARIABLE_VALUE_CHANGED("CleverTapVariableValueChanged"),
    CLEVERTAP_VARIABLES_FETCHED("CleverTapVariablesFetched"),
    CLEVERTAP_INAPPS_FETCHED("CleverTapInAppsFetched"),
    CLEVERTAP_VARIABLES_CHANGED_AND_NO_DOWNLOADS_PENDING("CleverTapVariablesChangedAndNoDownloadsPending"),
    CLEVERTAP_FILE_VARIABLE_READY("CleverTapVariableFileIsReady");

    public final String callbackName;

    private CleverTapUnityCallback(String callbackName) {
        this.callbackName = callbackName;
    }
}
