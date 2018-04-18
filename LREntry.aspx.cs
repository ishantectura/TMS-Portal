using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.IO;

public partial class LREntry : System.Web.UI.Page
{
    #region ****************************** Variable Declaration **********************************
    DataTable dtOperationalStatus = null;
    // DataTable dtCancellationReasonCode = null;
    bool UpdateFlagValue;
    #endregion

    #region ***************************** Page Load *********************************************
    /// <summary>
    /// Page Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["LRNo"] != null)
            {
                ViewState.Add("LRNo", Request.QueryString["LRNo"].ToString());
                string LRNo = Request.QueryString["LRNo"].ToString();
                BindGridview(LRNo);
                BindLRHeader(LRNo);
                EnableHeader_BasedOnUser();
                EnableDisableInvoiceButton(LRNo);
                CalendarExtender1.EndDate = DateTime.Now;
                CalendarExtender2.EndDate = DateTime.Now;
                CalendarExtender3.EndDate = DateTime.Now;
                CalendarExtender4.EndDate = DateTime.Now;
            }
        }
    }
    #endregion ***********************************************************************************

    #region **************************** Grid_event **********************************************
    protected void grdLR_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindGridview(ViewState["LRNo"].ToString());
        grdLR.PageIndex = e.NewPageIndex;
        grdLR.DataBind();
    }


    /// <summary>
    /// Row Data Bound Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdLR_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Int64 datakey = 0;
        try
        {
            int index = Convert.ToInt32(e.Row.DataItemIndex.ToString());
            if (index != -1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Retrieve the key value for the current row. Here it is an int.                   
                    object objTemp = grdLR.DataKeys[e.Row.RowIndex].Value as object;
                    if (objTemp != null)
                    {
                        datakey = Convert.ToInt32(objTemp); ;
                        //Do your operations
                    }

                    TextBox txtTransitLossQty = (e.Row.FindControl("txtTransitLossQty") as TextBox);
                    TextBox txtAccidentalLossQty = (e.Row.FindControl("txtAccidentalLossQty") as TextBox);
                    CheckBox chkVerifiedbyBill = (e.Row.FindControl("chkVerifiedbyBill") as CheckBox);
                    EnableDisableControls(datakey, UpdateFlagValue, txtTransitLossQty, txtAccidentalLossQty, chkVerifiedbyBill);
                }
            }
        }
        catch (Exception eg)
        {
        }
    }

    #endregion

    #region  btnSubmit_Click -Submit LR Event
    /// <summary>
    /// Button Update Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (Session["UserType"].ToString() == "Transporter")
        {
            Update_LR_Line_By_Transporter();
        }
        if (Session["UserType"].ToString() == "Third Party")
        {
            Update_LR_Line_By_CIPL();
        }

        #region commented code
        //string BreweryGateOutwardNo = txtBreweryGateOutwardNo.Text;

        //string GateOutDate = txtGateOutDate.SelectedDate.ToShortDateString();
        //string GOTime = txtGateOutTime.Hour.ToString() + ":" + txtGateOutTime.Minute.ToString() + ":" + txtGateOutTime.Second.ToString() + " " + txtGateOutTime.AmPm;
        //DateTime dt1 = DateTime.Parse(GOTime); // No error check
        //string GateOutTime = dt1.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);


        //string ReachedDestinationDate = txtReachedDestinationDate.SelectedDate.ToShortDateString();
        //string RDTime = txtReachedDestinationTime.Hour.ToString() + ":" + txtReachedDestinationTime.Minute.ToString() + ":" + txtReachedDestinationTime.Second.ToString() + " " + txtReachedDestinationTime.AmPm;
        //DateTime dt2 = DateTime.Parse(RDTime); // No error check
        //string ReachedDestinationTime = dt2.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

        //string TruckReceivedDate = txtTruckReceivedDate.SelectedDate.ToShortDateString();
        //string TRTime = txtTruckReceivedTime.Hour.ToString() + ":" + txtTruckReceivedTime.Minute.ToString() + ":" + txtTruckReceivedTime.Second.ToString() + " " + txtTruckReceivedTime.AmPm;
        //DateTime dt3 = DateTime.Parse(TRTime); // No error check
        //string TruckReceivedTime = dt3.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

        //string TruckReleaseDate = txtTruckReleaseDate.SelectedDate.ToShortDateString();
        //string TRLTime = txtTruckReleaseTime.Hour.ToString() + ":" + txtTruckReleaseTime.Minute.ToString() + ":" + txtTruckReleaseTime.Second.ToString() + " " + txtTruckReleaseTime.AmPm;
        //DateTime dt4 = DateTime.Parse(TRLTime); // No error check
        //string TruckReleaseTime = dt4.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);


        //if (GateOutDate == "01-01-0001" || GateOutTime == "" || ReachedDestinationDate == "01-01-0001" || ReachedDestinationTime == "" || TruckReceivedDate == "01-01-0001" || TruckReceivedTime == "" || TruckReleaseDate == "01-01-0001" || TruckReleaseTime == "")
        //{
        //    lblerr.Text = "Gate Out Date/Time, Reached Destination Date/Time ,Truck Received Date/Time or Truck Release Date/time is not filled by Transporter." + "\n" + Environment.NewLine +
        //         "Entries cannot be submitted before Transporter response";
        //    lblerr.Visible = true;
        //    lblerr.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}

        //UpdateFlagValue = true;
        //int recordUpdated = 0;
        //bool isDataSendtoNav = false;
        //try
        //{
        //    DataTable dt = new DataTable();

        //    dt.Columns.AddRange(new DataColumn[4] { new DataColumn("ID", typeof(int)),
        //                                        new DataColumn("TransitLossQty",  typeof(int)),
        //                                        new DataColumn("AccidentalLossQty", typeof(int)),                                              
        //                                        new DataColumn("VerifiedByBillProcessingTeam", typeof(string))});
        //    int a = grdLR.PageIndex;
        //    for (int i = 0; i < grdLR.PageCount; i++)
        //    {
        //        grdLR.SetPageIndex(i);
        //        foreach (GridViewRow row in grdLR.Rows)
        //        {
        //            string key = grdLR.DataKeys[row.RowIndex].Value.ToString();

        //            TextBox txtTransitLossQty = row.FindControl("txtTransitLossQty") as TextBox;
        //            TextBox txtAccidentalLossQty = row.FindControl("txtAccidentalLossQty") as TextBox;
        //            CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;

        //            DataRow dr = dt.NewRow();
        //            dr["ID"] = int.Parse(key);
        //            if (txtTransitLossQty.Text != "")
        //            {
        //                dr["TransitLossQty"] = int.Parse(txtTransitLossQty.Text);
        //            }
        //            if (txtAccidentalLossQty.Text != "")
        //            {
        //                dr["AccidentalLossQty"] = int.Parse(txtAccidentalLossQty.Text);
        //            }
        //            dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? "1" : "0";

        //            if (Session["UserType"].ToString() == "Transporter")
        //            {
        //                if (txtTransitLossQty.Text != "" || txtAccidentalLossQty.Text != "")
        //                    dt.Rows.Add(dr); // if no breakage value then dont add in DT
        //            }
        //            if (Session["UserType"].ToString() == "CIPL")
        //            {
        //                dt.Rows.Add(dr);// CIPL can update
        //            }
        //        }
        //    }
        //    grdLR.SetPageIndex(a);

        //    UpdateFlagValue = GetUpdateFlagValueFromGrid(dt);
        //    using (SqlConnection con = new SqlConnection())
        //    {
        //        con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("TMS_sp_update_LRLineEntry");
        //        SqlTransaction transaction;
        //        transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted);
        //        cmd.Connection = con;
        //        cmd.Transaction = transaction;
        //        #region Transaction
        //        try
        //        {
        //            cmd.CommandTimeout = 0;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMSRequestUpdateTable", dt);
        //            sqlParameter.SqlDbType = SqlDbType.Structured;
        //            cmd.Parameters.AddWithValue("@UserType", Session["UserType"].ToString());
        //            cmd.Parameters.AddWithValue("@LRNo", lblLRNo.Text.Trim());
        //            if (Session["UserType"].ToString() == "Transporter")
        //            {
        //                cmd.Parameters.AddWithValue("@ApprovalStatus", 3); //Set Pending for Bill processing Team
        //            }
        //            if (Session["UserType"].ToString() == "CIPL") // Check If all rows are verified
        //            {
        //                bool isAllrowVerified = false;
        //                foreach (GridViewRow row in grdLR.Rows)
        //                {
        //                    CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;
        //                    if (chkVerifiedbyBill.Checked)
        //                    {
        //                        isAllrowVerified = true;
        //                    }
        //                    else
        //                    {
        //                        isAllrowVerified = false;
        //                    }
        //                }
        //                if (isAllrowVerified)
        //                    cmd.Parameters.AddWithValue("@ApprovalStatus", 4); //Set Accepted                      
        //                else
        //                    cmd.Parameters.AddWithValue("@ApprovalStatus", 3);
        //            }
        //            cmd.Parameters.AddWithValue("@TransporterRemark", txtTransporterRemark.Text.Trim());
        //            cmd.Parameters.AddWithValue("@BillProcessingRemark", txtBillProcessingRemark.Text.Trim());
        //            //added new parameters on 27July2016
        //            cmd.Parameters.AddWithValue("@BreweryGateOutwardNo", BreweryGateOutwardNo);
        //            cmd.Parameters.AddWithValue("@GateOutDate", GateOutDate);
        //            cmd.Parameters.AddWithValue("@GateOutTime", GateOutTime);
        //            cmd.Parameters.AddWithValue("@ReachedDestinationDate", ReachedDestinationDate);
        //            cmd.Parameters.AddWithValue("@ReachedDestinationTime", ReachedDestinationTime);
        //            cmd.Parameters.AddWithValue("@TruckReceivedDate", TruckReceivedDate);
        //            cmd.Parameters.AddWithValue("@TruckReceivedTime", TruckReceivedTime);
        //            cmd.Parameters.AddWithValue("@TruckReleaseDate", TruckReleaseDate);
        //            cmd.Parameters.AddWithValue("@TruckReleaseTime", TruckReleaseTime);

        //            recordUpdated = cmd.ExecuteNonQuery(); //saved in database
        //            if (recordUpdated > 0)
        //            {
        //                Int32 isAllrowVerified = 0;
        //                isAllrowVerified = GetSQLData("select [dbo].[isAllLRLineVerified]('" + lblLRNo.Text.Trim() + "')");

        //                if (isAllrowVerified == 1)
        //                {
        //                    //Send updated Data to Nav-- Called Web service iof Navision
        //                    SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
        //                    isDataSendtoNav = objsenddatatonav.SendLRtoNAV(lblLRNo.Text.Trim());
        //                }
        //                else
        //                {
        //                    transaction.Commit();
        //                    con.Close();
        //                    lblerr.Text = "Entries submitted successfully";
        //                    lblerr.ForeColor = System.Drawing.Color.Green;
        //                    lblerr.Visible = true;
        //                    BindGridview(ViewState["LRNo"].ToString());
        //                    BindLRHeader(ViewState["LRNo"].ToString());
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                lblerr.Text = "Entries cannot be submitted at this time, Please contact adminstrator ";
        //                lblerr.Visible = true;
        //            }
        //        }
        //        catch (Exception ex1)
        //        {
        //            try
        //            {
        //                transaction.Rollback(); //manual                            
        //                con.Close();
        //                lblerr.Text = ex1.Message;
        //                lblerr.ForeColor = System.Drawing.Color.Red;
        //                lblerr.Visible = true;
        //            }
        //            catch (Exception ex2)
        //            {
        //                lblerr.Text = ex2.Message;
        //                lblerr.ForeColor = System.Drawing.Color.Red;
        //                lblerr.Visible = true;
        //            }
        //        }
        //        #endregion

        //        #region Commit Transaction
        //        if (recordUpdated > 0 && isDataSendtoNav == true)
        //        {
        //            transaction.Commit();
        //            con.Close();
        //            lblerr.Text = "Entries submitted successfully";
        //            lblerr.ForeColor = System.Drawing.Color.Green;
        //            lblerr.Visible = true;
        //        }
        //        else
        //        {
        //            transaction.Rollback();
        //            con.Close();
        //            lblerr.Text = "Entries cannot be submit at this time, Please contact adminstrator ";
        //            lblerr.ForeColor = System.Drawing.Color.Red;
        //            lblerr.Visible = true;
        //        }
        //        #endregion
        //    }
        //    BindGridview(ViewState["LRNo"].ToString());
        //    BindLRHeader(ViewState["LRNo"].ToString());
        //    DisableInvoiceButton(ViewState["LRNo"].ToString());
        //}
        //catch (Exception es)
        //{

        //}
        #endregion
    }
    #endregion

    #region btnInvoice_Click  - Redirect to Invoice Page
    /// <summary>
    /// Button Invoice Event 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnInvoice_Click(object sender, EventArgs e)
    {
        Response.Redirect("FreightInvoice.aspx?LRNo=" + ViewState["LRNo"].ToString());
    }
    #endregion

    #region Model Pop up - btnOk_Click
    protected void btnOK_Click(object sender, EventArgs e)
    {
        mpe.Hide();
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("ID", typeof(int)),
                                                new DataColumn("TransitLossQty",  typeof(int)),
                                                new DataColumn("AccidentalLossQty", typeof(int)),                                              
                                                new DataColumn("VerifiedByBillProcessingTeam", typeof(string))});
            foreach (GridViewRow row in grdLR.Rows)
            {
                string key = grdLR.DataKeys[row.RowIndex].Value.ToString();
                TextBox txtTransitLossQty = row.FindControl("txtTransitLossQty") as TextBox;
                TextBox txtAccidentalLossQty = row.FindControl("txtAccidentalLossQty") as TextBox;
                CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;
                DataRow dr = dt.NewRow();
                dr["ID"] = int.Parse(key);
                if (txtTransitLossQty.Text != "")
                {
                    dr["TransitLossQty"] = decimal.Parse(txtTransitLossQty.Text);
                }
                if (txtAccidentalLossQty.Text != "")
                {
                    dr["AccidentalLossQty"] = decimal.Parse(txtAccidentalLossQty.Text);
                }
                dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? "1" : "0";
                dt.Rows.Add(dr);
            }
            UpdateLR_byCIPL_and_SendDatatoNAV(dt);
        }
        catch (Exception es)
        {
        }
    }
    #endregion

    #region Model Pop up - btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpe.Hide();
        //do nothing
    }

    private void UpdateLR_byCIPL_and_SendDatatoNAV(DataTable dt)
    {
        int recordUpdated = 0;
        bool isDataSendtoNav = false;

        try
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                con.Open();
                SqlCommand cmd = new SqlCommand("TMS_sp_update_LRLine_By_CIPL");
                SqlTransaction transaction;
                transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted);
                cmd.Connection = con;
                cmd.Transaction = transaction;
                #region Transaction
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMSRequestUpdateTable", dt);
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@LRNo", lblLRNo.Text.Trim());
                    bool isAllrowVerified = false;
                    //int b = grdLR.PageIndex; //code to get all rows data including all paging
                    //for (int j = 0; j < grdLR.PageCount; j++)
                    //{
                    //    grdLR.SetPageIndex(j);
                    foreach (GridViewRow row in grdLR.Rows)
                    {
                        CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;
                        if (chkVerifiedbyBill.Checked)
                        {
                            isAllrowVerified = true;
                        }
                        else
                        {
                            isAllrowVerified = false;
                        }
                    }
                    //}
                    //grdLR.SetPageIndex(b);

                    if (isAllrowVerified)
                        cmd.Parameters.AddWithValue("@ApprovalStatus", 4); //Set Accepted                      
                    else
                    {
                        if (isTransitAccidentialValue_Grid(dt)) //if all rows has transit/accidential loss value by transporter
                        {
                            cmd.Parameters.AddWithValue("@ApprovalStatus", 2); //Dispute
                            //SendMailOfDisputeToTransporter(lblLRNo.Text.ToString());
                        }
                        else
                            cmd.Parameters.AddWithValue("@ApprovalStatus", 5); //Set 'Pending for Transporter Action'  
                    }

                    cmd.Parameters.AddWithValue("@BillProcessingRemark", txtBillProcessingRemark.Text.Trim());

                    recordUpdated = cmd.ExecuteNonQuery(); //saved in database
                    if (recordUpdated > 0)
                    {
                        Int32 isAll_rowVerified = 0;
                        isAll_rowVerified = GetSQLData("select [dbo].[isAllLRLineVerified]('" + lblLRNo.Text.Trim() + "')"); //call SQL function

                        if (isAll_rowVerified == 1)
                        {
                            //Send updated Data to Nav-- Called Web service of Navision
                            SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
                            isDataSendtoNav = objsenddatatonav.SendLRtoNAV(lblLRNo.Text.Trim());

                            if (isDataSendtoNav)
                            {
                                cmd = new SqlCommand();
                                cmd.CommandText = "Update TMS_LR_Header set [Approval Status] = 'Pending for Invoice' where [LR No_] = '" + lblLRNo.Text.Trim() + "'";
                                cmd.Connection = con;
                                cmd.Transaction = transaction;
                                int LrStatusUpdate = cmd.ExecuteNonQuery();
                                SendMailToTransporter(lblLRNo.Text);
                            }

                        }
                        else
                        {
                            transaction.Commit();
                            con.Close();
                            lblerr.Text = "Entries submitted successfully";
                            lblerr.ForeColor = System.Drawing.Color.Green;
                            lblerr.Visible = true;
                            BindGridview(ViewState["LRNo"].ToString());
                            BindLRHeader(ViewState["LRNo"].ToString());
                            EnableDisableInvoiceButton(ViewState["LRNo"].ToString());
                            return;
                        }
                    }
                    else
                    {
                        lblerr.Text = "Entries cannot be submitted at this time, Please contact adminstrator ";
                        lblerr.Visible = true;
                    }
                }
                catch (Exception ex1)
                {
                    try
                    {
                        transaction.Rollback(); //manual                            
                        con.Close();
                        lblerr.Text = ex1.Message;
                        lblerr.ForeColor = System.Drawing.Color.Red;
                        lblerr.Visible = true;

                    }
                    catch (Exception ex2)
                    {
                        lblerr.Text = ex2.Message;
                        lblerr.ForeColor = System.Drawing.Color.Red;
                        lblerr.Visible = true;

                    }
                }
                #endregion

                #region Commit Transaction
                if (recordUpdated > 0 && isDataSendtoNav == true)
                {
                    transaction.Commit();
                    con.Close();
                    lblerr.Text = "Entries updated and submitted to Nav system successfully";
                    lblerr.ForeColor = System.Drawing.Color.Green;
                    lblerr.Visible = true;
                }
                else
                {
                    transaction.Rollback();
                    con.Close();
                    lblerr.Text = "Entries cannot be submit at this time, Please contact adminstrator ";
                    lblerr.ForeColor = System.Drawing.Color.Red;
                    lblerr.Visible = true;
                }
                #endregion
            }
            BindGridview(ViewState["LRNo"].ToString());
            BindLRHeader(ViewState["LRNo"].ToString());
            EnableDisableInvoiceButton(ViewState["LRNo"].ToString());
        }
        catch (Exception es)
        {

        }

    }
    #endregion

    #region EnableHeader_BasedOnUser -  Enable- Disable controls as per the logged-in user
    protected void EnableHeader_BasedOnUser()
    {
        if (UpdateFlagValue) // i.e. All rows are verified by BillProcessing Team -> Then disable textbox
        {
            //txtBillProcessingRemark.Enabled = false;
            //txtTransporterRemark.Enabled = false;

            //txtBreweryGateOutwardNo.Enabled = false;
            //txtGateOutDate.Enabled = false;
            //txtGateOutTime.Enabled = false;
            //txtGateOutTime.ReadOnly = true;
            //txtReachedDestinationDate.Enabled = false;
            //txtReachedDestinationTime.Enabled = false;
            //txtReachedDestinationTime.ReadOnly = true;
            //txtTruckReceivedDate.Enabled = false;
            //txtTruckReceivedTime.Enabled = false;
            //txtTruckReceivedTime.ReadOnly = true;
            //txtTruckReleaseDate.Enabled = false;
            //txtTruckReleaseTime.Enabled = false;
            //txtTruckReleaseTime.ReadOnly = true;
            //btnSubmit.Enabled = false;


            foreach (Control ct in Page.Controls)
            {
                DisableControls(ct);
                txtGateOutTime.Enabled = false;
                txtGateOutTime.ReadOnly = true;
                txtReachedDestinationTime.Enabled = false;
                txtReachedDestinationTime.ReadOnly = true;
                txtTruckReceivedTime.Enabled = false;
                txtTruckReceivedTime.ReadOnly = true;
                txtTruckReleaseTime.Enabled = false;
                txtTruckReleaseTime.ReadOnly = true;
            }


        }

        else
        {
            if (Session["UserType"].ToString() == "Transporter")
            {
                txtTransporterRemark.Enabled = true;
            }
            else if (Session["UserType"].ToString() == "Third Party")
            {
                txtBillProcessingRemark.Enabled = true;
                EnableDisableSubmitButton(ViewState["LRNo"].ToString());

                txtBreweryGateOutwardNo.Enabled = false;
                txtGateOutDate.Enabled = false;
                txtGateOutTime.Enabled = false;
                txtGateOutTime.ReadOnly = true;
                txtReachedDestinationDate.Enabled = false;
                txtReachedDestinationTime.Enabled = false;
                txtReachedDestinationTime.ReadOnly = true;
                txtTruckReceivedDate.Enabled = false;
                txtTruckReceivedTime.Enabled = false;
                txtTruckReceivedTime.ReadOnly = true;
                txtTruckReleaseDate.Enabled = false;
                txtTruckReleaseTime.Enabled = false;
                txtTruckReleaseTime.ReadOnly = true;
            }
            else
            {
                foreach (Control ct in Page.Controls)
                {
                    DisableControls(ct);

                    txtGateOutTime.Enabled = false;
                    txtGateOutTime.ReadOnly = true;
                    txtReachedDestinationTime.Enabled = false;
                    txtReachedDestinationTime.ReadOnly = true;
                    txtTruckReceivedTime.Enabled = false;
                    txtTruckReceivedTime.ReadOnly = true;
                    txtTruckReleaseTime.Enabled = false;
                    txtTruckReleaseTime.ReadOnly = true;
                }

            }
        }
    }
    #endregion

    #region DisableControls - disable Page controls
    private void DisableControls(System.Web.UI.Control control)
    {
        foreach (System.Web.UI.Control c in control.Controls)
        {
            // Get the Enabled property by reflection.
            Type type = c.GetType();
            System.Reflection.PropertyInfo prop = type.GetProperty("Enabled");

            // Set it to False to disable the control.
            if (prop != null)
            {
                if (type != typeof(System.Web.UI.WebControls.Menu))
                {
                    prop.SetValue(c, false, null);
                }
            }

            // Recurse into child controls.
            if (c.Controls.Count > 0)
            {
                this.DisableControls(c);
            }
        }
    }
    #endregion

    #region EnableDisableInvoiceButton - Disable Invoice Button if any of the LR Line is not verified by the third party User
    private void EnableDisableInvoiceButton(string LRNo)
    {
        Int32 result = GetSQLData("select count(TL.[Verified by Bill Processing]) from TMS_LR_Lines TL  where [LR No_] ='" + LRNo + "' and TL.[Verified by Bill Processing]=0");
        if (result > 0)
        {
            btnInvoice.Enabled = false;
        }
        else
        {
            btnInvoice.Enabled = true;
        }

        int isPendingForInvoiced = GetSQLData("select count(*) from TMS_LR_Header where [LR No_] ='" + LRNo + "' and [Approval Status]='Pending for Invoice'");
        if (isPendingForInvoiced > 0)
        {
            btnSubmit.Enabled = false;
        }
    }
    #endregion

    #region EnableDisableSubmitButton  -  Disable Submit Button if any of the row does  not have transit/Accidential last entered amount
    private void EnableDisableSubmitButton(string LRNo)
    {
        Int32 hasAll_row_TransitAccidentalLoss = 0;
        hasAll_row_TransitAccidentalLoss = GetSQLData("Select  [dbo].[isAllLR_transitAccidential_EnteredByTransporter]('" + ViewState["LRNo"] + "')");      //call SQL function

        if (hasAll_row_TransitAccidentalLoss == 1)
        {
            btnSubmit.Enabled = true;
        }
        else
        {
            btnSubmit.Enabled = false;
        }
    }
    #endregion

    /// <summary>
    /// Update LR by Transporter
    /// </summary>
    protected void Update_LR_Line_By_Transporter()
    {

        string BreweryGateOutwardNo = txtBreweryGateOutwardNo.Text;

        string GateOutDate = Convert.ToDateTime(txtGateOutDate.Text).ToShortDateString();
        string GOTime = txtGateOutTime.Hour.ToString() + ":" + txtGateOutTime.Minute.ToString() + ":" + txtGateOutTime.Second.ToString() + " " + txtGateOutTime.AmPm;
        DateTime dt1 = DateTime.Parse(GOTime); // No error check
        string GateOutTime = dt1.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

        string ReachedDestinationDate = Convert.ToDateTime(txtReachedDestinationDate.Text).ToShortDateString();
        string RDTime = txtReachedDestinationTime.Hour.ToString() + ":" + txtReachedDestinationTime.Minute.ToString() + ":" + txtReachedDestinationTime.Second.ToString() + " " + txtReachedDestinationTime.AmPm;
        DateTime dt2 = DateTime.Parse(RDTime); // No error check
        string ReachedDestinationTime = dt2.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

        //string TruckReceivedDate = txtTruckReceivedDate.SelectedDate.ToShortDateString();
        string TruckReceivedDate = Convert.ToDateTime(txtTruckReceivedDate.Text).ToShortDateString();
        string TRTime = txtTruckReceivedTime.Hour.ToString() + ":" + txtTruckReceivedTime.Minute.ToString() + ":" + txtTruckReceivedTime.Second.ToString() + " " + txtTruckReceivedTime.AmPm;
        DateTime dt3 = DateTime.Parse(TRTime); // No error check
        string TruckReceivedTime = dt3.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

        string TruckReleaseDate = Convert.ToDateTime(txtTruckReleaseDate.Text).ToShortDateString();
        string TRLTime = txtTruckReleaseTime.Hour.ToString() + ":" + txtTruckReleaseTime.Minute.ToString() + ":" + txtTruckReleaseTime.Second.ToString() + " " + txtTruckReleaseTime.AmPm;
        DateTime dt4 = DateTime.Parse(TRLTime); // No error check
        string TruckReleaseTime = dt4.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

        if (GateOutDate == "01-01-0001" || GateOutTime == "" || ReachedDestinationDate == "01-01-0001" || ReachedDestinationTime == "" || TruckReceivedDate == "01-01-0001" || TruckReceivedTime == "" || TruckReleaseDate == "01-01-0001" || TruckReleaseTime == "")
        {
            lblerr.Text = "Gate Out Date/Time, Reached Destination Date/Time ,Truck Received Date/Time or Truck Release Date/time is not filled by Transporter." + "\n" + Environment.NewLine +
                 "Entries cannot be submitted before Transporter response";
            lblerr.Visible = true;
            lblerr.ForeColor = System.Drawing.Color.Red;
            return;
        }

        UpdateFlagValue = true;
        int recordUpdated = 0;
        bool isDataSendtoNav = false;
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("ID", typeof(int)),
                                                new DataColumn("TransitLossQty",  typeof(Int64)),
                                                new DataColumn("AccidentalLossQty", typeof(Int64)),                                              
                                                new DataColumn("VerifiedByBillProcessingTeam", typeof(string))});
            //int a = grdLR.PageIndex;  //code to add updated data in DT from all pages
            //for (int i = 0; i < grdLR.PageCount; i++)
            //{
            //grdLR.SetPageIndex(i);
            foreach (GridViewRow row in grdLR.Rows)
            {
                string key = grdLR.DataKeys[row.RowIndex].Value.ToString();

                TextBox txtTransitLossQty = row.FindControl("txtTransitLossQty") as TextBox;
                TextBox txtAccidentalLossQty = row.FindControl("txtAccidentalLossQty") as TextBox;
                CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;

                DataRow dr = dt.NewRow();
                dr["ID"] = int.Parse(key);
                if (txtTransitLossQty.Text != "")
                {
                    dr["TransitLossQty"] = decimal.Parse(txtTransitLossQty.Text);
                }
                if (txtAccidentalLossQty.Text != "")
                {
                    dr["AccidentalLossQty"] = decimal.Parse(txtAccidentalLossQty.Text);
                }
                dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? "1" : "0";


                //if (txtTransitLossQty.Text != "" || txtAccidentalLossQty.Text != "")
                dt.Rows.Add(dr); // if no breakage value then dont add in DT

            }
            //  }
            // grdLR.SetPageIndex(a);

            UpdateFlagValue = GetUpdateFlagValueFromGrid(dt);
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                con.Open();
                SqlCommand cmd = new SqlCommand("TMS_sp_update_LRLineEntry");
                SqlTransaction transaction;
                transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted);
                cmd.Connection = con;
                cmd.Transaction = transaction;
                #region Transaction
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMSRequestUpdateTable", dt);
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@UserType", Session["UserType"].ToString());
                    cmd.Parameters.AddWithValue("@LRNo", lblLRNo.Text.Trim());
                    if (isTransitAccidentialValue_Grid(dt))  //if all row has transit/accedential Loss - then status = Pending for Bill processing Team(3)     else - Pending for Transporter Action(5)
                        cmd.Parameters.AddWithValue("@ApprovalStatus", 3); //Set 'Pending for Bill processing Team'
                    else
                        cmd.Parameters.AddWithValue("@ApprovalStatus", 5); //Set 'Pending for Transporter Action'                   
                    cmd.Parameters.AddWithValue("@TransporterRemark", txtTransporterRemark.Text.Trim());
                    cmd.Parameters.AddWithValue("@BillProcessingRemark", txtBillProcessingRemark.Text.Trim());
                    cmd.Parameters.AddWithValue("@BreweryGateOutwardNo", BreweryGateOutwardNo);
                    cmd.Parameters.AddWithValue("@GateOutDate", GateOutDate);
                    cmd.Parameters.AddWithValue("@GateOutTime", GateOutTime);
                    cmd.Parameters.AddWithValue("@ReachedDestinationDate", ReachedDestinationDate);
                    cmd.Parameters.AddWithValue("@ReachedDestinationTime", ReachedDestinationTime);
                    cmd.Parameters.AddWithValue("@TruckReceivedDate", TruckReceivedDate);
                    cmd.Parameters.AddWithValue("@TruckReceivedTime", TruckReceivedTime);
                    cmd.Parameters.AddWithValue("@TruckReleaseDate", TruckReleaseDate);
                    cmd.Parameters.AddWithValue("@TruckReleaseTime", TruckReleaseTime);

                    recordUpdated = cmd.ExecuteNonQuery(); //saved in database
                    if (recordUpdated > 0)
                    {
                        //Int32 isAllrowVerified = 0;
                        //isAllrowVerified = GetSQLData("select  [dbo].[isAllLRLineVerified]('" + lblLRNo.Text.Trim() + "')");

                        //if (isAllrowVerified == 1)
                        //{
                        //    //Send updated Data to Nav-- Called Web service iof Navision
                        //    SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
                        //    isDataSendtoNav = objsenddatatonav.SendLRtoNAV(lblLRNo.Text.Trim());
                        //}
                        //else
                        //{
                        transaction.Commit();
                        con.Close();
                        lblerr.Text = "Entries submitted successfully";
                        lblerr.ForeColor = System.Drawing.Color.Green;
                        lblerr.Visible = true;
                        BindGridview(ViewState["LRNo"].ToString());
                        BindLRHeader(ViewState["LRNo"].ToString());
                        return;
                        //}
                    }
                    else
                    {
                        lblerr.Text = "Entries cannot be submitted at this time, Please contact adminstrator ";
                        lblerr.Visible = true;
                    }
                }
                catch (Exception ex1)
                {
                    try
                    {
                        transaction.Rollback(); //manual                            
                        con.Close();
                        lblerr.Text = ex1.Message;
                        lblerr.ForeColor = System.Drawing.Color.Red;
                        lblerr.Visible = true;
                    }
                    catch (Exception ex2)
                    {
                        lblerr.Text = ex2.Message;
                        lblerr.ForeColor = System.Drawing.Color.Red;
                        lblerr.Visible = true;
                    }
                }
                #endregion

                #region Commit Transaction
                if (recordUpdated > 0 && isDataSendtoNav == true)
                {
                    transaction.Commit();
                    con.Close();
                    lblerr.Text = "Entries submitted successfully";
                    lblerr.ForeColor = System.Drawing.Color.Green;
                    lblerr.Visible = true;
                }
                else
                {
                    transaction.Rollback();
                    con.Close();
                    lblerr.Text = "Entries cannot be submit at this time, Please contact adminstrator ";
                    lblerr.ForeColor = System.Drawing.Color.Red;
                    lblerr.Visible = true;
                }
                #endregion
            }
            BindGridview(ViewState["LRNo"].ToString());
            BindLRHeader(ViewState["LRNo"].ToString());
            EnableDisableInvoiceButton(ViewState["LRNo"].ToString());
        }
        catch (Exception es)
        {

        }



    }

    /// <summary>
    /// Update LR by CIPL user
    /// </summary>
    protected void Update_LR_Line_By_CIPL()
    {
        UpdateFlagValue = true;
        //int recordUpdated = 0;
        //bool isDataSendtoNav = false;
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("ID", typeof(int)),
                                                new DataColumn("TransitLossQty",  typeof(int)),
                                                new DataColumn("AccidentalLossQty", typeof(int)),                                              
                                                new DataColumn("VerifiedByBillProcessingTeam", typeof(string))});
            //int a = grdLR.PageIndex;
            //for (int i = 0; i < grdLR.PageCount; i++)
            //{
            //    grdLR.SetPageIndex(i);
            foreach (GridViewRow row in grdLR.Rows)
            {
                string key = grdLR.DataKeys[row.RowIndex].Value.ToString();
                TextBox txtTransitLossQty = row.FindControl("txtTransitLossQty") as TextBox;
                TextBox txtAccidentalLossQty = row.FindControl("txtAccidentalLossQty") as TextBox;
                CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;
                DataRow dr = dt.NewRow();
                dr["ID"] = int.Parse(key);
                if (txtTransitLossQty.Text != "")
                {
                    dr["TransitLossQty"] = decimal.Parse(txtTransitLossQty.Text);
                }
                if (txtAccidentalLossQty.Text != "")
                {
                    dr["AccidentalLossQty"] = decimal.Parse(txtAccidentalLossQty.Text);
                }
                dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? "1" : "0";
                dt.Rows.Add(dr);
            }
            //}
            //grdLR.SetPageIndex(a);

            UpdateFlagValue = GetUpdateFlagValueFromGrid(dt);

            if (UpdateFlagValue) //all row verified
            {
                UpdateLR_byCIPL_and_SendDatatoNAV(dt); //update LR and send data to nav
            }
        }
        catch (Exception es)
        {

        }
    }

    /// <summary>
    /// Bind Grid View
    /// </summary>
    /// <param name="LRno"></param>
    private void BindGridview(string LRno)
    {
        if (LRno != "")
        {
            LRDetailItem LRDetailItem = new LRDetailItem();
            IList<LRDetailItem> LRDetailItems = LRDetailItem.GetLREntryDetails(LRno);
            GetUpdateFlagValueFromData(LRDetailItems);
            grdLR.DataSource = LRDetailItems;
            grdLR.DataBind();
        }
    }

    /// <summary>
    /// Get UpdateFlagValue from IList<LRDeatailItems> : Based on VerifiedbyBillProcessingTeam
    /// </summary>
    /// <param name="LRDetailItems"></param>
    /// <returns></returns>
    private bool GetUpdateFlagValueFromData(IList<LRDetailItem> LRDetailItems)
    {
        UpdateFlagValue = true;
        foreach (LRDetailItem LRDetailItem in LRDetailItems)
        {
            if (LRDetailItem.VerifiedbyBillProcessingTeam.ToLower() == "0") //if anyone of verifyByBill checkbox set as NO
            {
                UpdateFlagValue = false;
                break;
            }
        }
        return UpdateFlagValue;
    }

    /// <summary>
    /// Get UpdateFlagValue from gridview - Based on VerifiedbyBillProcessingTeam
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private bool GetUpdateFlagValueFromGrid(DataTable dt)
    {
        UpdateFlagValue = true;

        foreach (DataRow dr in dt.Rows)
        {
            if (dr["VerifiedbyBillProcessingTeam"].ToString().ToLower() == "0")
            {
                UpdateFlagValue = false;
                if (Session["UserType"].ToString() == "Third Party")
                {
                    mpe.Show();
                }
                break;
            }
        }
        return UpdateFlagValue;
    }

    /// <summary>
    /// Get UpdateFlagValue from gridview - Based on VerifiedbyBillProcessingTeam
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private bool isTransitAccidentialValue_Grid(DataTable dt)
    {
        bool hasData = true;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["TransitLossQty"].ToString() == "" || dr["AccidentalLossQty"].ToString() == "")
            {
                hasData = false;
                break;
            }
        }
        return hasData;
    }

    /// <summary>
    /// Bind LR Header controls
    /// </summary>
    /// <param name="LRno"></param>
    private void BindLRHeader(string LRno)
    {
        LRHeader LRobj = (new LRHeader()).GetLREntryDetails(LRno);
        lblLRNo.Text = LRobj.LRNo;
        lblLRDate.Text = LRobj.LRDate.ToShortDateString() != "01-01-0001" ? LRobj.LRDate.ToShortDateString() : "";
        lblSize.Text = LRobj.Size;
        lblTransporterCode.Text = LRobj.TransporterCode;
        lblTruckNo.Text = LRobj.TruckNo;
        lblDestination.Text = LRobj.Destination;
        lblArrivalTime.Text = LRobj.ArrivalDateTime.ToShortDateString() != "01-01-0001" ? LRobj.ArrivalDateTime.ToShortDateString() : "";
        lblDepartureTime.Text = LRobj.DepartureDateTime.ToShortDateString() != "01-01-0001" ? LRobj.DepartureDateTime.ToShortDateString() : "";
        txtTransporterRemark.Text = LRobj.TransporterRemark;
        txtBillProcessingRemark.Text = LRobj.BillProcessingTeamRemarks;
        txtBreweryGateOutwardNo.Text = LRobj.BreweryGateOutwardNo;

        /************************************************ SET DATE and TIME Entries ***************************************************************/
        //Bind GateOutDate
        if (LRobj.GateOutDate != "" && LRobj.GateOutDate != "01-01-1753 00:00:00" && LRobj.GateOutDate != "01-01-1900 00:00:00")
            txtGateOutDate.Text = LRobj.GateOutDate.Substring(0, 11);
        //Bind GateOutTime         
        string time = LRobj.GateOutTime.Substring(11); //	"15:41:00"	= 3:41 PM     
        string time12HRformat = Convert24HrTo12Hr(Convert.ToInt16(time.Substring(0, 2)), Convert.ToInt16(time.Substring(3, 2))); //02:36 AM
        txtGateOutTime.Hour = (Convert.ToInt16(time12HRformat.Substring(0, 2)));
        txtGateOutTime.Minute = (Convert.ToInt16(time12HRformat.Substring(3, 2)));
        if (time12HRformat.Substring(6, 2) == "AM")
        {
            txtGateOutTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
        }
        else
        {
            txtGateOutTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
        }


        if (LRobj.ReachedDestinationDate != "" && LRobj.ReachedDestinationDate != "01-01-1753 00:00:00" && LRobj.ReachedDestinationDate != "01-01-1900 00:00:00")
            txtReachedDestinationDate.Text = LRobj.ReachedDestinationDate.Substring(0, 11);
        //Bind txtReachedDestinationTime  

        time = LRobj.ReachedDestinationTime.Substring(11); //	"12:41:00"	string     
        time12HRformat = Convert24HrTo12Hr(Convert.ToInt16(time.Substring(0, 2)), Convert.ToInt16(time.Substring(3, 2))); //02:36 AM
        txtReachedDestinationTime.Hour = (Convert.ToInt16(time12HRformat.Substring(0, 2)));
        txtReachedDestinationTime.Minute = (Convert.ToInt16(time12HRformat.Substring(3, 2)));
        if (time12HRformat.Substring(6, 2) == "AM")
            txtReachedDestinationTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
        else
            txtReachedDestinationTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;


        if (LRobj.TruckReceivedDate != "" && LRobj.TruckReceivedDate != "01-01-1753 00:00:00" && LRobj.TruckReceivedDate != "01-01-1900 00:00:00")
            txtTruckReceivedDate.Text = LRobj.TruckReceivedDate.Substring(0, 11);
        //Bind  txtTruckReceivedTime
        time = LRobj.TruckReceivedTime.Substring(11); //	"20:41:00"	     
        time12HRformat = Convert24HrTo12Hr(Convert.ToInt16(time.Substring(0, 2)), Convert.ToInt16(time.Substring(3, 2))); //02:36 AM
        txtTruckReceivedTime.Hour = (Convert.ToInt16(time12HRformat.Substring(0, 2)));
        txtTruckReceivedTime.Minute = (Convert.ToInt16(time12HRformat.Substring(3, 2)));
        if (time12HRformat.Substring(6, 2) == "AM")
            txtTruckReceivedTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
        else
            txtTruckReceivedTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;


        if (LRobj.TruckReleaseDate != "" && LRobj.TruckReleaseDate != "01-01-1753 00:00:00" && LRobj.TruckReleaseDate != "01-01-1900 00:00:00")
            txtTruckReleaseDate.Text = LRobj.TruckReleaseDate.Substring(0, 11);
        //Bind  txtTruckReceivedTime
        time = LRobj.TruckReleaseTime.Substring(11); //	 "00:41:00"    
        time12HRformat = Convert24HrTo12Hr(Convert.ToInt16(time.Substring(0, 2)), Convert.ToInt16(time.Substring(3, 2))); //02:36 AM
        txtTruckReleaseTime.Hour = (Convert.ToInt16(time12HRformat.Substring(0, 2)));
        txtTruckReleaseTime.Minute = (Convert.ToInt16(time12HRformat.Substring(3, 2)));
        if (time12HRformat.Substring(6, 2) == "AM")
            txtTruckReleaseTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
        else
            txtTruckReleaseTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;


        DataTable dt = GetOperationalStatus(ViewState["LRNo"].ToString());
        ddlApprovalStatus.DataSource = dt;
        ddlApprovalStatus.DataTextField = "Option Name";
        ddlApprovalStatus.DataValueField = "Option Id";
        ddlApprovalStatus.DataBind();
        SetOperationalStatus(ViewState["LRNo"].ToString());


    }

    /// <summary>
    /// Execute SQL Query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    private DataTable GetData(string query)
    {
        using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                DataTable dt = new DataTable();
                con.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
    }

    /// <summary>
    /// Execute SQL query - Returns Integer Value only
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    private Int32 GetSQLData(string query)
    {
        Int32 result = 0;
        using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
        {
            con.Open();
            SqlTransaction transaction;
            transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Transaction = transaction;
                    SqlDataReader dr = (cmd.ExecuteReader());
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = Convert.ToInt32(dr[0]);
                        }
                    }
                    dr.Close();

                    if (result > 0)
                    {
                        transaction.Commit();
                        con.Close();
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                con.Close();
                return 0;
            }
        }
    }

    /// <summary>
    /// Get Value of Approval_Status from database
    /// </summary>
    /// <param name="LRNo"></param>
    /// <returns></returns>
    private DataTable GetOperationalStatus(string LRNo)
    {
        if (dtOperationalStatus != null)
            return dtOperationalStatus;
        if (Session["RequestOptionOperationalStatus"] != null)
        {
            dtOperationalStatus = Session["RequestOptionOperationalStatus"] as DataTable;
            return dtOperationalStatus;
        }
        else
        {
            string sqlStatement = "SELECT [Option Id] ,[Option Name] FROM [TMS_LR_Header_Option_Approval_Status] ";

            using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                {
                    dtOperationalStatus = new DataTable();
                    con.Open();
                    dtOperationalStatus.Load(cmd.ExecuteReader());
                    Session.Add("RequestOptionOperationalStatus", dtOperationalStatus);
                    return dtOperationalStatus;
                }
            }
        }
    }

    /// <summary>
    /// Set Value of Approval_Status of current LR in Dropdown List
    /// </summary>
    /// <param name="LRNo"></param>
    private void SetOperationalStatus(string LRNo)
    {
        DataTable dtStatus = null;
        try
        {
            string sqlStatement = "SELECT [Option Id] ,[Option Name] FROM [TMS_LR_Header_Option_Approval_Status] A " +
                                   "join TMS_LR_Header H on H.[Approval Status]=A.[Option Name]" +
                                   "Where H.[LR No_]='" + LRNo + "'";

            using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                {
                    dtStatus = new DataTable();
                    con.Open();
                    dtStatus.Load(cmd.ExecuteReader());
                    Session.Add("RequestOptionOperationalStatus", dtOperationalStatus);
                    //return dtOperationalStatus;
                }
            }
            if (dtStatus.Rows.Count > 0)
                ddlApprovalStatus.Items.FindByValue(dtStatus.Rows[0]["Option Id"].ToString()).Selected = true;
        }
        catch
        {
            throw;
        }

    }

    /// <summary>
    /// Enable - Disable Gridview controls and LR header controls
    /// </summary>
    /// <param name="updateFlag"></param>
    /// <param name="txtTransitLossQty"></param>
    /// <param name="txtAccidentalLossQty"></param>
    /// <param name="chkVerifiedbyBill"></param>
    private void EnableDisableControls(Int64 datakey, bool updateFlag, TextBox txtTransitLossQty, TextBox txtAccidentalLossQty, CheckBox chkVerifiedbyBill)
    {
        if (Session["UserType"].ToString() == "Transporter")
        {
            if (chkVerifiedbyBill.Checked) //already verified row - invoice generated
            {
                //locked all fields
                txtTransitLossQty.ReadOnly = true;
                txtAccidentalLossQty.ReadOnly = true;
                //  txtTransporterRemark.Enabled = false;
            }
            else
            {
                txtTransitLossQty.ReadOnly = false;
                txtAccidentalLossQty.ReadOnly = false;
                //  txtTransporterRemark.Enabled = true;
                //To Do : Grid to be editable/Uneditable
            }
        }
        else if (Session["UserType"].ToString() == "Third Party")
        {
            if (!updateFlag) // i.e. All rows are not verified by Bill Processing Team -> Then enable checkbox
            {
                if (chkVerifiedbyBill.Checked) //already verified row - invoice generated
                {
                    chkVerifiedbyBill.Enabled = false;
                    // txtBillProcessingRemark.Enabled = false;
                }
                else
                {
                    // If transit Loss or Accidential charges are  not filled by Transporter -> Make the checkbox disable(CIPL Cannot verify)
                    Int32 hasAll_row_TransitAccidentalLoss = 0;
                    hasAll_row_TransitAccidentalLoss = GetSQLData("Select  [dbo].[isAllLR_transitAccidential_EnteredByTransporter]('" + ViewState["LRNo"] + "')");      //call SQL function

                    if (hasAll_row_TransitAccidentalLoss == 1)
                    {
                        chkVerifiedbyBill.Enabled = true;
                    }
                    else
                    {
                        chkVerifiedbyBill.Enabled = false;
                    }
                }
            }
            else // i.e. All Rows are verified
            {
                //  txtBillProcessingRemark.Enabled = false;
                chkVerifiedbyBill.Enabled = false;

            }
        }
    }

    #region Convert24HrTo12Hr Algorithm - By Anu
    public string Convert24HrTo12Hr(int HH, int MM) // 12:59, 00:45
    {
        int h = 00;
        string time = "";
        if (HH > 12)
        {
            h = HH - 12;
            if (h < 10)
            {
                if (MM < 10)
                {
                    time = "0" + h + ":" + "0" + MM + " PM";
                }
                else
                {
                    time = "0" + h + ":" + MM + " PM";
                }
            }
            else
            {
                if (MM < 10)
                {
                    time = h + ":" + "0" + MM + " PM";
                }
                else
                {
                    time = h + ":" + MM + " PM";
                }
            }
        }
        else if (HH == 12)
        {
            h = HH;
            if (MM < 10)
            {
                time = "0" + h + ":" + "0" + MM + " PM";
            }
            else
            {
                time = h + ":" + MM + " PM";
            }
        }
        else
        {
            h = HH;
            if (HH == 0 || HH == 00)
            {
                h = 12;
            }
            if (h < 10)
            {
                if (MM < 10)
                {
                    time = "0" + h + ":" + "0" + MM + " AM";
                }
                else
                {
                    time = "0" + h + ":" + MM + " AM";
                }
            }
            else
            {
                if (MM < 10)
                {
                    time = h + ":" + "0" + MM + " AM";
                }
                else
                {
                    time = h + ":" + MM + " AM";
                }
            }
        }
        return time;
    }
    #endregion

    #region SEND EMAIL TO TRANSPORTER
    private void SendMailToTransporter(string LRNo)
    {

        DataTable dTable = GetData("select PUS.ID, UTM.company_Name_FK , PUS.[Portal User Id], PUS.[E-mail ID]  from TMS_Portal_User_Setup PUS " +
                                    "INNER JOIN TMS_User_Transporter_Mapping  UTM on PUS.[Portal User Id]= UTM.[Portal User ID] " +
                                    "and UTM.[Transporter Code] = (Select LR.[Transporter Code] from TMS_LR_Header LR where [LR No_] = '" + LRNo + "') " +
                                    "and UTM.company_Name_FK =  (Select LR.company_Name_FK  from TMS_LR_Header LR where [LR No_] = '" + LRNo + "') " +
                                    "Where PUS.[User Type] = 'Transporter'");

        foreach (DataRow dr in dTable.Rows)
        {
            string to = dr[3].ToString(); //Email ID of Receiver
            string username = dr[2].ToString(); //User Name of Receiver
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = new SmtpClient("smtp.office365.com");
            try
            {
                DataTable dt = GetData("SELECT top 1 [E-Mail Address to Send Mails] as [FromEmailID] ,[Service Account] as [UserName] ,[Service Password] as [Password] FROM [dbo].[TMS_Setup] where company_Name_FK = (select t.company_Name_FK from TMS_LR_Header t where t.[LR No_] = '" + LRNo + "')");
                objMailMessage.From = new MailAddress(dt.Rows[0][0].ToString()); //Email ID of Sender
                objMailMessage.To.Add(new MailAddress(to));
                objMailMessage.Subject = "TMS : LR GENERATED & SHIPMENT DETAILS - " + DateTime.Now.ToShortDateString();

                DataTable QueryResultingData = GetData("SELECT 1 as [SR No.] , LRH.Location as [Brewery Code], UPPER(LRH.[From City]) as [From], UPPER(LRH.[To City]) as [To], CAST(SUM(LRL.Quantity) as decimal(38,2)) as [No. Of Cases] , LRH.[Invoice No_] as [Invoice/STN No.] " +
                                                        ", LRH.[Date of Dispatch] as [Date] , LRH.[LR No_] as [LR No.] FROM TMS_LR_Header LRH inner join TMS_LR_Lines LRL on LRH.[LR No_] = LRL.[LR No_] " +
                                                        "WHERE LRH.[LR No_] = '" + LRNo + "' GROUP BY Location , [From City], [To City] , LRH.[Invoice No_] , LRH.[Date of Dispatch] , LRH.[LR No_] ");
                
                StringBuilder sb = new StringBuilder();                
                sb.Append("DEAR, <br/> " +
                          "SGC Team, <br/> "+
                          "SUB: CIPL-LR GENERATED & SHIPMENT DETAILS- " + DateTime.Now.ToShortDateString() + "<br/><br/>" +
                          "We wish to inform you that below LRs are generated today against vehcile placed.<br/>" +
                          "Please ensure vehicle should reach at destination withing agreed timeliens on defined routes. <br/>" +
                          "Please keep updating the vehicle status report on daily basis. <br/><br/>");

                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gr = new GridView();
                gr.DataSource = QueryResultingData;
                gr.DataBind();
                gr.RenderControl(hw);

                sb.Append("<br/><br/> Thanks and Regards, <br/><br/>Logistics Team,<br/>CIPL-DHD");

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
                objSmtpClient = null;
            }
        }
    }
    #endregion
    
    #region SEND EMAIL TO TRANSPORTER ON DISPUTE CASE
    private void SendMailOfDisputeToTransporter(string LRNo)
    {

        DataTable dTable = GetData("select PUS.ID, UTM.company_Name_FK , PUS.[Portal User Id], PUS.[E-mail ID]  from TMS_Portal_User_Setup PUS " +
                                    "INNER JOIN TMS_User_Transporter_Mapping  UTM on PUS.[Portal User Id]= UTM.[Portal User ID] " +
                                    "and UTM.[Transporter Code] = (Select LR.[Transporter Code] from TMS_LR_Header LR where [LR No_] = '" + LRNo + "') " +
                                    "and UTM.company_Name_FK =  (Select LR.company_Name_FK  from TMS_LR_Header LR where [LR No_] = '" + LRNo + "') " +
                                    "Where PUS.[User Type] = 'Transporter'");

        foreach (DataRow dr in dTable.Rows)
        {
            string to = dr[3].ToString(); //Email ID of Receiver
            string username = dr[2].ToString(); //User Name of Receiver
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = new SmtpClient("smtp.office365.com");
            try
            {
                DataTable dt = GetData("SELECT top 1 [E-Mail Address to Send Mails] as [FromEmailID] ,[Service Account] as [UserName] ,[Service Password] as [Password] FROM [dbo].[TMS_Setup] where company_Name_FK = (select t.company_Name_FK from TMS_LR_Header t where t.[LR No_] = '" + LRNo + "')");
                objMailMessage.From = new MailAddress(dt.Rows[0][0].ToString()); //Email ID of Sender
                objMailMessage.To.Add(new MailAddress(to));
                objMailMessage.Subject = "TMS : LR DISPUTE DETAILS - " + DateTime.Now.ToShortDateString();

                DataTable QueryResultingData = GetData("SELECT 1 as [SR No.] , LRH.Location as [Brewery Code], UPPER(LRH.[From City]) as [From], UPPER(LRH.[To City]) as [To], CAST(SUM(LRL.Quantity) as decimal(38,2)) as [No. Of Cases] , LRH.[Invoice No_] as [Invoice/STN No.] " +
                                                        ", LRH.[Date of Dispatch] as [Date] , LRH.[LR No_] as [LR No.] FROM TMS_LR_Header LRH inner join TMS_LR_Lines LRL on LRH.[LR No_] = LRL.[LR No_] " +
                                                        "WHERE LRH.[LR No_] = '" + LRNo + "' GROUP BY Location , [From City], [To City] , LRH.[Invoice No_] , LRH.[Date of Dispatch] , LRH.[LR No_] ");

                StringBuilder sb = new StringBuilder();
                sb.Append("DEAR, <br/> " +
                          "SGC Team, <br/> " +
                          "SUB: LR Dispute - " + DateTime.Now.ToShortDateString() + "<br/><br/>" +
                          "There is a dispute on below LR. Please review the LR again ! <br/><br/>");

                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gr = new GridView();
                gr.DataSource = QueryResultingData;
                gr.DataBind();
                gr.RenderControl(hw);

                sb.Append("<br/><br/> Thanks and Regards, <br/><br/>Logistics Team,<br/>CIPL-DHD");

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
                objSmtpClient = null;
            }
        }
    }
    #endregion



}



