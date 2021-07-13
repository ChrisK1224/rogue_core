using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace files_and_folders
{
    public class FastDictionary<T> : IDictionary<long, T>
    {
        private readonly long count;
        private long[] indexes;
        private T[] store;

        public FastDictionary(int count)
        {
            this.count = count;
            this.store = new T[count];
            this.indexes = new long[count];
        }

        public IEnumerator<KeyValuePair<long, T>> GetEnumerator()
        {
            for (var i = 0; i < this.store.Length; i++)
            {
                if (this.ContainsKeyInternal(i))
                {
                    yield return new KeyValuePair<long, T>(i, this.store[i]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<long, T> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.store = new T[this.count];
            this.indexes = new long[this.count];
        }

        [Pure]
        public bool Contains(KeyValuePair<long, T> item)
        {
            return this.indexes[item.Key] > 0 && Equals(this.store[item.Key], item.Value);
        }

        public void CopyTo(KeyValuePair<long, T>[] array, int arrayIndex)
        {
            for (var i = arrayIndex; i < array.Length && i < this.store.Length; i++)
            {
                if (this.ContainsKeyInternal(i))
                {
                    array[i] = new KeyValuePair<long, T>(i, this.store[i]);
                }
            }
        }

        public bool Remove(KeyValuePair<long, T> item)
        {
            return this.Remove(item.Key);
        }

        public int Count
        {
            get
            {
                var result = 0;
                for (var i = 0; i < this.store.Length; i++)
                {
                    if (this.ContainsKeyInternal(i))
                    {
                        result++;
                    }
                }
                return result;
            }
        }

        public bool IsReadOnly => false;

        [Pure]
        public bool ContainsKey(long key)
        {
            return key < this.count && this.ContainsKeyInternal(key);
        }

        public void Add(long key, T value)
        {
            if (key >= this.count)
            {
                return;
            }
            this.store[key] = value;
            this.indexes[key] = 1;
        }

        public bool Remove(long key)
        {
            if (key >= this.count || !this.ContainsKeyInternal(key))
            {
                return false;
            }
            this.store[key] = default(T);
            this.indexes[key] = 0;
            return true;
        }

        /// <remarks>
        ///     IMPORTANT: key out of range intentionally missed here due to performance reasons.
        ///     You shouldn't pass key that out of size range to avoid undefined behaviour
        /// </remarks>
        public bool TryGetValue(long key, out T value)
        {
            if (this.indexes[key] == 0)
            {
                value = default(T);
                return false;
            }
            value = this.store[key];
            return true;
        }

        public T this[long key]
        {
            get { return this.store[key]; }
            set { this.store[key] = value; }
        }

        public ICollection<long> Keys
        {
            get { return this.Select(pair => pair.Key).ToArray(); }
        }

        public ICollection<T> Values
        {
            get { return this.Select(pair => pair.Value).ToArray(); }
        }

        private bool ContainsKeyInternal(long key)
        {
            return this.indexes[key] > 0;
        }
    }
}
