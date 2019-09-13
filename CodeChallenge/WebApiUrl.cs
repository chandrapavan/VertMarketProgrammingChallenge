using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallenge
{
    public class WebApiUrl
    {
        public static string GetConnectionStringForToken()
        {
            return "http://magazinestore.azurewebsites.net/api/token";
        }

        public static string GetConnectionStringForCategories(string token)
        {
            return "http://magazinestore.azurewebsites.net/api/categories/" + token;
        }

        public static string GetConnectionStringForSubscribers(string token)
        {
            return "http://magazinestore.azurewebsites.net/api/subscribers/" + token;
        }

        public static string GetConnectionStringForMagazines(string token, string category)
        {
            return "http://magazinestore.azurewebsites.net/api/magazines/" + token + "/" + category;
        }

        public static string GetConnectionStringForAnswer(string token)
        {
            return "http://magazinestore.azurewebsites.net/api/answer/" + token;
        }
    }
}
