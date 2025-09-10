using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllForWorkFlows;
using System;

namespace AllForWorkFlows.Tests
{
    public class DummyDelegateActivityContext
    {
        public string Message;
    }

    [TestClass]
    public class DelegateActivityTests
    {
        [TestMethod]
        public void DelegateActivityTest()
        {
            // Arrange
            DummyDelegateActivityContext ctx = new DummyDelegateActivityContext();
            Converter<DummyDelegateActivityContext, ActivityResult> body =
                delegate (DummyDelegateActivityContext c) { return ActivityResult.Success; };

            // Act
            DelegateActivity<DummyDelegateActivityContext> activity =
                new DelegateActivity<DummyDelegateActivityContext>("TestActivity", body);

            // Assert
            Assert.AreEqual("TestActivity", activity.Name);
            Assert.AreEqual(ActivityResult.Success, activity.Execute(ctx));
        }

        [TestMethod]
        public void ExecuteTest()
        {
            // Arrange
            DummyDelegateActivityContext ctx = new DummyDelegateActivityContext();
            bool executed = false;

            Converter<DummyDelegateActivityContext, ActivityResult> body =
                delegate (DummyDelegateActivityContext c)
                {
                    executed = true;
                    c.Message = "Ran";
                    return ActivityResult.Failure;
                };

            DelegateActivity<DummyDelegateActivityContext> activity =
                new DelegateActivity<DummyDelegateActivityContext>("ExecActivity", body);

            // Act
            ActivityResult result = activity.Execute(ctx);

            // Assert
            Assert.IsTrue(executed, "Delegate should have been invoked.");
            Assert.AreEqual("Ran", ctx.Message, "Context should have been modified by delegate.");
            Assert.AreEqual(ActivityResult.Failure, result, "Result should match delegate return value.");
        }
    }
}