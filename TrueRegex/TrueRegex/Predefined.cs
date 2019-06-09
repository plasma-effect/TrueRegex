using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static TrueRegex.Utility;

namespace TrueRegex
{
    public static class Predefined
    {
        /// <summary>
        /// One character expr(match when [c] is in chars)
        /// </summary>
        public class Chars : Expression
        {
            SortedSet<char> chars;

            public Chars(IEnumerable<char> chars)
            {
                this.chars = new SortedSet<char>(chars);
            }

            internal override int Instance(Regex regex)
            {
                var expr = new Atomic(this.chars.Contains);
                return expr.Instance(regex);
            }

            public static Chars Create(params char[] cs)
            {
                return new Chars(cs);
            }
        }

        /// <summary>
        /// Number string
        /// </summary>
        public static OneRepeat Number { get; } = +Atomic.Create(char.IsNumber);

        /// <summary>
        /// Name string
        /// </summary>
        public static OneRepeat Name { get; } = +Atomic.Create(char.IsLetterOrDigit);

        public class String : Expression
        {
            string str;
            public String(string str)
            {
                this.str = str;
            }

            internal class InstancedExpr : Instance
            {
                Instance next;
                char c;

                public InstancedExpr(Instance next, char c, Regex regex) : base(regex, false)
                {
                    this.next = next;
                    this.c = c;
                }

                public override void Next(BoolSet flags, char c)
                {
                    if (this.c == c)
                    {
                        flags[this.next.Index] = true;
                    }
                }
            }
            internal override int Instance(Regex regex)
            {
                var next = regex.Add(new TerminalInstance(regex, true));
                foreach (var c in this.str.Reverse())
                {
                    next = regex.Add(new InstancedExpr(next, c, regex));
                }
                return next.Index;
            }

            public static String Create(string str)
            {
                return new String(str);
            }
        }
    }
}
