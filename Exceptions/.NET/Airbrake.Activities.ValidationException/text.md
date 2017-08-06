# .NET Exceptions - System.Activities.ValidationException

Moving along through our in-depth [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll dig deeper into the System.Activities.ValidationException.  The `System.Activities.ValidationException` is meant to be thrown when a Windows Workflow Foundation element, such as an `Activity` or `Workflow`, is in an invalid state.

In this article we'll briefly examine what the Windows Workflow Foundation is, and how the `System.Activities.ValidationException` can be used in such applications.  We'll also look at where the `System.Activities.ValidationException` fits in the larger .NET Exception hierarchy, along with a few functional C# code samples that illustrate how a Windows Workflow Foundation application might be created, so let's get started!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.Activities.ValidationException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

Before we get into the `ValidationException` itself let's take a moment to explore the Windows Workflow Foundation (`WWF`) on which it relies.  The [official documentation](https://docs.microsoft.com/en-us/dotnet/framework/windows-workflow-foundation/index) features a great deal of useful information and tutorials, but the basic purpose of the `WWF` is to make it relatively easy to create `workflows` that closely resemble or mirror real world processes.  Typically this is accomplished by creating sets of `activities` that indicate a particular action.  A workflow typically consists if a series of activities, all linked together through various logical means, to form the whole of the process.

The `WWF` is designed to make it easy to create and manage workflows within a visual editor.  This allows members of the sales team, for example, to create and manipulate activities and adjust basic logic of the workflow process.

That said, it's rather difficult to illustrate and explain the creation and visual manipulation of workflows within Visual Studio using only words and limited screen captures, so we'll stick to creating _coded_ examples of workflows, activities, and sequences.  Both the visual and programmatic form of workflows are completely valid, but it will be much easier to follow along if everything is written in code.

With that said, let's start with the full code sample below, after which we'll examine each section in more detail to see how it all works:

```cs
using System;
using System.Activities;
using System.Activities.Statements;
using System.Threading;
using Utility;
using Utility.Activities;

namespace Airbrake.Activities.ValidationException
{
    class Program
    {
        static void Main(string[] args)
        {
            Logging.LineSeparator("WORKFLOW EXAMPLE", 40, '=');
            CreateWorkflowSequence();

            Thread.Sleep(2000);

            Logging.LineSeparator("TRY-CATCH EXAMPLE", 40, '=');
            CreateTryCatchSequence();
        }

        /// <summary>
        /// Create a Sequence and execute in a WorkflowApplication.
        /// </summary>
        internal static void CreateWorkflowSequence()
        {
            // Create a new Sequence activity.
            Activity sequence = new Sequence
            {
                // Add Activities.
                Activities =
                {
                    // Output start of sequence.
                    new LoggerActivity
                    {
                        String = "Workflow sequence started.",
                        IsLineSeparator = true
                    },
                    // Throw a new InArgument<Exception> as new ValidationException.
                    new Throw
                    {
                        Exception = new InArgument<Exception>(env => new System.Activities.ValidationException("Uh oh, a ValidationException occurred in CreateWorkflowSequence()."))
                    },
                    // Output end of sequence.
                    new LoggerActivity
                    {
                        String = "Workflow sequence ended.",
                        IsLineSeparator = true
                    }
                }
            };

            // Create WorkflowApplication from sequence Activity.
            var workflowApplication = new WorkflowApplication(sequence)
            {
                // On UnhandledException event output Exception to log and terminate workflow.
                OnUnhandledException = delegate(WorkflowApplicationUnhandledExceptionEventArgs e)
                {
                    // Output expected UnhandledException.
                    Logging.Log(e.UnhandledException);

                    // Terminate the workflow.
                    return UnhandledExceptionAction.Terminate;
                },
            };

            // Run the workflow application.
            workflowApplication.Run();
        }

        /// <summary>
        /// Create a TryCatch Activity with underlying Sequence as Try action.
        /// </summary>
        internal static void CreateTryCatchSequence()
        {
            // Exception type InArgument for later use.
            var exception = new DelegateInArgument<Exception>()
            {
                Name = "Exception"
            };

            // ValidationException type InArgument for later use.
            var validationException = new DelegateInArgument<System.Activities.ValidationException>()
            {
                Name = "ValidationException"
            };

            // Establish a Try-Catch Activity.
            Activity tryCatchSequence = new TryCatch
            {
                // For Try Activity, create new Sequence.
                Try = new Sequence
                {
                    // Add Activities to Sequence.
                    Activities =
                    {
                        // Output sequence start.
                        new LoggerActivity
                        {
                            String = "Try-catch sequence started.",
                            IsLineSeparator = true
                        },
                        // Throw a new InArgument<Exception> as new ValidationException.
                        new Throw
                        {
                            Exception = new InArgument<Exception>(env => new System.Activities.ValidationException("Uh oh, a ValidationException occurred in CreateTryCatchSequence()."))
                        },
                        // Output sequence end.
                        new LoggerActivity
                        {
                            String = "Try-catch sequence ended.",
                            IsLineSeparator = true
                        }
                    }
                },
                // Specify catch blocks.
                Catches =
                {
                    // Catch ValidationExceptions.
                    new Catch<System.Activities.ValidationException>
                    {
                        // Action to take when ValidationException is caught.
                        Action = new ActivityAction<System.Activities.ValidationException>
                        {
                            // Pass local validationException instance as Argument.
                            Argument = validationException,
                            // Assign LoggerActivity.Exception to new InArgument<Exception> obtained from current ActivityContext.
                            Handler = new LoggerActivity
                            {
                                Exception = new InArgument<Exception>(env => validationException.Get(env))
                            }
                        }
                    },
                    // Catch Exception.
                    new Catch<Exception>
                    {
                        // Action to take when Exception is caught.
                        Action = new ActivityAction<Exception>
                        {
                            // Pass local exception instance as Argument.
                            Argument = exception,
                            // Assign LoggerActivity.Exception to new InArgument<Exception> obtained from current ActivityContext.
                            Handler = new LoggerActivity
                            {
                                Exception = new InArgument<Exception>(env => exception.Get(env))
                            }
                        }
                    }
                },
                // Ensure final Activity fires, outputting message to console.
                Finally = new LoggerActivity
                {
                    String = "Try-catch finally block.",
                    IsLineSeparator = true
                }
            };

            // Invoke tryCatchSequence.
            WorkflowInvoker.Invoke(tryCatchSequence);
            // Pause automatic termination of process to allow workflows to finish.
            Console.ReadLine();
        }

    }
}

// <Activities>/LoggerActivity.cs
using System;
using System.Activities;

namespace Utility.Activities
{
    /// <summary>
    /// Activity used for logging messages to console.
    /// </summary>
    public class LoggerActivity : CodeActivity
    {
        /// <summary>
        /// Argument for Exception output.
        /// </summary>
        public InArgument<Exception> Exception { get; set; }

        /// <summary>
        /// Determines if output is a line separator.
        /// </summary>
        public InArgument<bool> IsLineSeparator { get; set; }

        /// <summary>
        /// Argument for String output.
        /// </summary>
        public InArgument<string> String { get; set; }

        /// <inheritdoc />
        protected override void Execute(CodeActivityContext context)
        {
            // Output Exception.
            if (context.GetValue(Exception) != null)
            {
                Logging.Log(context.GetValue(Exception));
            }

            if (context.GetValue(String) == null) return;
            // Check if LineSeparator is set.
            if (context.GetValue(IsLineSeparator))
            {
                // Output String as LineSeparator insert.
                Logging.LineSeparator(context.GetValue(String));
            }
            else
            {
                // Output String.
                Logging.Log(context.GetValue(String));
            }
        }
    }
}

// <Utility>/Logging.cs
using System;
using System.Diagnostics;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        private const char SeparatorCharacterDefault = '-';
        private const int SeparatorLengthDefault = 40;

        /// <summary>
        /// Determines type of output to be generated.
        /// </summary>
        public enum OutputType
        {
            /// <summary>
            /// Default output.
            /// </summary>
            Default,
            /// <summary>
            /// Output includes timestamp prefix.
            /// </summary>
            Timestamp
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(string value, OutputType outputType = OutputType.Default)
        {
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }
        
        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(Exception exception, bool expected = true, OutputType outputType = OutputType.Default)
        {
            var value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception}: {exception.Message}";

            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(Object)"/>.
        /// 
        /// ObjectDumper: http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object&amp;lt;/cref
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(object value, OutputType outputType = OutputType.Default)
        {
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                : ObjectDumper.Dump(value));
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            Debug.WriteLine(new string(@char, length));
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>,
        /// with inserted text centered in the middle.
        /// </summary>
        /// <param name="insert">Inserted text to be centered.</param>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(string insert, int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            // Default output to insert.
            var output = insert;

            if (insert.Length < length)
            {
                // Update length based on insert length, less a space for margin.
                length -= (insert.Length + 2);
                // Halve the length and floor left side.
                var left = (int) Math.Floor((decimal) (length / 2));
                var right = left;
                // If odd number, add dropped remainder to right side.
                if (length % 2 != 0) right += 1;

                // Surround insert with separators.
                output = $"{new string(@char, left)} {insert} {new string(@char, right)}";
            }
            
            // Output.
            Debug.WriteLine(output);
        }
    }
}
```

---

Since our goal is to show how an exception like a `System.Activities.ValidationException` can be thrown, we have two different avenues by which we've programmatically created a workflow.  We start with the `CreateWorkflowSequence()` method, which starts by creating a new `Sequence` activity and adds three `Activities` to it, which will be executed in order.  The second `Activity` is a `Throw` object, which allows us to throw an exception, so here we're throwing a `System.Activities.ValidationException` as a new `InArgument<Exception>` object:

```cs
/// <summary>
/// Create a Sequence and execute in a WorkflowApplication.
/// </summary>
internal static void CreateWorkflowSequence()
{
    // Create a new Sequence activity.
    Activity sequence = new Sequence
    {
        // Add Activities.
        Activities =
        {
            // Output start of sequence.
            new LoggerActivity
            {
                String = "Workflow sequence started.",
                IsLineSeparator = true
            },
            // Throw a new InArgument<Exception> as new ValidationException.
            new Throw
            {
                Exception = new InArgument<Exception>(env => new System.Activities.ValidationException("Uh oh, a ValidationException occurred in CreateWorkflowSequence()."))
            },
            // Output end of sequence.
            new LoggerActivity
            {
                String = "Workflow sequence ended.",
                IsLineSeparator = true
            }
        }
    };

    // Create WorkflowApplication from sequence Activity.
    var workflowApplication = new WorkflowApplication(sequence)
    {
        // On UnhandledException event output Exception to log and terminate workflow.
        OnUnhandledException = delegate(WorkflowApplicationUnhandledExceptionEventArgs e)
        {
            // Output expected UnhandledException.
            Logging.Log(e.UnhandledException);

            // Terminate the workflow.
            return UnhandledExceptionAction.Terminate;
        },
    };

    // Run the workflow application.
    workflowApplication.Run();
}
```

With the `Sequence` created, we then need a way to handle the exception, so we've created a new `WorkflowApplication` instance and passed our `sequence` object as the primary `Activity` that will be executed.  Within the `WorkflowApplication` declaration we specify a delegate to handle unhandled exceptions.  In this case, we're just outputting the `Exception` using the `Logging.Log(Exception)` method, then terminating the workflow that caused the issue.

Keen observers may also have noticed the `Utility.Activities.LoggerActivity` object we were using as our first and last `Activity`.  This is a custom class that inherits from [`CodeActivity`](https://docs.microsoft.com/en-us/dotnet/api/system.activities.codeactivity?view=netframework-4.7), which can be used to create custom activities with virtually any code and logic we want:

```cs
// <Activities>/LoggerActivity.cs
using System;
using System.Activities;

namespace Utility.Activities
{
    /// <summary>
    /// Activity used for logging messages to console.
    /// </summary>
    public class LoggerActivity : CodeActivity
    {
        /// <summary>
        /// Argument for Exception output.
        /// </summary>
        public InArgument<Exception> Exception { get; set; }

        /// <summary>
        /// Determines if output is a line separator.
        /// </summary>
        public InArgument<bool> IsLineSeparator { get; set; }

        /// <summary>
        /// Argument for String output.
        /// </summary>
        public InArgument<string> String { get; set; }

        /// <inheritdoc />
        protected override void Execute(CodeActivityContext context)
        {
            // Output Exception.
            if (context.GetValue(Exception) != null)
            {
                Logging.Log(context.GetValue(Exception));
            }

            if (context.GetValue(String) == null) return;
            // Check if LineSeparator is set.
            if (context.GetValue(IsLineSeparator))
            {
                // Output String as LineSeparator insert.
                Logging.LineSeparator(context.GetValue(String));
            }
            else
            {
                // Output String.
                Logging.Log(context.GetValue(String));
            }
        }
    }
}
```

Nothing too fancy going on in this class, but by inheriting from `CodeActivity` we can `override` the `Execute(CodeActivityContext)` method, which is fired when the `Activity` executes.  With a combination of `InArgument<T>` properties, we're able to use various `Logging.Log()` method overloads to output the passed property values of this activity.  TLDR: `LoggerActivity` allows us to spruce up our output, as compared to just using a `new WriteLine` object as an `Activity`.

With all that setup, now we can call the `CreateWorkflowSequence()` method:

```cs
Logging.LineSeparator("WORKFLOW EXAMPLE", 40, '=');
CreateWorkflowSequence();
``` 

The expectation is that our custom `Sequence` activity workflow will execute by starting with the first `LoggerActivity` output message, then it will `throw` of a new `System.Activities.ValidationException`, which should be caught by the `OnUnhandledException` delegate that was specified in the `WorkflowApplication` declaration.  This should then output our error and terminate the workflow, which means our third and final activity (another `LoggerActivity`) will not execute.

Sure enough, running this code produces the following output:

```
=========== WORKFLOW EXAMPLE ===========
------ Workflow sequence started. ------
[EXPECTED] System.Activities.ValidationException: Uh oh, a ValidationException occurred in CreateWorkflowSequence().
   at System.Activities.Statements.Throw.Execute(CodeActivityContext context)
   at System.Activities.CodeActivity.InternalExecute(ActivityInstance instance, ActivityExecutor executor, BookmarkManager bookmarkManager)
   at System.Activities.ActivityInstance.Execute(ActivityExecutor executor, BookmarkManager bookmarkManager)
   at System.Activities.Runtime.ActivityExecutor.ExecuteActivityWorkItem.ExecuteBody(ActivityExecutor executor, BookmarkManager bookmarkManager, Location resultLocation): Uh oh, a ValidationException occurred in CreateTryCatchSequence().
```

Alright, so creating a handler for `UnhandledExceptions` in a `WorkflowApplication` is all well and good, but what if we want more control over how exceptions are handled?  What if we want to handle _specific_ exceptions?  One solution is to create a base activity for our workflow as a [`TryCatch`](https://docs.microsoft.com/en-us/dotnet/api/system.activities.statements.trycatch?view=netframework-4.7) object, which inherits from the `NativeActivity` and allows for explicit `catch` blocks to be specified in response to exceptions thrown during execution of the workflow.  The `CreateTryCatchSequence()` method shows how to create such an entity:

```cs
/// <summary>
/// Create a TryCatch Activity with underlying Sequence as Try action.
/// </summary>
internal static void CreateTryCatchSequence()
{
    // Exception type InArgument for later use.
    var exception = new DelegateInArgument<Exception>()
    {
        Name = "Exception"
    };

    // ValidationException type InArgument for later use.
    var validationException = new DelegateInArgument<System.Activities.ValidationException>()
    {
        Name = "ValidationException"
    };

    // Establish a Try-Catch Activity.
    Activity tryCatchSequence = new TryCatch
    {
        // For Try Activity, create new Sequence.
        Try = new Sequence
        {
            // Add Activities to Sequence.
            Activities =
            {
                // Output sequence start.
                new LoggerActivity
                {
                    String = "Try-catch sequence started.",
                    IsLineSeparator = true
                },
                // Throw a new InArgument<Exception> as new ValidationException.
                new Throw
                {
                    Exception = new InArgument<Exception>(env => new System.Activities.ValidationException("Uh oh, a ValidationException occurred in CreateTryCatchSequence()."))
                },
                // Output sequence end.
                new LoggerActivity
                {
                    String = "Try-catch sequence ended.",
                    IsLineSeparator = true
                }
            }
        },
        // Specify catch blocks.
        Catches =
        {
            // Catch ValidationExceptions.
            new Catch<System.Activities.ValidationException>
            {
                // Action to take when ValidationException is caught.
                Action = new ActivityAction<System.Activities.ValidationException>
                {
                    // Pass local validationException instance as Argument.
                    Argument = validationException,
                    // Assign LoggerActivity.Exception to new InArgument<Exception> obtained from current ActivityContext.
                    Handler = new LoggerActivity
                    {
                        Exception = new InArgument<Exception>(env => validationException.Get(env))
                    }
                }
            },
            // Catch Exception.
            new Catch<Exception>
            {
                // Action to take when Exception is caught.
                Action = new ActivityAction<Exception>
                {
                    // Pass local exception instance as Argument.
                    Argument = exception,
                    // Assign LoggerActivity.Exception to new InArgument<Exception> obtained from current ActivityContext.
                    Handler = new LoggerActivity
                    {
                        Exception = new InArgument<Exception>(env => exception.Get(env))
                    }
                }
            }
        },
        // Ensure final Activity fires, outputting message to console.
        Finally = new LoggerActivity
        {
            String = "Try-catch finally block.",
            IsLineSeparator = true
        }
    };

    // Invoke tryCatchSequence.
    WorkflowInvoker.Invoke(tryCatchSequence);
    // Pause automatic termination of process to allow workflows to finish.
    Console.ReadLine();
}
```

As you can see, we have a `TryCatch` object as our main `Activity`, which tries to create a `Sequence` object just as we saw before.  This also contains three different activities, the second of which throws a `System.Activities.ValidationException`.  The key difference is that the `TryCatch` object contains a `Catches` collection, where we can specify `new Catch<T>` elements that indicate what type of `Exception` should be caught, and what action to perform when doing so.  We have two different catches, so the `ActivityAction<T>` for each corresponds to the appropriate type of exception we're handling.

You may also notice that we instantiated a few local variables at the top of this method, which were `DelegateInArgument<T>` instances that we can then pass as `Arguments` to the `ActivityAction<T>` elements.  Doing so allows the `Handler` of each action to use that localized `Argument` object to capture the contextual exception that was caught.  Attempting to forgo the `Argument = validationException` assignment produces a `System.Activities.InvalidWorkflowException`, because the `ActivityAction<T>` has no _runtime context_ while it is executing.  This means it cannot access locally-scoped variables, like `validationException`, _unless_ it is provided as an `Argument`, giving it the local context it needs to be used by the `Handler` property.

Finally, we have a, well, `Finally` element, which we assign to a single `LoggerActivity` to provide another output message.  We finish it all off by invoking our `TryCatch` sequence with `WorkflowInvoker.Invoke()`.  As expected, executing this method results in the following output:

```
========== TRY-CATCH EXAMPLE ===========
----- Try-catch sequence started. ------
[EXPECTED] System.Activities.ValidationException: Uh oh, a ValidationException occurred in CreateTryCatchSequence().
   at System.Activities.Statements.Throw.Execute(CodeActivityContext context)
   at System.Activities.CodeActivity.InternalExecute(ActivityInstance instance, ActivityExecutor executor, BookmarkManager bookmarkManager)
   at System.Activities.ActivityInstance.Execute(ActivityExecutor executor, BookmarkManager bookmarkManager)
   at System.Activities.Runtime.ActivityExecutor.ExecuteActivityWorkItem.ExecuteBody(ActivityExecutor executor, BookmarkManager bookmarkManager, Location resultLocation): Uh oh, a ValidationException occurred in CreateTryCatchSequence().
------- Try-catch finally block. -------
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into System.Activities.ValidationException in .NET, including C# code showing how to create programmatic workflows and exception handling.