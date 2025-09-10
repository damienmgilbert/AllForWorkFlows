using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllForWorkFlows;
using System;
using System.Collections.Generic;

namespace AllForWorkFlows.Tests
{
    public class DummyWorkflowContext
    {
        public string Message;
        public bool Flag;
    }

    [TestClass]
    public class WorkflowTests
    {
        [TestMethod]
        public void WorkflowTest()
        {
            // Arrange
            DummyWorkflowContext ctx = new DummyWorkflowContext();
            State<DummyWorkflowContext> s1 = new State<DummyWorkflowContext>("S1", true);
            List<IState<DummyWorkflowContext>> states = new List<IState<DummyWorkflowContext>> { s1 };

            // Act
            Workflow<DummyWorkflowContext> wf = new Workflow<DummyWorkflowContext>("TestFlow", ctx, "S1", states);

            // Assert
            Assert.AreEqual("TestFlow", wf.Name);
            Assert.AreEqual("S1", wf.InitialState);
            Assert.AreSame(ctx, wf.Context);
            Assert.IsFalse(wf.IsCompleted);
        }

        [TestMethod]
        public void StartTest()
        {
            // Arrange
            DummyWorkflowContext ctx = new DummyWorkflowContext();
            State<DummyWorkflowContext> s1 = new State<DummyWorkflowContext>("S1", true);
            s1.OnEnter.Add(new DelegateActivity<DummyWorkflowContext>(
                "EnterActivity",
                delegate (DummyWorkflowContext c) { c.Message = "Entered"; return ActivityResult.Success; }
            ));
            List<IState<DummyWorkflowContext>> states = new List<IState<DummyWorkflowContext>> { s1 };
            Workflow<DummyWorkflowContext> wf = new Workflow<DummyWorkflowContext>("TestFlow", ctx, "S1", states);

            // Act
            wf.Start();

            // Assert
            Assert.AreEqual("Entered", ctx.Message);
            Assert.AreEqual("S1", wf.CurrentState);
            Assert.IsTrue(wf.IsCompleted); // terminal state
        }

        [TestMethod]
        public void StepTest()
        {
            // Arrange
            DummyWorkflowContext ctx = new DummyWorkflowContext();
            State<DummyWorkflowContext> s1 = new State<DummyWorkflowContext>("S1", false);
            State<DummyWorkflowContext> s2 = new State<DummyWorkflowContext>("S2", true);

            s1.OnStep.Add(new DelegateActivity<DummyWorkflowContext>(
                "StepActivity",
                delegate (DummyWorkflowContext c) { c.Message = "Stepped"; c.Flag = true; return ActivityResult.Success; }
            ));

            s1.Transitions.Add(new Transition<DummyWorkflowContext>(
                "toS2", "S1", "S2", delegate (DummyWorkflowContext c) { return c.Flag; }
            ));

            List<IState<DummyWorkflowContext>> states = new List<IState<DummyWorkflowContext>> { s1, s2 };
            Workflow<DummyWorkflowContext> wf = new Workflow<DummyWorkflowContext>("TestFlow", ctx, "S1", states);

            // Act
            wf.Step(); // Start + Step

            // Assert
            Assert.AreEqual("Stepped", ctx.Message);
            Assert.AreEqual("S2", wf.CurrentState);
            Assert.IsTrue(wf.IsCompleted);
        }

        [TestMethod]
        public void RunToCompletionTest()
        {
            // Arrange
            DummyWorkflowContext ctx = new DummyWorkflowContext();
            State<DummyWorkflowContext> s1 = new State<DummyWorkflowContext>("S1", false);
            State<DummyWorkflowContext> s2 = new State<DummyWorkflowContext>("S2", true);

            s1.OnStep.Add(new DelegateActivity<DummyWorkflowContext>(
                "StepActivity",
                delegate (DummyWorkflowContext c) { c.Flag = true; return ActivityResult.Success; }
            ));

            s1.Transitions.Add(new Transition<DummyWorkflowContext>(
                "toS2", "S1", "S2", delegate (DummyWorkflowContext c) { return c.Flag; }
            ));

            List<IState<DummyWorkflowContext>> states = new List<IState<DummyWorkflowContext>> { s1, s2 };
            Workflow<DummyWorkflowContext> wf = new Workflow<DummyWorkflowContext>("TestFlow", ctx, "S1", states);

            // Act
            wf.RunToCompletion(10);

            // Assert
            Assert.AreEqual("S2", wf.CurrentState);
            Assert.IsTrue(wf.IsCompleted);
        }
    }
}