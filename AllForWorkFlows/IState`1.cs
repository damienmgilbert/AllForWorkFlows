// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;
using System.Collections.Generic;

namespace AllForWorkFlows
{
    public interface IState<TContext>
    {
        string Name { get; }
        IList<IActivity<TContext>> OnEnter { get; }
        IList<IActivity<TContext>> OnStep { get; }
        IList<IActivity<TContext>> OnExit { get; }
        IList<ITransition<TContext>> Transitions { get; }
        bool IsTerminal { get; }
    }
}