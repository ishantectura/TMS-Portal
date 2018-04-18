using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["s"] != null)
        {
            if (Request.QueryString["s"].ToString() == "1")
            {
                lblmsg.Text = "Password changed successfuly, Please login with the new password !";
            }
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (txtUserName.Value != "" && txtPassword.Value != "")
        {
            Session.Add("UserName", txtUserName.Value.ToString().Trim());            
            Session.Add("isLogin", "true");         
            ValidateLogin(txtUserName.Value.ToString(), txtPassword.Value.ToString());
        }
    }

    public void ValidateLogin(string username, string password)
    {
        if (ValidateUser(username, password))
        {

            string usertype = CheckUserType(username, password);

            FormsAuthenticationTicket tkt;
            string cookiestr;
            HttpCookie ck;
            tkt = new FormsAuthenticationTicket(1, txtUserName.Value, DateTime.Now, DateTime.Now.AddMinutes(30), chkPersistCookie.Checked, "your custom data");
            cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            if (chkPersistCookie.Checked)
                ck.Expires = tkt.Expiration;
            ck.Path = FormsAuthentication.FormsCookiePath;
            Response.Cookies.Add(ck);

            string strRedirect;
            strRedirect = Request["ReturnUrl"];
            if (strRedirect == null)
            {
                if (Session["UserType"].ToString() == "Transporter" || Session["UserType"].ToString() == "Third Party")
                {
                    strRedirect = "Licenseagreement.aspx";
                    int isLicenseAccepted = 0; //false
                    int isPasswordChanged = 0;
                    LicenseAccepted(username, password, out isLicenseAccepted, out isPasswordChanged);

                    Session.Add("ChangePassword", isPasswordChanged);
                    if (isLicenseAccepted == 0) //false
                    {
                        Response.Redirect(strRedirect, true);
                    }
                    else
                    {
                        if (isPasswordChanged == 0) //false
                        {
                            Response.Redirect("ChangePassword.aspx", true);
                        }
                        else
                        {
                            Response.Redirect("Dashboard.aspx", true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("Dashboard.aspx", true);
                }
            }


            else
            {
                lblError.Text = "Login Failed/Invalid User";
            }
        }
        else
        {
            lblError.Text = "User Name/ Password is incorrect !";
        }
    }


    private string CheckUserType(string username, string password)
    {
        SqlConnection conn;
        SqlCommand cmd;
        string userType = null;
        Cryptography objcs = new Cryptography();
        try
        {
            // Consult with your SQL Server administrator for an appropriate connection
            // string to use to connect to your local SQL Server.
            conn = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString());
            conn.Open();

            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand("SELECT [User Type] FROM [dbo].[TMS_Portal_User_Setup] where [Portal User Id]=@userName and [Password]=@password", conn);
            cmd.Parameters.Add("@userName", SqlDbType.VarChar, 25);
            cmd.Parameters["@userName"].Value = username;

            cmd.Parameters.Add("@password", SqlDbType.VarChar, 50);
            cmd.Parameters["@password"].Value =objcs.Encrypt(password);     // lmaru8748

           // cmd.Parameters["@password"].Value = password;




            // Execute command and fetch pwd field into lookupPassword string.
            userType = (string)cmd.ExecuteScalar();
            if(userType==null)
            {
                cmd = new SqlCommand("SELECT [User Type] FROM [dbo].[TMS_Portal_User_Setup] where [Portal User Id]=@userName and [Password]=@password", conn);
                cmd.Parameters.Add("@userName", SqlDbType.VarChar, 25);
                cmd.Parameters["@userName"].Value = username;

                cmd.Parameters.Add("@password", SqlDbType.VarChar, 50);
                cmd.Parameters["@password"].Value = password;
                userType = (string)cmd.ExecuteScalar();
            }

            Session.Add("UserType", userType);

            // Cleanup command and connection objects.
            cmd.Dispose();
            conn.Dispose();

           return userType;            
        }
        catch (Exception ex)
        {
            // Add error handling here for debugging.
            // This error message should not be sent back to the caller.
            System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception " + ex.Message);
        }
        return "";
    }

    private bool ValidateUser(string userName, string passWord)
    {
        //Do code for Validate Login Credentials///////////////////////////////////////        
        SqlConnection conn;
        SqlCommand cmd;
        Cryptography objcs = new Cryptography(); 
        string lookupPassword = null;
        string lookupPasswordUnEncrypted = null;

        // Check for invalid userName.  
        if ((null == userName) || (0 == userName.Length) || (userName.Length > 15))
        {
            System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of userName failed.");
            return false;
        }

        // Check for invalid passWord.
        // passWord must not be null and must be between 1 and 25 characters.
        if ((null == passWord) || (0 == passWord.Length) || (passWord.Length > 25))
        {
            System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of passWord failed.");
            return false;
        }

        try
        {
            // Consult with your SQL Server administrator for an appropriate connection
            // string to use to connect to your local SQL Server.
            conn = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString());
            conn.Open();

            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand("SELECT [Password] FROM [dbo].[TMS_Portal_User_Setup] where [Portal User Id]=@userName", conn);
            cmd.Parameters.Add("@userName", SqlDbType.VarChar, 25);
            cmd.Parameters["@userName"].Value = userName;

            // Execute command and fetch pwd field into lookupPassword string.
            

            lookupPasswordUnEncrypted = (string)cmd.ExecuteScalar();

            if (null == lookupPasswordUnEncrypted)
            {
                // You could write failed login attempts here to event log for additional security.
                cmd.Dispose();
                conn.Dispose();
                return false;
            }

            if (0 == string.Compare(lookupPasswordUnEncrypted, passWord, false))
            {
                cmd.Dispose();
                conn.Dispose();
                return true;

            }
            else
            {
                lookupPassword = objcs.Decrypt((string)cmd.ExecuteScalar());
                if (0 == string.Compare(lookupPassword, passWord, false))
                {
                    cmd.Dispose();
                    conn.Dispose();
                    return true;
                }
            }
            // Cleanup command and connection objects.
           
            cmd.Dispose();
            conn.Dispose();
            return false;
        }
        catch (Exception ex)
        {
            // Add error handling here for debugging.
            // This error message should not be sent back to the caller.
            System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception " + ex.Message);
            return false;
        }

        
    }

    private void LicenseAccepted(string userName, string passWord, out int isLicenseAccepted, out int isPasswordChanged)
    {
        isLicenseAccepted = 0;
        isPasswordChanged = 0;
        try
        {
            #region Code
            Cryptography obj = new Cryptography();
            using (SqlConnection conn = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT [Lic_ Term Aggrement Accepted] , [Change_Password], [ID] FROM [TMS_Portal_User_Setup] WHERE [Portal User Id]='" + userName + "' and [Password]='" + obj.Encrypt(passWord) + "'";  // lmaru8748

                   // cmd.CommandText = "SELECT [Lic_ Term Aggrement Accepted] , [Change_Password], [ID] FROM [TMS_Portal_User_Setup] WHERE [Portal User Id]='" + userName + "' and [Password]='" + passWord + "'";
                    cmd.Connection = conn;
                    conn.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                isLicenseAccepted = int.Parse(sdr[0].ToString()); //0= false , 1=true
                                isPasswordChanged = ((bool)(sdr[1])) ? 1 : 0;
                                Session.Add("ID", int.Parse(sdr[2].ToString()));
                            }
                        }
                    }
                    conn.Close();
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine(" Exception " + ex.Message);
        }
    }

    protected void btnForgot_Click(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
        //this.lblMessage.Text = "Your password has been sent successfuly on your registered Email ID.";
        Response.Redirect("ForgotPassword.aspx");
    }
    
    

}