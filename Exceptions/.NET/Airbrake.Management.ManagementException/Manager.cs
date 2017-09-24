// Manager.cs
using System;
using System.Management;
using Utility;

namespace Airbrake.Management.ManagementException
{
    /// <summary>
    /// Manages creation and manipulation of WMI objects.
    /// </summary>
    internal class Manager
    {
        private const string ScopePathDefault = "\\\\I7\\root\\cimv2";

        public ObjectQuery Query { get; }
        public string ScopePath { get; } = ScopePathDefault;

        internal ManagementObjectSearcher Searcher { get; }

        private readonly ManagementScope _scope;
        public ManagementScope Scope
        {
            get
            {
                // Update scope path.
                _scope.Path = new ManagementPath(ScopePath);
                return _scope;
            }
        }

        internal Manager(string query, string scope = ScopePathDefault)
        {
            Query = new ObjectQuery(query);
            _scope = new ManagementScope(scope);
            Searcher = new ManagementObjectSearcher(Scope, Query);
        }

        internal Manager(ManagementObjectSearcher searcher)
        {
            Query = searcher.Query;
            _scope = searcher.Scope;
            Searcher = searcher;
        }

        internal Manager(ObjectQuery query, ManagementObjectSearcher searcher, ManagementScope scope)
        {
            Query = query;
            Searcher = searcher;
            _scope = scope;
        }

        /// <summary>
        /// Output Searcher.Get() result property value of passed property.
        /// </summary>
        /// <param name="property">Property value to retrieve.</param>
        public void OutputPropertyValue(string property)
        {
            try
            {
                foreach (var element in Searcher.Get())
                {
                    Logging.Log(element.GetPropertyValue(property));
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

        /// <summary>
        /// Dumps all Searcher.Get() results to the log, as formatted text.
        /// </summary>
        public void DumpResults()
        {
            try
            {
                foreach (var element in Searcher.Get())
                {
                    Logging.Log(element.GetText(new TextFormat()));
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