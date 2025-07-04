using System.Collections.Generic;

namespace CleverTapSDK
{
    public class CleverTapDisplayUnit
    {
        public string Id { get; set; }
        public string MessageType { get; set; }
        public List<DisplayUnitContent> Content { get; set; }
        public string BackgroundColor { get; set; }
        public List<CustomKV> CustomKeyValuePairs { get; set; }

        public class DisplayUnitContent
        {
            public long Key { get; set; }
            public TextDetails Message { get; set; }
            public TextDetails Title { get; set; }
            public ActionDetails Action { get; set; }
            public Media Media { get; set; }
            public Media Icon { get; set; }
        }

        public class ActionDetails
        {
            public bool HasUrl { get; set; }
            public bool HasLinks { get; set; }
            public Url Url { get; set; }
            public List<Link> Links { get; set; }
        }

        public class Link
        {
            public string Type { get; set; }
            public string Text { get; set; }
            public string Color { get; set; }
            public string BackgroundColor { get; set; }
            public TextValue CopyText { get; set; }
            public Url Url { get; set; }
            public Dictionary<string, string> KeyValuePairs { get; set; }
        }

        public class TextDetails
        {
            public string Text { get; set; }
            public string Color { get; set; }
        }

        public class Url
        {
            public TextValue Android { get; set; }
            public TextValue IOS { get; set; }
        }

        public class Media
        {
            public string Url { get; set; }
            public string Key { get; set; }
            public string ContentType { get; set; }
        }

        public class CustomKV
        {
            public string Key { get; set; }
            public TextValue Value { get; set; }
        }

        public class TextValue
        {
            public string Text { get; set; }
        }
    }
}