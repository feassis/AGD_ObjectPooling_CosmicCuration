using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CosmicCuration.Utilities
{
    public class GenericObjectPool<T> where T : class
    {
        private List<PooledItem<T>> pool = new List<PooledItem<T>>();

        protected T GetItem()
        {
            if(pool.Count > 0)
            {
                var item = pool.Find(i => i.IsUsed == false);

                if(item != null)
                {
                    item.IsUsed = true;
                    return item.Item;
                }
            }

            return CreatePooledItem();
        }

        public void ReturnItem(T item)
        {
            var pooledItem = pool.Find(i => i.Item.Equals(item));
            pooledItem.IsUsed = false;
        }

        private T CreatePooledItem()
        {
            PooledItem<T> pooledItem = new PooledItem<T>();
            pooledItem.Item = CreateItem();
            pooledItem.IsUsed = true;
            pool.Add(pooledItem);
            return pooledItem.Item;
        }

        protected virtual T CreateItem()
        {
            throw new NotImplementedException("Chiled Not implemented Create Item");
        }

        public class PooledItem<T>
        {
            public T Item;
            public bool IsUsed;
        }
    }
}

