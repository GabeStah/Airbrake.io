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
            var exception = new System.Security.SecurityException();
            exception.GetType().LogInheritanceHierarchy();
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
