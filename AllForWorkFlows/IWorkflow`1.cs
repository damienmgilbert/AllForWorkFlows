// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;

namespace AllForWorkFlows
{
    public interface IWorkflow<TContext>
    {
        string Name { get; }
        string InitialState { get; }
        string CurrentState { get; }
        bool IsCompleted { get; }
        TContext Context { get; }

        event EventHandler<WorkflowEventArgs<TContext>> EnteredState;
        event EventHandler<WorkflowEventArgs<TContext>> ExitedState;
        event EventHandler<ActivityEventArgs<TContext>> ActivityExecuted;
        event EventHandler<TransitionEventArgs<TContext>> TransitionEvaluated;
        event EventHandler<TransitionEventArgs<TContext>> TransitionTaken;

        void Start();
        void Step();
        void RunToCompletion(int maxSteps); // safety to avoid infinite loops
    }
}