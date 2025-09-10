// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;

namespace AllForWorkFlows
{
    // Simple inline activity without sub classing (using anonymous methods)
    public sealed class DelegateActivity<TContext> : IActivity<TContext>
    {
        private readonly string _name;
        private readonly Converter<TContext, ActivityResult> _body;

        public DelegateActivity(string name, Converter<TContext, ActivityResult> body)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (body == null) throw new ArgumentNullException("body");
            _name = name;
            _body = body;
        }

        public string Name { get { return _name; } }

        public ActivityResult Execute(TContext context)
        {
            return _body(context);
        }
    }
}

