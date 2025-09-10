// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;
using System.Collections.Generic;

namespace AllForWorkFlows
{
    public sealed class TransitionEventArgs<TContext> : EventArgs
    {
        private readonly ITransition<TContext> _transition;
        private readonly bool _guardResult;
        private readonly TContext _context;

        public TransitionEventArgs(ITransition<TContext> transition, bool guardResult, TContext context)
        {
            _transition = transition;
            _guardResult = guardResult;
            _context = context;
        }

        public ITransition<TContext> Transition { get { return _transition; } }
        public bool GuardResult { get { return _guardResult; } }
        public TContext Context { get { return _context; } }
    }
}