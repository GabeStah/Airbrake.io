# .NET Exceptions - System.ComponentModel.LicenseException

Winding down our in-depth [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we're taking a look at the **System.ComponentModel.LicenseException**.  When creating proprietary applications and .NET components intended to be licensed, you may wish to implement some form of licensing using the built-in [`LicenseProvider`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.licenseprovider?view=netframework-4.7.1) class, which makes it relatively easy to add licensed content to your application.  If your application is executed and no valid license is detected a `LicenseException` is thrown to prompt an indication to the user or developer that licensing must be satisfied.

Throughout this article we'll explore the `LicenseException` by looking at where it sits in the massive .NET exception hierarchy.  We'll also look at some functional C# code samples that show a few different techniques for implementing licensing into your own applications, and how failing to do so can (and should) throw `LicenseExceptions`, so let's get started!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - `LicenseException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using System.Windows.Forms;
using Microsoft.Win32;
using Utility;

namespace Airbrake.ComponentModel.LicenseException
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form1();

            Logging.LineSeparator("Creating LicensedComboBox", 60);
            new LicensedComboBox();

            // Delete registry license entry.
            DeleteLicenseRegistryEntry(typeof(RegistryLicensedComboBox));

            Logging.LineSeparator("Creating RegistryLicensedComboBox", 60);
            new RegistryLicensedComboBox();

            // Add registry license entry.
            AddLicenseRegistryEntry(typeof(RegistryLicensedComboBox));

            Logging.LineSeparator("Creating RegistryLicensedComboBox w/ Registry Entry", 60);
            new RegistryLicensedComboBox();

            Application.Run(form);
        }

        /// <summary>
        /// Adds license registry entry for passed control type.
        /// </summary>
        /// <param name="type">Control type to create license registry of.</param>
        private static void AddLicenseRegistryEntry(Type type)
        {
            var license = new CustomLicense(type);

            // Check if key exists.
            var registryKey = Registry.CurrentUser.OpenSubKey(license.RegistryKey);
            if (registryKey != null) return;

            // Set key value to license key.
            var newKey = Registry.CurrentUser.CreateSubKey(license.RegistryKey);
            newKey?.SetValue(type.Name, license.LicenseKey);
            Logging.Log($"Added registry key value: {Registry.CurrentUser}\\{license.RegistryKey}\\{type.Name} = {license.LicenseKey}.");
        }

        /// <summary>
        /// Deletes license registry entry for passed control type.
        /// </summary>
        /// <param name="type">Control type to delete license registry of.</param>
        private static void DeleteLicenseRegistryEntry(Type type)
        {
            var license = new CustomLicense(type);

            // Check if key exists.
            var registryKey = Registry.CurrentUser.OpenSubKey(license.RegistryKey);
            if (registryKey == null) return;

            // Delete key.
            Registry.CurrentUser.DeleteSubKey(license.RegistryKey);
            Logging.Log($"Deleted registry key: {Registry.CurrentUser}\\{license.RegistryKey}.");
        }
    }
}
```

```cs
using System;
using System.ComponentModel;

namespace Airbrake.ComponentModel.LicenseException
{
    /// <summary>
    /// CustomLicense used for RegostryLicenseProvider example.
    /// </summary>
    public class CustomLicense : License
    {
        private readonly Type _type;

        public CustomLicense(Type type)
        {
            // Type is required and must not be null.
            _type = type ?? throw new NullReferenceException("The licensed type reference may not be null.");
        }

        public override void Dispose() { }

        /// <summary>
        /// Returns an explicit license key string for demonstration.
        /// </summary>
        public override string LicenseKey => "12345";

        /// <summary>
        /// The registry key where the license can be found.
        /// </summary>
        public string RegistryKey => "Software\\Airbrake\\Licenses";
    }
}
```

```cs
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Utility;

namespace Airbrake.ComponentModel.LicenseException
{
    [LicenseProvider(typeof(LicFileLicenseProvider))]
    public class LicensedComboBox : ComboBox
    {
        private License _license;

        public LicensedComboBox()
        {
            try
            {
                // Validate the license of this instance.
                _license = LicenseManager.Validate(typeof(LicensedComboBox), this);
                Logging.LineSeparator("VALID LicensedComboBox LICENSE", 60);
                Logging.Log(_license);
            }
            catch (System.ComponentModel.LicenseException exception)
            {
                // Output expected LicenseExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_license == null) return;

            _license.Dispose();
            _license = null;
        }
    }
}
```

```cs
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Utility;

namespace Airbrake.ComponentModel.LicenseException
{
    [LicenseProvider(typeof(RegistryLicenseProvider))]
    public class RegistryLicensedComboBox : ComboBox
    {
        private License _license;

        public RegistryLicensedComboBox()
        {
            try
            {
                // Validate the license of this instance.
                _license = LicenseManager.Validate(typeof(RegistryLicensedComboBox), this);
                Logging.LineSeparator("VALID RegistryLicensedComboBox LICENSE", 60);
                Logging.Log(_license);
            }
            catch (System.ComponentModel.LicenseException exception)
            {
                // Output expected LicenseExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_license == null) return;

            _license.Dispose();
            _license = null;
        }
    }
}
```

```cs
using System;
using System.ComponentModel;
using Microsoft.Win32;

namespace Airbrake.ComponentModel.LicenseException
{
    public class RegistryLicenseProvider : LicenseProvider
    {
        /// <summary>
        /// Attempts to retrieve a valid license for the passed object instance.
        /// </summary>
        /// <param name="context">Context of this license indicating if it should be checked in runtime or compile time.</param>
        /// <param name="type">Object type to retrieve license for.</param>
        /// <param name="instance">Instance to check license of.</param>
        /// <param name="allowExceptions">Determines if exceptions should be allowed.  If true, a LicenseException
        /// is thrown if no valid license can be obtained.</param>
        /// <returns>Valid CustomLicense instance.</returns>
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            var license = new CustomLicense(type);

            // If we're not in runtime mode license should be automatically approved.
            if (context.UsageMode != LicenseUsageMode.Runtime) return license;

            // Get the intended registry key.
            var registryKey = Registry.CurrentUser.OpenSubKey(license.RegistryKey);

            // Get the registry key value.
            var registryKeyValue = (string) registryKey?.GetValue(type.Name);

            if (registryKeyValue != null)
            {
                // Confirm that the keyValue from registry is equivalent to expected LicenseKey.
                if (string.CompareOrdinal(license.LicenseKey, registryKeyValue) == 0)
                {
                    // All verification successful, so return the valid license.
                    return license;
                }
            }

            // If exceptions are allowed, return a LicenseException indicating that the license is not valid.
            if (allowExceptions)
            {
                throw new System.ComponentModel.LicenseException(type, instance, $"The license of this {instance.GetType().Name} control is invalid.");
            }

            return null;
        }
    }
}
```

This code sample also uses the [`Logging.cs`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/csharp/Utility/Utility/Logging.cs) helper class, the full code of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/csharp/Utility/Utility/Logging.cs).

## When Should You Use It?

Since the `LicenseException` is thrown when an instantiated `Control` was unable to locate a valid license that it requires, we'll start right by looking at how application licensing works in .NET.  Unfortunately there isn't a great deal of easily-accessible official documentation on licensing, but the basics are not too complicated.  To start, .NET binds a license to just about any `Type` of object you want to.  This is accomplished by passing an `object` instance to the [`LicenseManager.Validate(Type type, object instance)`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.licensemanager.validate?view=netframework-4.7.1) method.  However, for most applications a license will be tied to an object that inherits from the `System.Windows.Forms.Control` class.  This ensure that the "main" `Form` of `Control` of an application can be checked for a valid license and react accordingly.  However, as we'll see in the sample code, we can license any type of object we wish.

The standard method for licensing is to apply the `LicenseProvider` attribute for the [`LicFileLicenseProvider`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.licfilelicenseprovider?view=netframework-4.7.1) type to the class you wish to license.  To illustrate, below we've created the custom `LicensedComboBox` class, which inherits from the built-in `ComboBox` control class:

```cs
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Utility;

namespace Airbrake.ComponentModel.LicenseException
{
    [LicenseProvider(typeof(LicFileLicenseProvider))]
    public class LicensedComboBox : ComboBox
    {
        private License _license;

        public LicensedComboBox()
        {
            try
            {
                // Validate the license of this instance.
                _license = LicenseManager.Validate(typeof(LicensedComboBox), this);
                Logging.LineSeparator("VALID LicensedComboBox LICENSE", 60);
                Logging.Log(_license);
            }
            catch (System.ComponentModel.LicenseException exception)
            {
                // Output expected LicenseExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_license == null) return;

            _license.Dispose();
            _license = null;
        }
    }
}
```

The only requirement for implementing a license check in our `LicensedComboBox` class is to perform the `LicenseManager.Validate(Type type, object instance)` check somewhere, then be sure to dispose of the license when a class instance is `Diposed`.  As you can see in the first line within our `try` block, we're attempting to `Validate(...)` the current instance (`this`) using the type that matches this instance (`LicensedComboBox`).

Let's try creating a new `LicensedComboBox` instance and see what happens when our constructor attempts to `Validate(...)` a license for this object:

```cs
[STAThread]
private static void Main()
{
    // ...

    Logging.LineSeparator("Creating LicensedComboBox", 60);
    new LicensedComboBox();
    
    // ...
}
```

Running the code above throws a `LicenseException`, as seen below:

```
---------------- Creating LicensedComboBox -----------------
[EXPECTED] System.ComponentModel.LicenseException: An instance of type 'Airbrake.ComponentModel.LicenseException.LicensedComboBox' was being created, and a valid license could not be granted for the type 'Airbrake.ComponentModel.LicenseException.LicensedComboBox'. Please,  contact the manufacturer of the component for more information.
```

As it happens, the default way that .NET attempts to _locate_ an actual license is by looking in the base directory of the executable for a license file with the following name format: `{AssemblyNamespace}.{ClassName}.lic`.  Thus, in our case our default license file name is `Airbrake.ComponentModel.LicenseException.LicensedComboBox.lic`.  Since we don't have any such file, we got the `LicenseException` seen above.

To remedy this we've manually created the `Airbrake.ComponentModel.LicenseException.LicensedComboBox.lic` file in our Visual Studio project and made sure it copies to the output directory during the build process.  However, the mere _existence_ of such a license file is not enough.  By default, .NET will look at the content of the `.lic` file for the following _exact_ keyword string:

```
{AssemblyNamespace}.{ClassName} is a licensed component.
```

So, in our case we'll add the following string to our `.lic` file:

```
Airbrake.ComponentModel.LicenseException.LicensedComboBox is a licensed component.
```

Let's try instantiating a `LicensedComboBox` again, now that our `.lic` file is in place:

```cs
Logging.LineSeparator("Creating LicensedComboBox", 60);
new LicensedComboBox();
```

Running this again now properly validates our license and produces the following output, which includes the generated `License` instance:

```
---------------- Creating LicensedComboBox -----------------
-------------- VALID LicensedComboBox LICENSE --------------
{System.ComponentModel.LicFileLicenseProvider+LicFileLicense(HashCode:36849274)}
  LicenseKey: "Airbrake.ComponentModel.LicenseException.LicensedComboBox is a licensed component."
```

Cool!  However, the default method of licensing by sticking a special string in a properly-named `.lic` file is a bit basic.  We could obviously obfuscate the produced key in some way so it cannot be manually created, but let's take a look at another technique that is commonly used for licensing: modifying the Windows registry.

This gets a fair bit more complicated because we now need to implement our own custom `License` and `LicenseProvider` classes.  The former allows us to modify what the valid `LicenseKey` value is and how it is checked, while the latter allows us to modify how the local license is retrieved.  Let's start with our `CustomLicense` class that inherits `License`:

```cs
using System;
using System.ComponentModel;

namespace Airbrake.ComponentModel.LicenseException
{
    /// <summary>
    /// CustomLicense used for RegistryLicenseProvider example.
    /// </summary>
    public class CustomLicense : License
    {
        private readonly Type _type;

        public CustomLicense(Type type)
        {
            // Type is required and must not be null.
            _type = type ?? throw new NullReferenceException("The licensed type reference may not be null.");
        }

        public override void Dispose() { }

        /// <summary>
        /// Returns an explicit license key string for demonstration.
        /// </summary>
        public override string LicenseKey => "12345";

        /// <summary>
        /// The registry key where the license can be found.
        /// </summary>
        public string RegistryKey => "Software\\Airbrake\\Licenses";
    }
}
```

Nothing fancy going on here, but using a custom `License` implementation allows us to override certain methods, like `LicenseKey`.  In this case we just have it return a constant value of `12345`, but we could certainly complicate things by retrieving this value from a third-party source like our licensing web service.  We've also specified the base `RegistryKey` path that we'll be using for this license.

The custom `LicenseProvider` is called `RegistryLicenseProvider` for us:

```cs
using System;
using System.ComponentModel;
using Microsoft.Win32;

namespace Airbrake.ComponentModel.LicenseException
{
    public class RegistryLicenseProvider : LicenseProvider
    {
        /// <summary>
        /// Attempts to retrieve a valid license for the passed object instance.
        /// </summary>
        /// <param name="context">Context of this license indicating if it should be checked in runtime or compile time.</param>
        /// <param name="type">Object type to retrieve license for.</param>
        /// <param name="instance">Instance to check license of.</param>
        /// <param name="allowExceptions">Determines if exceptions should be allowed.  If true, a LicenseException
        /// is thrown if no valid license can be obtained.</param>
        /// <returns>Valid CustomLicense instance.</returns>
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            var license = new CustomLicense(type);

            // If we're not in runtime mode license should be automatically approved.
            if (context.UsageMode != LicenseUsageMode.Runtime) return license;

            // Get the intended registry key.
            var registryKey = Registry.CurrentUser.OpenSubKey(license.RegistryKey);

            // Get the registry key value.
            var registryKeyValue = (string) registryKey?.GetValue(type.Name);

            if (registryKeyValue != null)
            {
                // Confirm that the keyValue from registry is equivalent to expected LicenseKey.
                if (string.CompareOrdinal(license.LicenseKey, registryKeyValue) == 0)
                {
                    // All verification successful, so return the valid license.
                    return license;
                }
            }

            // If exceptions are allowed, return a LicenseException indicating that the license is not valid.
            if (allowExceptions)
            {
                throw new System.ComponentModel.LicenseException(type, instance, $"The license of this {instance.GetType().Name} control is invalid.");
            }

            return null;
        }
    }
}
```

As you can see, all we're really doing here is overriding the `GetLicense(...)` method, which attempts to retrieve and return a valid `License` instance based on the passed parameters.  The most important of these parameters is the `Type type`, which we pass into the constructor call of our `CustomLicense` class.  Otherwise, our basic logic is to check if the target registry key exists and, if so, if the `LicenseKey` value matches the value in the registry at that key.  If so, we return a valid `CustomLicense` instance, otherwise we throw a `LicenseException` to indicate that the license could not be validated.

Our `RegistryLicensedComboBox` control is similar to the `LicensedComboBox` version, expect we're using the `typeof(RegistryLicenseProvider)` within the `LicenseProvider(...)` attribute call:

```cs
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Utility;

namespace Airbrake.ComponentModel.LicenseException
{
    [LicenseProvider(typeof(RegistryLicenseProvider))]
    public class RegistryLicensedComboBox : ComboBox
    {
        private License _license;

        public RegistryLicensedComboBox()
        {
            try
            {
                // Validate the license of this instance.
                _license = LicenseManager.Validate(typeof(RegistryLicensedComboBox), this);
                Logging.LineSeparator("VALID RegistryLicensedComboBox LICENSE", 60);
                Logging.Log(_license);
            }
            catch (System.ComponentModel.LicenseException exception)
            {
                // Output expected LicenseExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_license == null) return;

            _license.Dispose();
            _license = null;
        }
    }
}
```

Before we test this out, we probably want a programmatic way to add/remove the appropriate registry values for testing, so we don't have to manually edit the registry each time.  To accomplish this we have two helper methods, starting with `AddLicenseRegistryEntry(Type type)`:

```cs
/// <summary>
/// Adds license registry entry for passed control type.
/// </summary>
/// <param name="type">Control type to create license registry of.</param>
private static void AddLicenseRegistryEntry(Type type)
{
    var license = new CustomLicense(type);

    // Check if key exists.
    var registryKey = Registry.CurrentUser.OpenSubKey(license.RegistryKey);
    if (registryKey != null) return;

    // Set key value to license key.
    var newKey = Registry.CurrentUser.CreateSubKey(license.RegistryKey);
    newKey?.SetValue(type.Name, license.LicenseKey);
    Logging.Log($"Added registry key value: {Registry.CurrentUser}\\{license.RegistryKey}\\{type.Name} = {license.LicenseKey}.");
}
```

This creates a new `CustomLicense` instance of the passed `Type type`, then checks if the intended `RegistryKey` path exists.  If it doesn't exist, that sub-key path is created, then a new value with the passed `Type type` name and the `license.LicenseKey` value is added.

We also want to be able to remove this entry, so the `DeleteLicenseRegistryEntry(Type type)` method handles that:

```cs
/// <summary>
/// Deletes license registry entry for passed control type.
/// </summary>
/// <param name="type">Control type to delete license registry of.</param>
private static void DeleteLicenseRegistryEntry(Type type)
{
    var license = new CustomLicense(type);

    // Check if key exists.
    var registryKey = Registry.CurrentUser.OpenSubKey(license.RegistryKey);
    if (registryKey == null) return;

    // Delete key.
    Registry.CurrentUser.DeleteSubKey(license.RegistryKey);
    Logging.Log($"Deleted registry key: {Registry.CurrentUser}\\{license.RegistryKey}.");
}
```

Alright.  With everything setup we can test things out by creating a new `RegistryLicensedComboBox` instance and seeing what happens.  We begin by ensuring our registry entry doesn't exist, then create a new combo box:

```cs
// Delete registry license entry.
DeleteLicenseRegistryEntry(typeof(RegistryLicensedComboBox));

Logging.LineSeparator("Creating RegistryLicensedComboBox", 60);
new RegistryLicensedComboBox();
```

As you might suspect, without the proper registry value in place we catch a `LicenseException`, which includes our custom message:

```
------------ Creating RegistryLicensedComboBox -------------
[EXPECTED] System.ComponentModel.LicenseException: The license of this RegistryLicensedComboBox control is invalid.
```

Let's try that again, but we'll first add the license value to the registry:

```cs
// Add registry license entry.
AddLicenseRegistryEntry(typeof(RegistryLicensedComboBox));

Logging.LineSeparator("Creating RegistryLicensedComboBox w/ Registry Entry", 60);
new RegistryLicensedComboBox();
```

This time everything works as intended.  As the output confirms, we successfully added the new registry key and value for our `RegistryLicensedComboBox` entry.  We were then able to confirm the `LicenseKey's` expected value of `12345` against the registry entry that matches, which causes the `LicenseManager.Validate(...)` method call to return a valid `CustomLicense` instance, as also seen in the log:

```
Added registry key value: HKEY_CURRENT_USER\Software\Airbrake\Licenses\RegistryLicensedComboBox = 12345.
--- Creating RegistryLicensedComboBox w/ Registry Entry ----
---------- VALID RegistryLicensedComboBox LICENSE ----------
{Airbrake.ComponentModel.LicenseException.CustomLicense(HashCode:19575591)}
  LicenseKey: "12345"
  RegistryKey: "Software\Airbrake\Licenses"
```

There we have it!  That's a brief rundown of how component licensing works in .NET, but there's obviously a great deal more that can be added on to make licensing your own applications as robust and tamper-proof as you see fit.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the LicenseException in .NET, including functional C# code samples showing how to create both default and registry-based licenses.