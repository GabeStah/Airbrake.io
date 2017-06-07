# Debugging With Visual Studio - Part 2

During the first article in our [How to Debug](https://airbrake.io/blog/debugging/debug-best-practices) series we went over a few generalized debugging best practices that are applicable to most platforms and debugging tools.  Next we dove into Visual Studio with [Debugging with Visual Studio - Part 1](https://airbrake.io/blog/debugging/debugging-with-visual-studio-part-1) and saw how Visual Studio handles code navigation, debugging windows, tooltips, and much more.

Today we'll finish up our examination of Visual Studio and the powerful debugging features it provides including remote debugging, working with 64-bit applications, targeting different .NET Framework versions, and much more!

## Remote Debugging

In addition to the typical localized debugging tools Visual Studio provides, it also supports a powerful `remote debugging` capability which allows you to debug applications deployed on different computers.  Visual Studio supports debugging for ASP.NET applications running on IIS systems, ASP.NET on Azure, or C#/C++ applications running on a Windows-based network.

To begin using remote debugging start by installing the `Remote Tools` package [appropriate to your particular Visual Studio version](https://docs.microsoft.com/en-us/visualstudio/debugger/remote-debugging-csharp).  Once installed you can setup a remote debugger configuration by specifying the type of connection and location of the remote machine, along with the appropriate user credentials to connect.  The final step is to configure your local Visual Studio project properties and insert the remote machine name into the `Debug > Use remote machine` field so your system knows where to make a connection.

Check out example configurations in more detail in the [official documentation](https://docs.microsoft.com/en-us/visualstudio/debugger/remote-debugging).

## Working With 64-Bit Applications

As surprising as it may be, even the latest versions of Visual Studio are not 64-bit applications and instead continue to use the tried-and-true 32-bit architecture we all know and love (or hate, depending on your perspective).  In mid-2016 the Visual Studio Product Team [even responded to requests from users](https://visualstudio.uservoice.com/forums/121579-visual-studio-2015/suggestions/2255687-make-vs-scalable-by-switching-to-64-bit) asking that they upgrade Visual Studio to a 64-bit application.  While there are many pros and cons to both 32- and 64-bit versions of Visual Studio, at the time of their response, the Visual Studio Team felt the return on investment of building, testing, and supporting two different architectures of the product wasn't worth the pain and complexity in making the switch.

All that said, even though Visual Studio _itself_ isn't a 64-bit application it certainly supports the _creation and debugging of_ 64-bit applications.  To debug a 64-bit application locally you just need to add a new `configuration` in the `Configuration Manager` with a 64-bit CPU specified:

1. Open `Build > Configuration Manager`.
2. Select (or create a `New`) `Active solution configuration`.
3. Click `New` under `Active solution platform` and select `x64` (or whatever appropriate 64-bit CPU option you have available).
4. Close the `Configuration Manager` then start debugging as normal using the new 64-bit configuration you created.

Visual Studio also allows remote 64-bit application debugging, which is setup and performed much the same way as normal remote debugging.

For more information check out the [64-Bit Application Debugging documentation](https://docs.microsoft.com/en-us/visualstudio/debugger/debug-64-bit-applications).

## Using Different .NET Framework Versions

While not always relevant to your project most modern versions of Visual Studio allow you to specify the particular .NET Framework version your project/application should use during debugging.  This can be useful for projects that are intended to be used by other developers, such as `.DLL` libraries and the like, allowing you to easily test compatibility of your application with older (or even newer) .NET Framework versions.

### Setting .NET Framework Version for New Projects

When creating a new project in newer versions of Visual Studio you can set the targeted .NET Framework version using the dropdown at the top of the `New Project` dialog.

### Changing .NET Framework Version for Existing Projects

Changing the .NET Framework version for an existing project requires a few more steps but is still quite simple:

1. Open the `Project > [ProjectName] Properties` window.
2. Select the `Application` tab.
3. Choose the appropriate .NET Framework version from the `Target framework` dropdown.

You may see a warning popup indicating that you may need to manually modify some project files to allow your application to build.  This is usually necessary when your project uses outside resources and references that target .NET Framework versions which differ from than version you're now specifying.  Once any appropriate manual changes are made your project will now build and run using the new .NET Framework version you've selected.

To learn more check out the [official documentation](https://docs.microsoft.com/en-us/visualstudio/debugger/how-to-specify-a-dotnet-framework-version-for-debugging).

## Altering Debugger Window Attributes

While not specific to using Visual Studio the .NET Framework provides the very powerful ability to extend code-specific metadata from directly within written source code using `attributes`.  We won't go into the full details of attributes and how to use them here, but the short explanation is that attributes allow you to add keyword-like declarations which help to annotate elements of your code such as `types`, `methods`, and `properties`.  When the code is compiled it is converted to an intermediate language (`MSIL`) which can then be parsed and read by tools like Visual Studio in order to evaluate that extra metadata during execution and debugging.  Check out a lot more info about general attribute use in .NET within the [docs](https://docs.microsoft.com/en-us/dotnet/standard/attributes/).

This ability to add and alter `attributes` in your code makes it easy to change the look, feel, and behavior of the data presented throughout the various debugger windows found throughout Visual Studio.  For example, the [`DebuggerTypeProxy`](https://docs.microsoft.com/en-us/visualstudio/debugger/using-debuggertypeproxy-attribute) attribute can be applied to `structures`, `classes`, and `assemblies`, allowing you to change the way that particular object is displayed in debugger windows.  We can also use the [`DebuggerDisplay`](https://docs.microsoft.com/en-us/visualstudio/debugger/using-the-debuggerdisplay-attribute) attribute, which directly changes how `objects`, `properties`, `fields`, and the like are displayed in debugger windows throughout Visual Studio.

For example we can modify our simple `Book` class below by adding those two attributes (seen here in `C#`):

```cs
[DebuggerTypeProxy(typeof(BookDebugView))]
[DebuggerDisplay("{_title} by {_author}")]
public class Book
{
    private string _title { get; set; }
    private string _author { get; set; }

    public Book(string title, string author)
    {
        _title = title;
        _author = author;
    }

    internal class BookDebugView
    {
        private Book _book;

        public String Author
        {
            get { return _book._author; }
            set { _book._author = value; }
        }

        public String Title
        {
            get { return _book._title; }
            set { _book._title = value; }
        }

        public BookDebugView(Book book)
        {
            _book = book;
        }
    }
}
```

In this simple example we basically want to override the default behavior by hiding the private properties of our `Book` class (`_author` and `_title`) in the drill-down display of debugger windows.  To accomplish this we create our `BookDebugView` class as an `internal` and give it a private `_book` property to hold our `Book` class instance, which is passed as the only parameter to the constructor.  We can then use our `public` `Author` and `Title` property methods as normal, which will act as our proxy replacement for the fields displayed in our debugger window.

The result is that all Visual Studio debugger windows now use the `BookDebugView` class as their base object type to display thanks to the `DebuggerTypeProxy` attribute.  Plus, we've used the `DebuggerDisplay` attribute to alter how the `value` of our `Book` objects is displayed in our debugger windows.  Thus, when create a new instance of `Book` named `book` and view it in a debugger window, it changes from the default of this:

| Name | Value |
| --- | --- |
| + book | {VisualStudio.Program.Book} |
| :wrench: _author | "Dr. Seuss" |
| :wrench: _title | "Green Eggs and Ham" |

to this:

| Name | Value |
| --- | --- |
| + book | "Green Eggs and Ham" by "Dr. Seuss" |
| :wrench: Author | "Dr. Seuss" |
| :wrench: Title | "Green Eggs and Ham" |
| + Raw View |  |

## Custom Data Visualizers

Visual Studio also includes powerful user interface components known as `visualizers`.  A visualizer typically creates a dialog box or other interface to display a value or object in a custom manner that is most appropriate based on that data type.  When debugging with Visual Studio you'll often see little magnifying glass icons which are the mechanism used to choose the visualizer you want to use to view that object.

By default Visual Studio comes with visualizers for `HTML`, `JSON`, `Text`, `XML`, and a handful of others.  However, Visual Studio makes it easy to create your own custom visualizers.  Visualizers are effectively just compiled `.DLL` files that are referenced in whatever project you want to use them in, so creating a custom visualizer begins with creating a new `Class Library` project.  For example, here's a very simple visualizer that targets objects of type `System.String` and creates a message box dialog popup with the string value when the visualizer is called:

```cs
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(StringDialogVisualizer.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(System.String),
Description = "String Dialog Visualizer")]
namespace StringDialogVisualizer
{
    public class DebuggerSide : DialogDebuggerVisualizer
    {
        override protected void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MessageBox.Show(objectProvider.GetObject().ToString());
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DebuggerSide));
            visualizerHost.ShowVisualizer();
        }
    }
}
```

Check out all the cool stuff you can do with visualizers in the [official documentation](https://docs.microsoft.com/en-us/visualstudio/debugger/create-custom-visualizers-of-data).

## Dump Files

If you've ever found yourself trying to recreate a bug reported by another user which you simply can't seem to replicate then you'll probably wish you had access to Visual Studio's dump file support.  For those unfamiliar with them a `dump file` is a snapshot of an entire application at the exact moment the dump was recorded.  It includes tons of metadata such as the processes and modules that were running, memory records, execution and stack traces, and much more.

With a dump file in hand you can open it in Visual Studio and be presented with the `Dump File Summary` screen which outlines the basic information about the state of the application when the dump was taken.  This includes stuff like the process name, exception codes, and the list of modules (`.DLL` and `.exes`) that were used by the application.

From there you can use Visual Studio to load up an exact recreation of the debugging process from the moment the dump was recorded.  If you have access to the source code (or symbol files therein) that are used by the application you'll be able to see exactly what line of code execution halted at when the dump was recorded, the values of local variables, and basically everything else you're used to viewing within the Visual Studio debugger.

Learn more in the [Using Dump Files docs](https://docs.microsoft.com/en-us/visualstudio/debugger/using-dump-files).

## IntelliTrace

The final debugging feature of Visual Studio we'll discuss is `IntelliTrace`.  IntelliTrace is an advanced feature available to professional-grade Visual Studio versions that essentially acts as a _recorder for debugging sessions_.  When you begin a debugging session IntelliTrace will monitor for all "important" events that take place and record said events.  These might include user interactions like clicking buttons, file IO access, database connections and executions, exceptions, and so forth.  

With IntelliTrace enabled during an active debugging session the `Diagnostic Tools` debugger window will feature a list of events that were tracked and recorded by IntelliTrace.  Again, these event types might include gestures by the user within the application interface, database queries, and so forth.  With the currently live debugging session paused you can then navigate through the event log and select a particular event you wish to view then click `Activate Historical Debugging`.  This will immediately enter `Historical Debugging` mode which shows your application code (along with all relevant local variables) in the state they were in at that specific moment of the accessed event.  

This incredibly powerful feature allows you to perform historical diagnostics during a debugging session so you can more easily investigate particular bugs or issues when you aren't sure exactly what event in the code might've caused them.

As usual, feel free to check out the [official documentation](https://docs.microsoft.com/en-us/visualstudio/debugger/intellitrace) for more information on IntelliTrace in Visual Studio.

---

__META DESCRIPTION__

A closer look at debugging with Visual Studio including remote debugging, 64-bit applications, .NET Framework versions, and more!

---

__SOURCES__

- https://docs.microsoft.com/en-us/visualstudio/debugger/index