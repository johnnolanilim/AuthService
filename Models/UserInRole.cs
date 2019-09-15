using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace AuthService.Models
{
    /// <summary>
    /// Normal response for HTTP request AccessController.IsUserInRole
    /// </summary>
    public class UserInRole
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserInRole(string role, bool isInRole, IIdentity identity)
        {
            this.role = role;
            this.isInRole = isInRole;
            this.user = new UserInfo(identity);
        }

        /// <summary>
        /// Role being verified against
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// Verification result
        /// </summary>
        public bool isInRole { get; set; }

        /// <summary>
        /// Current user details
        /// </summary>
        public UserInfo user { get; set; }

    }

    /// <summary>
    /// Normal response for HTTP request AccessController.IsUserInOneRole
    /// </summary>
    public class UserInOneRole
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserInOneRole(string[] roles, bool isInRole, IIdentity identity)
        {
            this.roles = roles;
            this.isInRole = isInRole;
            this.user = new UserInfo(identity);
        }

        /// <summary>
        /// Role being verified against
        /// </summary>
        public string[] roles { get; set; }

        /// <summary>
        /// Verification result
        /// </summary>
        public bool isInRole { get; set; }

        /// <summary>
        /// Current user details
        /// </summary>
        public UserInfo user { get; set; }

    }

    /// <summary>
    /// Describes belonging to one user group.
    /// </summary>
    public class InRoleInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InRoleInfo(string role, bool isInRole)
        {
            this.role = role;
            this.isInRole = isInRole;
        }

        /// <summary>
        /// Role name.
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// Verification result
        /// </summary>
        public bool isInRole { get; set; }
    }

    /// <summary>
    /// Normal response for HTTP request AccessController.IsUserInEachRole
    /// </summary>
    public class UserInEachRole
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserInEachRole(InRoleInfo[] result, IIdentity identity)
        {
            this.result = result;
            this.user = new UserInfo(identity);
        }

        /// <summary>
        /// Role being verified against
        /// </summary>
        public InRoleInfo[] result { get; set; }

        /// <summary>
        /// Current user details
        /// </summary>
        public UserInfo user { get; set; }

    }

}