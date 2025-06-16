#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections.Generic;
using System.Threading.Tasks;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeNotificationViewedEventQueue : UnityNativeBaseEventQueue
    {
        protected override string QueueName => nameof(UnityNativeNotificationViewedEventQueue);

        internal UnityNativeNotificationViewedEventQueue(UnityNativeCoreState coreState,
            UnityNativeNetworkEngine networkEngine) :
            base(coreState, networkEngine)
        {

        }

        protected override string GetRequestBaseURL()
        {
            return networkEngine.SpikyURI;
        }

        protected override Dictionary<string, string> GetRequestHeader()
        {
            string jsonContent = eventsQueue.Peek()[0].JsonContent;
            string wzrk_ref = GetWZRKFields(jsonContent);

            if (string.IsNullOrEmpty(wzrk_ref))
            {
                CleverTapLogger.LogError("Notification viewed event does not have any wzrk_* fields for headers.");
                return null;
            }

            return new Dictionary<string, string> { { UnityNativeConstants.EventMeta.WZRK_REF, wzrk_ref } };
        }

        internal override async Task<List<UnityNativeEvent>> FlushEvents()
        {
            return await FlushEventsCore(request => networkEngine.ExecuteRequest(request));
        }

        private string GetWZRKFields(string jsonString)
        {
            Dictionary<string, object> jsonDict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            Dictionary<string, object> eventData = jsonDict[UnityNativeConstants.Event.EVENT_DATA] as Dictionary<string, object>;
            Dictionary<string, object> wzrkParams = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> item in eventData)
            {
                if (!item.Key.StartsWith("wzrk_"))
                {
                    continue;
                }

                wzrkParams[item.Key] = item.Value;
            }

            if (wzrkParams.Count == 0)
            {
                return null;
            }

            return Json.Serialize(wzrkParams);
        }
    }
}
#endif