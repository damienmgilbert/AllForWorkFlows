// Target Framework: .NET 2.0, C# 2.0
// Namespace: AllForWorkFlows

using System;
using System.Collections.Generic;

namespace AllForWorkFlows
{
    public sealed class Workflow<TContext> : IWorkflow<TContext>
    {
        private readonly string _name;
        private readonly TContext _context;
        private readonly Dictionary<string, IState<TContext>> _states;
        private readonly string _initialState;
        private string _currentState;
        private readonly object _sync = new object();
        private bool _started;
        private bool _completed;

        public Workflow(string name, TContext context, string initialState, IEnumerable<IState<TContext>> states)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (context == null) throw new ArgumentNullException("context");
            if (initialState == null) throw new ArgumentNullException("initialState");
            if (states == null) throw new ArgumentNullException("states");

            _name = name;
            _context = context;
            _initialState = initialState;
            _states = new Dictionary<string, IState<TContext>>();

            foreach (IState<TContext> s in states)
            {
                if (s == null) throw new ArgumentException("Null state in collection.");
                _states[s.Name] = s;
            }

            if (!_states.ContainsKey(initialState))
                throw new ArgumentException("Initial state not found: " + initialState);
        }

        public string Name { get { return _name; } }
        public string InitialState { get { return _initialState; } }
        public string CurrentState { get { return _currentState; } }
        public bool IsCompleted { get { return _completed; } }
        public TContext Context { get { return _context; } }

        public event EventHandler<WorkflowEventArgs<TContext>> EnteredState;
        public event EventHandler<WorkflowEventArgs<TContext>> ExitedState;
        public event EventHandler<ActivityEventArgs<TContext>> ActivityExecuted;
        public event EventHandler<TransitionEventArgs<TContext>> TransitionEvaluated;
        public event EventHandler<TransitionEventArgs<TContext>> TransitionTaken;

        public void Start()
        {
            lock (_sync)
            {
                if (_started) return;
                _started = true;
                _currentState = _initialState;
                IState<TContext> state = _states[_currentState];

                RaiseEnteredState(_currentState);
                ExecuteActivities(state.OnEnter);
                if (state.IsTerminal) _completed = true;
            }
        }

        public void Step()
        {
            lock (_sync)
            {
                if (!_started) Start();
                if (_completed) return;

                IState<TContext> state = _states[_currentState];

                // Execute step activities
                ActivityResult last = ExecuteActivities(state.OnStep);

                // Evaluate transitions (first-true wins)
                ITransition<TContext> taken = null;
                foreach (ITransition<TContext> t in state.Transitions)
                {
                    bool ok = t.Guard(_context);
                    RaiseTransitionEvaluated(t, ok);
                    if (ok)
                    {
                        taken = t;
                        break;
                    }
                }

                if (taken != null)
                {
                    // Exit old state
                    ExecuteActivities(state.OnExit);
                    RaiseExitedState(state.Name);

                    // Move
                    _currentState = taken.ToState;
                    IState<TContext> next = _states[_currentState];

                    // Enter new state
                    RaiseTransitionTaken(taken, true);
                    RaiseEnteredState(next.Name);
                    ExecuteActivities(next.OnEnter);

                    if (next.IsTerminal)
                    {
                        _completed = true;
                    }
                }
                else
                {
                    // No transition: remain, continue next Step() call
                    // Optional: if last == Failure, decide policy externally.
                }
            }
        }

        public void RunToCompletion(int maxSteps)
        {
            if (maxSteps <= 0) throw new ArgumentOutOfRangeException("maxSteps");
            int steps = 0;
            while (!IsCompleted && steps < maxSteps)
            {
                Step();
                steps++;
            }
        }

        private ActivityResult ExecuteActivities(IList<IActivity<TContext>> activities)
        {
            ActivityResult last = ActivityResult.None;
            for (int i = 0; i < activities.Count; i++)
            {
                IActivity<TContext> a = activities[i];
                last = a.Execute(_context);
                RaiseActivityExecuted(_currentState, a, last);
                if (last == ActivityResult.Failure)
                {
                    // Policy: stop executing remaining activities in this phase
                    break;
                }
            }
            return last;
        }

        private void RaiseEnteredState(string state)
        {
            EventHandler<WorkflowEventArgs<TContext>> h = EnteredState;
            if (h != null) h(this, new WorkflowEventArgs<TContext>(state, _context));
        }

        private void RaiseExitedState(string state)
        {
            EventHandler<WorkflowEventArgs<TContext>> h = ExitedState;
            if (h != null) h(this, new WorkflowEventArgs<TContext>(state, _context));
        }

        private void RaiseActivityExecuted(string state, IActivity<TContext> a, ActivityResult r)
        {
            EventHandler<ActivityEventArgs<TContext>> h = ActivityExecuted;
            if (h != null) h(this, new ActivityEventArgs<TContext>(state, a, r, _context));
        }

        private void RaiseTransitionEvaluated(ITransition<TContext> t, bool ok)
        {
            EventHandler<TransitionEventArgs<TContext>> h = TransitionEvaluated;
            if (h != null) h(this, new TransitionEventArgs<TContext>(t, ok, _context));
        }

        private void RaiseTransitionTaken(ITransition<TContext> t, bool ok)
        {
            EventHandler<TransitionEventArgs<TContext>> h = TransitionTaken;
            if (h != null) h(this, new TransitionEventArgs<TContext>(t, ok, _context));
        }
    }
}