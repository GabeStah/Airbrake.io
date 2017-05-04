# How to Debug: Best Practices

Debugging is a critical part throughout every application development life cycle.  Debugging allows developers to recognize an exception has occurred, then proceed to systematically move through execution until the culprit code can be honed in on and eventually pinpointed.  Whether the fix requires fixing a minor typo or even an entire rewrite of a massive component in the system, the simple act of debugging allows developers to (eventually) solve nearly every issue, given enough time and manpower to do so.

However, as powerful a practice as debugging is, it can also be somewhat overwhelming.  With hundreds of active programming languages and frameworks used across dozens of code editors and integrated development environments (`IDEs`), knowing exactly how to get started with debugging for your project can be somewhat overwhelming at first.

Therefore, this series of articles attempts to answer the simple question of: **How to Debug?**.  In this first article we'll take a broader approach to that answer by providing a handful of fundamental tips and best practices that apply across numerous languages and development environments.  We'll then dig a bit deeper for future articles, looking in more detail at specific programming languages and code editing tools for additional tips and tricks.  Let's get debuggin'!

## Work the Steps

The first important distinction to make between the process of `debugging` and normal application execution is that debugging _allows for some form of additional interaction on your part as the developer._  When you normally execute your application, it runs on its own and executes all code by its own volition, based on the logic and instructions provided in the codebase, typically without the need for user interaction.

On the other hand, a crucial component of debugging is that ability for an individual to interact with the application _while it is executing_.  This `run-time` interaction opens up a number of useful features and functionalities that would normally be unavailable.

The most critical of these features provided by debugging is the ability to control the `flow of execution`.  Just as with a normal application, you can issue some basic commands to the application to get it to start and stop execution, but debugging allows for much more control over the execution through additional commands.

To illustrate, we'll use a very simple snippet of `C#` code:

```cs
DateTime birthday = new DateTime(2000, 1, 1);
int daysOld = (DateTime.Today - birthday).Days;
Logging.Log($"Age is {daysOld} days old.");
```

As with most applications this code would be executed line by line, in order from top to bottom.  Therefore, we'd first create a `birthday` `DateTime`, then get the difference in days between `DateTime.Today` and `birthday`, then finally output the result as the number of days old:

```
Age is 6332 days old.
```

For normal execution, this process occurs extremely quickly and begins when you issue the `start` command to the application, telling it to begin execution.  However, debugging introduces a few additional commands that allow you to halt execution and step through it line by line, if desired.  The rough list of debugging execution flow commands can be found below:

- Start
- Pause
- Continue
- Step Into
- Step Over
- Step Out
- Stop

`Start` and `stop` are pretty self-explanatory.  However, once we consider the ability to `pause` midway through execution is where things start to get interesting.  Consider our simple three-line application above, but as soon as we begin execution we hit `pause`.  When that occurs, the debugger process that is running in the background halts the current execution _at the exact line of code_ it was at when we issued that `pause` command.  Perhaps we halted execution on the second line of our application: `int daysOld = (DateTime.Today - birthday).Days;`.  This means that our application has already executed the first line, so we have a `DateTime` value called `birthday`, but the second and third line have not yet executed (meaning our `daysOld` variable is `null`).

Now we can start using the `step` capabilities of our debugger:

#### Step Into

Most applications consist of many lines of code that often make calls to other functions or methods.  A given method might have many lines of code to execute inside it, and it may even call additional methods or functions inside itself.  This process can continue ad nauseam, deeper and deeper through many levels of execution.  The `Step Into` command allows you to continue line-by-line execution while _traversing into_ all those sub-functions and sub-method calls.  In our case, if we are on line 2 of our application and use `Step Into`, we'd just move ahead to line 3.  However, issuing a `Step Into` on line 3 _steps into_ our `Logging.Log` method, which is part of a simple helper class to output information during debugging:

```cs
/// <summary>
/// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
/// otherwise uses standard <see cref="Console.WriteLine"/>.
/// </summary>
/// <param name="value">Value to be output to log.</param>
public static void Log(string value)
{
#if DEBUG
    Debug.WriteLine(value);
#else
    Console.WriteLine(value);
#endif
}
```

After a few more `Step Into` commands we'd traverse through all the executed lines of `Logging.Log`, then we'd jump back out to our calling main method before the application shuts down.

#### Step Over

While `Step Into` is the most detailed form of step command, digging into every single procedure call that the code makes and following it down the rabbit hole, the `Step Over` command _does not_ traverse inside method or function calls when it reaches them.  Instead, as the name implies, the `Step Over` command simply steps to the _next statement_ within the current procedure.  Thus, when we reach line 3 of our simple application above where there's a call to the `Logging.Log` method, a `Step Over` command will tell the application to continue execution (and will therefore run the `Logging.Log` call as normal), but it will do so behind the scenes, without stepping into that method via the debugger.

#### Step Out

Finally, `Step Out` takes this trend one step further (no pun intended).  When issuing a `Step Out` command, all remaining lines of the procedure that contains the current execution point are executed, after which the debugger jumps _out_ to the next statement that follows the procedure call that placed execution inside the inner call in the first place.  In other words, `Step Out` jumps outside of the current method or function, executing the rest of it automatically and without halting.

For example, if we're inside the `Logging.Log` method and we're sitting at the `Debug.WriteLine(value)` line, if we issue a `Step Out` command the application will immediately execute all remaining lines inside that calling procedure (`Logging.Log`), then it will halt execution at wait at the third line of our calling procedure: `Logging.Log($"Age is {daysOld} days old.");`.

## Break It Down With Breakpoints

`Steps` are all well and good, but when applications become any bigger than a few lines of code, halting execution at the proper spot becomes nearly impossible by trying to time the issuance of a `pause` command.  Luckily, in steps the `breakpoint.`

A `breakpoint` is an intentional marker placed at a particular line of code that instructs the debugger to temporarily suspend execution when it reaches that line of code (to `break` at that `point`).  In most code editors and IDEs, breakpoints can be placed by simply clicking on the left-hand gutter next to a line of code.  This usually creates a colored dot or other indicator that a breakpoint has been made.

Now, when executing the application in debug mode, execution will halt at the first line of code with an associated `breakpoint` on it.  From there, normal `step` commands can be used to traverse through the relevant sections of code that need inspection, rather than the whole of the application.

### To See What Condition My Condition Was In

As useful as `breakpoints` are, there are many scenarios where you only wish to halt execution at a specific breakpoint under certain circumstances.  For example, if you have a snippet of code that loops through thousands of objects and you need to debug that code for only one specific iteration of that loop, it can be very painstaking to sit there and `step` through that looped code thousands of times just to reach the single loop iteration you care about.

That's why most debuggers allow you to create `breakpoint conditions`.  A `condition` is usually just one or more `boolean` (`true/false`) statements, usually written in the code of the application in question, that is used to determine _when_ a particular breakpoint should trigger and halt execution.

For example, here we have two lines of code that use our `PizzaBuilder` class to create a tasty little pizza, then we output the resulting instance to the logger:

```cs
// Set Medium size, add Sauce, add Provolone cheese, add Pepperoni, add Olives, then build.
var pizza = new PizzaBuilder(Size.Medium)
                        .AddSauce()
                        .AddCheese(Cheese.Provolone)
                        .AddPepperoni()
                        .AddOlives()
                        .Build();
Logging.Log(pizza);
```

Perhaps we're creating a bunch of pizzas and we have a breakpoint on the `Logging.Log(pizza)` line, but we only want that breakpoint to trigger (and suspend execution) if our pizza has provolone cheese on it.  Thus, our `breakpoint condition` would contain this simple statement: `pizza.Cheese == Cheese.Provolone`

Now, when this breakpoint is reached during execution, our debugger will execute the statement above and confirm that our pizza _does_, in fact, contain provolone cheese.  Since this `condition` is true, our breakpoint triggers and the debugger halts at the `Logging.Log(pizza)` line, awaiting further commands from us.

## Get to Know the Locals

Stepping through your code line by line, or skipping around as desired, is a great feature, but it's not very useful if you cannot examine and evaluate how each line of executed code is altering values and states of objects.  This is where `locals` come into the debugging process.

For most debuggers, a `local` is simply a representation of a local variable (one that is defined within the local scope, such as the currently executing method or function).  Thus, the collection of `locals` is a full, automatically populated list of all local variables within the current execution scope at whatever line execution is suspended at.

For example, pausing execution at the end of our three-line age output method shows us the following list of `locals`:

Name | Value | Type
--- | --- | ---
args | {string[0]} | string[]
birthday | {1/1/2000 12:00:00 AM} | System.DateTime
daysOld | 6332 | int

### Inline Run-Time Inspection

While automatically generated `locals` are certainly useful to check on variable values and the like, most debugging tools provide `inline inspection`, which allows you to examine objects, variables, statements, and so forth directly inline while the debugger is running.  For example, let's suspend execution at this line: `Logging.Log($"Age is {daysOld} days old.");`.  The debugger I'm using allows me to simply mouse over the `daysOld` variable in that line and a tooltip immediately pops out showing me what the current value of `daysOld` is at this point in execution (`6332`).

## Night Gathers and Now My Watch Begins

In many cases while debugging, it may be necessary to evaluate a variable, object, or line of code that isn't actually being executed at the time (either it wasn't written at all in the code base, or maybe it's being executed elsewhere but you need to check it right now).  This is where the wonderful `watch` functionality comes to the rescue.

Most debuggers allow you to create `watches`, which are local code statements that are executed/evaluated automatically during the debugging process, every time execution steps to a new line of code.

For example, let's remind ourselves of our simple three-line program:

```cs
DateTime birthday = new DateTime(2000, 1, 1); // Break here.
int daysOld = (DateTime.Today - birthday).Days;
Logging.Log($"Age is {daysOld} days old.");
```

Let's start by suspending execution at the first line, so before anything has executed.  Then let's add a few new items to our `watch` list: `birthday`, `DateTime.Today`, `(DateTime.Today - birthday).Days`, and `daysOld`.

Name | Value | Type
--- | --- | ---
birthday | {1/1/0001 12:00:00 AM} | System.DateTime
DateTime.Today | {5/3/2017 12:00:00 AM} | System.DateTime
(DateTime.Today - birthday).Days  | 736451 | int
daysOld | 0 | int

A few important things to note here.  While not the case with all languages, for this example using `C#` and .NET, the common language runtime is smart enough to evaluate the `type` of objects by looking ahead at our code, even when it hasn't actually assigned those any values yet.  Thus, even though we haven't initialized our `birthday` variable yet, our code `declared` it already within the compiler, which automatically gives it a default value.  In this case, since `birthday` is a `System.DateTime`, its default value is `Midnight, January 1st of the year 1`.  A similar process of default values occurs for our other variables.

However, as you may have noticed we can add not just variables but full, executable code statements as a `watch`.  `(DateTime.Today - birthday).Days` is just such a statement.  It is executed and evaluated every time we break at a new line of code (that is, every time we execute a section of code).  Therefore, while the value is currently showing as some `736,451` days (since it's counting from year 1), once we `Step Over` to the second line of code, all our `watches` update automatically:

```
Name | Value | Type
--- | --- | ---
birthday | {1/1/2000 12:00:00 AM} | System.DateTime
DateTime.Today | {5/3/2017 12:00:00 AM} | System.DateTime
(DateTime.Today - birthday).Days  | 6332 | int
daysOld | 0 | int
```

Now that the first line has executed, `birthday` was changed to our intended date of `January 1st, 2000` and, therefore, our third `watch` statement also updated to a new value of `6332`.

## Profile Your Performance

## Don't Be Put Out By Output



---

__META DESCRIPTION__

---

__SOURCES__

- 