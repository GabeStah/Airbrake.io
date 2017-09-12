using System;
using System.Xml;
using System.Xml.XPath;
using Utility;

namespace Airbrake.Xml.XPath.XPathException
{
    class Program
    {
        private static void Main(string[] args)
        {
            var xPath = @"//Airbrake:Books/Book/Title/text()";
            Logging.LineSeparator(xPath, 60);
            GetXPathNodesFromXml(@"books.xml", xPath);

            xPath = @"//Airbrake:Books/Book/Title/text()";
            Logging.LineSeparator($"{xPath} w/ Namespace", 60);
            GetXPathNodesFromXml(@"books.xml", xPath, "Airbrake", "https://airbrake.io");

            xPath = @"//Airbrake:Books/Book/*";
            Logging.LineSeparator($"{xPath} w/ Namespace", 60);
            GetXPathNodesFromXml(@"books.xml", xPath, "Airbrake", "https://airbrake.io");
        }

        /// <summary>
        /// Outputs the node values using XPath, from passed XML file.
        /// </summary>
        /// <param name="file">XML file path.</param>
        /// <param name="xPath">XPath expression.</param>
        /// <param name="manager">Namespace manager used to select namespaced elements, if applicable.</param>
        internal static void GetXPathNodesFromXml(string file, string xPath, XmlNamespaceManager manager = null)
        {
            try
            {
                // Create a new XPath document from XML file.
                var document = new XPathDocument(file);
                // Create navigator and select nodes using passed xPath expression and manager.
                var navigator = document.CreateNavigator();
                var nodes = navigator.Select(xPath, manager);
                // Iterator through all nodes.
                while (nodes.MoveNext())
                {
                    // Output current node value.
                    Logging.Log(nodes.Current.Value);
                }
            }
            catch (System.Xml.XPath.XPathException exception)
            {
                // Output expected XPathExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Outputs the node values using XPath, from passed XML file.
        /// 
        /// Allows specification of XML namespace.
        /// </summary>
        /// <param name="file">XML file path.</param>
        /// <param name="xPath">XPath expression.</param>
        /// <param name="namespace">Namespace to look within.</param>
        /// <param name="namespaceUrl">Namespace URL.</param>
        internal static void GetXPathNodesFromXml(string file, string xPath, string @namespace, string namespaceUrl)
        {
            // Create new namespace manager with empty name table.
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            // Add namespace to manager.
            namespaceManager.AddNamespace(@namespace, namespaceUrl);

            // Forward execution to base method signature.
            GetXPathNodesFromXml(file, xPath, namespaceManager);
        }
    }
}
