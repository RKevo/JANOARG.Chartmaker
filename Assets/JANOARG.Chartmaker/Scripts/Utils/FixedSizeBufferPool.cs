using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using buf = Unity.Collections.NativeArray<byte>;

namespace JANOARG.Chartmaker.Utils.Memory
{
    /// <summary>
    /// How unsafe, wouldn't be hard to use correctly tho. (Rent on Render > Entry into Queue > Read
    /// from Queue > Write into Stream > Return). Configure `reserve` as need be. Leak is safe, but
    /// returning one then using it afterward means UB.
    /// Mainly to reduce GC Pressure as rendering buffers can be quite large, and while .Persistant is
    /// slow, we're only doing it a couple of times and the buffers are supposed to be reused multiple
    /// times (no alloc when that happens)
    /// </summary>
    public sealed class FixedSizeBufferPool : IDisposable
    {
        private ConcurrentBag<buf> backing = new();
  
        private readonly int size;
        private int _disposed;
        private int alloced;
        private int max_alloc;

        public FixedSizeBufferPool(int arraySize, int reserve = 4, int max = 64) // try 8?
        {
            size = arraySize;
            alloced += reserve;
            for (int i = 0; i < reserve; i++)
            {
                var _ref = new buf(size, Allocator.Persistent);
                backing.Add(_ref);
            }
        }

        /// <remarks>
        /// Return after renting to avoid leaks.
        /// </remarks>
        /// <returns>Wrapper over a <see cref="NativeArray{T}"/> </returns>
        public bool Rent(out FixedSizeEntry rental)
        {
            if (backing.TryTake(out var item))
            {
                rental = new FixedSizeEntry()
                {
                    _ref = item,
                    _cachedSize = size
                };
                return true;
            }

            if (alloced == max_alloc)
            {
                rental = new FixedSizeEntry();
                return false;
            }

            var new_buf = new buf(size, Allocator.Persistent);
            Interlocked.Add(ref alloced, 1);
            rental = new FixedSizeEntry()
            {
                _ref = new_buf,
                _cachedSize = size
            };
            return true;
        }

        /// 
        /// <param name="item">The item to be returned</param>
        /// <returns>Whether the return was successful</returns>
        public bool Return(FixedSizeEntry item)
        {
            if (item._cachedSize != size)
            {
                return false;
            }
            backing.Add(item._ref);
            return true;
        }

        public struct FixedSizeEntry
        {
            internal NativeArray<byte> _ref;
            internal readonly int _cachedSize { init; get; }

            public bool CopyFrom(NativeArray<byte> src)
            {
                if (src.Length != _cachedSize)
                {
                    return false;
                }
                _ref.CopyFrom(src);
                return true;
            }
            
            public bool CopyTo(NativeArray<byte> dst)
            {
                if (dst.Length != _cachedSize)
                {
                    return false;
                }
                _ref.CopyTo(dst);
                return true;
            }
            
            public bool CopyFromU8(byte[] src)
            {
                if (src.Length != _cachedSize)
                {
                    return false;
                }
                _ref.CopyFrom(src);
                return true;
            }
            
            public bool CopyToU8(byte[] dst)
            {
                if (dst.Length != _cachedSize)
                {
                    return false;
                }
                _ref.CopyTo(dst);
                return true;
            }

            public ReadOnlySpan<byte> AsSpan() => _ref.AsReadOnlySpan();

        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                var present = new HashSet<int>();
                UnityEngine.Debug.Log($"Capacity: {backing.Count}");
                UnityEngine.Debug.Log($"Allocated: {alloced}");
                if (backing.Count != alloced)
                {
                    UnityEngine.Debug.LogWarning($"Possible multiple returns of attempted in this pool.");  
                }
                foreach (var each in backing)
                {
                    each.Dispose();
                }

                if (disposing)
                {
                    backing = null;
                }
            }
        }

        ~FixedSizeBufferPool() => Dispose(false);
    }
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}