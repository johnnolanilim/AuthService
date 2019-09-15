using System.Security.Principal;

namespace AuthService
{
    /// <summary>
    /// Public extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Retrieves domain name from the user's full name
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetDomain(this IIdentity identity)
        {
            string s = identity.Name;
            int stop = s.IndexOf("\\");
            return (stop > -1) ? s.Substring(0, stop) : string.Empty;
        }

        /// <summary>
        /// Retrieves user's login name from the full name
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetLogin(this IIdentity identity)
        {
            string s = identity.Name;
            int stop = s.IndexOf("\\");
            return (stop > -1) ? s.Substring(stop + 1, s.Length - stop - 1) : string.Empty;
        }
    }
}
