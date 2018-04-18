using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SignOut : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["UserName"] !=null && Session["isLogin"] !=null)
        //{
        //    Response.Redirect("~/Login.aspx", false);
        //    return;
        //}       

        Session["UserName"] = null;
        Session["isLogin"] = null;
        Session["ID"] = null;
        //Session["Password"] = "";       

        FormsAuthentication.SignOut();
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();


        // Clear authentication cookie
        HttpCookie rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
        rFormsCookie.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(rFormsCookie);

        // Clear session cookie 
        HttpCookie rSessionCookie = new HttpCookie("ASP.NET_SessionId", "");
        rSessionCookie.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(rSessionCookie);

        // Invalidate the Cache on the Client Side
        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        Response.Cache.AppendCacheExtension("no-cache");
        Response.Cache.SetNoStore();
        Response.Expires = 0;

        FormsAuthentication.SignOut();
        Response.Redirect("Login.aspx", true);    
        //  FormsAuthentication.RedirectToLoginPage();
    }    
}