using System;
using System.Collections.Generic;

namespace TrueRegex
{
    public abstract class Expression:IRegex
    {
        internal abstract int Instance(Regex regex);
        public static OneRepeat operator+(Expression expr)
        {
            return new OneRepeat(expr);
        }
        public static ZeroRepeat operator~(Expression expr)
        {
            return new ZeroRepeat(expr);
        }
        public static Sequence operator+(Expression lhs,Expression rhs)
        {
            return new Sequence(lhs, rhs);
        }
        public static Optional operator-(Expression expr)
        {
            return new Optional(expr);
        }
        public static Select operator |(Expression lhs, Expression rhs)
        {
            return new Select(lhs, rhs);
        }

        private Regex internalRegex;
        private Regex InternalRegex
        {
            get
            {
                if(this.internalRegex is null)
                {
                    this.internalRegex = new Regex(this);
                }
                return this.internalRegex;
            }
        }
        public bool Match(IEnumerable<char> str)
        {
            return this.InternalRegex.Match(str);
        }

        public int? FirstMatch(IEnumerable<char> str)
        {
            return this.InternalRegex.FirstMatch(str);
        }

        public int? LastMatch(IEnumerable<char> str)
        {
            return this.InternalRegex.FirstMatch(str);
        }
    }

    internal abstract class Instance
    {
        protected Instance(Regex regex, bool goal)
        {
            this.Index = regex.Size;
            this.Goal = goal;
            this.Epsilons = new SortedSet<int>();
        }

        public virtual void Next(BoolSet flags, char c)
        {

        }
        
        public void AddEpsilon(params int[] indexes)
        {
            foreach(var i in indexes)
            {
                this.Epsilons.Add(i);
            }
        }
        public int Index { get; }
        public bool Goal { get; set; }
        public SortedSet<int> Epsilons { get; }
    }

    internal class TerminalInstance : Instance
    {
        public TerminalInstance(Regex regex, bool goal) : base(regex, goal)
        {

        }
    }
}
