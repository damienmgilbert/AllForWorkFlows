using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllForWorkFlows;
using System;

namespace AllForWorkFlows.Tests
{
    public class DummyTransitionContext
    {
        public bool Flag;
    }

    [TestClass]
    public class TransitionTests
    {
        [TestMethod]
        public void TransitionTest()
        {
            // Arrange
            string name = "TestTransition";
            string from = "StateA";
            string to = "StateB";
            TransitionGuard<DummyTransitionContext> guard = delegate (DummyTransitionContext ctx) { return ctx.Flag; };

            // Act
            Transition<DummyTransitionContext> transition = new Transition<DummyTransitionContext>(name, from, to, guard);

            // Assert
            Assert.AreEqual(name, transition.Name);
            Assert.AreEqual(from, transition.FromState);
            Assert.AreEqual(to, transition.ToState);

            DummyTransitionContext ctxTrue = new DummyTransitionContext { Flag = true };
            DummyTransitionContext ctxFalse = new DummyTransitionContext { Flag = false };

            Assert.IsTrue(transition.Guard(ctxTrue), "Guard should return true when Flag is true.");
            Assert.IsFalse(transition.Guard(ctxFalse), "Guard should return false when Flag is false.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transition_NullName_Throws()
        {
            new Transition<DummyTransitionContext>(null, "From", "To", delegate { return true; });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transition_NullFromState_Throws()
        {
            new Transition<DummyTransitionContext>("Name", null, "To", delegate { return true; });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transition_NullToState_Throws()
        {
            new Transition<DummyTransitionContext>("Name", "From", null, delegate { return true; });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transition_NullGuard_Throws()
        {
            new Transition<DummyTransitionContext>("Name", "From", "To", null);
        }
    }
}