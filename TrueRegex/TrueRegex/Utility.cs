using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueRegex
{
    internal static class Utility
    {
#if DEBUG
        public static void DebugWrite(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj);
        }
#endif
        internal static Func<T, bool> PredicateOr<T>(params Func<T, bool>[] funcs)
        {
            bool Return(T c)
            {
                foreach(var func in funcs)
                {
                    if (func(c))
                    {
                        return true;
                    }
                }
                return false;
            }
            return Return;
        }
        internal static Func<T,bool> PredicateAnd<T>(params Func<T,bool>[] funcs)
        {
            bool Return(T c)
            {
                foreach(var func in funcs)
                {
                    if (!func(c))
                    {
                        return false;
                    }
                }
                return true;
            }
            return Return;
        }
        internal class RangeType : IEnumerable<int>
        {
            int start;
            int count;
            int step;

            public RangeType(int start, int count, int step)
            {
                this.start = start;
                this.count = count;
                this.step = step;
            }

            public IEnumerator<int> GetEnumerator()
            {
                for (var i = 0; i < this.count; ++i)
                {
                    yield return this.start + i * this.step;
                }
            }

            internal RangeType Reverse()
            {
                return new RangeType(this.start + (this.count - 1) * this.step, this.count, -this.step);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        internal static RangeType Range(int min, int max, int step = 1)
        {
            return new RangeType(min, (max - min) / step, step);
        }
    }
}
