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
            var terminal = new TerminalInstance(regex.Instances.Count, true);
            regex.Instances.Add(terminal);
            var instance = new InstancedExpr(regex.Instances.Count, terminal, this.func);
            regex.Instances.Add(instance);
            return instance.Index;
        }

        internal class InstancedExpr : Instance
        {
            TerminalInstance next;
            Func<char, bool> func;

            public InstancedExpr(int index, TerminalInstance next, Func<char, bool> func) : base(index, false)
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
    }

    /// <summary>
    /// One character expr(match when [c] is in chars)
    /// </summary>
    public class Chars:Atomic
    {
        public Chars(params char[] chars) : base(c => chars.Contains(c))
        {

        }
    }

}
