# .NET Exceptions - System.Deployment.Application.DeploymentException

We finish up the current run of our detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series with a dive into the **System.Deployment.Application.DeploymentException**.  The `DeploymentException` is actually a larger parent class to all exceptions that occur during deployment.  For example, using the common `ClickOnce` method of application deployment might run into some issues, and the exception(s) thrown during this process are all derived from `DeploymentException`.

In today's article we'll explore the `DeploymentException` in more detail by first looking at where it sits in the overall .NET exception hierarchy.  We'll also dig into some fully functional C# code samples that will illustrate how a common `ClickOnce` application deployment might work, and how attempting to automatically update that application from future releases could result in any number of `DeploymentExceptions` if you aren't careful.  Let's get to it!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - `DeploymentException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Windows.Forms;
using Utility;

namespace Airbrake.DeploymentException
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateApplication();
        }

        private void UpdateApplication()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return;

            var currentDeployment = ApplicationDeployment.CurrentDeployment;

            try
            {
                currentDeployment.CheckForUpdateCompleted += CheckForUpdateCompleted;
                currentDeployment.CheckForUpdateProgressChanged += CheckForUpdateProgressChanged;

                currentDeployment.CheckForUpdateAsync();
            }
            catch (DeploymentDownloadException dde)
            {
                MessageBox.Show(@"The application cannot check for the existence of a new version at this time. 

Please check your network connection, or try again later. Error: " + dde);
            }
            catch (InvalidDeploymentException ide)
            {
                MessageBox.Show(@"The application cannot check for an update. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show(@"This application cannot check for an update. This most often happens if the application is already in the process of updating. Error: " + ioe.Message);
            }
        }

        private void CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            downloadStatus.Text =
                $@"Downloading: {GetProgressString(e.State)}. {e.BytesCompleted / 1024:D}K of {e.BytesTotal / 1024:D}K downloaded.";
        }

        private string GetProgressString(DeploymentProgressState state)
        {
            switch (state)
            {
                case DeploymentProgressState.DownloadingApplicationFiles:
                    return "application files";
                case DeploymentProgressState.DownloadingApplicationInformation:
                    return "application manifest";
                case DeploymentProgressState.DownloadingDeploymentInformation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            return "deployment manifest";
        }

        private void CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(
$@"[{e.Error.GetType()}]:  Could not retrieve new version of the application. Message: 
" + e.Error.Message
                );
                // Output error to log.
                Logging.Log(e.Error);
                MessageBox.Show(e.Error.ToString());
                return;
            }
            if (e.Cancelled)
            {
                MessageBox.Show(@"The update was cancelled.");
            }

            // Check if update is available.
            if (!e.UpdateAvailable) return;

            // Ask the user if they would like to update the application now.
            if (!e.IsUpdateRequired)
            {
                var dialogResult = MessageBox.Show(
                    @"An update is available. Would you like to update the application now?",
                    @"Update Available",
                    MessageBoxButtons.OKCancel
                );

                if (DialogResult.OK == dialogResult)
                {
                    BeginUpdate();
                }
            }
            else
            {
                MessageBox.Show(
                    @"A mandatory update is available for your application. We will install the update now, after which we will save all of your in-progress data and restart your application."
                );
                BeginUpdate();
            }
        }

        private void BeginUpdate()
        {
            var currentDeployment = ApplicationDeployment.CurrentDeployment;
            currentDeployment.UpdateCompleted += UpdateCompleted;

            // Indicate progress in the application's status bar.
            currentDeployment.UpdateProgressChanged += UpdateProgressChanged;
            currentDeployment.UpdateAsync();
        }

        private void UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            var progressText =
                $"{e.BytesCompleted / 1024:D}K out of {e.BytesTotal / 1024:D}K downloaded - {e.ProgressPercentage:D}% complete";
            downloadStatus.Text = progressText;
        }

        private static void UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show(@"The update of the application's latest version was cancelled.");
                return;
            }
            if (e.Error != null)
            {
                MessageBox.Show(
@"ERROR: Could not install the latest version of the application. Reason: 
" + e.Error.Message + @"
Please report this error to the system administrator."
                );
                return;
            }

            var dialogResult = MessageBox.Show(
                @"The application has been updated. Restart? (If you do not restart now, the new version will not take effect until after you quit and launch the application again.)", 
                @"Restart Application", 
                MessageBoxButtons.OKCancel
            );

            if (DialogResult.OK == dialogResult)
            {
                Application.Restart();
            }
        }
    }
}
```

This code sample also uses the [`Logging.cs`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/csharp/Utility/Utility/Logging.cs) helper class, the full code of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/csharp/Utility/Utility/Logging.cs).

## When Should You Use It?

As discussed in the [official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.deployment.application.applicationdeployment?view=netframework-4.7.1), the .NET framework includes a handy `ApplicationDeployment` class, which allows an application to perform programmatic updates and file downloads.  A common technique for deploying .NET applications is using the [`ClickOnce`](https://docs.microsoft.com/en-us/dotnet/framework/winforms/clickonce-deployment-for-windows-forms) technology, which is designed to make deployment of Windows Forms applications as painless as possible.  The scope of using `ClickOnce` is well beyond the scope of this article, but suffice to say that most of the built-in functionalities of `ClickOnce` are usually good enough for most developing applications.

That said, sometimes you'll need more control over how your application is deployed and, more importantly, how it is updated when new versions are released.  For this purpose we'll look at a simple example from the official documentation showing how to programmatically check for and download application updates.

For this sample we've created the `Application.DeploymentException` Windows Forms application Visual Studio.  We've also added a `TextBox` control to `Form1` with the name `downloadStatus`.  Now let's take a look at the code in `Form1.cs`:

```cs
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        UpdateApplication();
    }

    private void UpdateApplication()
    {
        if (!ApplicationDeployment.IsNetworkDeployed) return;

        var currentDeployment = ApplicationDeployment.CurrentDeployment;

        try
        {
            currentDeployment.CheckForUpdateCompleted += CheckForUpdateCompleted;
            currentDeployment.CheckForUpdateProgressChanged += CheckForUpdateProgressChanged;

            currentDeployment.CheckForUpdateAsync();
        }
        catch (DeploymentDownloadException dde)
        {
            MessageBox.Show(@"The application cannot check for the existence of a new version at this time. 

Please check your network connection, or try again later. Error: " + dde);
        }
        catch (InvalidDeploymentException ide)
        {
            MessageBox.Show(@"The application cannot check for an update. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
        }
        catch (InvalidOperationException ioe)
        {
            MessageBox.Show(@"This application cannot check for an update. This most often happens if the application is already in the process of updating. Error: " + ioe.Message);
        }
    }

// ...
}
```

We begin with the `UpdateApplication()` method, which starts by checking if the application is configured as a network deployed application.  If so, we proceed to assign a few event delegates, then call the [`ApplicationDeployment.CheckForUpdateAsync()`](https://docs.microsoft.com/ru-ru/dotnet/api/system.deployment.application.applicationdeployment.checkforupdateasync?view=netframework-4.7.1) method, which checks the provided `UpdateLocation` for a new update.

The `CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)` method modifies the text of our the `downloadStatus` `TextBox` when progress of the download changes.  We also use the `GetProgressString(DeploymentProgressState state)` method to indicate _what_ is actually being downloaded at any given moment:

```cs
private void CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
{
    downloadStatus.Text =
        $@"Downloading: {GetProgressString(e.State)}. {e.BytesCompleted / 1024:D}K of {e.BytesTotal / 1024:D}K downloaded.";
}

private string GetProgressString(DeploymentProgressState state)
{
    switch (state)
    {
        case DeploymentProgressState.DownloadingApplicationFiles:
            return "application files";
        case DeploymentProgressState.DownloadingApplicationInformation:
            return "application manifest";
        case DeploymentProgressState.DownloadingDeploymentInformation:
            break;
        default:
            throw new ArgumentOutOfRangeException(nameof(state), state, null);
    }
    return "deployment manifest";
}
```

The `CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)` method is where most of the actual business logic takes place:

```cs
private void CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
{
    if (e.Error != null)
    {
        MessageBox.Show(
$@"[{e.Error.GetType()}]:  Could not retrieve new version of the application. Message: 
" + e.Error.Message
        );
        // Output error to log.
        Logging.Log(e.Error);
        MessageBox.Show(e.Error.ToString());
        return;
    }
    if (e.Cancelled)
    {
        MessageBox.Show(@"The update was cancelled.");
    }

    // Check if update is available.
    if (!e.UpdateAvailable) return;

    // Ask the user if they would like to update the application now.
    if (!e.IsUpdateRequired)
    {
        var dialogResult = MessageBox.Show(
            @"An update is available. Would you like to update the application now?",
            @"Update Available",
            MessageBoxButtons.OKCancel
        );

        if (DialogResult.OK == dialogResult)
        {
            BeginUpdate();
        }
    }
    else
    {
        MessageBox.Show(
            @"A mandatory update is available for your application. We will install the update now, after which we will save all of your in-progress data and restart your application."
        );
        BeginUpdate();
    }
}
```

Here we're determining if the actual update check event resulted in an error and outputting that, otherwise we process a valid update, if required.  Doing so invokes the `BeginUpdate()` custom method:

```cs
private void BeginUpdate()
{
    var currentDeployment = ApplicationDeployment.CurrentDeployment;
    currentDeployment.UpdateCompleted += UpdateCompleted;

    // Indicate progress in the application's status bar.
    currentDeployment.UpdateProgressChanged += UpdateProgressChanged;
    currentDeployment.UpdateAsync();
}
```

`BeginUpdate()` merely creates event delegations and invokes the `CurrentDeployment.UpdateAsync()` method, which performs the actual updating process.

```cs
private void UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
{
    var progressText =
        $"{e.BytesCompleted / 1024:D}K out of {e.BytesTotal / 1024:D}K downloaded - {e.ProgressPercentage:D}% complete";
    downloadStatus.Text = progressText;
}

private static void UpdateCompleted(object sender, AsyncCompletedEventArgs e)
{
    if (e.Cancelled)
    {
        MessageBox.Show(@"The update of the application's latest version was cancelled.");
        return;
    }
    if (e.Error != null)
    {
        MessageBox.Show(
@"ERROR: Could not install the latest version of the application. Reason: 
" + e.Error.Message + @"
Please report this error to the system administrator."
        );
        return;
    }

    var dialogResult = MessageBox.Show(
        @"The application has been updated. Restart? (If you do not restart now, the new version will not take effect until after you quit and launch the application again.)", 
        @"Restart Application", 
        MessageBoxButtons.OKCancel
    );

    if (DialogResult.OK == dialogResult)
    {
        Application.Restart();
    }
}
```

The `UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)` method performs another simple text change indicating the update progress status, while `UpdateCompleted(object sender, AsyncCompletedEventArgs e)` determines if the update was successful, and whether the user wishes to restart the application.

Alright cool.  Now the only thing left to do is to actually deploy our application and see what happens when we try to launch it.  Since we're explicitly performing an update check when the application loads, we'll immediately get feedback on a successful or failed update process:

```cs
private void Form1_Load(object sender, EventArgs e)
{
    UpdateApplication();
}
```

There are many ways to publish an application, but for this example we're just using the `Publish` dialog for the project in Visual Studio.  We'll be publishing to the local `publish\` directory, then we've specified the `Update location` to the UNC path of `\\localhost\invalid\`, which will cause the update process to look for the `Airbrake.DeploymentException.application` file in the invalid `\\localhost\invalid\` path.

With our application now published let's execute it and see what happens.  As you can probably guess, a `DeploymentException` is thrown -- specifically, the `DeploymentDownloadException` child class, indicating that the file could not be found at the specified location:

```
[System.Deployment.Application.DeploymentDownloadException]: Could not retrieve new version of the application.  Message: 
Downloading file:://localhost/invalid/Airbrake.DeploymentException.application did not succeed.
```

Not too surprising.  Now, let's try changing the `Update location` UNC path to match the local path where our `Airbrake.DeploymentException.application` file can be found (in the release directory).  Publishing again, then performing a follow-up release build (to force a new version for the update script do detect) results in a successful remote update:

```
The application has been updated. Restart? (If you do not restart now, the new version will not take effect until after you quit and launch the application again.)
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the DeploymentException in .NET, including sample code showing how to include programmatic updates within a Windows Form application.