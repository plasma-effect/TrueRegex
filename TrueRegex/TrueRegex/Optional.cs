using System;
using System.Collections.Generic;
using System.Text;

namespace TrueRegex
{
    /// <summary>
    /// Optional expr
    /// </summary>
    public class Optional : Expression
    {
        Expression expr;

        public Optional(Expression expr)
        {
            this.expr = expr;
        }

        internal override int Instance(Regex regex)
        {
            var start = regex.Size;
            var index = this.expr.Instance(regex);
            var size = regex.Size;
            var instance = regex.Add(new TerminalInstance(regex, true));
            regex[index].AddEpsilon(instance.Index);
            for(var i = start; i < size; ++i)
            {
                var inst = regex[i];
                if (inst.Goal)
                {
                    inst.AddEpsilon(instance.Index);
                    inst.Goal = false;
                }
            }
            return index;
        }
    }
}
