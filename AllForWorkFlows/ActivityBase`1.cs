// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;

namespace AllForWorkFlows
{
    public abstract class ActivityBase<TContext> : IActivity<TContext>
    {
        private readonly string _name;

        protected ActivityBase(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
        }

        public string Name { get { return _name; } }

        public ActivityResult Execute(TContext context)
        {
            return OnExecute(context);
        }

        protected abstract ActivityResult OnExecute(TContext context);
    }
}