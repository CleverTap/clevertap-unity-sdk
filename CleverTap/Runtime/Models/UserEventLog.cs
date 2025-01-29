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
    }
}
