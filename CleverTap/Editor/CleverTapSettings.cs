using UnityEngine;

[System.Serializable]
public class CleverTapSettings : ScriptableObject
{
    public string CleverTapAccountId;
    public string CleverTapAccountToken;
    public string CleverTapAccountRegion;
    public string CleverTapProxyDomain;
    public string CleverTapSpikyProxyDomain;
    public bool CleverTapDisableIDFV;

    public bool CleverTapIOSUseAutoIntegrate { get; set; } = true;
    public bool CleverTapIOSUseUNUserNotificationCenter { get; set; } = true;
    public bool CleverTapIOSPresentNotificationOnForeground;

    public override string ToString()
    {
        return $"CleverTapSettings:\n" +
               $"CleverTapAccountId: {CleverTapAccountId}\n" +
               $"CleverTapAccountToken: {CleverTapAccountToken}\n" +
               $"CleverTapAccountRegion: {CleverTapAccountRegion}\n" +
               $"CleverTapProxyDomain: {CleverTapProxyDomain}\n" +
               $"CleverTapSpikyProxyDomain: {CleverTapSpikyProxyDomain}\n" +
               $"CleverTapDisableIDFV: {CleverTapDisableIDFV}\n" +
               $"CleverTapIOSUseAutoIntegrate: {CleverTapIOSUseAutoIntegrate}\n" +
               $"CleverTapIOSUseUNUserNotificationCenter: {CleverTapIOSUseUNUserNotificationCenter}\n" +
               $"CleverTapIOSPresentNotificationOnForeground: {CleverTapIOSPresentNotificationOnForeground}";
    }

    internal static readonly string settingsPath = "Assets/CleverTapSettings.asset";
}