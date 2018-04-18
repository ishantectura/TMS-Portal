using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ContactUs : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.QueryString["key"] != null)
        {
            if (Request.QueryString["key"] == "ol")
            {
                this.MasterPageFile = "~/SiteMaster.master";
            }
            else if (Request.QueryString["key"] == "rd")
            {
                this.MasterPageFile = "~/RoleBaseMaster.master";
            }
            else
            {
                Response.Redirect("Login.aspx"); //Later to be change with signout page - To abandon all session & Logout from the Application
            }

        }
        


    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}