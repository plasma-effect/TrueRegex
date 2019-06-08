using System;
using System.Collections.Generic;
using System.Text;
using static TrueRegex.Utility;

namespace TrueRegex
{
    /// <summary>
    /// Repeat expr more than or equal to zero times
    /// </summary>
    public class ZeroRepeat : Expression
    {
        Expression expr;

        public ZeroRepeat(Expression expr)
        {
            this.expr = expr;
        }

        internal override int Instance(Regex regex)
        {
            var start = regex.Size;
            var index = this.expr.Instance(regex);
            var size = regex.Size;
            var instance = regex.Add(new TerminalInstance(regex, true));
            instance.AddEpsilon(index);
            foreach (var i in Range(start, size))
            {
                var inst = regex[i];
                if (inst.Goal)
                {
                    inst.AddEpsilon(instance.Index);
                    inst.Goal = false;
                }
            }
            return instance.Index;
        }
    }
    
    /// <summary>
    /// Repeat expr more than or equal to one time
    /// </summary>
    public class OneRepeat : Expression
    {
        Expression expr;

        public OneRepeat(Expression expr)
        {
            this.expr = expr;
        }

        internal override int Instance(Regex regex)
        {
            var start = regex.Size;
            var index = this.expr.Instance(regex);
            foreach (var i in Range(start, regex.Size))
            {
                var inst = regex[i];
                if (inst.Goal)
                {
                    inst.AddEpsilon(index);
                }
            }
            return index;
        }
    }

}
