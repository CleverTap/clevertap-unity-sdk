package com.clevertap.unity;

import androidx.annotation.Nullable;

public enum CleverTapUnityCallback {
    CLEVERTAP_PROFILE_INITIALIZED_CALLBACK("CleverTapProfileInitializedCallback", true),
    CLEVERTAP_PROFILE_UPDATES_CALLBACK("CleverTapProfileUpdatesCallback"),
    CLEVERTAP_DEEP_LINK_CALLBACK("CleverTapDeepLinkCallback", true),
    CLEVERTAP_PUSH_OPENED_CALLBACK("CleverTapPushOpenedCallback", true),
    CLEVERTAP_INAPP_NOTIFICATION_DISMISSED_CALLBACK("CleverTapInAppNotificationDismissedCallback", true),
    CLEVERTAP_INAPP_NOTIFICATION_SHOW_CALLBACK("CleverTapInAppNotificationShowCallback", true),
    CLEVERTAP_ON_PUSH_PERMISSION_RESPONSE_CALLBACK("CleverTapOnPushPermissionResponseCallback"),
    CLEVERTAP_INBOX_DID_INITIALIZE("CleverTapInboxDidInitializeCallback", true),
    CLEVERTAP_INBOX_MESSAGES_DID_UPDATE("CleverTapInboxMessagesDidUpdateCallback"),
    CLEVERTAP_ON_INBOX_BUTTON_CLICKED("CleverTapInboxCustomExtrasButtonSelect"),
    CLEVERTAP_ON_INBOX_ITEM_CLICKED("CleverTapInboxItemClicked"),
    CLEVERTAP_ON_INAPP_BUTTON_CLICKED("CleverTapInAppNotificationButtonTapped", true),
    CLEVERTAP_DISPLAY_UNITS_UPDATED("CleverTapNativeDisplayUnitsUpdated", true),
    CLEVERTAP_FEATURE_FLAG_UPDATED("CleverTapFeatureFlagsUpdated", true),
    CLEVERTAP_PRODUCT_CONFIG_INITIALIZED("CleverTapProductConfigInitialized", true),
    CLEVERTAP_PRODUCT_CONFIG_FETCHED("CleverTapProductConfigFetched"),
    CLEVERTAP_PRODUCT_CONFIG_ACTIVATED("CleverTapProductConfigActivated"),
    CLEVERTAP_INIT_CLEVERTAP_ID_CALLBACK("CleverTapInitCleverTapIdCallback"),
    CLEVERTAP_VARIABLES_CHANGED("CleverTapVariablesChanged"),
    CLEVERTAP_VARIABLE_VALUE_CHANGED("CleverTapVariableValueChanged"),
    CLEVERTAP_VARIABLES_FETCHED("CleverTapVariablesFetched"),
    CLEVERTAP_INAPPS_FETCHED("CleverTapInAppsFetched"),
    CLEVERTAP_VARIABLES_CHANGED_AND_NO_DOWNLOADS_PENDING("CleverTapVariablesChangedAndNoDownloadsPending"),
    CLEVERTAP_FILE_VARIABLE_READY("CleverTapVariableFileIsReady");

    @Nullable
    public static CleverTapUnityCallback fromName(String callbackName) {
        for (CleverTapUnityCallback callback : values()) {
            if (callback.callbackName.equals(callbackName)) {
                return callback;
            }
        }
        return null;
    }

    public final String callbackName;
    public final boolean bufferable;

    CleverTapUnityCallback(String callbackName, boolean bufferable) {
        this.callbackName = callbackName;
        this.bufferable = bufferable;
    }

    CleverTapUnityCallback(String callbackName) {
        this(callbackName, false);
    }
}
