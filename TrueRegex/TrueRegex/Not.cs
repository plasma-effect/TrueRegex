using System;
using System.Collections.Generic;
using System.Text;

namespace TrueRegex
{
    public class Not:Expression
    {
        Expression expr;

        public Not(Expression expr)
        {
            this.expr = expr;
        }

        internal override int Instance(Regex regex)
        {
            var ret = this.expr.Instance(regex);
            regex.NotFlag = !regex.NotFlag;
            return ret;
        }
    }
}
