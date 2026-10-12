using System.Collections.Generic;

namespace CleverTapSDK
{
    public static class CleverTapGameEvents
    {
        public enum ResourceFlow  { Earned, Spent }
        public enum LevelStatus   { Started, Completed, Failed }
        public enum AdEvent       { Shown, Clicked, Skipped, RewardGranted }
        public enum AdFormat      { Banner, Interstitial, RewardedVideo, OfferWall, Playable }
        public enum ErrorSeverity { Low, Medium, High, Critical }
        public enum SubscriptionStatus
        {
            TrialStarted, PaidConversion, Renewed, Cancelled,
            BillingFailed, Expired, Restored, Refunded
        }
        public enum SubscriptionPeriod { Weekly, Monthly, Yearly }

        public static void RecordResourceEvent(
        ResourceFlow flow,
        string currency,
        float amount,
        string category,
        string itemName)
        {
            var props = new Dictionary<string, object>
            {
                { "flow",     flow.ToString() },
                { "currency", currency },
                { "amount",   amount },
                { "category", category },
                { "item",     itemName }
            };
            CleverTap.RecordEvent("GameEvent_Resource", props);
        }

        public static void RecordLevelEvent(
        LevelStatus status,
        string world,
        string level = null,
        string stage = null,
        int? score = null)
        {
            var props = new Dictionary<string, object>
            {
                { "status", status.ToString() },
                { "world",  world }
            };
            if (level != null) props["level"] = level;
            if (stage != null) props["stage"] = stage;
            if (score.HasValue) props["score"] = score.Value;
            CleverTap.RecordEvent("GameEvent_Level", props);
        }

        public static void RecordGameEvent(
        string eventName,
        float? value = null)
        {
            var props = new Dictionary<string, object>
            {
                { "event", eventName }
            };
            if (value.HasValue) props["value"] = value.Value;
            CleverTap.RecordEvent("GameEvent_Design", props);
        }

        public static void RecordAdEvent(
        AdEvent action,
        AdFormat? format = null,
        string placement = null)
        {
            var props = new Dictionary<string, object>
            {
                { "action", action.ToString() }
            };
            if (format.HasValue)   props["format"]    = format.Value.ToString();
            if (placement != null) props["placement"] = placement;
            CleverTap.RecordEvent("GameEvent_Ad", props);
        }

        public static void RecordErrorEvent(
        ErrorSeverity severity,
        string description)
        {
            var props = new Dictionary<string, object>
            {
                { "severity",    severity.ToString() },
                { "description", description }
            };
            CleverTap.RecordEvent("GameEvent_Error", props);
        }

        public static void RecordSubscriptionEvent(
        SubscriptionStatus status,
        string productId,
        float? price = null,
        string currency = null,
        SubscriptionPeriod? period = null)
        {
            var props = new Dictionary<string, object>
            {
                { "status",     status.ToString() },
                { "product_id", productId }
            };
            if (price.HasValue)                 props["price"]    = price.Value;
            if (!string.IsNullOrEmpty(currency)) props["currency"] = currency;
            if (period.HasValue)                props["period"]   = period.Value.ToString();
            CleverTap.RecordEvent("GameEvent_Subscription", props);
        }
    }
}
 