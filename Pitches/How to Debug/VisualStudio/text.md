# How to Debug: Visual Studio - Part 1

In our first part of our [How to Debug](https://airbrake.io/blog/debugging/debug-best-practices) series we took a look at general best practices that apply to most debugging platforms and tools.  Now we're free to dig a bit deeper by looking at specific languages and tools.  This week we'll be examining tips for debugging with Microsoft's prominent `Visual Studio` IDE.  While there are many versions of Visual Studio on the market, it's always advisable to use the latest tools available whenever possible, so we'll be using the latest Visual Studio 2017 as the basis for all our examples throughout this article.

Since Visual Studio offers and incredible breadth and depth of debugging features and functionality, it's probably best that we just jump right in and get to exploring!

## Code Navigation

As we discussed in our [How to Debug: Best Practices](https://airbrake.io/blog/debugging/debug-best-practices) article, debuggers typically provide a means to halt execution and step through code one line at a time, giving you the ability to evaluate the process of the application's execution and see what's really going on.  Visual Studio is no exception and makes it easy to navigate through your code via buttons or hotkeys.  We won't go into too much detail since these concepts are fairly well-understood already (and covered in a [previous article](https://airbrake.io/blog/debugging/debug-best-practices) anyway), but at the very least it's worth learning the hotkeys you'll commonly be using in Visual Studio to handle execution during debugging.

Hotkey | Function
--- | ---
**F5** | Start Debugging (Prior to Execution)
**F5** | Continue (During Debugging)
**Shift + F5** | Stop Debugging
**Ctrl + Shift + F5** | Restart Debugging
**F10** | Step Over
**F11** | Step Into
**Shift + F11** | Step Out
**F9** | Toggle Breakpoint

While there are many other hotkeys that can be used during debugging, these are the primary functions you'll be repeatedly performing.

## Debugger Windows

Since Visual Studio is a full-blown integrated development environment (`IDE`) it offers a robust set of tools to make debugging easier.  Many of these tools are visual components, which are part of the Visual Studio user interface and they can be accessed through the `Debug > Windows` dropdown menu.  We'll just give a brief overview of each window and the purpose it serves here, but it's always a good idea to check out the [official documentation](https://docs.microsoft.com/en-us/visualstudio/debugger/debugger-windows) for more details.

- **Breakpoints**: Displays a convenient list of all current breakpoints, including their location, hit count, what action is taken when the breakpoint is hit, and much more.  Provides an easy way to customize breakpoint behavior in a simple window.
- **Exception Settings**: While Visual Studio features many intelligent default behaviors for debugging, the `Exception Settings` window gives you direct access to a full list of `Exceptions` that may be thrown during execution.  This window allows you to set what behavior should occur when a particular exception type is thrown, such as whether to break execution at that line.  You can also specify additional conditions used to trigger actions based on the particular exception type.
- **Output**: Displays any IDE outputs that Visual Studio needs to spit out during execution.
- **Show Diagnostic Tools**: Houses the basic, built-in diagnostic tools such as the CPU and memory profilers.
- **GPU Threads**: For performance-heavy applications such as games, the `GPU Threads` window provides a way to examine and manipulate threads that are running on the GPU, as opposed to the normal CPU threads.
- **Tasks**: For applications that use `System.Threading.Tasks` and employ a [`Task-based Asynchronous Pattern`](https://msdn.microsoft.com/en-us/library/hh873175(v=vs.110).aspx), the `Tasks` window is an easy way to interact with the currently active list of tasks during debugging.
- **Parallel Stacks**: For multi-threaded applications the `Parallel Stacks` window provides a diagrammed view of all active threads (or tasks), including call stacks for each thread to make it easy to trace what's going on where.
- **Parallel Watch**: A bit like the normal `Watch` window, the `Parallel Watch` window allows you to simultaneously track variables and expressions across multiple threads.
- **Watch**: A convenient way to see how variables and expression values change during debugging.
- **Autos**: Automatically populated with variables used around the current line of execution. 
- **Locals**: Automatically populated with currently-scoped variables as you navigate through the code. 
- **Immediate**: This window basically acts as an interactive console, allowing you to enter expressions and other code to be executed immediately, even while actively debugging.
- **Live Visual Tree**: For WPF-style applications that rely on XAML the `Live Visual Tree` window lets you examine XAML code in real-time.
- **Live Property Explorer**: Also for WPF-style applications, the `Live Property Explorer` allows you to view XAML properties in real-time.
- **Call Stack**: An easy way to see the current call stack while paused during debug execution.
- **Threads**: A visual view for viewing and interacting with all active threads during execution.
- **Modules**: Displays all `DLL` and `EXE` files used by your application.
- **Processes**: Displays all active processes that are attached to your application or the debugger itself.
- **Memory**: A table view of memory space used by your application.  This can be useful for evaluating larger or cumbersome data sets that do not work well inside normal watch windows, or for evaluating specific memory addresses during execution.
- **Disassembly**: Shows assembly code that is created by the compiler, including memory addresses, derived source code, symbol names, and more.
- **Registers**: Displays a list of the current register contents, which can viewed in real-time during debugging code navigation.

## Just My Code

When debugging an application it is typically the case that you'll only want to debug your own code, ignoring all other third-party module or .NET framework calls.  Debugging just your own written code in this manner is the default behavior of Visual Studio and is referred to as the `Just My Code` debugging option.

For example, here we've created a simple `Book` class and created a new instance of it.  We've then called a third-party module class (`Logging`) to output some information about our new object instance:

```cs
class Program
{
    static void Main(string[] args)
    {
        Book book = new Book("Green Eggs and Ham", "Dr. Seuss");
        Logging.Log(book);
    }
}

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }
}
```

Using the default `Just In Time` option during debugging, if we're paused at the `Logging.Log(book)` call and press `F11` to issue a `Step Into` command, the debugger will merely ignore that `Step Into` command and will issue a `Step Over` command instead, moving to the next line within our own code automatically.  This is because the `Logging.Log()` method call is not _our code_ -- it is from a third-party `DLL` reference within our application.  This means that we don't have direct access to the source code, so we cannot inspect the source code of such method calls.  This behavior is also performed for all .NET Framework calls by default, ignoring and skipping over them entirely.

However, Visual Studio provides a means to actually debug such third-party code, such as modules or the .NET Framework.  To do this simply uncheck the `Just My Code` option.  You'll also need access to the relevant [`symbol files`](https://docs.microsoft.com/en-us/visualstudio/debugger/specify-symbol-dot-pdb-and-source-files-in-the-visual-studio-debugger), typically stored in `.pdb` files, which act as a source code map, mapping between identifiers within the actual source code to the identifiers used in the compiled executable created during debugging.  This `source mapping` technique is common in many languages and can be a handy way to dig deeper into third-party libraries and the like when trying to really figure out the cause of a particular bug.

## Attach a Process

Even though the application you're currently working on is the most common target for your debugging efforts, in some cases you may wish to debug a different application, such as a remote app or something not originally developed in Visual Studio.  Once you've begun the debug process yourself, you can then begin debugging another application by selecting the `Debug > Attach to Process` dropdown command.

A new window will appear with the list of local processes, but you can also connect to other remote target systems as well and attach to running processes therein.  Just select the process you wish to attach to and off you go!

## Working with Console Applications

While windows-based or web-based applications have a very obvious method of stopping the application, such as closing the main window or shutting down the web server, most console applications developed in Visual Studio may not have any built-in means that requires outside input before completing execution.  Thus, it is common when debugging console applications to launch the application with the debugger, then have execution immediately complete, closing the debugger once again before you even have a chance to halt execution and start the debugging process.

Therefore, when debugging console applications it's a good idea to make sure execution will halt at least once so you can begin debugging as intended.  This might be from setting an explicit break point where you know you need one, or by right-clicking somewhere in the code and selecting the `Run to Cursor` option, which will cause the debugger to start and execute up until that line, where it will pause and await your command.

## DataTips

As the name implies, debugging in Visual Studio can be, well, quite _visual_.  One feature that really helps you visualize what's going on during debugging is known as `DataTips`.  These are simply little tooltips that pop out when debug execution is halted and you mouse over any currently-scoped variable.  This will display the value of the variable in question and allow you to drill down as deep as you wish if the variable is a complex object of some sort.  Simply removing the cursor from the variable in code will hide the DataTip.

While that default behavior is extremely useful, you can go even further by `pinning` and `commenting` on DataTips.  Simply click the pin icon on the side of a DataTip to pin that tip inline next to the code.  This will create a constant display of that variable's DataTip during all future debug executions, behaving a bit like an inline form of a variable `Watch`.

You can even add comments to pinned DataTips, which are a great way to make notes about particular variables or snippets of code from one debugging execution to the next.

## Customizing Native Object Views

There may be some cases where you are viewing a variable in a debugger window within Visual Studio, but a particular field you're interested in is extremely difficult to drill down to and find because the parent object housing that field is so complex.  It can be time-consuming and frustrating to halt debugging and then have to dig through a series of a dozen or more obtuse parent objects in the debugger window just to finally get to the field you care about.

Thankfully, Visual Studio provides the [`Natvis`](https://docs.microsoft.com/en-us/visualstudio/debugger/create-custom-views-of-native-objects) framework, which is an XML-based syntax that allows you to define unique, custom visualization rules for any data type you wish.  This means you can alter the behavior of Visual Studio's debugger windows for that type, making that critical field you were looking for just one level down rather than dozens or more.

## Edit and Continue

Another great feature is the ability to make edits to your code while debug execution is halted, then continue execution as before, with the newly edited code being interwoven just as if it were there when execution began.  This can save a great deal of time since you don't need to stop execution and recompile every time you want to make a minor change.

While most changes can be made without actually stopping debug execution, there are a few restrictions that will apply depending on the language you're using.  We won't go into the details here, but feel free to check out the [official documentation](https://docs.microsoft.com/en-us/visualstudio/debugger/supported-code-changes-csharp) for more details.

---

We've covered a good chunk of the major features which Visual Studio brings to the debugging process.  Stay tuned for Part 2 in which we finish up with Visual Studio!

---

__META DESCRIPTION__

A closer look at the debugging capabilities of the Visual Studio IDE, including navigation, Just My Code, debugger windows, multi-threaded support, and more!

---

__SOURCES__

- https://docs.microsoft.com/en-us/visualstudio/debugger/index