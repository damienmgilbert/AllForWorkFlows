// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;

namespace AllForWorkFlows
{
    public interface ITransition<TContext>
    {
        string Name { get; }
        string FromState { get; }
        string ToState { get; }
        TransitionGuard<TContext> Guard { get; }
    }
}