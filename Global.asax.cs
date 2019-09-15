using System;
using System.Web.Http;
using WebApiContrib.Formatting.Jsonp;

namespace AuthService
{
    /// <summary>
    /// Application class.
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application Start event handler.
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.AddJsonpFormatter();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        /// <summary>
        /// Application BeginRequest event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string baseUrl = String.Format("{0}://{1}{2}/", Request.Url.Scheme, Request.Url.Authority, Request.ApplicationPath.TrimEnd('/'));
            if (Request.Url.AbsoluteUri == baseUrl)
            {
                // When request is for the root, redirect to the Swagger API page:
                Response.RedirectPermanent("swagger");
            }
        }
    }
}
