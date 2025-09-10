# AllForWorkFlows

**AllForWorkFlows** is a lightweight, dependency‑free **.NET 2.0** workflow engine base library.  
It provides reusable, extensible primitives for building **state‑driven workflows** with clear lifecycle hooks, guarded transitions, and deterministic execution — all while staying **DRY**, **SOLID**, and **KISS**.

---

## ✨ Features

- **State‑based execution** — Define named states with enter, step, and exit activities.
- **Guarded transitions** — Move between states only when conditions are met.
- **Lifecycle events** — Hook into `EnteredState`, `ExitedState`, `ActivityExecuted`, `TransitionEvaluated`, and `TransitionTaken`.
- **Extensible activities** — Implement `IActivity<TContext>` or use `DelegateActivity<TContext>` for inline logic.
- **Thread‑safe engine** — Safe to call `Start()`, `Step()`, and `RunToCompletion()` from multiple threads.
- **.NET 2.0 compatible** — No LINQ, no async, no extension methods.

---

## 📦 Installation

Copy the `AllForWorkFlows` source files into your project or compile them into a separate class library.  
No external dependencies are required.

---

## 🧩 Core Concepts

| Concept | Description |
|---------|-------------|
| **Workflow** | The engine that runs states and transitions for a given context. |
| **State** | A named stage in the workflow with `OnEnter`, `OnStep`, and `OnExit` activities. |
| **Activity** | A unit of work that operates on the workflow context and returns an `ActivityResult`. |
| **Transition** | A guarded link from one state to another. The guard decides if the transition is taken. |
| **Context** | Your domain object that carries data through the workflow. |

---

## 🚀 Quick Start

### 1. Define your context
```csharp
public class OrderContext
{
    public bool IsValid;
    public bool PaymentCaptured;
    public string LastMessage;
}

### 2. Build your workflow
```csharp
using AllForWorkFlows;

OrderContext ctx = new OrderContext();

WorkflowBuilder<OrderContext> builder = new WorkflowBuilder<OrderContext>("OrderFlow", ctx);

builder.State("Created", false)
       .State("Validated", false)
       .State("Paid", false)
       .State("Completed", true)
       .Initial("Created");

builder.OnStep("Created", new DelegateActivity<OrderContext>(
    "Validate",
    delegate(OrderContext c) { c.IsValid = true; c.LastMessage = "Validated."; return ActivityResult.Success; }
));

builder.Transition("ToValidated", "Created", "Validated",
    delegate(OrderContext c) { return c.IsValid; });

builder.OnStep("Validated", new DelegateActivity<OrderContext>(
    "CapturePayment",
    delegate(OrderContext c) { c.PaymentCaptured = true; c.LastMessage = "Payment captured."; return ActivityResult.Success; }
));

builder.Transition("ToPaid", "Validated", "Paid",
    delegate(OrderContext c) { return c.PaymentCaptured; });

builder.Transition("ToCompleted", "Paid", "Completed",
    delegate(OrderContext c) { return true; });

IWorkflow<OrderContext> workflow = builder.Build();
```

### 3. Subscribe to events (optional)
```csharp
workflow.EnteredState += delegate(object s, WorkflowEventArgs<OrderContext> e)
{
    Console.WriteLine("Entered: " + e.State);
};
workflow.TransitionTaken += delegate(object s, TransitionEventArgs<OrderContext> e)
{
    Console.WriteLine("Transition: " + e.Transition.Name);
};
```

### 4. Run the workflow
```csharp
workflow.RunToCompletion(100);
Console.WriteLine("Final State: " + workflow.CurrentState);
Console.WriteLine("Message: " + workflow.Context.LastMessage);
````

## 📡 Lifecycle
1. Start() — Sets the current state to the initial state and runs OnEnter activities.

2. Step() — Runs OnStep activities, evaluates transitions, and moves to the next state if a guard passes.

3. RunToCompletion(maxSteps) — Repeatedly calls Step() until a terminal state is reached or the step limit is hit.

## 🛠 Extending
* Custom Activities — Implement IActivity<TContext> for reusable logic.

* Custom Guards — Write TransitionGuard<TContext> delegates to control flow.

* Persistence — Save CurrentState and Context between runs.

* Visualization — Walk the state/transition graph to generate DOT/GraphViz diagrams.

## 📜 License
This library is provided as‑is, with no warranty. You are free to use, modify, and distribute it in your own projects.