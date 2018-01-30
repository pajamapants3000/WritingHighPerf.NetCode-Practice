using System;
using System.Collections.Generic;

namespace Main
{
    class PoolManager
    {
        private class Pool
        {
            public int PooledSize { get; set; }
            public int Count { get { return Stack.Count; } }
            public Stack<IPoolableObject> Stack { get; private set; }
            public Pool()
            {
                Stack = new Stack<IPoolableObject>();
            }
        }
        const int MaxSizePerType = 10 * (1 << 10);  // 10,240, or 10MB (interpreted as kB units, apparently)

        Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();

        public int TotalCount
        {
            get
            {
                int sum = 0;
                foreach (var pool in pools.Values)
                {
                    sum += pool.Count;
                }
                return sum;
            }
        }

        public T GetObject<T>() where T: class, IPoolableObject, new()
        {
            T valueToReturn = null;
            if (pools.TryGetValue(typeof(T), out Pool pool))
            {
                if (pool.Stack.Count > 0)
                {
                    valueToReturn = pool.Stack.Pop() as T;
                }
            }
            if (valueToReturn == null)
            {
                valueToReturn = new T();
            }
            valueToReturn.SetPoolManager(this);
            return valueToReturn;
        }

        public void ReturnObject<T>(T value) where T: class, IPoolableObject, new()
        {
            if (!pools.TryGetValue(typeof(T), out Pool pool))
            {
                pool = new Pool();
                pools[typeof(T)] = pool;
            }

            if (value.Size + pool.PooledSize < MaxSizePerType)
            {
                pool.PooledSize += value.Size;
                value.Reset();
                pool.Stack.Push(value);
            }
        }
    }

    class MyObject : IPoolableObject
    {
        private PoolManager poolManager;
        public byte[] Data { get; set; }
        public int UsableLength { get; set; }

        public int Size
        {
            get { return Data != null ? Data.Length : 0; }
        }

        void IPoolableObject.Reset()
        {
            UsableLength = 0;
        }

        void IPoolableObject.SetPoolManager(PoolManager _poolManager)
        {
            poolManager = _poolManager;
        }

        public void Dispose()
        {
            poolManager.ReturnObject(this);
        }
    }
}
