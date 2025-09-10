using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllForWorkFlows;
using System;
using System.Collections.Generic;

namespace AllForWorkFlows.Tests
{
    file class TestWorkflowContextData
    {
        public bool Flag;
        public string Message;
    }

    [TestClass]
    public class WorkflowBuilderTests
    {
        [TestMethod]
        public void WorkflowBuilderTest()
        {
            TestWorkflowContextData ctx = new TestWorkflowContextData();
            WorkflowBuilder<TestWorkflowContextData> builder = new WorkflowBuilder<TestWorkflowContextData>("TestFlow", ctx);

            Assert.IsNotNull(builder);
        }

        [TestMethod]
        public void StateTest()
        {
            TestWorkflowContextData ctx = new TestWorkflowContextData();
            WorkflowBuilder<TestWorkflowContextData> builder = new WorkflowBuilder<TestWorkflowContextData>("TestFlow", ctx);

            builder.State("S1", false);
            IWorkflow<TestWorkflowContextData> wf = builder.Build();

            Assert.AreEqual("S1", wf.InitialState);
        }

        [TestMethod]
        public void InitialTest()
        {
            TestWorkflowContextData ctx = new TestWorkflowContextData();
            WorkflowBuilder<TestWorkflowContextData> builder = new WorkflowBuilder<TestWorkflowContextData>("TestFlow", ctx);

            builder.State("S1", false)
                   .State("S2", false)
                   .Initial("S2");

            IWorkflow<TestWorkflowContextData> wf = builder.Build();

            Assert.AreEqual("S2", wf.InitialState);
        }

        [TestMethod]
        public void OnEnterTest()
        {
            TestWorkflowContextData ctx = new TestWorkflowContextData();
            WorkflowBuilder<TestWorkflowContextData> builder = new WorkflowBuilder<TestWorkflowContextData>("TestFlow", ctx);

            builder.State("S1", true)
                   .OnEnter("S1", new DelegateActivity<TestWorkflowContextData>(
                       "EnterActivity",
                       delegate (TestWorkflowContextData c) { c.Message = "Entered"; return ActivityResult.Success; }
                   ));

            IWorkflow<TestWorkflowContextData> wf = builder.Build();
            wf.Start();

            Assert.AreEqual("Entered", ctx.Message);
        }

        [TestMethod]
        public void OnStepTest()
        {
            TestWorkflowContextData ctx = new TestWorkflowContextData();
            WorkflowBuilder<TestWorkflowContextData> builder = new WorkflowBuilder<TestWorkflowContextData>("TestFlow", ctx);

            builder.State("S1", false) // not terminal
                   .OnStep("S1", new DelegateActivity<TestWorkflowContextData>(
                       "StepActivity",
                       delegate (TestWorkflowContextData c) { c.Message = "Stepped"; return ActivityResult.Success; }
                   ));

            IWorkflow<TestWorkflowContextData> wf = builder.Build();
            wf.Step(); // Start + Step will now run OnStep

            Assert.AreEqual("Stepped", ctx.Message);
        }

        [TestMethod]
        public void OnExitTest()
        {
            TestWorkflowContextData ctx = new TestWorkflowContextData();
            WorkflowBuilder<TestWorkflowContextData> builder = new WorkflowBuilder<TestWorkflowContextData>("TestFlow", ctx);

            builder.State("S1", false)
                   .State("S2", true)
                   .OnExit("S1", new DelegateActivity<TestWorkflowContextData>(
                       "ExitActivity",
                       delegate (TestWorkflowContextData c) { c.Message = "Exited"; return ActivityResult.Success; }
                   ))
                   .Transition("toS2", "S1", "S2", delegate (TestWorkflowContextData c) { return true; });

            IWorkflow<TestWorkflowContextData> wf = builder.Build();
            wf.Step(); // Start + Step triggers exit from S1 to S2

            Assert.AreEqual("Exited", ctx.Message);
        }

        [TestMethod]
        public void TransitionTest()
        {
            TestWorkflowContextData ctx = new TestWorkflowContextData();
            WorkflowBuilder<TestWorkflowContextData> builder = new WorkflowBuilder<TestWorkflowContextData>("TestFlow", ctx);

            builder.State("S1", false)
                   .State("S2", true)
                   .Transition("toS2", "S1", "S2", delegate (TestWorkflowContextData c) { return true; });

            IWorkflow<TestWorkflowContextData> wf = builder.Build();
            wf.Step();

            Assert.AreEqual("S2", wf.CurrentState);
        }

        [TestMethod]
        public void BuildTest()
        {
            TestWorkflowContextData ctx = new TestWorkflowContextData();
            WorkflowBuilder<TestWorkflowContextData> builder = new WorkflowBuilder<TestWorkflowContextData>("TestFlow", ctx);

            builder.State("S1", true);
            IWorkflow<TestWorkflowContextData> wf = builder.Build();

            Assert.IsNotNull(wf);
            Assert.AreEqual("S1", wf.InitialState);
            Assert.AreSame(ctx, wf.Context);
        }
    }
}