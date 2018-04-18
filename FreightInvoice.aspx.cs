using BusinessLayerClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FreightInvoice : System.Web.UI.Page
{

    #region ****************************** Variable Declaration **********************************
    bool UpdateFlagValue;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            if (Request.QueryString["LRNo"] != null)
            {
                ViewState.Add("LRNo", Request.QueryString["LRNo"].ToString());
                string LRNo = Request.QueryString["LRNo"].ToString();
                BindLRHeader(LRNo);
                EnableHeader_BasedOnUser();
                if (Session["UserType"].ToString() == "Transporter")
                {
                    Generate_Initial_Invoice(LRNo);
                }
                Bind_gridInvoice(LRNo);
            }
        }
    }
    #endregion

    #region Row Data Bound Event
    protected void grdInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.Row.DataItemIndex.ToString());
            if (index != -1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkboxVerifiedByCIPL = (e.Row.FindControl("chkVerifiedbyBill") as CheckBox);
                    if (Session["UserType"].ToString() == "Transporter")
                    {
                        TextBox txtAmtConfirmed = (e.Row.FindControl("txtAmountConfirmed") as TextBox);
                        if (!chkboxVerifiedByCIPL.Checked)
                        {
                            txtAmtConfirmed.Enabled = true;
                        }

                    }
                    if (Session["UserType"].ToString() == "Third Party")
                    {
                        chkboxVerifiedByCIPL.Enabled = true;
                    }

                }
            }
        }
        catch (Exception eg)
        {
            throw;
        }
    }
    #endregion

    #region btnUpdate Click Event
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string invoicereport = "TPTInvoiceReport";
        if (Session["UserType"].ToString() == "Transporter")
        {
            Update_Invoice_By_Transporter();
            Update_Invoice_By_CIPL(0);

            Session["LRNO"] = lblLRNo.Text.ToString();
            Response.Write("<script>window.open ('Report.aspx?Report=" + invoicereport + "','_blank');</script>");
        }
        if (Session["UserType"].ToString() == "Third Party")
        {
            Update_Invoice_By_CIPL(0);
        }
        BindLRHeader(ViewState["LRNo"].ToString());
        EnableHeader_BasedOnUser();
        Bind_gridInvoice(ViewState["LRNo"].ToString());
    }
    #endregion

    #region ddlSpecialDiscount_SelectedIndexChanged Event - Discount Scheme changed from dropdown
    protected void ddlSpecialDiscount_SelectedIndexChanged(object sender, EventArgs e)
    {
       // string TotalBillAmount = grdInvoice.Rows[9].Cells[1].Text;
        //TextBox TransporterTotalBillAmount = grdInvoice.Rows[9].Cells[2].Controls[1] as TextBox;
       // SpecialDiscount SpecialDiscount = new SpecialDiscount();
       // string DiscountedAmount = SpecialDiscount.GetSpecialDiscount(ddlSpecialDiscount.SelectedItem.Text, lblTransporterCode.Text, TotalBillAmount);
       // BindInvoiceGridWithAmount(DiscountedAmount, 7);
        //  Generate_Initial_Invoice(ViewState["LRNo"].ToString());
        Update_Invoice_Values(0, 1);
    }
    #endregion

    #region ddlDelayPenalty_SelectedIndexChanged Event
    protected void ddlDelayPenalty_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DelayPenalty DelayPenalty = new DelayPenalty();
        //string DelayPenaltyAmount = DelayPenalty.GetDelayPenalty(ddlDelayPenalty.SelectedItem.Text);
        //BindInvoiceGridWithAmount(DelayPenaltyAmount, 8);
        Update_Invoice_Values(1, 0);
    }
    #endregion

    #region ddlRejectionReason_SelectedIndexChanged Event
    protected void ddlRejectionReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        Update_Invoice_Values(0, 0);
    }
    #endregion

    #region Model Pop up - btnOk_Click Event
    protected void btnOK_Click(object sender, EventArgs e)
    {
        mpe.Hide();
        if (ddlRejectionReason.SelectedIndex == 0)
        {
            lblmandatoryReasonCode.Visible = true;            
        }
        ddlRejectionReason.Enabled = true;
    }
    #endregion

    #region Model Pop up - btnCancel_Click Event
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpe.Hide();
    }
    #endregion

    #region Bind LR Header Values and all dropdowns
    private void BindLRHeader(string LRno)
    {
        LRHeader LRobj = (new LRHeader()).GetLREntryDetails(LRno);
        lblLRNo.Text = LRobj.LRNo;
        lblLRDate.Text = LRobj.LRDate.ToShortDateString();
        lblSize.Text = LRobj.Size;
        lblTransporterCode.Text = LRobj.TransporterCode;
        lblTruckNo.Text = LRobj.TruckNo;
        lblDestination.Text = LRobj.Destination;
        //Bind Special Discount Code

        BindSpecialCode(LRobj.TransporterCode.ToString().Trim());
        BindDelayPenalty();

        FreightInvoices ob = new FreightInvoices();
        IList<BusinessLayerClasses.FreightInvoice> lstinvoice = ob.GetFreightInvoice(LRno);

        if (lstinvoice != null)
        {
            if (lstinvoice.Count > 0)
            {
                if (lstinvoice[0].Penalty_Reason_Code_ID.ToString() != "0")
                    ddlDelayPenalty.Items.FindByValue(lstinvoice[0].Penalty_Reason_Code_ID.ToString()).Selected = true;

                if (lstinvoice[0].Payment_Discount_Scheme_ID.ToString() != "0")
                    ddlSpecialDiscount.Items.FindByValue(lstinvoice[0].Payment_Discount_Scheme_ID.ToString()).Selected = true;

                txttransporterRemark.Text = lstinvoice[0].TransporterRemarks;
                txtBillProcessTeamRemarks.Text = lstinvoice[0].BillProcessingTeamRemarks;           

            }
        }

        DataTable dt = new DataTable();
        dt = GetData("SELECT  [Option Id]  ,[Option Name]  FROM [dbo].[TMS_LR_Header_Option_Approval_Status]");
        ddlApprovalStatus.DataSource = dt;
        ddlApprovalStatus.DataTextField = "Option Name";
        ddlApprovalStatus.DataValueField = "Option Id";
        ddlApprovalStatus.DataBind();
        SetApprovalStatus_ForInvoice(ViewState["LRNo"].ToString(), lstinvoice);


        DataTable dt1 = GetData("SELECT [ID] ,[Code],[Description] FROM [dbo].[TMS_Rejection_Reason_Code] where company_Name_FK = (select t.company_Name_FK from TMS_LR_Header t where t.[LR No_] = '"+lblLRNo.Text +"')");
        ddlRejectionReason.DataSource = dt1;
        ddlRejectionReason.DataTextField = "Code";
        ddlRejectionReason.DataValueField = "ID";
        ddlRejectionReason.DataBind();
        ddlRejectionReason.Items.Insert(0, new ListItem("-- Select Rejection Reason --", "0"));
        //---------------------Set Rejection reason --------------------------//
        if (LRobj.RejectionReason != null && LRobj.RejectionReason!="")
                    ddlRejectionReason.Items.FindByText(LRobj.RejectionReason.ToString()).Selected = true;
  
        // ddlRejectionReason(ViewState["LRNo"].ToString());
    }
    #endregion

    #region Select Dropdown Approval_Status of current LR
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
    #endregion

    #region Select Approval Status of Invoice
    private void SetApprovalStatus_ForInvoice(string LRNo, IList<BusinessLayerClasses.FreightInvoice> lstInvoice)
    {
        try
        {
            if (lstInvoice != null)
            {
                if (lstInvoice[0].Approval_Status_Invoice != null || lstInvoice[0].Approval_Status_Invoice != "")
                {
                    ddlApprovalStatus.Items.FindByText(lstInvoice[0].Approval_Status_Invoice).Selected = true;
                }
            }
            else
                ddlApprovalStatus.Items.FindByText("Open").Selected = true;
        }
        catch
        {
            ddlApprovalStatus.Items.FindByText("Open").Selected = true;
        }
    }
    #endregion

    #region Bind DiscountSchemeCode Dropdown List
    private void BindSpecialCode(string transporterCode)
    {
        DataTable dt = new DataTable();        
        dt = GetData("select PDS.ID,PDS.Code from TMS_LR_Header  LR left outer join [TMS_Payment_Discount_Scheme] PDS  on LR.company_Name_FK= PDS.company_Name_FK   where LR.[LR No_]='" + ViewState["LRNo"] + "' and ( LR.[Transporter Code]='" + transporterCode + "' or LR.[Transporter Code]='')");
        ddlSpecialDiscount.DataSource = dt;
        ddlSpecialDiscount.DataTextField = "Code";
        ddlSpecialDiscount.DataValueField = "ID";
        ddlSpecialDiscount.DataBind();
        ddlSpecialDiscount.Items.Insert(0, new ListItem("--Select Discount--", "0"));
    }
    #endregion

    #region Bind Delay Penalty Dropdown List
    private void BindDelayPenalty()
    {

        DataTable dt = new DataTable();
        dt = GetData("Select ID,Code From [dbo].[TMS_Penalty_Reason_Code] where company_Name_FK = (select T.company_Name_FK from TMS_LR_Header T where [LR No_] = '" + ViewState["LRNo"] + "') order by [Code_Name] ");
        ddlDelayPenalty.DataSource = dt;
        ddlDelayPenalty.DataTextField = "Code";
        ddlDelayPenalty.DataValueField = "ID";
        ddlDelayPenalty.DataBind();
        ddlDelayPenalty.Items.Insert(0, new ListItem("--Select Delay Penalty--", "0"));

    }
    #endregion

    #region Bind GridView grdInvoice
    protected void Bind_gridInvoice(string LRNo)
    {
        grdInvoice.DataSource = (new FreightInvoices()).GetFreightInvoice(LRNo);
        grdInvoice.DataBind();
    }
    #endregion

    #region GetData - Execute SQL Query
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
    #endregion

    #region Generate Initial Invoice (First time calculation when logged in by Transporter only)
    private void Generate_Initial_Invoice(string LRno)
    {

        string query = "Select count(ID) from [TMS_Portal_Freight_Invoice] " +
                         "Where [Last Entered Amt By Transporter] is not null and [Verified By Bill Processing Team] is not null and Approval_Status_Invoice is not null " +
                         "and [LR No_] =  '" + LRno + "'";
        DataTable dt = GetData(query);
        if (dt.Rows[0].ItemArray[0].ToString() == "0")
        {
            int rowInserted = 0;
            if (LRno != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("TMS_sp_Generate_Initial_Invoice", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@pLRNo", LRno);

                            cmd.Parameters.AddWithValue("@pDelayPenaltyCode", ddlDelayPenalty.SelectedIndex == 0 ? "" : ddlDelayPenalty.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@pDiscountShemeCode", ddlSpecialDiscount.SelectedIndex == 0 ? "" : ddlSpecialDiscount.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@pTransporterRemarks", txttransporterRemark.Text);
                            cmd.Parameters.AddWithValue("@pBillProcessingTeamRemarks", txtBillProcessTeamRemarks.Text);

                            ////Add the output parameter to the command object
                            //SqlParameter outPutParameter1 = new SqlParameter();
                            //outPutParameter1.ParameterName = "@pTransporterRemarks";
                            //outPutParameter1.SqlDbType = System.Data.SqlDbType.VarChar;
                            //outPutParameter1.Direction = System.Data.ParameterDirection.Output;
                            //outPutParameter1.Size = 250;
                            //cmd.Parameters.Add(outPutParameter1);

                            con.Open();
                            rowInserted = cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception E)
                {
                    lblerr.Text = E.Message;
                    lblerr.ForeColor = System.Drawing.Color.Red;
                    lblerr.Visible = true;
                }
            }
        }
    }
    #endregion
     
    #region Update LR table and Invoice Staging table
    private bool Update_LR_and_InvoiceStaging()
    {
        int recordUpdated = 0;
        bool isDataSendtoNav = false;
        bool result = false;

        try
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                con.Open();
                SqlCommand cmd = new SqlCommand("TMS_sp_Insert_InvoiceStaging");
                SqlTransaction transaction;
                transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted);
                cmd.Connection = con;
                cmd.Transaction = transaction;
                #region Transaction
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LRNo_", lblLRNo.Text.Trim());
                    if (ddlSpecialDiscount.SelectedItem.Value == "0")
                    {
                        cmd.Parameters.AddWithValue("@Payment_Discount_Scheme_Code", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Payment_Discount_Scheme_Code", ddlSpecialDiscount.SelectedItem.Text);
                    }
                    if (ddlDelayPenalty.SelectedItem.Value == "0")                    
                    {
                        cmd.Parameters.AddWithValue("@Penalty_Reason_Code", "");
                    }
                    else
                        cmd.Parameters.AddWithValue("@Penalty_Reason_Code", ddlDelayPenalty.SelectedItem.Text);


                    cmd.Parameters.AddWithValue("@TransporterRemarks", txttransporterRemark.Text);
                    cmd.Parameters.AddWithValue("@BillProcessingTeamRemark", txtBillProcessTeamRemarks.Text);

                    recordUpdated = cmd.ExecuteNonQuery(); //saved in database

                    if (recordUpdated > 0)
                    {
                        //Bind_gridInvoice(ViewState["LRNo"].ToString());
                        //BindLRHeader(ViewState["LRNo"].ToString());   

                        SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
                        isDataSendtoNav = objsenddatatonav.SendInvoiceDatatoNav(lblLRNo.Text.Trim()); //send invoice

                    }
                    else
                    {
                        lblerr.Text = "Entries cannot be submitted at this time, Please contact adminstrator ";
                        lblerr.Visible = true;
                    }
                }
                catch (Exception ex1)
                {
                    transaction.Rollback(); //manual                
                    Int32 changeBackApprovalstatus = GetSQLData("Update TMS_LR_Header Set [Approval Status] ='Pending for Invoice' where [LR No_] = '" + ViewState["LRNo"].ToString() + "'"); // lmaru8748
                    Int32 ChangeBacktoFreightInvoiceStatus = GetSQLData(" UPDATE tms_Portal_Freight_Invoice Set [Approval_Status_Invoice] ='Pending for Invoice' where [LR No_] = '" + ViewState["LRNo"].ToString() + "'");  // lmaru8748
                    con.Close();
                    lblerr.Text = ex1.Message;
                    lblerr.ForeColor = System.Drawing.Color.Red;
                    lblerr.Visible = true;
                }
                #endregion

                #region Commit Transaction
                if (recordUpdated > 0 && isDataSendtoNav == true)
                {
                    transaction.Commit();
                    con.Close();
                    lblerr.Text = "Entries updated and submitted at Nav system successfully";
                    lblerr.ForeColor = System.Drawing.Color.Green;
                    lblerr.Visible = true;
                    result = true;
                }
                else
                {
                    transaction.Rollback();
                    Int32 changeBackApprovalstatus = GetSQLData("Update TMS_LR_Header Set [Approval Status] ='Pending for Invoice' where [LR No_] = '" + ViewState["LRNo"].ToString() + "'");
                    Int32 ChangeBacktoFreightInvoiceStatus = GetSQLData(" UPDATE tms_Portal_Freight_Invoice Set [Approval_Status_Invoice] ='Pending for Invoice' where [LR No_] = '" + ViewState["LRNo"].ToString() + "'");
                    con.Close();
                    if (!isDataSendtoNav)
                        lblerr.Text = "The Invoice Stagging Table entry already exists in Nav System, Cannot be post twice.";
                    else
                        lblerr.Text = "Entries Cannot be submitted at this moment, Please contact administrator !  ";
                    lblerr.ForeColor = System.Drawing.Color.Red;
                    lblerr.Visible = true;
                }
                #endregion

            }
        }
        catch (Exception ed)
        {

        }
        return result;
    }
    #endregion
    
    #region Execute Update SQL command
    /// <summary>
    /// Execute SQL query - Returns Integer Value only
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    private Int32 GetSQLData(string query)
    {
        Int32 rowaffected = 0;
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
                    rowaffected= cmd.ExecuteNonQuery();

                    if (rowaffected > 0)
                    {
                        transaction.Commit();
                        con.Close();
                    }
                    return rowaffected;
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
    #endregion

    #region Update_Invoice_By_Transporter
    protected void Update_Invoice_By_Transporter()
    {
        int recordupdated = 0;
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("ID", typeof(int)),
                                                    new DataColumn("LRNo",  typeof(string)),
                                                    new DataColumn("Head", typeof(string)),         
                                                    new DataColumn("LastEnteredAmtByTransporter", typeof(decimal)),   
                                                    new DataColumn("VerifiedByBillProcessingTeam", typeof(int))});

            foreach (GridViewRow row in grdInvoice.Rows)
            {
                string key = grdInvoice.DataKeys[row.RowIndex].Value.ToString();

                TextBox txtLastEnteredAmt = row.FindControl("txtAmountConfirmed") as TextBox;
                CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;

                DataRow dr = dt.NewRow();
                dr["ID"] = int.Parse(key);
                dr["LRNo"] = ViewState["LRNo"].ToString();
                dr["Head"] = row.Cells[0].Text;
                dr["LastEnteredAmtByTransporter"] = txtLastEnteredAmt.Text;
                dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? 1 : 0;
                dt.Rows.Add(dr);// CIPL can update                 
            }

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                con.Open();
                SqlCommand cmd = new SqlCommand("TMS_sp_generate_invoice");
                SqlTransaction transaction;
                transaction = con.BeginTransaction("UPDATE");
                cmd.Connection = con;
                cmd.Transaction = transaction;
                #region Transaction
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMS_freightInvoiceTable", dt);
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@LRNo_", lblLRNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Payment_Discount_Scheme_ID", ddlSpecialDiscount.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@Penalty_Reason_Code_ID", ddlDelayPenalty.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@ApprovalStatus", 3); //Set status as 'Pending for Bill processing Team'
                    cmd.Parameters.AddWithValue("@TransporterRemarks", txttransporterRemark.Text);
                    cmd.Parameters.AddWithValue("@BillProcessingTeamRemark", txtBillProcessTeamRemarks.Text);

                    cmd.Parameters.AddWithValue("@pDelayPenaltyText", ddlDelayPenalty.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pTransporterText", ddlSpecialDiscount.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pChangeDelayPenalty", 0);
                    cmd.Parameters.AddWithValue("@pChangeSchemeCode", 0);
                    cmd.Parameters.AddWithValue("@RejectionReasonCode","");


                    recordupdated = cmd.ExecuteNonQuery(); //saved in database

                    if (recordupdated > 0)
                    {
                        transaction.Commit();
                        con.Close();
                        lblerr.Text = "Entries updated ! ";
                        lblerr.ForeColor = System.Drawing.Color.Green;
                        lblerr.Visible = true;
                    }
                    else
                    {
                        lblerr.Text = "Entries cannot be submitted at this time, Please contact adminstrator ";
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
        catch (Exception et)
        {
        }

        Bind_gridInvoice(ViewState["LRNo"].ToString());
        BindLRHeader(ViewState["LRNo"].ToString());
    }
    #endregion

    #region Update_Invoice_By_CIPL
    protected void Update_Invoice_By_CIPL(int chnageindelay)
    {
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("ID", typeof(int)),
                                                    new DataColumn("LRNo",  typeof(string)),
                                                    new DataColumn("Head", typeof(string)),         
                                                    new DataColumn("LastEnteredAmtByTransporter", typeof(decimal)),   
                                                    new DataColumn("VerifiedByBillProcessingTeam", typeof(int))});

            foreach (GridViewRow row in grdInvoice.Rows)
            {
                string key = grdInvoice.DataKeys[row.RowIndex].Value.ToString();

                TextBox txtLastEnteredAmt = row.FindControl("txtAmountConfirmed") as TextBox;
                CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;

                DataRow dr = dt.NewRow();
                dr["ID"] = int.Parse(key);
                dr["LRNo"] = ViewState["LRNo"].ToString();
                dr["Head"] = row.Cells[0].Text;
                dr["LastEnteredAmtByTransporter"] = txtLastEnteredAmt.Text;
                dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? 1 : 0;
                dt.Rows.Add(dr);// CIPL can update                 
            }

            bool AllLinesVerified = isAllLineVerified(dt);

            if (AllLinesVerified) //all row verified
            {
                if (Update_Invoice_Values(0, 0))
                {
                    if (Update_LR_and_InvoiceStaging())
                    {
                        ddlDelayPenalty.Enabled = false;
                        txtBillProcessTeamRemarks.Enabled = false;
                        grdInvoice.Enabled = false;
                        btnUpdate.Enabled = false;
                    }
                }
            }
            else
            {
                Update_Invoice_Values(0, 0);
            }
        }
        catch (Exception et)
        {
        }


    }
    #endregion

    #region isAllLineVerified (Get data of LR Line Verified by Third party
    private bool isAllLineVerified(DataTable dt)
    {
        bool result = true;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["VerifiedbyBillProcessingTeam"].ToString().ToLower() == "0") //if any of the row is not verified
            {
                if (Session["UserType"].ToString() == "Third Party")
                {
                    result = false;
                    mpe.Show();
                }
                break;
            }
        }
        UpdateFlagValue = result;
        return result;
    }
    #endregion
       
    #region Update_Invoice_Values (int changeDelayPenalty, int ChangeSchemeCode)
    protected bool Update_Invoice_Values(int changeDelayPenalty, int ChangeSchemeCode)
    {
        int recordupdated = 0;
        bool result = false;
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("ID", typeof(int)),
                                                    new DataColumn("LRNo",  typeof(string)),
                                                    new DataColumn("Head", typeof(string)),         
                                                    new DataColumn("LastEnteredAmtByTransporter", typeof(decimal)),   
                                                    new DataColumn("VerifiedByBillProcessingTeam", typeof(int))});

            foreach (GridViewRow row in grdInvoice.Rows)
            {
                string key = grdInvoice.DataKeys[row.RowIndex].Value.ToString();

                TextBox txtLastEnteredAmt = row.FindControl("txtAmountConfirmed") as TextBox;
                CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;

                DataRow dr = dt.NewRow();
                dr["ID"] = int.Parse(key);
                dr["LRNo"] = ViewState["LRNo"].ToString();
                dr["Head"] = row.Cells[0].Text;
                dr["LastEnteredAmtByTransporter"] = txtLastEnteredAmt.Text;
                dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? 1 : 0;
                dt.Rows.Add(dr);
            }

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                con.Open();
                SqlCommand cmd = new SqlCommand("TMS_sp_generate_invoice");
                SqlTransaction transaction;
                transaction = con.BeginTransaction("UPDATE");
                cmd.Connection = con;
                cmd.Transaction = transaction;
                #region Transaction
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMS_freightInvoiceTable", dt);
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@LRNo_", lblLRNo.Text.Trim());
                    if (ddlSpecialDiscount.SelectedIndex == 0)
                        cmd.Parameters.AddWithValue("@Payment_Discount_Scheme_ID", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Payment_Discount_Scheme_ID", ddlSpecialDiscount.SelectedItem.Value);
                    if(ddlDelayPenalty.SelectedIndex == 0)
                    cmd.Parameters.AddWithValue("@Penalty_Reason_Code_ID", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Penalty_Reason_Code_ID", ddlDelayPenalty.SelectedItem.Value);
                    if (UpdateFlagValue)
                        cmd.Parameters.AddWithValue("@ApprovalStatus", 11); //sent for invoice
                    else
                    {
                        if (Session["UserType"].ToString() == "Transporter")
                        {
                            cmd.Parameters.AddWithValue("@ApprovalStatus", 0); // Open
                        }
                        if (Session["UserType"].ToString() == "Third Party")
                        {
                            cmd.Parameters.AddWithValue("@ApprovalStatus", 2); // dispute
                        }
                    }
                    cmd.Parameters.AddWithValue("@TransporterRemarks", txttransporterRemark.Text);
                    cmd.Parameters.AddWithValue("@BillProcessingTeamRemark", txtBillProcessTeamRemarks.Text);
                    cmd.Parameters.AddWithValue("@pDelayPenaltyText", ddlDelayPenalty.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pTransporterText", ddlSpecialDiscount.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pChangeDelayPenalty", changeDelayPenalty);
                    cmd.Parameters.AddWithValue("@pChangeSchemeCode", ChangeSchemeCode);                   
                    cmd.Parameters.AddWithValue("@RejectionReasonCode",(ddlRejectionReason.SelectedIndex >0 ? ddlRejectionReason.SelectedItem.Text : "")); //pass rejection Code (For Report purpose- added by anu on 6th sep)
                    
                    recordupdated = cmd.ExecuteNonQuery(); //saved in database

                    if (recordupdated > 0)
                    {
                        transaction.Commit();
                        con.Close();
                        result = true;
                        lblerr.Text = "Entries submitted successfuly";
                        lblerr.Visible = true;
                        lblerr.ForeColor = System.Drawing.Color.Green;
                        ddlRejectionReason.Enabled = false;
                        lblmandatoryReasonCode.Visible = false;
                    }
                    else
                    {
                        lblerr.Text = "Entries cannot be submitted at this time, Please contact adminstrator ";
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
        catch (Exception et)
        {           
        }
     
        Bind_gridInvoice(ViewState["LRNo"].ToString());
        BindLRHeader(ViewState["LRNo"].ToString());
        return result;

    }
    #endregion
     
    #region Class InvoiceItem
    public class InvoiceItem
    {
        public string Head { get; set; }
        public string Amount { get; set; }
        public string AmountConfirmedByTransporter { get; set; }
        public string AmountConfirmedByThirdParty { get; set; }
    }
    #endregion
    
    #region Enable- Disable controls as per the logged-in user
    protected void EnableHeader_BasedOnUser()
    {
        ddlApprovalStatus.Enabled = false;
        if (Session["UserType"].ToString() == "Transporter")
        {
            txttransporterRemark.Enabled = true;
        }
        if (Session["UserType"].ToString() == "Third Party")
        {
            txtBillProcessTeamRemarks.Enabled = true;
            ddlSpecialDiscount.Enabled = false;  //third party cannot change scheme code
        }
        if (Session["UserType"].ToString() == "CIPL")
        {

            foreach (Control ct in Page.Controls)
            {
                DisableControls(ct);
            }
        }
        if (ddlApprovalStatus.SelectedItem.Text == "Invoiced")
        {
            txttransporterRemark.Enabled = false;
            txtBillProcessTeamRemarks.Enabled = false;
            ddlDelayPenalty.Enabled = false;
            ddlSpecialDiscount.Enabled = false;
            btnUpdate.Enabled = false;
            lblerr.Text = "This LR is Invoiced. Now changes cannot be done !";
            lblerr.ForeColor = System.Drawing.Color.Green;
            lblerr.Visible = true;
            grdInvoice.Enabled = false;
        }
    }
    #endregion

    #region DisableControls
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

   
}