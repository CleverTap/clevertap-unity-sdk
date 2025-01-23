using System.Collections.Generic;
using CleverTapSDK.Utilities;

namespace CleverTapSDK
{
    public class CleverTapInboxMessageJSONParser
    {
        public static List<CleverTapInboxMessage> ParseJson(string jsonString)
        {
            JSONArray jsonArray = JSON.Parse(jsonString).AsArray;
            var result = new List<CleverTapInboxMessage>();

            foreach (JSONNode jsonNode in jsonArray)
            {
                var root = new CleverTapInboxMessage
                {
                    Id = jsonNode["id"].Value != string.Empty ? jsonNode["id"] : jsonNode["_id"],
                    Message = ParseMessageData(jsonNode["msg"]),
                    IsRead = jsonNode["isRead"].AsBool,
                    DateTs = jsonNode["date"].AsLong,
                    ExpiresTs = jsonNode["wzrk_ttl"].AsLong,
                    CampaignId = jsonNode["wzrk_id"]
                };
                result.Add(root);
            }

            return result;
        }

        private static CleverTapInboxMessage.MessageData ParseMessageData(JSONNode msgNode)
        {
            return new CleverTapInboxMessage.MessageData
            {
                MessageType = msgNode["type"],
                Content = ParseContentList(msgNode["content"].AsArray),
                BackgroundColor = msgNode["bg"],
                Orientation = msgNode["orientation"],
                EnableTags = msgNode["enableTags"].AsBool,
                Tags = ParseStringList(msgNode["tags"].AsArray),
                CustomKeyValuePairs = ParseCustomKVList(msgNode["custom_kv"].AsArray)
            };
        }

        private static List<CleverTapInboxMessage.Content> ParseContentList(JSONArray contentArray)
        {
            var contents = new List<CleverTapInboxMessage.Content>();
            foreach (JSONNode item in contentArray)
            {
                var content = new CleverTapInboxMessage.Content
                {
                    Key = item["key"].AsLong,
                    Message = ParseMessageDetails(item["message"]),
                    Title = ParseMessageDetails(item["title"]),
                    Action = ParseActionDetails(item["action"]),
                    Media = ParseMedia(item["media"]),
                    Icon = ParseMedia(item["icon"])
                };
                contents.Add(content);
            }
            return contents;
        }

        private static CleverTapInboxMessage.TextDetails ParseMessageDetails(JSONNode node)
        {
            return new CleverTapInboxMessage.TextDetails
            {
                Text = node["text"],
                Color = node["color"]
            };
        }

        private static CleverTapInboxMessage.ActionDetails ParseActionDetails(JSONNode node)
        {
            return new CleverTapInboxMessage.ActionDetails
            {
                HasUrl = node["hasUrl"].AsBool,
                HasLinks = node["hasLinks"].AsBool,
                Url = ParseUrl(node["url"]),
                Links = ParseLinksList(node["links"].AsArray)
            };
        }

        private static CleverTapInboxMessage.Url ParseUrl(JSONNode node)
        {
            return new CleverTapInboxMessage.Url
            {
                Android = ParseUrlText(node["android"]),
                IOS = ParseUrlText(node["ios"])
            };
        }

        private static CleverTapInboxMessage.TextValue ParseUrlText(JSONNode node)
        {
            return new CleverTapInboxMessage.TextValue
            {
                Text = node["text"]
            };
        }

        private static List<CleverTapInboxMessage.Link> ParseLinksList(JSONArray linksArray)
        {
            var links = new List<CleverTapInboxMessage.Link>();
            foreach (JSONNode item in linksArray)
            {
                var link = new CleverTapInboxMessage.Link
                {
                    Type = item["type"],
                    Text = item["text"],
                    Color = item["color"],
                    BackgroundColor = item["bg"],
                    CopyText = new CleverTapInboxMessage.TextValue { Text = item["copyText"]["text"] },
                    Url = ParseUrl(item["url"]),
                    KeyValuePairs = ParseDictionary(item["kv"])
                };
                links.Add(link);
            }
            return links;
        }

        private static CleverTapInboxMessage.Media ParseMedia(JSONNode node)
        {
            return new CleverTapInboxMessage.Media
            {
                Url = node["url"],
                Key = node["key"],
                ContentType = node["content_type"]
            };
        }

        private static List<CleverTapInboxMessage.CustomKV> ParseCustomKVList(JSONArray customKvArray)
        {
            var customKvs = new List<CleverTapInboxMessage.CustomKV>();
            foreach (JSONNode item in customKvArray)
            {
                var customKv = new CleverTapInboxMessage.CustomKV
                {
                    Key = item["key"],
                    Value = new CleverTapInboxMessage.TextValue
                    {
                        Text = item["value"]["text"]
                    }
                };
                customKvs.Add(customKv);
            }
            return customKvs;
        }

        private static List<string> ParseStringList(JSONArray array)
        {
            var list = new List<string>();
            foreach (JSONNode item in array)
            {
                list.Add(item);
            }
            return list;
        }

        private static Dictionary<string, string> ParseDictionary(JSONNode node)
        {
            var dict = new Dictionary<string, string>();
            if (node.Count == 0)
                return dict;

            JSONClass kv = node.AsObject;
            foreach (KeyValuePair<string, JSONNode> pair in kv)
            {
                dict[pair.Key] = pair.Value.Value;
            }
            return dict;
        }
    }
}