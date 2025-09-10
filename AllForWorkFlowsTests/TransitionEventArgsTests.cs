using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllForWorkFlows;
using System;

namespace AllForWorkFlows.Tests
{
    public class DummyTransitionEventArgsContext
    {
        public string Name;
    }

    public class DummyTransitionEventArgs : ITransition<DummyTransitionEventArgsContext>
    {
        public string Name { get { return "DummyTransitionEventArgs"; } }
        public string FromState { get { return "FromState"; } }
        public string ToState { get { return "ToState"; } }
        public TransitionGuard<DummyTransitionEventArgsContext> Guard
        {
            get { return delegate { return true; }; }
        }
    }

    [TestClass]
    public class TransitionEventArgsTests
    {
        [TestMethod]
        public void TransitionEventArgsTest()
        {
            // Arrange
            DummyTransitionEventArgsContext ctx = new DummyTransitionEventArgsContext { Name = "TestCtx" };
            DummyTransitionEventArgs transition = new DummyTransitionEventArgs();
            bool guardResult = true;

            // Act
            TransitionEventArgs<DummyTransitionEventArgsContext> args =
                new TransitionEventArgs<DummyTransitionEventArgsContext>(transition, guardResult, ctx);

            // Assert
            Assert.AreSame(transition, args.Transition, "Transition property should reference the same object passed in.");
            Assert.AreEqual(guardResult, args.GuardResult, "GuardResult property should match constructor argument.");
            Assert.AreSame(ctx, args.Context, "Context property should reference the same object passed in.");
        }
    }
}