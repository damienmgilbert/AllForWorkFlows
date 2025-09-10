// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;

namespace AllForWorkFlows
{
    public sealed class WorkflowEventArgs<TContext> : EventArgs
    {
        private readonly string _state;
        private readonly TContext _context;

        public WorkflowEventArgs(string state, TContext context)
        {
            _state = state;
            _context = context;
        }

        public string State { get { return _state; } }
        public TContext Context { get { return _context; } }
    }
}