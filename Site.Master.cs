using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TDKT
{
    public partial class SiteMaster : MasterPage
    {
        public static string _vConnectString = ConfigurationManager.ConnectionStrings["StringConnect"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}