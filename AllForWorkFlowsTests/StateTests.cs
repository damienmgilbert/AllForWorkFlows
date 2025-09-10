using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllForWorkFlows;
using System;
using System.Collections.Generic;

namespace AllForWorkFlows.Tests
{
    public class DummyStateContext
    {
        public string Message;
    }

    public class DummyStateActivity : IActivity<DummyStateContext>
    {
        public string Name { get { return "DummyStateActivity"; } }
        public ActivityResult Execute(DummyStateContext context)
        {
            context.Message = "Ran";
            return ActivityResult.Success;
        }
    }

    public class DummyStateTransition : ITransition<DummyStateContext>
    {
        public string Name { get { return "DummyStateTransition"; } }
        public string FromState { get { return "From"; } }
        public string ToState { get { return "To"; } }
        public TransitionGuard<DummyStateContext> Guard
        {
            get { return delegate { return true; }; }
        }
    }

    [TestClass]
    public class StateTests
    {
        [TestMethod]
        public void StateTest()
        {
            // Arrange
            string stateName = "TestState";
            bool isTerminal = true;

            // Act
            State<DummyStateContext> state = new State<DummyStateContext>(stateName, isTerminal);

            // Assert basic properties
            Assert.AreEqual(stateName, state.Name);
            Assert.AreEqual(isTerminal, state.IsTerminal);

            // Assert lists are initialized
            Assert.IsNotNull(state.OnEnter);
            Assert.IsNotNull(state.OnStep);
            Assert.IsNotNull(state.OnExit);
            Assert.IsNotNull(state.Transitions);

            // Add and verify activities
            DummyStateActivity activity = new DummyStateActivity();
            state.OnEnter.Add(activity);
            Assert.AreSame(activity, state.OnEnter[0]);

            // Add and verify transitions
            DummyStateTransition transition = new DummyStateTransition();
            state.Transitions.Add(transition);
            Assert.AreSame(transition, state.Transitions[0]);
        }
    }
}