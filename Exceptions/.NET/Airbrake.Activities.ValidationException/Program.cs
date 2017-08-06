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
