using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TrueRegex.Utility;

namespace TrueRegex
{
    public class Regex
    {
        internal List<Instance> Instances { get; }
        int startIndex;
        public Regex(Expression expr)
        {
            this.Instances = new List<Instance>();
            this.startIndex = expr.Instance(this);
        }
        
        public bool Match(IEnumerable<char> str)
        {
            var flags = new BoolSet();
            var nexts = new BoolSet();
            var epsil = new BoolSet();
            var enext = new BoolSet();
            flags[this.startIndex] = true;
            EpsilonMove(flags, epsil, enext);
            foreach (var c in str)
            {
                OneStep(flags, nexts, epsil, enext, c);
            }
            return flags.Any(i => this.Instances[i].Goal);
        }

        private void OneStep(BoolSet flags, BoolSet nexts, BoolSet epsil, BoolSet enext, char c)
        {
            nexts.Reset();
            foreach (var i in flags)
            {
                this.Instances[i].Next(nexts, c);
            }
            EpsilonMove(nexts, epsil, enext);
            flags.Swap(nexts);
        }

        private IEnumerable<int> Range()
        {
            return Enumerable.Range(0, this.Instances.Count);
        }

        private void EpsilonMove(BoolSet nexts, BoolSet epsil, BoolSet enext)
        {
            epsil.Reset();
            foreach (var i in nexts)
            {
                foreach (var n in this.Instances[i].Epsilons)
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
                    foreach (var n in this.Instances[i].Epsilons.Where(d => !nexts[d]))
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
            stringBuilder.AppendLine($"vertexes count: {this.Instances.Count}");
            stringBuilder.AppendLine($"start Index:{this.startIndex}");
            stringBuilder.Append("goal Indexes:");
            foreach(var i in this.Instances.Where(instance => instance.Goal))
            {
                stringBuilder.Append($"{i.Index} ");
            }
            stringBuilder.AppendLine();
            foreach(var i in Range())
            {
                if(this.Instances[i] is Atomic.InstancedExpr atomic)
                {
                    stringBuilder.AppendLine($"{i} {atomic.NextInstance} func");
                }
                foreach(var e in this.Instances[i].Epsilons)
                {
                    stringBuilder.AppendLine($"{i} {e} ε");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
