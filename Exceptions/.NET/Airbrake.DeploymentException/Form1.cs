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
