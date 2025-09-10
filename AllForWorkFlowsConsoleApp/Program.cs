using AllForWorkFlows;
using AllForWorkFlowsConsoleApp;

OrderContext ctx = new OrderContext();

WorkflowBuilder<OrderContext> builder = new WorkflowBuilder<OrderContext>("OrderFlow", ctx);

builder.State("Created", false)
       .State("Validated", false)
       .State("Paid", false)
       .State("Completed", true)
       .Initial("Created");

builder.OnStep("Created", new DelegateActivity<OrderContext>(
    "Validate",
    delegate (OrderContext c) { c.IsValid = true; c.LastMessage = "Validated."; return ActivityResult.Success; }
));

builder.Transition("ToValidated", "Created", "Validated",
    delegate (OrderContext c) { return c.IsValid; });

builder.OnStep("Validated", new DelegateActivity<OrderContext>(
    "CapturePayment",
    delegate (OrderContext c) { c.PaymentCaptured = true; c.LastMessage = "Payment captured."; return ActivityResult.Success; }
));

builder.Transition("ToPaid", "Validated", "Paid",
    delegate (OrderContext c) { return c.PaymentCaptured; });

builder.Transition("ToCompleted", "Paid", "Completed",
    delegate (OrderContext c) { return true; });

IWorkflow<OrderContext> workflow = builder.Build();

workflow.EnteredState += delegate (object s, WorkflowEventArgs<OrderContext> e)
{
    Console.WriteLine("Entered: " + e.State);
};
workflow.TransitionTaken += delegate (object s, TransitionEventArgs<OrderContext> e)
{
    Console.WriteLine("Transition: " + e.Transition.Name);
};

workflow.RunToCompletion(100);

Console.WriteLine("Final State: " + workflow.CurrentState);
Console.WriteLine("Message: " + workflow.Context.LastMessage);

Console.ReadLine();