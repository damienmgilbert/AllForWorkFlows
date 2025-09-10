// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;
using System.Collections.Generic;

namespace AllForWorkFlows
{



    public sealed class WorkflowBuilder<TContext>
    {
        private readonly string _name;
        private readonly TContext _context;
        private readonly Dictionary<string, State<TContext>> _states;
        private string _initial;

        public WorkflowBuilder(string name, TContext context)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (context == null) throw new ArgumentNullException("context");
            _name = name;
            _context = context;
            _states = new Dictionary<string, State<TContext>>();
        }

        public WorkflowBuilder<TContext> State(string name, bool terminal)
        {
            _states[name] = new State<TContext>(name, terminal);
            if (_initial == null) _initial = name; // default first state as initial
            return this;
        }

        public WorkflowBuilder<TContext> Initial(string name)
        {
            _initial = name;
            return this;
        }

        public WorkflowBuilder<TContext> OnEnter(string state, IActivity<TContext> activity)
        {
            _states[state].OnEnter.Add(activity);
            return this;
        }

        public WorkflowBuilder<TContext> OnStep(string state, IActivity<TContext> activity)
        {
            _states[state].OnStep.Add(activity);
            return this;
        }

        public WorkflowBuilder<TContext> OnExit(string state, IActivity<TContext> activity)
        {
            _states[state].OnExit.Add(activity);
            return this;
        }

        public WorkflowBuilder<TContext> Transition(string name, string from, string to, TransitionGuard<TContext> guard)
        {
            _states[from].Transitions.Add(new Transition<TContext>(name, from, to, guard));
            return this;
        }

        public IWorkflow<TContext> Build()
        {
            List<IState<TContext>> list = new List<IState<TContext>>();
            foreach (KeyValuePair<string, State<TContext>> kv in _states)
            {
                list.Add(kv.Value);
            }
            return new Workflow<TContext>(_name, _context, _initial, list);
        }
    }
}

