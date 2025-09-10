// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;

namespace AllForWorkFlows
{
    public sealed class Transition<TContext> : ITransition<TContext>
    {
        private readonly string _name;
        private readonly string _from;
        private readonly string _to;
        private readonly TransitionGuard<TContext> _guard;

        public Transition(string name, string fromState, string toState, TransitionGuard<TContext> guard)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (fromState == null) throw new ArgumentNullException("fromState");
            if (toState == null) throw new ArgumentNullException("toState");
            if (guard == null) throw new ArgumentNullException("guard");

            _name = name;
            _from = fromState;
            _to = toState;
            _guard = guard;
        }

        public string Name { get { return _name; } }
        public string FromState { get { return _from; } }
        public string ToState { get { return _to; } }
        public TransitionGuard<TContext> Guard { get { return _guard; } }
    }
}

