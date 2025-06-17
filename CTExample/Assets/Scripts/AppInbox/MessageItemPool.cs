using System;
using System.Collections.Generic;
using UnityEngine;

namespace CTExample
{
    [Serializable]
    public sealed class MessageItemPool
    {
        [SerializeField] private MessageItem _messageItemPrefab;
        [SerializeField] private Transform _parent;

        private List<MessageItem> _pool = new List<MessageItem>();

        public MessageItem Get()
        {
            foreach (var item in _pool)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    item.gameObject.SetActive(true);
                    return item;
                }
            }

            // No inactive item found, create a new one
            MessageItem newItem = GameObject.Instantiate(_messageItemPrefab, _parent);
            _pool.Add(newItem);
            return newItem;
        }

        public void SetMessageItemAsInactive(MessageItem item)
        {
            item.gameObject.SetActive(false);
        }

        public void DeactivateAll()
        {
            foreach (var item in _pool)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}