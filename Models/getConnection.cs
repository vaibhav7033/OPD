using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OPD.Models
{
    public static class getConnection
    {

        public static string strConnection = Convert.ToString(ConfigurationManager.ConnectionStrings["Sqlconnection"]);
    }
}