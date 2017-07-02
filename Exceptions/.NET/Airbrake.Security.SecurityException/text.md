# .NET Exceptions - System.Security.SecurityException

Next up in our journey through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll take a gander at the `System.Security.SecurityException`.  `System.Security.SecurityExceptions` occur when the caller -- that is, the executing user account -- doesn't have proper permissions to access a particular resource.  

Throughout this article we'll examine the `System.Security.SecurityException` in more detail, including where it sits in the .NET exception hierarchy, along with some code samples using C# that will illustrate how `System.Security.SecurityExceptions` might be thrown, so you can better understand how to handle them yourself.  Let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.Security.SecurityException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

Permissions can be a tricky thing to deal with during development, particularly as an application grows in size and scope, thereby requiring the use of additional libraries, modules, services, connections, and so forth.  Each additional component added into the application is just another potential security risk, and something that needs to be properly managed with permissions that allow the application (and its own processes therein) to perform the tasks it needs to, while simultaneously disallowing unintended access.

Due to these potential security risks the .NET Framework includes a vast array of security-related capabilities.  The entire [`System.Security`](https://docs.microsoft.com/en-us/dotnet/api/system.security?view=netframework-4.7) namespace provides numerous classes and helpers designed to make security management as easy and robust as possible.  And, of course, the `System.Security.SecurityException` we're looking at today is among those many tools.

As previously mentioned, a `System.Security.SecurityException` is thrown when the application's caller attempts to access a resource it doesn't have permission for.  These resources could be all sorts of things, but for our purposes here we'll use a similar example to that given by the official documentation: trying to manipulate the Windows `registry` without appropriate permissions.

We'll start with the full working code example below, after which we'll break the code down a bit more to see what's going on:

```cs
using System;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;
using Utility;

namespace Airbrake.Security.SecurityException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set company as parent key element.
            const string company = "TestCompany";
            // Create subkey tree.
            var subKeyTree = $"{company}\\Applications\\TestApplication";

            // Create key with no permissions.
            CreateRegistryKey(subKeyTree, PermissionState.None);
            Logging.LineSeparator();

            // Create key with unrestricted permissions.
            CreateRegistryKey(subKeyTree, PermissionState.Unrestricted);
            Logging.LineSeparator();

            // Delete key with no permissions.
            DeleteRegistryKey(company, PermissionState.None);
            Logging.LineSeparator();

            // Delete key with unrestricted permissions.
            DeleteRegistryKey(company, PermissionState.Unrestricted);
            Logging.LineSeparator();
        }

        /// <summary>
        /// Create a registry key tree using self-assigned permissions.
        /// </summary>
        /// <param name="subKeyTree">The registry key tree to be created.</param>
        /// <param name="state">Permission state to apply to local permission set.</param>
        /// <returns>Success or failure of key creation.</returns>
        private static bool CreateRegistryKey(string subKeyTree, PermissionState state)
        {
            // Create empty permission set.
            var permissionSet = new PermissionSet(null);
            // Add RegistryPermission to permission set with passed state value.
            permissionSet.AddPermission(new RegistryPermission(state));
            // Ensure that only the above permissions are applied, regardless
            // of permissions assigned to executing user account.
            permissionSet.PermitOnly();

            try
            {
                // Create registry key.
                var key = Registry.CurrentUser.CreateSubKey(subKeyTree);
                // Check that result is valid.
                if (key != null)
                {
                    // Output creation message.
                    Logging.Log($"Created registry key: {key.Name}.");
                    return true;
                }
            }
            catch (System.Security.SecurityException exception)
            {
                // Log potential security exceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Log any inexplicit exceptions.
                Logging.Log(exception, false);
            }
            return false;
        }

        /// <summary>
        /// Delete a registry parent key (and all child keys).
        /// </summary>
        /// <param name="parentKey">Parent key to delete.</param>
        /// <param name="state">Permission state to apply to local permission set.</param>
        /// <returns>Success or failure of key deletion.</returns>
        private static bool DeleteRegistryKey(string parentKey, PermissionState state)
        {
            // Create empty permission set.
            var permissionSet = new PermissionSet(null);
            // Add RegistryPermission to permission set with passed state value.
            permissionSet.AddPermission(new RegistryPermission(state));
            // Ensure that only the above permissions are applied, regardless
            // of permissions assigned to executing user account.
            permissionSet.PermitOnly();

            try
            {
                // Delete the provided registry key (and child keys).
                Registry.CurrentUser.DeleteSubKeyTree(parentKey);
                // Output confirmation message of deletion.
                Logging.Log($"Deleted registry key (with children): {parentKey}.");
                return true;
            }
            catch (System.Security.SecurityException exception)
            {
                // Log potential security exceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Log any inexplicit exceptions.
                Logging.Log(exception, false);
            }
            return false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
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

        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        public static void Log(Exception exception, bool expected = true)
        {
            string value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}";
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="System.Diagnostics.Debug.WriteLine"/> 
        /// if DEBUG mode is enabled, otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        public static void LineSeparator()
        {
#if DEBUG
            Debug.WriteLine(new string('-', 20));
#else
            Console.WriteLine(new string('-', 20));
#endif
        }
    }
}
```

Since we're trying to manipulate registry keys we start with the `CreateRegistryKey()` method:

```cs
/// <summary>
/// Create a registry key tree using self-assigned permissions.
/// </summary>
/// <param name="subKeyTree">The registry key tree to be created.</param>
/// <param name="state">Permission state to apply to local permission set.</param>
/// <returns>Success or failure of key creation.</returns>
private static bool CreateRegistryKey(string subKeyTree, PermissionState state)
{
    // Create empty permission set.
    var permissionSet = new PermissionSet(null);
    // Add RegistryPermission to permission set with passed state value.
    permissionSet.AddPermission(new RegistryPermission(state));
    // Ensure that only the above permissions are applied, regardless
    // of permissions assigned to executing user account.
    permissionSet.PermitOnly();

    try
    {
        // Create registry key.
        var key = Registry.CurrentUser.CreateSubKey(subKeyTree);
        // Check that result is valid.
        if (key != null)
        {
            // Output creation message.
            Logging.Log($"Created registry key: {key.Name}.");
            return true;
        }
    }
    catch (System.Security.SecurityException exception)
    {
        // Log potential security exceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Log any inexplicit exceptions.
        Logging.Log(exception, false);
    }
    return false;
}
```

The `subKeyTree` parameter is the registry key path we're trying to create.  The `PermissionState state` parameter is the permission state we want to use while attempting to make our registry change.  To do so, we create a new `PermissionSet` and add a new `RegistryPermission` element with the `state` parameter value.  Finally, we need to ensure that the current process doesn't use any inherent permissions it may have from the operating system user account or the like, and instead, only uses the permissions we've granted within the method.  This is exactly what the call to `permissionSet.PermitOnly()` accomplishes.  With everything set we can then try to create the registry key with the `CreateSubKey()` method.

To test our `CreateRegistryKey()` method we first declare the registry key string, then call `CreateRegistryKey()` with `PermissionState.None` passed in:

```cs
// Set company as parent key element.
const string company = "TestCompany";
// Create subkey tree.
var subKeyTree = $"{company}\\Applications\\TestApplication";

// Create key with no permissions.
CreateRegistryKey(subKeyTree, PermissionState.None);
Logging.LineSeparator();
```

Since the temporary permission set we used includes `PermissionState.None` it isn't much surprise that this ended up throwing a `System.Security.SecurityException` at us:

```
[EXPECTED] System.Security.SecurityException: Request for the permission of type 'System.Security.Permissions.RegistryPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' failed.
The action that failed was:
Demand
```

In this case we can resolve our issue by passing `PermissionState.Unrestricted` to the `CreateRegistryKey()` method, which should ensure that our `RegistryPermission` object has unrestricted access:

```cs
// Create key with unrestricted permissions.
CreateRegistryKey(subKeyTree, PermissionState.Unrestricted);
Logging.LineSeparator();
```

Sure enough, this works just fine and our log indicates that the registry key was generated:

```
Created registry key: HKEY_CURRENT_USER\TestCompany\Applications\TestApplication.
```

Opening `regedit.exe` and drilling down to that location in the registry also confirms that the registry entry was created.

In order to clean up after ourselves and not leave any unnecessary registry keys lying around, we've also defined the `DeleteRegistryKey()` method:

```cs
/// <summary>
/// Delete a registry parent key (and all child keys).
/// </summary>
/// <param name="parentKey">Parent key to delete.</param>
/// <param name="state">Permission state to apply to local permission set.</param>
/// <returns>Success or failure of key deletion.</returns>
private static bool DeleteRegistryKey(string parentKey, PermissionState state)
{
    // Create empty permission set.
    var permissionSet = new PermissionSet(null);
    // Add RegistryPermission to permission set with passed state value.
    permissionSet.AddPermission(new RegistryPermission(state));
    // Ensure that only the above permissions are applied, regardless
    // of permissions assigned to executing user account.
    permissionSet.PermitOnly();

    try
    {
        // Delete the provided registry key (and child keys).
        Registry.CurrentUser.DeleteSubKeyTree(parentKey);
        // Output confirmation message of deletion.
        Logging.Log($"Deleted registry key (with children): {parentKey}.");
        return true;
    }
    catch (System.Security.SecurityException exception)
    {
        // Log potential security exceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Log any inexplicit exceptions.
        Logging.Log(exception, false);
    }
    return false;
}
```

This effectively attempts to reverse the registry key creation process we performed earlier, also by using permission sets.  The use of the underlying .NET method `DeleteSubKeyTree()` forces us to provide the _parent_ key name to our method, rather than the full subkey tree as we did with `CreateRegistryKey()`:

```cs
// Set company as parent key element.
const string company = "TestCompany";
// Create subkey tree.
var subKeyTree = $"{company}\\Applications\\TestApplication";

// Delete key with no permissions.
DeleteRegistryKey(company, PermissionState.None);
Logging.LineSeparator();

// Delete key with unrestricted permissions.
DeleteRegistryKey(company, PermissionState.Unrestricted);
Logging.LineSeparator();
```

Once again we're trying two different calls to the `DeleteRegistryKey()` method; one with no permissions, and one with unrestricted permissions.  The log output result shows both an initial failure on the first call, followed by success on the second:

```
[EXPECTED] System.Security.SecurityException: Request for the permission of type 'System.Security.Permissions.RegistryPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' failed.
The action that failed was:
Demand
--------------------
Deleted registry key (with children): TestCompany.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A detailed look at the System.Security.SecurityException in .NET, including functional C# code samples and a brief overview of custom permission sets.