// Airbrake.Management.ManagementException.Program.cs
using System;
using Utility;

namespace Airbrake.Management.ManagementException
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Logging.LineSeparator("LOCAL DISK INFO");
            GetDiskInfo();
            Logging.LineSeparator("FULL LOCAL DISK INFO");
            GetFullDiskInfo();
            Logging.LineSeparator("LOCAL DISK INFO - INVALID QUERY");
            GetDiskInfo_InvalidQuery();
        }

        private static void GetDiskInfo()
        {
            // Perform query and create Manager.
            var manager = new Manager("SELECT * FROM Win32_LogicalDisk");
            // Loop through query results.
            foreach (var disk in manager.Searcher.Get())
            {
                var free = (ulong) disk.GetPropertyValue("FreeSpace");
                var total = (ulong) disk.GetPropertyValue("Size");
                // Output disk space info.
                Logging.Log($"The {disk.GetPropertyValue("Name")} drive has {free.ToStorageString()} free, out of {total.ToStorageString()} total.");
            }
        }

        private static void GetFullDiskInfo()
        {
            // Perform query and create Manager.
            var manager = new Manager("SELECT * FROM Win32_LogicalDisk");
            // Output full result data.
            manager.DumpResults();
        }

        private static void GetDiskInfo_InvalidQuery()
        {
            try
            {
                // Perform query and create Manager.
                var manager = new Manager("SELECT * FROM Win32_PhysicalDisk");
                // Loop through query results.
                foreach (var disk in manager.Searcher.Get())
                {
                    var free = (ulong)disk.GetPropertyValue("FreeSpace");
                    var total = (ulong)disk.GetPropertyValue("Size");
                    // Output disk space info.
                    Logging.Log($"The {disk.GetPropertyValue("Name")} drive has {free.ToStorageString()} free, out of {total.ToStorageString()} total.");
                }
            }
            catch (System.Management.ManagementException exception)
            {
                // Output expected ManagementExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }
}
