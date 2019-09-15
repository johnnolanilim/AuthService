using System.Security.Principal;

namespace AuthService.Models
{
    /// <summary>
    /// User account details. It is also a normal response for request AccessController.GetUserDetails
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identity">user identity</param>
        public UserInfo(IIdentity identity)
        {
            fullName = identity.Name.ToUpper();
            domain = identity.GetDomain().ToUpper();
            name = identity.GetLogin().ToUpper();
            isAuthenticated = identity.IsAuthenticated;

            WindowsIdentity wi = (WindowsIdentity)identity;

            isAnonymous = wi.IsAnonymous;
            isGuest = wi.IsGuest;
            isSystem = wi.IsSystem;
        }

        /// <summary>
        /// Full name of the current user (including the domain)
        /// </summary>
        public string fullName { get; set; }

        /// <summary>
        /// User domain
        /// </summary>
        public string domain { get; set; }

        /// <summary>
        /// Short user name (without domain)
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Indicates when the user account is anonymous
        /// </summary>
        public bool isAnonymous { get; set; }

        /// <summary>
        /// Indicates when the user account is authenticated
        /// </summary>
        public bool isAuthenticated { get; set; }

        /// <summary>
        /// Indicates when the user account is a guest
        /// </summary>
        public bool isGuest { get; set; }

        /// <summary>
        /// Indicates when it is a system user
        /// </summary>
        public bool isSystem { get; set; }

    }
}
