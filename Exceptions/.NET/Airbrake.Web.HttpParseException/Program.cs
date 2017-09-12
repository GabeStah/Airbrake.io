using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Utility;

namespace Airbrake.Web.HttpParseException
{
    class Program
    {
        static void Main(string[] args)
        {
            var control = new MyHtmlSelectBuilderWithparseException();
            control.GetChildControlType("something", new Dictionary<string, string>());
            Logging.Log(control);
        }
    }

    // Define a child control for the custom HtmlSelect.
    public class MyCustomOption
    {
        string _id;
        string _value;

        public string optionid
        {
            get
            { return _id; }
            set
            { _id = value; }
        }

        public string value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }

    }

    // Define a custom HtmlSelectBuilder.
    public class MyHtmlSelectBuilderWithparseException : HtmlSelectBuilder
    {
        [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
        public override Type GetChildControlType(string tagName, IDictionary attribs)
        {
            // Distinguish between two possible types of child controls.
            if (tagName.ToLower().EndsWith("mycustomoption"))
            {
                return typeof(MyCustomOption);
            }
            else
            {
                throw new System.Web.HttpParseException("This custom HtmlSelect control" + "requires child elements of the form \"MyCustomOption\"");
            }
        }

    }

    [ControlBuilder(typeof(MyHtmlSelectBuilderWithparseException))]
    public class CustomHtmlSelectWithHttpParseException : HtmlSelect
    {
        // Override the AddParsedSubObject method.
        protected override void AddParsedSubObject(object obj)
        {

            string _outputtext;
            if (obj is MyCustomOption)
            {
                _outputtext = "custom select option : " + ((MyCustomOption)obj).value;
                ListItem li = new ListItem(_outputtext, ((MyCustomOption)obj).value);
                base.Items.Add(li);
            }
        }
    }
}
