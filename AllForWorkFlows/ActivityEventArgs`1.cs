// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;

namespace AllForWorkFlows
{
    public sealed class ActivityEventArgs<TContext> : EventArgs
    {
        private readonly string _state;
        private readonly IActivity<TContext> _activity;
        private readonly ActivityResult _result;
        private readonly TContext _context;

        public ActivityEventArgs(string state, IActivity<TContext> activity, ActivityResult result, TContext context)
        {
            _state = state;
            _activity = activity;
            _result = result;
            _context = context;
        }

        public string State { get { return _state; } }
        public IActivity<TContext> Activity { get { return _activity; } }
        public ActivityResult Result { get { return _result; } }
        public TContext Context { get { return _context; } }
    }
}