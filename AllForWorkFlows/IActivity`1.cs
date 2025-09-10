// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;
using System.Collections.Generic;

namespace AllForWorkFlows
{
    public interface IActivity<TContext>
    {
        string Name { get; }
        ActivityResult Execute(TContext context);
    }
}