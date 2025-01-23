using System;
using System.Collections.Generic;

namespace CleverTapSDK
{
    public class CleverTapInboxMessage
    {
        #region Inbox Message properties
        /// <summary>
        /// Returns the message identifier of the inbox message.
        /// </summary>
        public string Id { get; set; }

        public MessageData Message { get; set; }

        /// <summary>
        /// Returns true if the inbox message is read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Returns the delivery timestamp of the inbox message.
        /// </summary>
        public long DateTs { get; set; }

        /// <summary>
        /// Returns the delivery UTC date of the inbox message.
        /// </summary>
        public DateTime? DateUtcDate => DateTs > 0 ? DateTimeOffset.FromUnixTimeSeconds(DateTs).UtcDateTime : null;

        /// <summary>
        /// Returns the expiry timestamp of the inbox message.
        /// </summary>
        public long ExpiresTs { get; set; }

        /// <summary>
        /// Returns the expiry UTC date of the inbox message.
        /// </summary>
        public DateTime? ExpiresUtcDate => ExpiresTs > 0 ? DateTimeOffset.FromUnixTimeSeconds(ExpiresTs).UtcDateTime : null;

        /// <summary>
        /// Returns the campaign identifier.
        /// </summary>
        public string CampaignId { get; set; }

        #endregion

        #region Inbox Message nested classes
        public class MessageData
        {
            public string MessageType { get; set; }
            public bool EnableTags { get; set; }
            public List<Content> Content { get; set; }
            public string BackgroundColor { get; set; }
            public string Orientation { get; set; }

            /// <summary>
            /// The inbox message Filter tags.
            /// </summary>
            public List<string> Tags { get; set; }

            /// <summary>
            /// The inbox message Custom Key-Value Pairs.
            /// </summary>
            public List<CustomKV> CustomKeyValuePairs { get; set; }
        }

        public class Content
        {
            public long Key { get; set; }
            public TextDetails Message { get; set; }
            public TextDetails Title { get; set; }
            public ActionDetails Action { get; set; }
            public Media Media { get; set; }
            public Media Icon { get; set; }
        }

        /// <summary>
        /// The inbox message Actions.
        /// </summary>
        public class ActionDetails
        {
            public bool HasUrl { get; set; }
            public bool HasLinks { get; set; }

            /// <summary>
            /// The On Message Call to Action Url.
            /// </summary>
            public Url Url { get; set; }

            /// <summary>
            /// The inbox message On Link links.
            /// </summary>
            public List<Link> Links { get; set; }
        }

        /// <summary>
        /// The inbox message Link.
        /// </summary>
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
        #endregion
    }
}