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