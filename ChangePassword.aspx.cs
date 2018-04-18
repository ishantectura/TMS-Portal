using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

public partial class ChangePassword : System.Web.UI.Page
{
    BusinessLayerClasses.Cryptography objCrypto = new BusinessLayerClasses.Cryptography();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lblmsg.Text = "";
        }
        else
        {            
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        int recordUpdated = 0;
        bool isDataSendtoNav = false;
        if (txtPassword.Value != "" && txtConfirmPassword.Value != null && txtOldPassword.Value != null)
        {
            if (txtPassword.Value.Trim() == txtConfirmPassword.Value.Trim())
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("TMS_sp_ChangePassword");
                    SqlTransaction transaction;
                    transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted); 
                    cmd.Connection = con;
                    cmd.Transaction = transaction;
                    #region Transaction
                    try
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter parm = new SqlParameter("@Result", SqlDbType.Int);
                        parm.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(parm);
                        cmd.Parameters.AddWithValue("@OldPassword", objCrypto.Encrypt(txtOldPassword.Value.Trim()));
                        cmd.Parameters.AddWithValue("@NewPassword", objCrypto.Encrypt(txtPassword.Value.Trim()));
                        cmd.Parameters.AddWithValue("@Username", Session["UserName"].ToString());
                        cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(Session["ID"]));
                        cmd.Parameters.AddWithValue("@Result", 0);
                                          
                        cmd.ExecuteNonQuery();
                        recordUpdated = Convert.ToInt16(parm.Value);

                        if (recordUpdated == 1)
                        {
                            //Send updated Data to Nav
                            SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
                            isDataSendtoNav = objsenddatatonav.SendPortalUserDatatoNav(Session["ID"].ToString(), Session["UserName"].ToString());  
                        }
                        else if (recordUpdated == 0)
                        {
                            lblmsg.Text = "Old Password is not Correct";
                            lblmsg.Visible = true;
                        }
                        else
                        {
                            lblmsg.Text = "Some Error Occurs at the moment, Please try again or contact Administrator";
                            lblmsg.Visible = true;
                            btnOk.Visible = false;
                        }

                    }
                    catch (Exception ex1)
                    {
                        try
                        {
                            transaction.Rollback(); //manual
                            ResetForm(); // Reset form 
                            con.Close();
                            lblmsg.Text = ex1.Message;
                            lblmsg.Visible = true;
                            return;
                        }
                        catch (Exception ex2)
                        {
                            lblmsg.Text = ex2.Message;
                            lblmsg.Visible = true;
                        }
                    }
                    #endregion

                    #region Commit Transaction
                    if (recordUpdated > 0 && isDataSendtoNav == true)
                    {
                        transaction.Commit();
                        con.Close();
                        Response.Redirect("Login.aspx?s=1");  //moved to Login page again
                    }
                    else
                    {
                        transaction.Rollback(); 
                        ResetForm(); // Reset form 
                        con.Close();
                        lblmsg.Text = "Sorry, Operation cannot be performed , Please contact Administrator !! ";
                        lblmsg.Visible = true;
                    }
                    #endregion

                }
            }
        }
    }

    protected void ResetForm()
    {
        txtPassword.Value = "";
        txtConfirmPassword.Value = "";
        txtOldPassword.Value = "";
    }

}