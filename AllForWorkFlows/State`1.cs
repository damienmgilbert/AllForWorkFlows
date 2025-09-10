// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;
using System.Collections.Generic;

namespace AllForWorkFlows
{
    public sealed class State<TContext> : IState<TContext>
    {
        private readonly string _name;
        private readonly bool _isTerminal;
        private readonly IList<IActivity<TContext>> _onEnter;
        private readonly IList<IActivity<TContext>> _onStep;
        private readonly IList<IActivity<TContext>> _onExit;
        private readonly IList<ITransition<TContext>> _transitions;

        public State(string name, bool isTerminal)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
            _isTerminal = isTerminal;
            _onEnter = new List<IActivity<TContext>>();
            _onStep = new List<IActivity<TContext>>();
            _onExit = new List<IActivity<TContext>>();
            _transitions = new List<ITransition<TContext>>();
        }

        public string Name { get { return _name; } }
        public bool IsTerminal { get { return _isTerminal; } }

        public IList<IActivity<TContext>> OnEnter { get { return _onEnter; } }
        public IList<IActivity<TContext>> OnStep { get { return _onStep; } }
        public IList<IActivity<TContext>> OnExit { get { return _onExit; } }
        public IList<ITransition<TContext>> Transitions { get { return _transitions; } }
    }
}

