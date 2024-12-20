﻿using UnityEngine;

[System.Serializable]
public class CleverTapSettings : ScriptableObject
{
    public string CleverTapAccountId;
    public string CleverTapAccountToken;
    public string CleverTapAccountRegion;
    public bool CleverTapEnablePersonalization { get; set; } = true;
    public bool CleverTapDisableIDFV;
    public string CleverTapProxyDomain;
    public string CleverTapSpikyProxyDomain;

    public override string ToString()
    {
        return $"CleverTapSettings:\n" +
               $"CleverTapAccountId: {CleverTapAccountId}\n" +
               $"CleverTapAccountToken: {CleverTapAccountToken}\n" +
               $"CleverTapAccountRegion: {CleverTapAccountRegion}\n" +
               $"CleverTapProxyDomain: {CleverTapProxyDomain}\n" +
               $"CleverTapSpikyProxyDomain: {CleverTapSpikyProxyDomain}\n" +
               $"CleverTapEnablePersonalization: {CleverTapEnablePersonalization}\n" +
               $"CleverTapDisableIDFV: {CleverTapDisableIDFV}";
    }

    internal static readonly string settingsPath = "Assets/CleverTapSettings.asset";
}