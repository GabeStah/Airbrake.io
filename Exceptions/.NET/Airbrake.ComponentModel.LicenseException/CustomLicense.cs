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