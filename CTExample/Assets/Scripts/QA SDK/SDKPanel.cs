using System.Collections.Generic;
using CleverTapSDK;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public class SDKPanel : MonoBehaviour
    {
        public GameObject optionsPanel;

        public GameObject messagesPanel;
        public GameObject pushPanel;

        public Button messagesButton;
        public Button pushButton;

        public Button consoleButton;
        public GameObject console;

        public Button pauseSDKButton;
        public Button resumeSDKButton;
        public Button testGameEventsButton;
        public Button testSubscriptionEventsButton;
        public Button testSetLocaleButton;

        void Start()
        {
#if !(UNITY_IOS || UNITY_ANDROID) || UNITY_EDITOR
            messagesButton.interactable = false;
#endif
#if !(UNITY_IOS || UNITY_ANDROID) || UNITY_EDITOR
            pushButton.interactable = false;
#endif
            messagesButton.onClick.AddListener(DidTapMessages);
            pushButton.onClick.AddListener(DidTapPush);
            consoleButton.onClick.AddListener(DidTapConsole);
            pauseSDKButton.onClick.AddListener(DidTapPauseSDK);
            resumeSDKButton.onClick.AddListener(DidTapResumeSDK);
            testGameEventsButton.onClick.AddListener(DidTapTestGameEvents);
            testSubscriptionEventsButton.onClick.AddListener(CallSubscriptionEvents);
            testSetLocaleButton.onClick.AddListener(CallSetLocale);

            Restore();
            RefreshContentHelper.RefreshContentFitters((RectTransform)transform);
        }

        public void Restore()
        {
            optionsPanel.SetActive(true);
            messagesPanel.SetActive(false);
            pushPanel.SetActive(false);
        }

        public void DidTapMessages()
        {
            optionsPanel.SetActive(false);
            messagesPanel.SetActive(true);
        }

        public void DidTapPush()
        {
            optionsPanel.SetActive(false);
            pushPanel.SetActive(true);
        }

        public void DidTapConsole()
        {
            console.SetActive(!console.activeInHierarchy);
            var text = consoleButton.GetComponentInChildren<Text>();
            text.text = console.activeInHierarchy ? "Hide Console" : "Show Console";
        }

        public void DidTapPauseSDK()
        {
            CleverTap.PauseSDK();
            Logger.Log("SDK Paused");
            Toast.Show("SDK Paused");
        }

        public void DidTapResumeSDK()
        {
            CleverTap.ResumeSDK();
            Logger.Log("SDK Resumed");
            Toast.Show("SDK Resumed");
        }

        public void DidTapTestGameEvents()
        {
            CleverTapGameEvents.RecordResourceEvent(
                CleverTapGameEvents.ResourceFlow.Earned,
                "Gems", 100f, "DailyReward", "LoginBonus");

            CleverTapGameEvents.RecordLevelEvent(
                CleverTapGameEvents.LevelStatus.Completed,
                "World1", "Level3", score: 4200);

            CleverTapGameEvents.RecordGameEvent("Chest:Opened:Rare", 1f);

            CleverTapGameEvents.RecordAdEvent(
                CleverTapGameEvents.AdEvent.RewardGranted,
                CleverTapGameEvents.AdFormat.RewardedVideo,
                "PostLevel");

            CleverTapGameEvents.RecordErrorEvent(
                CleverTapGameEvents.ErrorSeverity.Low,
                "Inventory failed to load");
        }

        public void CallSubscriptionEvents()
        {
            Logger.Log("[GameEvents] Firing 8 subscription lifecycle events...");

            const string product = "com.clevertap.premium_monthly";
            var monthly = CleverTapGameEvents.SubscriptionPeriod.Monthly;

            CleverTapGameEvents.RecordSubscriptionEvent(
                CleverTapGameEvents.SubscriptionStatus.TrialStarted, product, period: monthly);
            Logger.Log("[GameEvents] Subscription fired: status=TrialStarted period=Monthly");

            CleverTapGameEvents.RecordSubscriptionEvent(
                CleverTapGameEvents.SubscriptionStatus.PaidConversion, product, 4.99f, "USD", monthly);
            Logger.Log("[GameEvents] Subscription fired: status=PaidConversion price=4.99 currency=USD");

            CleverTapGameEvents.RecordSubscriptionEvent(
                CleverTapGameEvents.SubscriptionStatus.Renewed, product, 4.99f, "USD", monthly);
            Logger.Log("[GameEvents] Subscription fired: status=Renewed price=4.99 currency=USD");

            CleverTapGameEvents.RecordSubscriptionEvent(
                CleverTapGameEvents.SubscriptionStatus.BillingFailed, product);
            Logger.Log("[GameEvents] Subscription fired: status=BillingFailed");

            CleverTapGameEvents.RecordSubscriptionEvent(
                CleverTapGameEvents.SubscriptionStatus.Cancelled, product);
            Logger.Log("[GameEvents] Subscription fired: status=Cancelled");

            CleverTapGameEvents.RecordSubscriptionEvent(
                CleverTapGameEvents.SubscriptionStatus.Expired, product);
            Logger.Log("[GameEvents] Subscription fired: status=Expired");

            CleverTapGameEvents.RecordSubscriptionEvent(
                CleverTapGameEvents.SubscriptionStatus.Restored, product, period: monthly);
            Logger.Log("[GameEvents] Subscription fired: status=Restored period=Monthly");

            CleverTapGameEvents.RecordSubscriptionEvent(
                CleverTapGameEvents.SubscriptionStatus.Refunded, product, 4.99f, "USD");
            Logger.Log("[GameEvents] Subscription fired: status=Refunded price=4.99 currency=USD");

            Logger.Log("[GameEvents] All 8 subscription events sent.");
            Toast.Show("Subscription events sent");
        }

        private bool _useFrenchLocale = true;
        
        public void CallSetLocale()
        {
            string locale = _useFrenchLocale ? "fr_FR" : "en_US";
            _useFrenchLocale = !_useFrenchLocale;

            CleverTap.SetLocale(locale);
            CleverTap.RecordEvent("Locale Test", new Dictionary<string, object> { { "locale_set", locale } });

            Logger.Log($"[Locale] SetLocale({locale}) called, 'Locale Test' event recorded");
            Toast.Show($"Locale set to {locale}");
        }
    }
}