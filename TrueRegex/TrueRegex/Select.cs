using System;
using System.Collections.Generic;
using System.Text;

namespace TrueRegex
{
    public class Select:Expression
    {
        Expression lhs;
        Expression rhs;

        public Select(Expression lhs, Expression rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        internal override int Instance(Regex regex)
        {
            var leftInstance = this.lhs.Instance(regex);
            var rightInstace = this.rhs.Instance(regex);
            var ret = leftInstance;
            if(this.lhs is Select)
            {
                regex[leftInstance].AddEpsilon(rightInstace);
            }
            else
            {
                var instance = regex.Add(new TerminalInstance(regex, false));
                regex[instance.Index].AddEpsilon(leftInstance, rightInstace);
                ret = instance.Index;
            }
            return ret;
        }
    }
}
