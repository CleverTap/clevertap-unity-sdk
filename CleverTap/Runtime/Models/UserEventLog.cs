using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;

namespace CleverTapSDK
{
    public class UserEventLog
    {
        public string EventName { get; }

        public string NormalizedEventName { get; }

        public long FirstTS { get; }

        public long LastTS { get; }

        public int CountOfEvents { get; }

        public string DeviceID { get; }

        public UserEventLog(string eventName, string normalizedEventName, long firstTS, long lastTS, int countOfEvents, string deviceID)
        {
            EventName = eventName;
            NormalizedEventName = normalizedEventName;
            FirstTS = firstTS;
            LastTS = lastTS;
            CountOfEvents = countOfEvents;
            DeviceID = deviceID;
        }

        public override string ToString()
        {
            return $"UserEventLog:\n" +
                $"EventName: {EventName},\n" +
                $"NormalizedEventName: {NormalizedEventName},\n" +
                $"FirstTS: {FirstTS},\n" +
                $"LastTS: {LastTS},\n" +
                $"CountOfEvents: {CountOfEvents},\n" +
                $"DeviceID: {DeviceID}";
        }

        public static UserEventLog Parse(string message)
        {
            try
            {
                var json = JSON.Parse(message);
                var userEventLog = new UserEventLog(
                    json["eventName"],
                    json["normalizedEventName"],
                    json["firstTS"].AsLong,
                    json["lastTS"].AsLong,
                    json["countOfEvents"].AsInt,
                    json["deviceID"]
                );
                return userEventLog;
            }
            catch (Exception ex)
            {
                CleverTapLogger.LogError($"Unable to parse user event log JSON: {ex}.");
            }
            return null;
        }

        public static Dictionary<string, UserEventLog> ParseLogsDictionary(string jsonString)
        {
            var userEventLogs = new Dictionary<string, UserEventLog>();
            try
            {
                var json = JSON.Parse(jsonString);
                foreach (KeyValuePair<string, JSONNode> kvp in json.AsObject)
                {
                    var key = kvp.Key;
                    var userEventLogJson = kvp.Value.ToString();
                    var userEventLog = Parse(userEventLogJson);

                    if (userEventLog != null)
                    {
                        userEventLogs[key] = userEventLog;
                    }
                }
            }
            catch (Exception ex)
            {
                CleverTapLogger.LogError($"Error parsing UserEventLog dictionary: {ex}");
            }

            return userEventLogs;
        }
    }
}
