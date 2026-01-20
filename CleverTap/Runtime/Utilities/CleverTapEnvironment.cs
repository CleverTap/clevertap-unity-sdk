using System;

namespace CleverTapSDK.Utilities
{
    public enum CleverTapEnvironmentKey
    {
        TEST = 0,
        DEV = 1,
        STAGE = 2,
        PROD = 3
    }

    [Serializable]
    public struct CleverTapEnvironmentCredential
    {
        #region Project Settings
        /// <summary>
        /// CleverTap project Account Id (Project ID).
        /// </summary>
        public string CleverTapAccountId;

        /// <summary>
        /// CleverTap project Account token (Project Token).
        /// </summary>
        public string CleverTapAccountToken;

        /// <summary>
        /// CleverTap Region Code.
        /// </summary>
        public string CleverTapAccountRegion;

        /// <summary>
        /// Custom Proxy Domain.
        /// </summary>
        public string CleverTapProxyDomain;

        /// <summary>
        /// Spiky Proxy Domain.
        /// </summary>
        public string CleverTapSpikyProxyDomain;
        #endregion
    }
}