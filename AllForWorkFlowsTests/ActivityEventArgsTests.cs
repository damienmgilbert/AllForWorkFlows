using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllForWorkFlows;
using System;

namespace AllForWorkFlows.Tests
{
    public class DummyContextActivityEventArgs
    {
        public string Name;
    }

    public class DummyActivityEventArgs : IActivity<DummyContextActivityEventArgs>
    {
        public string Name { get { return "DummyActivityEventArgs"; } }

        public ActivityResult Execute(DummyContextActivityEventArgs context)
        {
            return ActivityResult.Success;
        }
    }

    [TestClass]
    public class ActivityEventArgsTests
    {
        [TestMethod]
        public void ActivityEventArgsTest()
        {
            // Arrange
            DummyContextActivityEventArgs ctx = new DummyContextActivityEventArgs { Name = "TestCtx" };
            DummyActivityEventArgs activity = new DummyActivityEventArgs();
            string stateName = "TestState";
            ActivityResult result = ActivityResult.Success;

            // Act
            ActivityEventArgs<DummyContextActivityEventArgs> args =
                new ActivityEventArgs<DummyContextActivityEventArgs>(stateName, activity, result, ctx);

            // Assert
            Assert.AreEqual(stateName, args.State, "State property should match constructor argument.");
            Assert.AreSame(activity, args.Activity, "Activity property should reference the same object passed in.");
            Assert.AreEqual(result, args.Result, "Result property should match constructor argument.");
            Assert.AreSame(ctx, args.Context, "Context property should reference the same object passed in.");
        }
    }
}