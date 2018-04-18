using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Licenseagreement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void chklicense_CheckedChanged(object sender, EventArgs e)
    {
        if (chklicense.Checked)
        {
            if (UpdateLicTerm())
            {
                if (Convert.ToInt16(Session["ChangePassword"]) == 0)  //First Time User login
                {
                    Response.Redirect("ChangePassword.aspx");
                }
                if (Convert.ToInt16(Session["ChangePassword"]) == 1)
                {
                    Response.Redirect("Dashboard.aspx");
                }
            }            
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    private bool UpdateLicTerm()
    {
        int recordUpdated = 0;
        bool result = false;
        try
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                con.Open();
                SqlCommand cmd = new SqlCommand("TMS_sp_update_LicTermCondition");
                SqlTransaction transaction;
                transaction = con.BeginTransaction("Update PortalUserSetup");
                cmd.Connection = con;
                cmd.Transaction = transaction;
                #region Transaction
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(Session["ID"].ToString()));
                    cmd.Parameters.AddWithValue("@LicTermAggrement", 1); // set as true => checked marked

                    recordUpdated = cmd.ExecuteNonQuery(); //saved in database
                    if (recordUpdated > 0)
                    {
                        transaction.Commit();
                        con.Close();
                        result = true;
                    }
                    else
                    {
                        transaction.Rollback();
                        con.Close();
                        lblerr.Text = "Entries cannot be submitted at this time , please try later !!";
                        lblerr.ForeColor = System.Drawing.Color.Red;
                        lblerr.Visible = true;                        
                    }
                }
                catch (Exception ex1)
                {
                    transaction.Rollback(); //manual                            
                    con.Close();
                    lblerr.Text = ex1.Message;
                    lblerr.ForeColor = System.Drawing.Color.Red;
                    lblerr.Visible = true;
                }
                #endregion
            }
        }
        catch (Exception es)
        {
            lblerr.Text = es.Message;
            lblerr.ForeColor = System.Drawing.Color.Red;
            lblerr.Visible = true;      
        }
        return result;
    }
   
}