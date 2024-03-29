using UnityEngine;
using System;
using CleverTapSDK.Utilities;
using System.Collections.Generic;

namespace CleverTapSDK.Models {
  public class ApplePushNotification {
    public ApplePushNotificationAlert Alert { get; set; }

    public int Badge { get; set; }

    public string Sound { get; set; }

    public int ContentAvailable { get; set; }

    public IDictionary<string, object> Extra { get; set; }

    public ApplePushNotification(JSONClass json) {
      Alert = new ApplePushNotificationAlert(json["alert"].AsObject);
      Badge = json["badge"].AsInt;
      Sound = json["sound"];
      ContentAvailable = json["content-available"].AsInt;
      Extra = new Dictionary<string, object>();
      JSONNode ExtraNode = json["extra"];
      if (ExtraNode.GetType() == typeof(JSONClass)) {
        foreach (KeyValuePair<string, JSONNode> KeyValuePairNode in ExtraNode.AsObject) {
          Extra.Add(KeyValuePairNode.Key, this.getJSONNodeValue(KeyValuePairNode.Value));
        }
      } else {
        Debug.Log("Value of key Extra isn't a dictionary. Stop parsing the Extra dictionary");     
      }
    }

    private object getJSONNodeValue(JSONNode node) {
      int intNode = 0;
      if (int.TryParse(node.Value, out intNode)) {
        return intNode;
      }

      float floatNode = 0.0f;
      if (float.TryParse(node.Value, out floatNode)) {
        return floatNode;
      }

      double doubleNode = 0.0;
      if (double.TryParse(node.Value, out doubleNode)) {
        return doubleNode;
      }

      bool boolNode = false;
      if (bool.TryParse(node.Value, out boolNode)) {
        return boolNode;
      }

      if (node.GetType() == typeof(JSONArray)) {
        IList<object> nodeList = new List<object>();
        foreach (JSONNode nodeInArray in node.AsArray) {
          nodeList.Add(this.getJSONNodeValue(nodeInArray));
        }
        return nodeList;
      }

      if (node.GetType() == typeof(JSONClass)) {
        IDictionary<string, object> nodeDictionary = new Dictionary<string, object>();
        foreach (KeyValuePair<string, JSONNode> kvp in node.AsObject) {
          nodeDictionary.Add(kvp.Key, this.getJSONNodeValue(kvp.Value));
        }
        return nodeDictionary;
      }

      return node.ToString();
    }

    public override string ToString() {
      string extraString = "{";
      foreach (KeyValuePair<string, object> N in Extra) {
        if (extraString.Length > 2) {
          extraString += ", ";
        }
        extraString += "\"" + N.Key + "\":" + N.Value.ToString();
      }
      extraString += "}";
      return String.Format("PushNotification[Alert={0}, Badge={1}, Sound={2}, ContentAvailable={3}, Extra={4}]", 
                           Alert.ToString(), Badge, Sound, ContentAvailable, extraString);
    } 
  }
}

