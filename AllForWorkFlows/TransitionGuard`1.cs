// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;

namespace AllForWorkFlows
{
    public delegate bool TransitionGuard<TContext>(TContext context);
}