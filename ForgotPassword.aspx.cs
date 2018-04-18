using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUserName.Value != "" && txtEmail.Value != null)
            {
                using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TMS_sp_ForgotPassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlParameter parm = new SqlParameter("@Result", SqlDbType.Int);
                        parm.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(parm);
                        cmd.Parameters.AddWithValue("@Username", txtUserName.Value.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Value.Trim());
                        //cmd.Parameters.AddWithValue("@Result", 0);

                        int row= cmd.ExecuteNonQuery();
                       
                        int Result = Convert.ToInt16(parm.Value);
                        if (Result== 1) //User Exist
                        {
                            //Send Email                        
                            sendmail(txtEmail.Value, txtUserName.Value);
                            lblmsg.Text = "Reset-Password request has been sent to Administrator ! We will get back to you on your registerd Email-Id";
                            lblmsg.ForeColor = System.Drawing.Color.Green;
                            lblmsg.Visible = true;
                            txtUserName.Value = "";
                            txtEmail.Value = "";
                        }
                        else
                        {
                            lblmsg.Text = "";
                            lblmsg.Text = "Email ID/User Name does not exist in TMS sytem database. Please contact Administrator !";
                            lblmsg.Visible = true;
                        }
                        con.Close();
                    }
                }
            }
        }
        catch (Exception d)
        {
            lblmsg.Text ="";
            lblmsg.Text = "Some error occured at the moment ! Please try after some time  !" +d.Message;
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Visible = true;
        }  
    }

    private void sendmail(string to, string username)
    {
        MailMessage objMailMessage = new MailMessage();
        System.Net.NetworkCredential objSMTPUserInfo = new System.Net.NetworkCredential();
        SmtpClient objSmtpClient = new SmtpClient("smtp.office365.com");
        try
        {
            DataTable dt = GetData("SELECT top 1 [E-Mail Address to Send Mails] as [FromEmailID] ,[Service Account] as [UserName] ,[Service Password] as [Password] FROM [dbo].[TMS_Setup]");
            objMailMessage.From = new MailAddress(dt.Rows[0][0].ToString());
            objMailMessage.To.Add(new MailAddress(to));
            objMailMessage.Subject = "TMS - Reset Password Reuqest ";
            objMailMessage.Body = "Reset Password request for TMS : " + Environment.NewLine + "User Name : " + username + Environment.NewLine + "Email ID : " + to;

            objSmtpClient.Port = 587;
            objSmtpClient.UseDefaultCredentials = false;
            objSmtpClient.EnableSsl = true;
            objSmtpClient.Credentials = new System.Net.NetworkCredential(dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString());
            objSmtpClient.Send(objMailMessage);
        }
        catch (Exception ex)
        { throw ex; }

        finally
        {
            objMailMessage = null;
            objSMTPUserInfo = null;
            objSmtpClient = null;
        }
    }
    
    //private void sendmail(string to, string username) //Correct- Running code
    //{
    //        MailMessage objMailMessage = new MailMessage();
    //        System.Net.NetworkCredential objSMTPUserInfo = new System.Net.NetworkCredential();
    //        SmtpClient objSmtpClient = new SmtpClient("smtp.office365.com");
          
    //        try
    //        {
    //            objMailMessage.From = new MailAddress("anu.verma@tectura.com");
    //            objMailMessage.To.Add(new MailAddress(to));
    //            objMailMessage.Subject = "TMS - Reset Password Reuqest ";
    //            objMailMessage.Body = "Reset Password request for TMS : " + Environment.NewLine + "User Name : " + username + Environment.NewLine + "Email ID : " + to;
                             
    //            objSmtpClient.Port = 587;
    //            objSmtpClient.UseDefaultCredentials = false;
    //            objSmtpClient.EnableSsl = true;  
    //            objSmtpClient.Credentials = new System.Net.NetworkCredential("averma8546@tectura.com","intel.1005"); 
    //            objSmtpClient.Send(objMailMessage);
    //        }
    //        catch (Exception ex)
    //        { throw ex; }

    //        finally
    //        {
    //            objMailMessage = null;
    //            objSMTPUserInfo = null;
    //            objSmtpClient = null;
    //        }
    //    }
  
    private DataTable GetData(string query)
    {
        using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
        {
            using (SqlConnection connection = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            {
                DataTable dt = new DataTable();
                connection.Open();
                string sqlStatement = query;
                SqlCommand sqlCmd = new SqlCommand(sqlStatement, connection);
                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);
                sqlDa.Fill(dt);
                return dt;
            }
        }
    }


    


}