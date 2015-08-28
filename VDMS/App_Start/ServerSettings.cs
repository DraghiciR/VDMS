using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace VDMS
{
    public class ServerSettings
    {
        public static string ServerMail
        {
            get
            {
                return WebConfigurationManager.AppSettings["EmailServer"] ?? string.Empty;
            }
        }
        public static string SenderAccount
        {
            get
            {
                return WebConfigurationManager.AppSettings["SenderAccount"] ?? string.Empty;
            }
        }
        public static string CredentialsUserName
        {
            get
            {
                return WebConfigurationManager.AppSettings["CredentialsUserName"] ?? string.Empty;
            }
        }
        public static string CredentialsPassword
        {
            get
            {
                return WebConfigurationManager.AppSettings["CredentialsPassword"] ?? string.Empty;
            }
        }
        public static bool EnableSsl
        {
            get
            {
                return Convert.ToBoolean(WebConfigurationManager.AppSettings["EnableSsl"]);
            }
        }
    }
}