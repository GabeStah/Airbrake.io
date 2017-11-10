using System;
using System.ComponentModel;
using System.Windows.Forms;
using Utility;

namespace Airbrake.ComponentModel.LicenseException
{
    [LicenseProvider(typeof(LicFileLicenseProvider))]
    public class LicensedComboBox : ComboBox
    {
        public License License;

        public LicensedComboBox()
        {
            try
            {
                // Validate the license of this instance.
                License = LicenseManager.Validate(typeof(LicensedComboBox), this);
                Logging.LineSeparator("VALID LicensedComboBox LICENSE", 60);
                Logging.Log(License);
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
            if (License == null) return;

            License.Dispose();
            License = null;
        }
    }
}