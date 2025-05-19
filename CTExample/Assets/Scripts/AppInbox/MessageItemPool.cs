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

        public void ReturnToPool(MessageItem item)
        {
            if (!_pool.Contains(item))
            {
                _pool.Add(item);
            }

            item.gameObject.SetActive(false);
        }

        public void ReleaseAll()
        {
            foreach (var item in _pool)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}