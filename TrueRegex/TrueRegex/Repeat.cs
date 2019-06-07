using System;
using System.Collections.Generic;
using System.Text;

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
            var start = regex.Instances.Count;
            var index = this.expr.Instance(regex);
            var instance = new TerminalInstance(regex.Instances.Count, true);
            instance.Epsilons.Add(index);
            regex.Instances.Add(instance);
            for (var i = start; i <= index; ++i)
            {
                if (regex.Instances[i].Goal)
                {
                    regex.Instances[i].AddEpsilon(instance.Index);
                    regex.Instances[i].Goal = false;
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
            var start = regex.Instances.Count;
            var index = this.expr.Instance(regex);
            for(var i = start; i <= index; ++i)
            {
                if (regex.Instances[i].Goal)
                {
                    regex.Instances[i].AddEpsilon(index);
                }
            }
            return index;
        }
    }

}
