using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TrueRegex.Utility;

namespace TrueRegex
{
    public interface IRegex
    {
        bool Match(IEnumerable<char> str);
        int? FirstMatch(IEnumerable<char> str);
        int? LastMatch(IEnumerable<char> str);
    }

    internal class Regex:IRegex
    {
        List<Instance> instances;
        int startIndex;
        public Regex(Expression expr)
        {
            this.instances = new List<Instance>();
            this.startIndex = expr.Instance(this);
        }

        private bool Check(BoolSet flags)
        {
            bool inner(int i)
            {
                return this.instances[i].Goal;
            }
            return flags.Any(inner);
        }
        public bool Match(IEnumerable<char> str)
        {
            InitialSet(out var flags, out var nexts, out var epsil, out var enext);
            foreach (var c in str)
            {
                OneStep(flags, nexts, epsil, enext, c);
            }
            return Check(flags);
        }
        public int? FirstMatch(IEnumerable<char> str)
        {
            InitialSet(out var flags, out var nexts, out var epsil, out var enext);
            var len = 0;
            if (Check(flags))
            {
                return len;
            }
            foreach(var c in str)
            {
                ++len;
                OneStep(flags, nexts, epsil, enext, c);
                if (Check(flags))
                {
                    return len;
                }
            }
            return null;
        }

        public int? LastMatch(IEnumerable<char> str)
        {
            InitialSet(out var flags, out var nexts, out var epsil, out var enext);
            int? ret = null;
            var index = 0;
            if (Check(flags))
            {
                ret = index;
            }
            foreach (var c in str)
            {
                ++index;
                OneStep(flags, nexts, epsil, enext, c);
                if (Check(flags))
                {
                    ret = index;
                }
            }
            return ret;
        }

        private void InitialSet(out BoolSet flags, out BoolSet nexts, out BoolSet epsil, out BoolSet enext)
        {
            flags = new BoolSet();
            nexts = new BoolSet();
            epsil = new BoolSet();
            enext = new BoolSet();
            flags[this.startIndex] = true;
            EpsilonMove(flags, epsil, enext);
        }

        private void OneStep(BoolSet flags, BoolSet nexts, BoolSet epsil, BoolSet enext, char c)
        {
            nexts.Reset();
            foreach (var i in flags)
            {
                this.instances[i].Next(nexts, c);
            }
            EpsilonMove(nexts, epsil, enext);
            flags.Swap(nexts);
        }

        private IEnumerable<int> Range()
        {
            return Utility.Range(0, this.instances.Count);
        }

        private void EpsilonMove(BoolSet nexts, BoolSet epsil, BoolSet enext)
        {
            epsil.Reset();
            foreach (var i in nexts)
            {
                foreach (var n in this.instances[i].Epsilons)
                {
                    epsil[n] = true;
                }
            }
            foreach(var i in epsil)
            {
                nexts[i] = true;
            }
            do
            {
                enext.Reset();
                foreach (var i in epsil)
                {
                    foreach (var n in this.instances[i].Epsilons.Where(d => !nexts[d]))
                    {
                        nexts[n] = true;
                        enext[n] = true;
                    }
                }
                epsil.Swap(enext);
            } while (epsil);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"vertexes count: {this.instances.Count}");
            stringBuilder.AppendLine($"start Index:{this.startIndex}");
            stringBuilder.Append("goal Indexes:");
            foreach(var i in this.instances.Where(instance => instance.Goal))
            {
                stringBuilder.Append($"{i.Index} ");
            }
            stringBuilder.AppendLine();
            foreach(var i in Range())
            {
                if(this.instances[i] is Atomic.InstancedExpr atomic)
                {
                    stringBuilder.AppendLine($"{i} {atomic.NextInstance} func");
                }
                foreach(var e in this.instances[i].Epsilons)
                {
                    stringBuilder.AppendLine($"{i} {e} ε");
                }
            }
            return stringBuilder.ToString();
        }

        internal Instance Add(Instance instance)
        {
            this.instances.Add(instance);
            return instance;
        }
        internal int Size
        {
            get
            {
                return this.instances.Count;
            }
        }

        internal Instance this[int index]
        {
            get
            {
                return this.instances[index];
            }
        }
    }
}
