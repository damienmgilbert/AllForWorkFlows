// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;
using System.Collections.Generic;

namespace AllForWorkFlows
{
    public enum ActivityResult
    {
        None = 0,
        Success = 1,
        Failure = 2,
        Defer = 3   // No decision yet; remain in state and continue stepping.
    }
}

