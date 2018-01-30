using System;

namespace Main
{
    interface IPoolableObject : IDisposable
    {
        int Size { get; }
        void Reset();
        void SetPoolManager(PoolManager poolManager);
    }
}
