using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;
using Utility;

namespace Airbrake.Web.HttpException
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                var settings = new FriendlyUrlSettings();
                settings.AutoRedirectMode = RedirectMode.Permanent;
                routes.EnableFriendlyUrls(settings);
            }
            catch (Exception e)
            {
                Logging.Log(e);
            }
        }
    }
}
