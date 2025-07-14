using System.Collections.Generic;

namespace CleverTapSDK.Utilities
{
    public class CleverTapDisplayUnitsJSONParser
    {
        public static List<CleverTapDisplayUnit> ParseJsonArray(string jsonString)
        {
            var result = new List<CleverTapDisplayUnit>();

            if (string.IsNullOrEmpty(jsonString))
                return result;

            JSONArray jsonArray;

            try
            {
                jsonArray = JSON.Parse(jsonString).AsArray;
            }
            catch (System.Exception ex)
            {
                CleverTapLogger.LogError($"Failed to parse display units JSON: {ex.Message}");
                return result;
            }

            foreach (JSONNode jsonNode in jsonArray)
            {
                var inboxMessage = ParseJsonNode(jsonNode);

                if (inboxMessage != null)
                    result.Add(inboxMessage);
            }

            return result;
        }

        private static CleverTapDisplayUnit ParseJsonNode(JSONNode jsonNode)
        {
            if (jsonNode == null || jsonNode.Count == 0)
                return null;

            CleverTapDisplayUnit displayUnit = ParseDisplayUnitDataData(jsonNode);
            return displayUnit;
        }

        private static CleverTapDisplayUnit ParseDisplayUnitDataData(JSONNode unitNode)
        {
            return new CleverTapDisplayUnit
            {
                Id = unitNode["wzrk_id"],
                MessageType = unitNode["type"],
                Content = ParseContentList(unitNode["content"].AsArray),
                BackgroundColor = unitNode["bg"],
                CustomKeyValuePairs = ParseCustomKVList(unitNode["customKVData"].AsArray)
            };
        }

        private static List<CleverTapDisplayUnit.DisplayUnitContent> ParseContentList(JSONArray contentArray)
        {
            var contents = new List<CleverTapDisplayUnit.DisplayUnitContent>();

            if (contentArray == null || contentArray.Count == 0)
                return contents;

            foreach (JSONNode item in contentArray)
            {
                var content = new CleverTapDisplayUnit.DisplayUnitContent
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

        private static CleverTapDisplayUnit.TextDetails ParseMessageDetails(JSONNode node)
        {
            return new CleverTapDisplayUnit.TextDetails
            {
                Text = node["text"],
                Color = node["color"]
            };
        }

        private static CleverTapDisplayUnit.ActionDetails ParseActionDetails(JSONNode node)
        {
            return new CleverTapDisplayUnit.ActionDetails
            {
                HasUrl = node["hasUrl"].AsBool,
                Url = ParseUrl(node["url"]),
                Links = ParseLinksList(node["links"].AsArray)
            };
        }

        private static CleverTapDisplayUnit.Url ParseUrl(JSONNode node)
        {
            return new CleverTapDisplayUnit.Url
            {
                Android = ParseUrlText(node["android"]),
                IOS = ParseUrlText(node["ios"])
            };
        }

        private static CleverTapDisplayUnit.TextValue ParseUrlText(JSONNode node)
        {
            return new CleverTapDisplayUnit.TextValue
            {
                Text = node["text"]
            };
        }

        private static List<CleverTapDisplayUnit.Link> ParseLinksList(JSONArray linksArray)
        {
            var links = new List<CleverTapDisplayUnit.Link>();
            foreach (JSONNode item in linksArray)
            {
                var link = new CleverTapDisplayUnit.Link
                {
                    Type = item["type"],
                    Text = item["text"],
                    Color = item["color"],
                    BackgroundColor = item["bg"],
                    CopyText = item["copyText"] != null ? new CleverTapDisplayUnit.TextValue { Text = item["copyText"]["text"] } : null,
                    Url = ParseUrl(item["url"]),
                    KeyValuePairs = ParseDictionary(item["kv"])
                };
                links.Add(link);
            }
            return links;
        }

        private static CleverTapDisplayUnit.Media ParseMedia(JSONNode node)
        {
            return new CleverTapDisplayUnit.Media
            {
                Url = node["url"],
                Key = node["key"],
                ContentType = node["content_type"]
            };
        }

        private static List<CleverTapDisplayUnit.CustomKV> ParseCustomKVList(JSONArray customKvArray)
        {
            var customKvs = new List<CleverTapDisplayUnit.CustomKV>();
            foreach (JSONNode item in customKvArray)
            {
                var customKv = new CleverTapDisplayUnit.CustomKV
                {
                    Key = item["key"],
                    Value = new CleverTapDisplayUnit.TextValue
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