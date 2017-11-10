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