using System;
using System.Collections.Generic;
using System.Text;

namespace TrueRegex
{
    public class Sequence : Expression
    {
        Expression lhs;
        Expression rhs;

        public Sequence(Expression lhs, Expression rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        internal override int Instance(Regex regex)
        {
            var rhsStart = this.rhs.Instance(regex);
            var start = regex.Size;
            var index = this.lhs.Instance(regex);
            for (var i = start; i < regex.Size; ++i)
            {
                var inst = regex[i];
                if (inst.Goal)
                {
                    inst.AddEpsilon(rhsStart);
                    inst.Goal = false;
                }
            }
            return index;
        }
    }
}
