using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Utility;

namespace Airbrake.Web.HttpParseException
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Instantiate a new select builder.
            var selectBuilder = new MySelectBuilder
            {
                ID = "names"
            };

            // Attempt to get builder control with MyCustomOption suffix.
            Logging.LineSeparator("TESTING MyCustomOption SUFFIX");
            GetChildControlOfControlBuilder(selectBuilder, $"Select-{MyCustomOption.Suffix}");

            // Attempt to get builder control with Invalid suffix.
            Logging.LineSeparator("TESTING Invalid SUFFIX");
            GetChildControlOfControlBuilder(selectBuilder, $"Select-Invalid");
        }

        private static void GetChildControlOfControlBuilder(ControlBuilder builder, string tagName)
        {
            try
            {
                // Get the child control type of passed 
                // builder using passed tag name.
                var type = builder.GetChildControlType(
                    tagName, 
                    new Dictionary<string, string>()
                );
                Logging.Log($"Child Control Type of {builder.GetType().Name} (ID: {builder.ID}) is {type}");
            }
            catch (System.Web.HttpParseException exception)
            {
                // Output expected HttpParseExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }

    /// <summary>
    /// A basic custom Select Option.
    /// </summary>
    public class MyCustomOption
    {
        public const string Suffix = "MyCustomOption";
        public string Id { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Custom HtmlSelectBuilder, which only allows MyCustomOption child controls.
    /// </summary>
    public class MySelectBuilder : HtmlSelectBuilder
    {
        [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
        public override Type GetChildControlType(string tagName, IDictionary attributes)
        {
            // Verify that child control tag ends with MyCustomOption suffix.
            if (tagName != null && tagName.EndsWith(MyCustomOption.Suffix))
            {
                // Return MyCustomOption type.
                return typeof(MyCustomOption);
            }
            // If a different tagName is passed, throw HttpParseException.
            throw new System.Web.HttpParseException($"Unable to get child control type.  '{GetType()}' control requires child element type of '{MyCustomOption.Suffix}.'");
        }
    }

    /// <summary>
    /// Custom HtmlSelect that uses MySelectBuilder.
    /// </summary>
    [ControlBuilder(typeof(MySelectBuilder))]
    public class CustomHtmlSelect : HtmlSelect
    {
        // Override the AddParsedSubObject method.
        protected override void AddParsedSubObject(object obj)
        {
            // Create new custom option.
            var option = obj as MyCustomOption;
            // Ensure option is not null.
            if (option == null) return;
            // Create select option text.
            var text = $"{option.Id} : {option.Value}";
            // Add option to Items list.
            var listItem = new ListItem(text, option.Value);
            Items.Add(listItem);
        }
    }
}
