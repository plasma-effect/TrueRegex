using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static TrueRegex.Utility;

namespace TrueRegex.Predefind
{
    /// <summary>
    /// One character expr(match when [c] is in chars)
    /// </summary>
    public class Chars : Atomic
    {
        public Chars(params char[] chars) : base(c => chars.Contains(c))
        {

        }
        public static Chars Create(params char[] chars)
        {
            return new Chars(chars);
        }
    }

    /// <summary>
    /// Number string
    /// </summary>
    public class Number : OneRepeat
    {
        public Number() : base(new Atomic(char.IsNumber))
        {
        }
    }

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

            public InstancedExpr(Instance next,char c, Regex regex) : base(regex, false)
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

    public class Name : OneRepeat
    {
        public Name() : base(Atomic.Create(char.IsLetterOrDigit))
        {

        }
    }
}
