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
