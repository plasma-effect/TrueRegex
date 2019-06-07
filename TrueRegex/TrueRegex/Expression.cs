using System;
using System.Collections.Generic;

namespace TrueRegex
{
    public abstract class Expression
    {
        internal abstract int Instance(Regex regex);
    }

    internal abstract class Instance
    {
        protected Instance(int index, bool goal)
        {
            this.Index = index;
            this.Goal = goal;
            this.Epsilons = new List<int>();
        }

        public virtual void Next(BoolSet flags, char c)
        {

        }
        
        public void AddEpsilon(int index)
        {
            this.Epsilons.Add(index);
        }
        public int Index { get; }
        public bool Goal { get; set; }
        public List<int> Epsilons { get; }
    }

    internal class TerminalInstance : Instance
    {
        public TerminalInstance(int index, bool goal) : base(index, goal)
        {

        }
    }
}
