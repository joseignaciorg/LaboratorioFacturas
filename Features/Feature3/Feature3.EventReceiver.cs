using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace LaboratorioFacturas.Features.Feature3
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("3306643b-f277-4cb8-8751-aa0bd55acdfe")]
    public class Feature3EventReceiver : SPFeatureReceiver
    {
        const string timerJobName = "TotalFacturasJob";

        private void deleteJob(SPWebApplication webApplication)

        {
            foreach (SPJobDefinition job in webApplication.JobDefinitions)

            {
                if (job.Name.Equals(timerJobName))

                {
                    job.Delete();
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)

        {
            SPWebApplication webApplication = ((SPSite)properties.Feature.Parent).WebApplication;

            deleteJob(webApplication);
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)

        {
            SPWebApplication webApplication = ((SPSite)properties.Feature.Parent).WebApplication;

            deleteJob(webApplication);

            ManejoFacturasTimerJob timerJob = new ManejoFacturasTimerJob(timerJobName, webApplication, null, SPJobLockType.Job);

            SPMinuteSchedule schedule = new SPMinuteSchedule();

            schedule.BeginSecond = 1;

            schedule.EndSecond = 5;

            schedule.Interval = 2;

            timerJob.Schedule = schedule;

            SPSecurity.RunWithElevatedPrivileges(delegate

            {
                timerJob.Update();

            });
        }
        // Uncomment the method below to handle the event raised after a feature has been activated.

        //public override void FeatureActivated(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
