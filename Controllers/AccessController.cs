using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text;
using System.Web.Http.Description;
using AuthService.Models;
using System.Web;
using System.Web.Http.Filters;
using System.Security.Principal;
using System.Configuration;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;

namespace AuthService.Controllers
{

    /// <summary>
    /// User access controller.
    /// </summary>
    public class AccessController : ApiController
    {
        private string envName = null;
        private HttpClient client = null;

        AccessController()
        {
            envName = GetEnvironmentName();
            string schemaUrl = ConfigurationManager.AppSettings["schemaUrl_" + envName];

            if (!string.IsNullOrEmpty(schemaUrl))
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(schemaUrl);

                // Add an Accept header for JSON format:
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            }
        }

        /// <summary>
        /// Reads and returns environment name, according to the IIS configuration we use across all servers.
        /// </summary>
        /// <returns></returns>
        private static string GetEnvironmentName()
        {
            string env = ""; // environment name
            try
            {
                var configPath = HttpContext.Current.Server.MapPath("/") + @"conf\applicationHost.config";
                if (File.Exists(configPath))
                {
                    var doc = new XPathDocument(configPath);
                    var nav = doc.CreateNavigator();
                    var node = nav.SelectSingleNode("/configuration/system.webServer/httpProtocol/customHeaders/add[@name='Environment']");
                    if (node != null)
                    {
                        env = node.GetAttribute("value", "");
                    }
                    if (!string.IsNullOrEmpty(env))
                    {
                        return env;
                    }
                }
            }
            catch (Exception)
            {
            }
            return ConfigurationManager.AppSettings["Environment"];

        }

        /// <summary>
        /// Gets the current user details.
        /// </summary>
        /// <remarks>
        /// Retrieves all useful account details for the current user.
        /// </remarks>
        [HttpGet]
        [Route("access/user")]
        [ResponseType(typeof(UserInfo))]
        public HttpResponseMessage GetUserDetails()
        {
            if (client == null)
            {
                return InvalidConfig();
            }

            try
            {
                var resp = HttpContext.Current.Response.Headers.AllKeys;
                string userName = User.Identity.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return CreateResponse(new { error = "Cannot determine the user." }, HttpStatusCode.Unauthorized);
                }
                return CreateResponse(new UserInfo(User.Identity), HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return CreateResponse(new { error = e.Message }, (HttpStatusCode)500);
            }
        }

        /// <summary>
        /// Checks if the current user is in a specific role.
        /// </summary>
        /// <remarks>
        /// Verifies whether the current user is in the specified role in Active Directory.
        /// </remarks>
        /// <param name="role">AD Role Name</param>
        [HttpGet]
        [Route("access/user-in-role")]
        [ResponseType(typeof(UserInRole))]
        public HttpResponseMessage IsUserInRole(string role)
        {
            if (client == null)
            {
                return InvalidConfig();
            }

            try
            {
                string userName = User.Identity.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return CreateResponse(new { error = "Cannot determine the user." }, HttpStatusCode.Unauthorized);
                }

                if (string.IsNullOrEmpty(role))
                {
                    return CreateResponse(new { error = "Parameter 'role' is invalid." }, (HttpStatusCode)422);
                }
                bool isInRole = User.IsInRole(role);
                return CreateResponse(new UserInRole(role, isInRole, User.Identity), HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return CreateResponse(new { error = e.Message }, (HttpStatusCode)500);
            }
        }

        /// <summary>
        /// Checks if the current user is at least in one role listed.
        /// </summary>
        /// <remarks>
        /// Verifies whether the current user is in one or more specified roles in Active Directory.
        /// </remarks>
        /// <param name="roles">Comma-separated list of AD role names</param>
        [HttpGet]
        [Route("access/user-in-one-role")]
        [ResponseType(typeof(UserInRole))]
        public HttpResponseMessage IsUserInOneRole(string roles)
        {
            if (client == null)
            {
                return InvalidConfig();
            }

            try
            {
                string userName = User.Identity.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return CreateResponse(new { error = "Cannot determine the user." }, HttpStatusCode.Unauthorized);
                }

                if (string.IsNullOrEmpty(roles))
                {
                    return CreateResponse(new { error = "Invalid list of roles." }, (HttpStatusCode)422);
                }

                string[] roleNames = roles.Split(',');

                bool inRole = false;
                foreach (string r in roleNames)
                {
                    if (User.IsInRole(r))
                    {
                        inRole = true;
                        break;
                    }
                }

                return CreateResponse(new UserInOneRole(roleNames, inRole, User.Identity), HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return CreateResponse(new { error = e.Message }, (HttpStatusCode)500);
            }
        }

        /// <summary>
        /// Checks whether the current user is in each role listed.
        /// </summary>
        /// <remarks>
        /// Verifies whether the current user is in each of the specified roles in Active Directory.
        /// </remarks>
        /// <param name="roles">Comma-separated list of AD role names</param>
        [HttpGet]
        [Route("access/user-in-each-role")]
        [ResponseType(typeof(UserInEachRole))]
        public HttpResponseMessage IsUserInEachRole(string roles)
        {
            if (client == null)
            {
                return InvalidConfig();
            }

            try
            {
                string userName = User.Identity.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return CreateResponse(new { error = "Cannot determine the user." }, HttpStatusCode.Unauthorized);
                }

                if (string.IsNullOrEmpty(roles))
                {
                    return CreateResponse(new { error = "Invalid list of roles." }, (HttpStatusCode)422);
                }

                string[] roleNames = roles.Split(',');

                InRoleInfo[] info = new InRoleInfo[roleNames.Length];
                for (int i = 0; i < roleNames.Length; i++)
                {
                    string r = roleNames[i];
                    info[i] = new InRoleInfo(r, User.IsInRole(r));
                }

                return CreateResponse(new UserInEachRole(info, User.Identity), HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return CreateResponse(new { error = e.Message }, (HttpStatusCode)500);
            }
        }

        private HttpResponseMessage InvalidConfig()
        {
            return CreateResponse(new { error = "Invalid environment configuration.", environment = envName }, HttpStatusCode.InternalServerError);
        }

        private HttpResponseMessage CreateResponse(Object data, HttpStatusCode sc)
        {
            return Request.CreateResponse(sc, data);
        }

    }

}
