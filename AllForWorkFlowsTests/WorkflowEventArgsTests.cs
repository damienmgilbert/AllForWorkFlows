using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllForWorkFlows;
using System;

namespace AllForWorkFlows.Tests
{
    public class DummyWorkflowEventArgsContext
    {
        public string Name;
    }

    [TestClass]
    public class WorkflowEventArgsTests
    {
        [TestMethod]
        public void WorkflowEventArgsTest()
        {
            // Arrange
            string expectedState = "TestState";
            DummyWorkflowEventArgsContext expectedContext = new DummyWorkflowEventArgsContext { Name = "TestCtx" };

            // Act
            WorkflowEventArgs<DummyWorkflowEventArgsContext> args =
                new WorkflowEventArgs<DummyWorkflowEventArgsContext>(expectedState, expectedContext);

            // Assert
            Assert.AreEqual(expectedState, args.State, "State property should match constructor argument.");
            Assert.AreSame(expectedContext, args.Context, "Context property should reference the same object passed in.");
        }
    }
}