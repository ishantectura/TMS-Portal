using BusinessLayerClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class PendingForGateEntry : System.Web.UI.Page
{
    #region Variable Declaration
    DataTable dtOperationalStatus = null;
    DataTable dtCancellationReasonCode = null;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGridview("","");
            HideControl();
            txtcity.Text = "";
           
        }
        else
        {
            
            lblmsg.Text = "";
            lblErr.Text = "";        
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserType"].ToString() == "Transporter"  )
            {
                this.grdPendingRequest.Columns[(this.grdPendingRequest.Columns.Count - 4)].Visible = false;
                this.grdPendingRequest.Columns[(this.grdPendingRequest.Columns.Count - 1)].Visible = false;

            }
        }
    }
    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    #endregion

    #region Grid_event : OnPaging
    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        BindGridview("","");
        grdPendingRequest.PageIndex = e.NewPageIndex;
        grdPendingRequest.DataBind();
    }
    #endregion

    #region Grid_event: OnRowDataBound
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        int index = Convert.ToInt32(e.Row.DataItemIndex.ToString());
        if (index != -1)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region disable dropdowns on the basis of logged In user
                int datakey = 0;
                object objTemp = grdPendingRequest.DataKeys[e.Row.RowIndex].Value as object;
                if (objTemp != null)
                {
                    datakey = Convert.ToInt32(objTemp);

                }
               
                        DropDownList ddlReasonCode = (e.Row.FindControl("ddlReasonCode") as DropDownList);
                        DataTable dt1 = new DataTable();
                        dt1 = GetCancellationReasonCode(datakey);
                       if (Session["UserType"].ToString() == "CIPL")
                        {
                            ddlReasonCode.DataSource = dt1;
                            ddlReasonCode.DataTextField = "Description";
                            ddlReasonCode.DataValueField = "Code";
                            ddlReasonCode.DataBind();

                            //Set dropdown Selected text- row wise
                            Label lReason = (e.Row.FindControl("lblReasonCode") as Label);
                            if (lReason.Text != "")
                            {
                                ddlReasonCode.Items.FindByValue(lReason.Text).Selected = true;
                                lReason.Visible = false;
                            }
                            else
                            {
                                ddlReasonCode.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                                ddlReasonCode.SelectedIndex = 0;
                            }
                      }
               
                if (Session["UserType"].ToString() == "Transporter") // Disable ReasonCode dropdown for Transporter user
                {
                    //ddlReasonCode.Enabled = false;
                    //ddlReasonCode.BackColor = System.Drawing.Color.Yellow;
                    //// Disable Cancel Button for Transporter user
                    //Button btncancel = (e.Row.Cells[14].Controls[0] as Button);
                    //btncancel.Enabled = false;
                    //btncancel.BackColor = System.Drawing.Color.Yellow;      
                }
                if (Session["UserType"].ToString() == "CIPL") // disable TransporterResponse ropdown for CIPL user
                {
                    // Disable transporter response and select Row checkbox for CIPL User
                    DropDownList ddlTransporterResponse = (e.Row.FindControl("ddlTransporterResponse") as DropDownList);

                    Label lblTransResponse = (e.Row.FindControl("lblT") as Label);
                    if (lblTransResponse.Text != "")
                    {
                        ddlTransporterResponse.Items.FindByText(lblTransResponse.Text).Selected = true;                       
                    }
                    lblTransResponse.Visible = false;
                    ddlTransporterResponse.Enabled = false;
                    ddlTransporterResponse.BackColor = System.Drawing.Color.Cornsilk;
                    CheckBox chkSelectRow = (e.Row.FindControl("chkSelectedRow") as CheckBox);
                   // CheckBox chkSelectRow = (e.Row.FindControl("chkEmp") as CheckBox);
                    chkSelectRow.Visible = false;
                }
                #endregion
                
                #region Commented Code
                //commented by ANU - As no dropdown needed- replacing it by label
                //DropDownList ddlTransporterResponse = (e.Row.FindControl("ddlTransporterResponse") as DropDownList);
                //DataTable dt = new DataTable();
                //dt = GetOperationalStatus();
                //ddlTransporterResponse.DataSource = dt;
                //ddlTransporterResponse.DataTextField = "Option Name";
                //ddlTransporterResponse.DataValueField = "Option Id";
                //ddlTransporterResponse.DataBind();
                //Label l = (e.Row.FindControl("lblTransporterResponse") as Label);
                //if (l.Text != "")
                //{
                //    ddlTransporterResponse.Items.FindByText(l.Text).Selected = true;
                //    l.Visible = false;
                //}
                //if (Session["UserType"].ToString() == "CIPL")
                //{
                //    ddlTransporterResponse.Enabled = false;
                //    ddlTransporterResponse.BackColor = System.Drawing.Color.Yellow;
                //}
                #endregion
            }           
        }
    }
    #endregion

    #region Grid_event: OnRowDataBoundExcel
    protected void OnRowDataBoundExcel(object sender, GridViewRowEventArgs e)
    {
        int index = Convert.ToInt32(e.Row.DataItemIndex.ToString());
        if (index != -1)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region disable dropdowns on the basis of logged In user
                int datakey = 0;
                object objTemp = grdPendingRequestExcel.DataKeys[e.Row.RowIndex].Value as object;
                if (objTemp != null)
                {
                    datakey = Convert.ToInt32(objTemp);

                }

                DropDownList ddlReasonCode = (e.Row.FindControl("ddlReasonCode") as DropDownList);
                DataTable dt1 = new DataTable();
                dt1 = GetCancellationReasonCode(datakey);
                if (Session["UserType"].ToString() == "CIPL")
                {
                    ddlReasonCode.DataSource = dt1;
                    ddlReasonCode.DataTextField = "Description";
                    ddlReasonCode.DataValueField = "Code";
                    ddlReasonCode.DataBind();

                    //Set dropdown Selected text- row wise
                    Label lReason = (e.Row.FindControl("lblReasonCode") as Label);
                    if (lReason.Text != "")
                    {
                        ddlReasonCode.Items.FindByValue(lReason.Text).Selected = true;
                        lReason.Visible = false;
                    }
                    else
                    {
                        ddlReasonCode.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                        ddlReasonCode.SelectedIndex = 0;
                    }
                }

                if (Session["UserType"].ToString() == "Transporter") // Disable ReasonCode dropdown for Transporter user
                {
                    //ddlReasonCode.Enabled = false;
                    //ddlReasonCode.BackColor = System.Drawing.Color.Yellow;
                    //// Disable Cancel Button for Transporter user
                    //Button btncancel = (e.Row.Cells[14].Controls[0] as Button);
                    //btncancel.Enabled = false;
                    //btncancel.BackColor = System.Drawing.Color.Yellow;      
                }
                if (Session["UserType"].ToString() == "CIPL") // disable TransporterResponse ropdown for CIPL user
                {
                    // Disable transporter response and select Row checkbox for CIPL User
                    DropDownList ddlTransporterResponse = (e.Row.FindControl("ddlTransporterResponse") as DropDownList);

                    Label lblTransResponse = (e.Row.FindControl("lblT") as Label);
                    if (lblTransResponse.Text != "")
                    {
                        ddlTransporterResponse.Items.FindByText(lblTransResponse.Text).Selected = true;
                    }
                    lblTransResponse.Visible = false;
                    ddlTransporterResponse.Enabled = false;
                    ddlTransporterResponse.BackColor = System.Drawing.Color.Cornsilk;
                    CheckBox chkSelectRow = (e.Row.FindControl("chkSelectedRow") as CheckBox);
                    // CheckBox chkSelectRow = (e.Row.FindControl("chkEmp") as CheckBox);
                    chkSelectRow.Visible = false;
                }
                #endregion

                #region Commented Code
                //commented by ANU - As no dropdown needed- replacing it by label
                //DropDownList ddlTransporterResponse = (e.Row.FindControl("ddlTransporterResponse") as DropDownList);
                //DataTable dt = new DataTable();
                //dt = GetOperationalStatus();
                //ddlTransporterResponse.DataSource = dt;
                //ddlTransporterResponse.DataTextField = "Option Name";
                //ddlTransporterResponse.DataValueField = "Option Id";
                //ddlTransporterResponse.DataBind();
                //Label l = (e.Row.FindControl("lblTransporterResponse") as Label);
                //if (l.Text != "")
                //{
                //    ddlTransporterResponse.Items.FindByText(l.Text).Selected = true;
                //    l.Visible = false;
                //}
                //if (Session["UserType"].ToString() == "CIPL")
                //{
                //    ddlTransporterResponse.Enabled = false;
                //    ddlTransporterResponse.BackColor = System.Drawing.Color.Yellow;
                //}
                #endregion
            }
        }
    }
    #endregion

    #region Grid_Event : OnRowCommand
    protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "NextGrid")
        {
            LinkButton lb = (LinkButton)e.CommandSource;
            GridViewRow gvr = (GridViewRow)lb.NamingContainer;
            Label lblReqNo = gvr.FindControl("lblRequestNo") as Label;
            string description = lblReqNo.Text;
            grdPendingRequest.Visible = false;
            grdPendingRequest.Visible = true;
            if (lblReqNo != null)
            {
                BindGridRequestPermit(description); //Fill the Data for GridView2 here and pass description as parameter                
            }
        }
        else if (e.CommandName == "CancelRequest") //GridView Button- btnCancel click Event
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvRow = grdPendingRequest.Rows[rowIndex];
            //Get the value of column from the DataKeys using the RowIndex.
            string key = grdPendingRequest.DataKeys[rowIndex].Values[0].ToString();

            DropDownList ddlReasonCode = gvRow.FindControl("ddlReasonCode") as DropDownList;
            Label lblmandatoryReasonCode = gvRow.FindControl("lblmandatoryReasonCode") as Label;
            if (ddlReasonCode != null)
            {
                if (ddlReasonCode.SelectedItem.Text == "") // nothing selected
                {
                    lblmandatoryReasonCode.Text = "Please select Cancellation Reason code";
                    lblmandatoryReasonCode.Visible = true;
                }
                else
                {
                    SubmitCIPLRequest(key, gvRow);
                }
            }
        }
        else
        {
          
            grdRequestPermit.Visible = false;
            grdRequestPermit = null;
            lblpermit.Text = "";
        }
    }
    #endregion
    
    //#region GridView Dropdown - ddlReasonCode selected index changed Event
    //protected void ddlReasonCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
    //    DropDownList ddlReasonCode = gvRow.FindControl("ddlReasonCode") as DropDownList;
    //    Label lblmandatoryReasonCode = gvRow.FindControl("lblmandatoryReasonCode") as Label;
    //    if (ddlReasonCode.SelectedIndex != 0) // nothing selected
    //    {
    //        lblmandatoryReasonCode.Text = "";
    //        lblmandatoryReasonCode.Visible = false;
    //    }
    //}
    //#endregion

    #region Submit Button Click Event
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Session["UserType"].ToString() == "Transporter")
        {
            SubmitTransporterRequest();
        }     
    }
    #endregion

    #region Submit button by Transporter
    private void SubmitTransporterRequest()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ID"));
        dt.Columns.Add(new DataColumn("ReqNo")); 
        dt.Columns.Add(new DataColumn("Transporter Response"));
        dt.Columns.Add(new DataColumn("Cancellation Reason Code"));
        dt.Columns.Add(new DataColumn("Ready for Sending"));
        dt.Columns.Add(new DataColumn("Cancellation Remarks"));

        foreach (GridViewRow row in grdPendingRequest.Rows)
        {
            string key = grdPendingRequest.DataKeys[row.RowIndex].Value.ToString();
            DropDownList ddlTransporterResponse = row.FindControl("ddlTransporterResponse") as DropDownList;
            DropDownList ddlReasonCode = row.FindControl("ddlReasonCode") as DropDownList;
            //ddlReasonCode.SelectedIndex
            CheckBox chkSelectedRow = row.FindControl("chkSelectedRow") as CheckBox;
            

            TextBox txtTransporterRemark = row.FindControl("txtTransporterRemark") as TextBox;

            if (chkSelectedRow.Checked)
            {
                //Label lblTransporterResponse = row.FindControl("lblTransporterResponse") as Label;
                //if (ddlTransporterResponse.SelectedIndex == 0) // nothing selected
                //{
                //    lblTransporterResponse.Text = "Please select Transporter Response";
                //    lblTransporterResponse.Visible = true;
                //    return;
                //}

                DataRow dr = dt.NewRow();
                dr["ID"] = key;
                dr["ReqNo"] =  grdPendingRequest.Rows[row.RowIndex].Cells[0].Text;
                dr["Transporter Response"] = ddlTransporterResponse.SelectedItem.Text;
               // string a = ddlReasonCode.SelectedItem.Value;
                if (Session["UserType"].ToString() == "Transporter")
                    dr["Cancellation Reason Code"] = "";
                else
                    dr["Cancellation Reason Code"] = ddlReasonCode.SelectedItem.Value;
                dr["Ready for Sending"] = chkSelectedRow.Checked ? 1 : 0;
                dr["Cancellation Remarks"] = txtTransporterRemark.Text;
                dt.Rows.Add(dr);
            }
        }

        /****************************************************************** DataTable to get Mail data from stored procedure ***********************/
        DataTable dtForMail = new DataTable();
        dtForMail.Columns.Add(new DataColumn("ReqNo"));
        foreach (GridViewRow row1 in grdPendingRequest.Rows)
        {
            string key = grdPendingRequest.DataKeys[row1.RowIndex].Value.ToString();
            CheckBox chkSelectedRow = row1.FindControl("chkSelectedRow") as CheckBox;
            if (chkSelectedRow.Checked)
            {
                DataRow dr1 = dtForMail.NewRow();
                dr1["ReqNo"] = grdPendingRequest.Rows[row1.RowIndex].Cells[0].Text;
                dtForMail.Rows.Add(dr1);
            }        
        }


        if (dt.Rows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                con.Open();
                SqlCommand cmd = new SqlCommand("TMS_sp_update_Pending_Request");
                SqlTransaction transaction;
                transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted); // Start a local transaction.
                cmd.Connection = con;
                cmd.Transaction = transaction;
                #region Transaction
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMSRequestUpdateTable", dt);
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    int rowaffected = cmd.ExecuteNonQuery();
                    string mail_subject = "";
                    string body = "";
                    foreach (DataRow drow in dt.Rows)
                    {
                        if (drow[2].ToString() == "Rejected")
                        {
                            SendDatatoNAV objSendDatatoNAV = new SendDatatoNAV(drow[1].ToString());
                            mail_subject = "TMS : Request Rejected By Transporter - " + DateTime.Now.ToShortDateString();
                            body = "Following requests are rejected ! ";
                        }
                        else
                        {
                            mail_subject = "TMS : Request Accepted By Transporter - " + DateTime.Now.ToShortDateString();
                            body = "Following requests are accepted ! ";
                        }
                    }
                   // SendMailToCIPL(dtForMail, mail_subject, body);
                    transaction.Commit();
                    con.Close();
                    BindGridview("","");
                    lblmsg.Text = "Request successfully updated";
                    lblmsg.Visible = true;
                    lblmsg.ForeColor = System.Drawing.Color.Green;

                }
                catch (Exception ex1)
                {
                    try
                    {
                        transaction.Rollback(); //manual                             
                        con.Close();
                        lblmsg.Text = ex1.Message;
                        lblmsg.Visible = true;
                    }
                    catch (Exception ex2)
                    {
                        lblmsg.Text = ex2.Message;
                        lblmsg.Visible = true;
                    }
                }
                #endregion
            }   
        }
        else
        {
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = "Please select row(s) to submit";
            lblmsg.Visible = true;
        }
    }
    #endregion

    #region Submit request by CIPL User
    private void SubmitCIPLRequest(string RowDatakey, GridViewRow row)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ID"));
        dt.Columns.Add(new DataColumn("ReqNo"));  //commented by anu - As status updation no longer needed
        dt.Columns.Add(new DataColumn("Transporter Response"));
        dt.Columns.Add(new DataColumn("Cancellation Reason Code"));
        dt.Columns.Add(new DataColumn("Ready for Sending"));
        dt.Columns.Add(new DataColumn("Cancellation Remarks"));
       
        DropDownList ddlReasonCode = row.FindControl("ddlReasonCode") as DropDownList;
        
        // CheckBox chkSelectedRow = row.FindControl("chkSelectedRow") as CheckBox;
        TextBox txtTransporterRemark = row.FindControl("txtTransporterRemark") as TextBox;

        if (ddlReasonCode.SelectedIndex != 0 || ddlReasonCode.SelectedItem.Text != "")
        {
            DataRow dr = dt.NewRow();
            dr["ID"] = RowDatakey;
            dr["ReqNo"]= grdPendingRequest.Rows[row.RowIndex].Cells[0].Text;
            dr["Transporter Response"] = ""; //No use in SP
            dr["Cancellation Reason Code"] = ddlReasonCode.SelectedItem.Value;
            dr["Ready for Sending"] = 1;  // to be discuss for CIPL //rigt now not used in SP - No updation performed
            dr["Cancellation Remarks"] = txtTransporterRemark.Text;     
            dt.Rows.Add(dr);
        }

        /****************************************************************** DataTable to send Mail data from stored procedure ***********************/
        DataTable dtForMail = new DataTable();   
        dtForMail.Columns.Add(new DataColumn("ReqNo"));  
        DataRow dr1 = dtForMail.NewRow();
        dr1["ReqNo"] = grdPendingRequest.Rows[row.RowIndex].Cells[0].Text;
        dtForMail.Rows.Add(dr1);
            
        /**********************************************************************************************************************************************/

        if (dt.Rows.Count > 0)
        {
                 
            using (SqlConnection con = new SqlConnection())
                 {
                     con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                     con.Open();
                     SqlCommand cmd = new SqlCommand("TMS_sp_update_Pending_Request_for_CIPL");
                     SqlTransaction transaction;
                     transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted); // Start a local transaction.
                     cmd.Connection = con;
                     cmd.Transaction = transaction;
                     #region Transaction
                     try
                     {                         
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMSRequestUpdateTable", dt);
                        sqlParameter.SqlDbType = SqlDbType.Structured;                        
                        int rowaffected= cmd.ExecuteNonQuery();
                      
                         SendDatatoNAV objSendDatatoNAV = new SendDatatoNAV(dt.Rows[0][1].ToString());  
                         transaction.Commit();
                         con.Close();                         
                         BindGridview("","");
                         lblmsg.Text = " Canceled Successfully";
                         lblmsg.Visible = true;
                         lblmsg.ForeColor = System.Drawing.Color.Green;
                         SendMailToTransporter(dtForMail); // send email                            
                     }
                     catch (Exception ex1)
                     {
                         try
                         {
                             transaction.Rollback(); //manual                             
                             con.Close();
                             lblmsg.Text = ex1.Message;
                             lblmsg.Visible = true;
                         }
                         catch (Exception ex2)
                         {
                             lblmsg.Text = ex2.Message;
                             lblmsg.Visible = true;
                         }
                     }
                     #endregion              
                 }
        }
    }
    #endregion

    #region Bind Gridview
    private void BindGridview(string city,string ReqNo)
    {

        if (Session["UserType"].ToString() == "Transporter")
        {
            if (txtcity.Text == "")
            {
                grdPendingRequest.DataSource = (new PendingRequestForGateEntry()).GetPendingRequestsForGateEntry(Session["UserName"].ToString(), city, ReqNo);
                //Added by Jyothi
                grdPendingRequestExcel.DataSource = (new PendingRequestForGateEntry()).GetPendingRequestsForGateEntry(Session["UserName"].ToString(), city, ReqNo);
            }
            else
            {
                grdPendingRequest.DataSource = (new PendingRequestForGateEntry()).GetPendingRequestsForGateEntry(Session["UserName"].ToString(), city, ReqNo);
                //Added by Jyothi
                grdPendingRequestExcel.DataSource = (new PendingRequestForGateEntry()).GetPendingRequestsForGateEntry(Session["UserName"].ToString(), city, ReqNo);
            }
        }
        if (Session["UserType"].ToString() == "CIPL")
        {
            if (txtcity.Text == "")
            {
                grdPendingRequest.DataSource = (new PendingRequestForGateEntry()).GetPendingRequestsForGateEntryCIPL(Session["UserName"].ToString(), city, ReqNo);
                //Added by Jyothi
                grdPendingRequestExcel.DataSource = (new PendingRequestForGateEntry()).GetPendingRequestsForGateEntryCIPL(Session["UserName"].ToString(), city, ReqNo);
            }
            else
            {
                grdPendingRequest.DataSource = (new PendingRequestForGateEntry()).GetPendingRequestsForGateEntryCIPL(Session["UserName"].ToString(), city, ReqNo);
                //Added by Jyothi
                grdPendingRequestExcel.DataSource = (new PendingRequestForGateEntry()).GetPendingRequestsForGateEntryCIPL(Session["UserName"].ToString(), city, ReqNo);
            }
        }

        grdPendingRequest.DataBind();
        //Added by jyothi
        grdPendingRequestExcel.DataBind();
        
    }
    #endregion

    #region Bind RequestPermit Gridview
    public void BindGridRequestPermit(string reqid)
    {
        try
        {
            VeiwPermits viewPermits = new VeiwPermits();
            if (viewPermits.GetPermits(reqid).Count > 0)
            {
                grdRequestPermit.DataSource = viewPermits.GetPermits(reqid);
                grdRequestPermit.DataBind();
                grdRequestPermit.Visible = true;
                lblpermit.Text = "Permit Details";
                lblpermit.Visible = true;
            }
            else
            {
                lblErr.Text = "No Permit details Available";            
                grdRequestPermit = null;
                grdRequestPermit.Visible = false;
            }
        }
        catch (Exception e)
        {
            lblErr.Text = " Some error occured, data missing in column, please contact Administrator/team";
        }
    }
    #endregion

    #region Get OperationalStatus from master table
    private DataTable GetOperationalStatus()
    {
        if (dtOperationalStatus != null)
            return dtOperationalStatus;
        if (Application["RequestOptionOperationalStatus"] != null)
        {
            dtOperationalStatus = Application["RequestOptionOperationalStatus"] as DataTable;
            return dtOperationalStatus;
        }
        else
        {
            string sqlStatement = "SELECT [Option Id] ,[Option Name] FROM [TMS_Request_Option_Operational_Status]";
            using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                {
                    dtOperationalStatus = new DataTable();
                    con.Open();
                    dtOperationalStatus.Load(cmd.ExecuteReader());
                    Application["RequestOptionOperationalStatus"] = dtOperationalStatus;
                    return dtOperationalStatus;
                }
            }
        }

    }
    #endregion

    #region Get Cancellation ReasonCode from Master table
    private DataTable GetCancellationReasonCode(int ReqID)
    {
            string sqlStatement = "SELECT [Code],[Description] FROM [dbo].[TMS_Cancellation_Reason_Code] where company_Name_FK = (select t.company_Name_FK from TMS_Request t where t.ID = " + ReqID + ")";
            using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                {
                    dtCancellationReasonCode = new DataTable();
                    con.Open();
                    dtCancellationReasonCode.Load(cmd.ExecuteReader());
                    Application["CancellationReasonCodes"] = dtCancellationReasonCode;
                    return dtCancellationReasonCode;
                }
            }        
    }
    #endregion
    
    #region Reset controls
    private void ResetControls()
    {
        lblmsg.Text = "";
        lblmsg.Visible = false;
    }
    #endregion

    #region Hide Control
    private void HideControl()
    {
        if (Session["UserType"].ToString() == "CIPL")
        {
            btnSubmit.Visible = false;
        }      

    }
    #endregion


    //public void SendMailToCIPL(DataTable dtRequestNoList, string MailSubject , string body)
    //{
    //    DataTable dtForMail = new DataTable();
    //    dtForMail.Columns.Add("emailid", typeof(string));
    //    dtForMail.Columns.Add("PortalUserId", typeof(string));
    //    DataTable dtConsolidated = new DataTable();
    //    dtConsolidated.Columns.Add(new DataColumn("to"));
    //    dtConsolidated.Columns.Add(new DataColumn("username"));

    //    foreach (DataRow dfRow in dtRequestNoList.Rows)
    //    {
    //        DataTable dtk = new DataTable();
    //        dtk = GetData("select PUS.ID, UTM.company_Name_FK , PUS.[Portal User Id] as PortalUserId, PUS.[E-mail ID] as emailid from TMS_Portal_User_Setup PUS " +
    //                                    "with (nolock) INNER JOIN TMS_User_Transporter_Mapping  UTM on PUS.[Portal User Id]= UTM.[Portal User ID] " +
    //                                    "and UTM.[Transporter Code] = (Select LR.[Transporter Code] from TMS_Request LR where No_ = '" + dfRow[0] + "') " +
    //                                    "and UTM.company_Name_FK =  (Select LR.company_Name_FK from TMS_Request LR where No_  = '" + dfRow[0] + "') " +
    //                                    "Where PUS.[User Type] = 'CIPL'");
    //        if(dtk.Rows.Count>0)
    //        {
    //            for (int i = 0; i < dtk.Rows.Count; i++)
    //            {
    //                dtForMail.Rows.Add(dtk.Rows[i][3].ToString(), dtk.Rows[i][2].ToString());
    //            }
    //        }
    //      }
        
    //    dtForMail = RemoveDuplicateRows(dtForMail, "emailid");
    //    foreach (DataRow dr in dtForMail.Rows)
    //    {
    //        string to = dr[0].ToString(); //Email ID of Receiver
    //        string username = dr[1].ToString(); //User Name of Receiver
    //        MailMessage objMailMessage = new MailMessage();
    //        SmtpClient objSmtpClient = new SmtpClient("smtp.office365.com");
    //        try
    //        {
    //            DataTable dt = GetData("SELECT top 1 [E-Mail Address to Send Mails] as [FromEmailID] ,[Service Account] as [UserName] ,[Service Password] as [Password] FROM [dbo].[TMS_Setup] with (nolock) where company_Name_FK = (select t.company_Name_FK from TMS_Request t where t.[No_] = '" + dtRequestNoList.Rows[0][0].ToString() + "')");
    //            if (dt.Rows.Count > 0)
    //            {
    //                objMailMessage.From = new MailAddress(dt.Rows[0][0].ToString()); //Email ID of Sender
    //                objMailMessage.To.Add(new MailAddress(to));
    //            }
    //            objMailMessage.Subject = MailSubject;  
                
    //            DataTable QueryResultingData = new DataTable();
    //            using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
    //            {
    //                con.Open();
    //                SqlTransaction transaction;
    //                transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted);                    
    //                SqlCommand cmd = new SqlCommand("TMS_sp_get_MailDataForRequest");
    //                cmd.Transaction = transaction;
    //                cmd.Connection = con;
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@RequestNoTable", dtRequestNoList);
    //                sqlParameter.SqlDbType = SqlDbType.Structured;                  
    //                QueryResultingData.Load(cmd.ExecuteReader());
    //                transaction.Commit();
    //                con.Close();
    //            }

    //            StringBuilder sb = new StringBuilder();
    //            sb.Append("DEAR Team, <br/> " +
    //                      "<br/> " +
    //                      "SUB: CIPL-VEHICLE PLACEMENT INDENT-DTD - " + DateTime.Now.ToShortDateString() + "<br/><br/>" +
    //                       body +"<br/> " +
    //                      "Acceptance of Vehicles reaching beyond agreed terms is on solely carlsberg's decision. <br>"+
    //                      "Below are the rquest accepted by Transporter - <br/><br/><br/>");                        

    //            StringWriter sw = new StringWriter(sb);
    //            HtmlTextWriter hw = new HtmlTextWriter(sw);
    //            GridView gr = new GridView();
    //            gr.DataSource = QueryResultingData;
    //            gr.DataBind();
    //            gr.RenderControl(hw);

    //            sb.Append("<br/><br/> Thanks and Regards, <br/><br/>Logistics Team<br/>");

    //            objMailMessage.Body = sb.ToString();   // sb.ToString() will be included in email body
    //            objMailMessage.IsBodyHtml = true;

    //            objSmtpClient.Port = 587;
    //            objSmtpClient.UseDefaultCredentials = false;
    //            objSmtpClient.EnableSsl = true;
    //            objSmtpClient.Credentials = new System.Net.NetworkCredential(dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString());
    //            objSmtpClient.Send(objMailMessage);
    //        }
    //        catch (Exception ex)
    //        { throw ex; }

    //        finally
    //        {
    //            objMailMessage = null;
    //            objSmtpClient = null;
    //        }
    //    }
    //}


    /// <summary>
    /// Execute SQL Query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    private DataTable GetData(string query)
    {
        using (SqlConnection con1 = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
        {
            con1.Open();
            SqlTransaction transaction;
            transaction = con1.BeginTransaction(IsolationLevel.ReadUncommitted);
            using (SqlCommand cmd = new SqlCommand(query, con1, transaction))
            {
               // Start a local transaction.  
                DataTable dt = new DataTable();              
                dt.Load(cmd.ExecuteReader());
                transaction.Commit();
                con1.Close();
                return dt;
            }
        }
    }


    public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();

        //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
        //And add duplicate item value in arraylist.
        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
                duplicateList.Add(drow);
            else
                hTable.Add(drow[colName], string.Empty);
        }

        //Removing a list of duplicate items from datatable.
        foreach (DataRow dRow in duplicateList)
            dTable.Rows.Remove(dRow);

        //Datatable which contains unique records will be return as output.
        return dTable;
    }


    private void SendMailToTransporter(DataTable dtRequestNoList)
    {       
        DataTable dTable = GetData("select PUS.ID, UTM.company_Name_FK , PUS.[Portal User Id], PUS.[E-mail ID]  from TMS_Portal_User_Setup PUS " +
                                    "INNER JOIN TMS_User_Transporter_Mapping  UTM on PUS.[Portal User Id]= UTM.[Portal User ID] " +
                                    "and UTM.[Transporter Code] = (Select TR.[Transporter Code] from TMS_Request TR where [No_] = '" + dtRequestNoList.Rows[0][0].ToString() + "') " +
                                    "and UTM.company_Name_FK =  (Select TR.company_Name_FK  from TMS_Request TR where [No_] = '" + dtRequestNoList.Rows[0][0].ToString() + "') " +
                                    "Where PUS.[User Type] = 'Transporter'");
        foreach (DataRow dr in dTable.Rows)
        {

            string to = dr[3].ToString();
            string username = dr[2].ToString();
            MailMessage objMailMessage = new MailMessage();
            System.Net.NetworkCredential objSMTPUserInfo = new System.Net.NetworkCredential();
            SmtpClient objSmtpClient = new SmtpClient("smtp.office365.com");
            try
            {
                DataTable dt = GetData("SELECT top 1 [E-Mail Address to Send Mails] as [FromEmailID] ,[Service Account] as [UserName] ,[Service Password] as [Password] FROM [dbo].[TMS_Setup]");
                objMailMessage.From = new MailAddress(dt.Rows[0][0].ToString());
                objMailMessage.To.Add(new MailAddress(to));
                objMailMessage.Subject = "TMS : REQUEST CANCELED - " + DateTime.Now.ToShortDateString();

               DataTable QueryResultingData = GetData("select ROW_NUMBER() OVER (ORDER BY TL.Name) AS [SR No.], TR.No_  as [Reuqest No],   TR.[Transporter Code],TL.Name as [From], CASE WHEN TR.[Shipment Type]= 'Sales' Then TC.Name  When TR.[Shipment Type] = 'Stock Transfer' then TLN.Name Else '' end as [To] "+
                            ",TR.[Destination City] as [City], TR.[Truck Size] from TMS_Request TR "+
                            "Inner Join TMS_Location TL on TR.[Responsibility Center] = TL.Code "+
                            " left outer join TMS_Customer TC on TR.Destination = TC.No_  and TR.company_Name_FK = TC.company_Name_FK " +
                            " left outer join TMS_Location TLN on TR.Destination = TLN.Code and TR.company_Name_FK = TLN.company_Name_FK " +
                            "where TR.No_ = '" + dtRequestNoList.Rows[0][0].ToString() + "'" +
                            "group by TR.No_ ,TR.[Transporter Code],TL.Name,TR.[Shipment Type],TC.Name,TLN.Name,TR.[Destination City], TR.[Truck Size]");
                   
                StringBuilder sb = new StringBuilder();

                sb.Append("DEAR Team, <br/> SUB:Following request is canceled by CIPL - " + DateTime.Now.ToShortDateString() + "<br/><br/>" +
                          "Please keep updating the vehicle status report on daily basis. <br/><br/>");

                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gr = new GridView();
                gr.DataSource = QueryResultingData;
                gr.DataBind();
                gr.RenderControl(hw);

                sb.Append("<br/><br/> Thanks and Regards, <br/><br/>Logistics Team<br/>");

                objMailMessage.Body = sb.ToString();   // sb.ToString() will be included in email body
                objMailMessage.IsBodyHtml = true;

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
    }

    
 protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)grdPendingRequest.HeaderRow.FindControl("chkboxSelectAll");
        foreach (GridViewRow row in grdPendingRequest.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelectedRow");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }

 protected void txtcity_TextChanged(object sender, EventArgs e)
 {
     if (txtcity.Text != "")
         BindGridview(txtcity.Text.ToString(), "");
     else if ((txtcity.Text != "") && (txtReqNo.Text != ""))
         BindGridview(txtcity.Text.ToString(), txtReqNo.Text.ToString());
     else
         BindGridview("", "");
 }
 protected void txtReqNo_TextChanged(object sender, EventArgs e)
 {
     if (txtReqNo.Text != "")
         BindGridview("",txtReqNo.Text.ToString());
     else if ((txtcity.Text != "") && (txtReqNo.Text != ""))
         BindGridview(txtcity.Text.ToString(), txtReqNo.Text.ToString());
     else
         BindGridview("","");
 }
 //Added by Jyothi
 #region ExportToExcel
 protected void btnExport_Click(object sender, EventArgs e)
 {
     grdPendingRequestExcel.Visible = true;
     ExportGridToExcel(grdPendingRequestExcel);
     grdPendingRequestExcel.Visible = false;
 }

 private void ExportGridToExcel(GridView gridview)
 {
     Response.Clear();
     Response.Buffer = true;
     Response.ClearContent();
     Response.ClearHeaders();
     Response.Charset = "";
     string FileName = "PendingForGateEntry" + DateTime.Now + ".xls";
     StringWriter strwritter = new StringWriter();
     HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
     Response.Cache.SetCacheability(HttpCacheability.NoCache);
     Response.ContentType = "application/vnd.ms-excel";
     Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
     gridview.GridLines = GridLines.Both;
     gridview.HeaderStyle.Font.Bold = true;
     gridview.RenderControl(htmltextwrtter);
     Response.Write(strwritter.ToString());
     Response.End();

 }
 #endregion
}