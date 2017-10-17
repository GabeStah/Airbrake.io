# What is the Runtime Broker Application?

The **Runtime Broker** application is a Microsoft program included with Windows 8 (and newer versions) that handles permissions for all local [Universal Windows Platform](https://docs.microsoft.com/en-us/windows/uwp/index) (`UWP`) applications.  Normally, the `Runtime Broker` application is completely harmless and can be left alone to do its thing.  However, in some rare instances you may be experiencing slowdown on your computer, only to find that `RuntimeBroker.exe` is using an abundance of memory and/or an excess of CPU time.

In this article we'll explore exactly what the `Runtime Broker` application is, examining not only what it does, but why it might be affecting computer performance in some scenarios.  Let's get to it!

## What is Universal Windows Platform?

To understand the purpose of the `Runtime Broker` app we need to first briefly explore what the [Universal Windows Platform](https://docs.microsoft.com/en-us/windows/uwp/index) (`UWP`) is.  Put simply, the `UWP` is an application design and programming framework created by Microsoft that attempts to make it easy to create applications compatible with _any_ Windows-based device.  This includes not just desktop PCs, but also Windows phones and even the Xbox One gaming console.  To accomplish this, the `UWP` supports a wide variety of screen displays, creating dynamic applications using a common API and in-depth extension software development kits (`SDKs`).

Each `UWP` application must also includes a `Package.appxmanifest` file that informs the application how to be packaged up into a distributable format known as `AppX`.  This manifest configures a great deal about the application -- everything from the application name, supported screen configurations, visual assets, built-in functionality declarations, packaging requirements, and also the list of system features and devices the application should have access to.  This manifest is written in a simple `XML` file, a sample of which can be seen below for the `MyTestApplication` app we'll use in just a moment:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Package xmlns="http://schemas.microsoft.com/appx/manifest/foundation/windows10" xmlns:mp="http://schemas.microsoft.com/appx/2014/phone/manifest" xmlns:uap="http://schemas.microsoft.com/appx/manifest/uap/windows10" xmlns:uap3="http://schemas.microsoft.com/appx/manifest/uap/windows10/3" xmlns:uap4="http://schemas.microsoft.com/appx/manifest/uap/windows10/4" IgnorableNamespaces="uap mp uap3 uap4">
  <Identity Name="7e889c7f-ef55-45b0-bc9a-6e47eb8cb1d8" Publisher="CN=Gabe" Version="1.0.0.0" />
  <mp:PhoneIdentity PhoneProductId="7e889c7f-ef55-45b0-bc9a-6e47eb8cb1d8" PhonePublisherId="00000000-0000-0000-0000-000000000000" />
  <Properties>
    <DisplayName>MyTestApplication</DisplayName>
    <PublisherDisplayName>Gabe</PublisherDisplayName>
    <Logo>Assets\StoreLogo.png</Logo>
  </Properties>
  <Dependencies>
    <TargetDeviceFamily Name="Windows.Universal" MinVersion="10.0.0.0" MaxVersionTested="10.0.0.0" />
  </Dependencies>
  <Resources>
    <Resource Language="x-generate" />
  </Resources>
  <Applications>
    <Application Id="App" Executable="$targetnametoken$.exe" EntryPoint="MyTestApplication.App">
      <uap:VisualElements DisplayName="MyTestApplication" Square150x150Logo="Assets\Square150x150Logo.png" Square44x44Logo="Assets\Square44x44Logo.png" Description="MyTestApplication" BackgroundColor="transparent">
        <uap:DefaultTile Wide310x150Logo="Assets\Wide310x150Logo.png">
        </uap:DefaultTile>
        <uap:SplashScreen Image="Assets\SplashScreen.png" />
      </uap:VisualElements>
      <Extensions>
        <uap:Extension Category="windows.fileOpenPicker">
          <uap:FileOpenPicker>
            <uap:SupportedFileTypes>
              <uap:SupportsAnyFileType />
            </uap:SupportedFileTypes>
          </uap:FileOpenPicker>
        </uap:Extension>
      </Extensions>
    </Application>
  </Applications>
  <Capabilities>
    <Capability Name="internetClient" />
    <Capability Name="allJoyn" />
    <Capability Name="internetClientServer" />
    <Capability Name="privateNetworkClientServer" />
    <Capability Name="codeGeneration" />
    <uap:Capability Name="appointments" />
    <uap3:Capability Name="backgroundMediaPlayback" />
    <uap:Capability Name="enterpriseAuthentication" />
    <uap:Capability Name="phoneCall" />
    <uap:Capability Name="objects3D" />
    <uap4:Capability Name="offlineMapsManagement" />
    <uap:Capability Name="musicLibrary" />
    <uap:Capability Name="picturesLibrary" />
    <uap3:Capability Name="remoteSystem" />
    <uap:Capability Name="removableStorage" />
    <uap:Capability Name="voipCall" />
    <uap:Capability Name="videosLibrary" />
    <uap3:Capability Name="userNotificationListener" />
    <uap:Capability Name="userAccountInformation" />
    <uap:Capability Name="sharedUserCertificates" />
    <uap:Capability Name="contacts" />
    <uap:Capability Name="chat" />
    <uap:Capability Name="blockedChatMessages" />
    <DeviceCapability Name="bluetooth" />
    <DeviceCapability Name="location" />
    <DeviceCapability Name="microphone" />
    <DeviceCapability Name="pointOfService" />
    <DeviceCapability Name="proximity" />
    <DeviceCapability Name="webcam" />
  </Capabilities>
</Package>
```

The last feature of the app manifest file -- these system and device permissions -- are particularly important to the `Runtime Broker` application.  These `UWP` application `capabilities` are what `Runtime Broker` manages, ensuring that all `UWP` apps only have all appropriate permissions and necessary access to your local computer.  As it happens, the list of requested `capabilities` (permissions) of the `MyTestApplication` app can be seen in the final section of the XML above, in the `<Capabilities>` element.

## What Does Runtime Broker Monitor?

The purpose of `Runtime Broker` is to monitor and manage all permissions granted to running `UWP` applications.  If you've ever opened the `Windows Store`, or launched any `UWP` application from a built-in start menu "tile", chances are this application has used the `Runtime Broker` to verify its proper permissions and `capabilities`.

As such, the `Runtime Broker` application is _typically_ idling with no CPU usage and low memory usage (just a dozen or so megabytes).  However, in some scenarios, `Runtime Broker` may experience significant spikes in CPU and/or memory usage, as a result of a currently monitored or recently launched `UWP` application.

## Altering Runtime Broker's Performance

To illustrate how `Runtime Broker` might experience varying performance let's look at the simple `MyTestApplication` `UWP` app that we created.  We're using [`Visual Studio 2017`](https://www.visualstudio.com/downloads/) to create a new `UWP` application, which includes a few default files.  We already looked at our modified `Package.appxmanifest` XML file above, so the only other code we've altered from the default is in the `App.xaml.cs`:

```cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MyTestApplication
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private List<StorageFile> Files { get; } = new List<StorageFile>();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            // Infinite loop.
            while (true)
            {
                // Create file and get task.
                var task = CreateFile($"{Guid.NewGuid()}.txt");
                // Output task status with ticks timestamp.
                Debug.WriteLine($"{DateTimeOffset.Now.UtcTicks}: {task.Status}");              
                // Get file and add to list.
                Files.Add(task.Result);
            }
        }

        /// <summary>
        /// Create test file.
        /// </summary>
        /// <param name="name">File name with extension.</param>
        /// <param name="character">Character content to add to file.</param>
        /// <param name="characterCount">Times to repeat character.</param>
        /// <returns></returns>
        private static async Task<StorageFile> CreateFile(string name, char character = 'a', int characterCount = 100_000_000)
        {
            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(name);
            await FileIO.WriteTextAsync(file, new string(character, characterCount));
            return file;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
```

For testing purposes we've added the `CreateFile(string name, char character = 'a', int characterCount = 100_000_000)` method, which uses asynchronous methods to create a new text file in the application's temporary folder on our local drive.  It then writes a number of characters to the file to give it some content, then returns the resulting file within our infinite `while (true)` loop, adding said file to the `List<StorageFile> Files` class property.  We're just trying to give our application some behavior to repeat while it's running, to see if this impacts `Runtime Broker's` performance in anyway.

As shown above, we've also explicitly selected every single `capability` available to our application, which indicates that our application needs permissions and access to all sorts of local machine features and devices, from bluetooth and contacts to the music library and VOIP calling.  Since `Runtime Broker` handles the permissions for all these `capabilities`, we want to make sure our application requires as much work as possible from `Runtime Broker`.

With everything setup, we can test the performance of `Runtime Broker` by running a performance monitor of the active `RuntimeBroker.exe` process.  This allows us to track various performance metrics over time, including the current CPU usage of `RuntimeBroker.exe`.  Even for `unmanaged` code applications like this (which simply means an application that is running as an executable or `DLL`, rather than within Visual Studio) we can see from this performance report how much CPU time is being used.

While monitoring the performance of `Runtime Broker` we then launch the `MyTestApplication` app and look at the CPU usage:

Function Name | Total CPU (ms) | Total CPU (%) | Total of All CPU (%)
--- | --- | --- | ---
RuntimeBroker.exe (PID: 8032) | 1005 | 100.00 % | NA
  RuntimeBroker.exe!0x007ff7093114b5 | 14 | 1.39 % | 8.54 %
  RuntimeBroker.exe!0x007ff7093114db | 6 | 0.60 % | 3.76 %
  RuntimeBroker.exe!0x007ff709311983 | 4 | 0.40 % | 0.40 %
  RuntimeBroker.exe!0x007ff70931187c | 4 | 0.40 % | 0.40 %
  RuntimeBroker.exe!0x007ff709311a68 | 2 | 0.20 % | 0.20 %
  RuntimeBroker.exe!0x007ff709311941 | 2 | 0.20 % | 0.20 %
  RuntimeBroker.exe!0x007ff7093117e5 | 2 | 0.20 % | 0.20 %

Don't worry if this table doesn't make too much sense.  What's important is that the top row shows the `RuntimeBroker.exe` process that we're monitoring, and the total number of CPU cycles that were used during our monitoring.  Below that, each `!0x00...` entry is an internal function call that `RuntimeBroker.exe` executed, showing how many CPU cycles it uses.  Most importantly, the last column shows the `Total of All CPU (%)` used by each function during monitoring.  As we can see, in total the execution of `MyTestApplication` with all `capabilities` used approximately `13.5%` of all CPU usage during the brief period of time while it was initially launching.  This indicates that `Runtime Broker` is only performing permission checks when our `UWP` application first loads, which is logical since that's when it needs to grant or deny system access.

Interestingly, there seems to be some form of in-memory (or local) storage of recent permissions that `Runtime Broker` has already granted to an application.  We can conclude this because, if we shut down our `MyTestApplication` app and then relaunch it without making any changes or altering `Runtime Broker` in anyway, `Runtime Broker` doesn't seem to notice `MyTestApplication` or process anything at all.  Here's the performance report from relaunching `MyTestApplication` with all `capabilities` a second time:

Function Name | Total CPU (ms) | Total CPU (%) | Total of All CPU (%)
--- | --- | --- | ---
RuntimeBroker.exe (PID: 8032) | 2 | 100.00 % | NA

Note that the `100.00 %` isn't the amount of total CPU that was used.  Instead, we see absolutely no internal function calls executed by `RuntimeBroker.exe`, so it was just running its main threads and waiting for some event indicating that it should perform some processing.  Yet, again, for some reason it doesn't process the relaunching of `MyTestApplication` a second time, presumably because it's somehow keeping track of permissions in memory or a local file.  Nothing critical to note, but somewhat interesting.

By comparison, let's take a look at a proper and efficiently-written `UWP` application.  Below we can see the `RuntimeBroker.exe` CPU usage profile while launching the official `Netflix` `UWP` application:

Function Name | Total CPU (ms) | Total CPU (%) | Total of All CPU (%)
--- | --- | --- | ---
RuntimeBroker.exe (PID: 8032) |	47 | 100.00 % | NA
  RuntimeBroker.exe!0x007ff709311622 | 2 | 4.26 % | 3.25 %
  RuntimeBroker.exe!0x007ff7093114db | 2 | 4.26 % | 1.01 %

Likely because the Netflix app requires far fewer `capabilities` (permissions), there are far fewer internal function calls made.  Additionally, total CPU usage is much lower than we saw with `MyTestApplication`, clocking in around `4.25%` at peak CPU usage during initial load.

## Resolving Excessive Runtime Broker Memory/CPU Usage

While neither of our `UWP` test applications above produced any unexpected CPU or memory usage from `Runtime Broker`, it's entirely possible that you could experience such issues if your system is running an inefficient or buggy `UWP` application.  You can check `Runtime Broker's` performance by opening the `Task Manager` (`Ctrl + Shift + ESC`) and scrolling down to `Runtime Broker`.  If the CPU usage is consistently above ~1 - 5%, _or_ memory usage is frequently above ~100 MB, you may be experiencing a problem with an active `UWP` application.

The resolve this, the first thing to try is simply restarting your computer.  This will force `Runtime Broker` (and any associated applications) to reload, and may fix the problem.

Another potential fix was recommended by Reddit user `/u/owldyn` in [this post](https://www.reddit.com/r/Windows10/comments/3fe1lx/runtime_broker_cpu_usage_fix/):

- Press the `Start Menu` key, type `Settings`, and hit `Enter`.
- Open `System`.
- Select `Notifications & actions` on the left.
- Under the `Notifications` header, check if `Get tips, tricks, and suggestions as you use Windows` is selected.  If so, turn that option `Off`.  This may reportedly fix high CPU usage of `Runtime Broker`.

The only other potential resolution is to find the particular `UWP` application that is causing the trouble.  This can only really be done through guesswork and trial & error.  To see what `UWP` applications you have on your computer follow these steps (for Windows 10):

- Press the `Start Menu` key, type `Store`, and hit `Enter`.
- Click the ellipses (`...`) at the top right of the `Windows Store` application and select `My Library` from the dropdown.
- The list of applications under the topmost `Apps` category shows all the `UWP` applications currently installed on your system.
- Pick one of these listed applications you think might be the cause of your `Runtime Broker` issue and, if you know how, close it down manually.
- If you don't know how to close the application or it's not responding, open the `Task Manager` (`Ctrl + Shift + ESC`) and locate the application process here, then manually close it with `End Task`.
- Repeat until the problematic `UWP` application is located.

---

__META DESCRIPTION__

A close look at what the Runtime Broker application is, with examples of performance metrics across various UWP applications.