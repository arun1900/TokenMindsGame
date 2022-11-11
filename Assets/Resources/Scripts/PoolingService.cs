using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts
{
    public class PoolingService<T> : MonoSingletonGeneric<PoolingService<T>> where T : class
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once CollectionNeverUpdated.Local
        [SerializeField] private List<PoolingItem<T>> _pooledItems = new List<PoolingItem<T>>();

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Dictionary<string, List<PoolingItem<T>>> _itemsToPool = new Dictionary<string, List<PoolingItem<T>>>();

        protected virtual T GetItem(string key)
        {
            if (_itemsToPool.Count <= 0) return CreateNewPooledItem(key);
            if (!_itemsToPool.ContainsKey(key)) return CreateNewPooledItem(key);
            var pooledItem = _itemsToPool[key].Find(i => !i.IsActive);
            if (pooledItem == null) return CreateNewPooledItem(key);
            pooledItem.IsActive = true;
            //Debug.Log("Got item from Pool");
            return pooledItem.Item;
        }

        public virtual void ReturnItem(string key,T item)
        {
            PoolingItem<T> poolingItem = _itemsToPool[key].Find(i => i.Item.Equals(item));
            poolingItem.IsActive = false;
            //Debug.Log("Returned Item to pool");
        }

        private T CreateNewPooledItem(string key)
        {
            PoolingItem<T> poolItem = new PoolingItem<T>
            {
                Item = CreateItem(key),
                IsActive = true
            };
            if (_itemsToPool.ContainsKey(key))
            {
                _itemsToPool[key].Add(poolItem);
            }
            else
            {
                _itemsToPool.Add(key,new List<PoolingItem<T>>(){poolItem});
                //Debug.Log("Adding New key " + key);
            }
            //_pooledItems.Add(poolItem);
            return poolItem.Item;
        }

        protected virtual T CreateItem(string key)
        {
            return (T)null;
        }

#pragma warning disable 693
        private class PoolingItem<T>
#pragma warning restore 693
        {
            public T Item;
            public bool IsActive;
        }

   
    }
}