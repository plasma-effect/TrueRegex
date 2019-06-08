using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TrueRegex.Utility;

namespace TrueRegex
{
    /// <summary>
    /// One character expr(match when func(c) is true)
    /// </summary>
    public class Atomic : Expression
    {
        Func<char, bool> func;

        public Atomic(Func<char, bool> func)
        {
            this.func = func;
        }

        internal override int Instance(Regex regex)
        {
            var terminal = regex.Add(new TerminalInstance(regex, true));
            var instance = regex.Add(new InstancedExpr(regex, terminal, this.func));
            return instance.Index;
        }

        internal class InstancedExpr : Instance
        {
            Instance next;
            Func<char, bool> func;

            public InstancedExpr(Regex regex, Instance next, Func<char, bool> func) : base(regex, false)
            {
                this.next = next;
                this.func = func;
            }

            public override void Next(BoolSet flags, char c)
            {
                if (!flags[this.next.Index] && this.func(c))
                {
                    flags[this.next.Index] = true;
                }
            }

            internal int NextInstance
            {
                get
                {
                    return this.next.Index;
                }
            }
        }

        public static Atomic Create(Func<char,bool> func)
        {
            return new Atomic(func);
        }
    }
}
