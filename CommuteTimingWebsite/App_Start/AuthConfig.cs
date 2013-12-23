using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using CommuteTimingWebsite.Models;
using WebMatrix.WebData;

namespace CommuteTimingWebsite
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");
            if (!WebSecurity.Initialized)
            {
                using (var db = DatabaseHelpers.GetEntityModel())
                {
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "ID", "Name",
                                                             autoCreateTables: true);
                }
            }
            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "621792781217115",
            //    appSecret: "c825a65c6fc8c76a147df80522e425ff");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
