using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TrueRegex
{
    internal class BoolSet : IEnumerable<int>
    {
        SortedSet<int> set;

        public BoolSet()
        {
            this.set = new SortedSet<int>();
        }

        public bool this[int index]
        {
            get
            {
                return this.set.Contains(index);
            }
            set
            {
                var f = this[index];
                if (value)
                {
                    this.set.Add(index);
                }
                else if(f)
                {
                    this.set.Remove(index);
                }
            }
        }
        public IEnumerator<int> GetEnumerator()
        {
            return ((IEnumerable<int>)this.set).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<int>)this.set).GetEnumerator();
        }

        public void Reset()
        {
            this.set.Clear();
        }

        public static bool operator true(BoolSet set)
        {
            return set.set.Count != 0;
        }
        public static bool operator false(BoolSet set)
        {
            return set.set.Count == 0;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder("{, n,}");
            foreach(var i in this.set)
            {
                stringBuilder.Replace(", n,", $"{i}, n,");
            }
            stringBuilder.Replace(", n,", "");
            return stringBuilder.ToString();
        }

        public void Swap(BoolSet set)
        {
            var impl = this.set;
            this.set = set.set;
            set.set = impl;
        }
    }
}
