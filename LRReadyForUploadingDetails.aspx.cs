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
using System.Globalization;


public partial class LRReadyForUploadingDetails : System.Web.UI.Page
{
    #region ****************************** Variable Declaration **********************************
    DataTable dtOperationalStatus = null;
    // DataTable dtCancellationReasonCode = null;
    bool UpdateFlagValue;
    string Isbtnclicked = "";
    static bool IsbtnCalclicked = false; //Added by Jyothi
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

                //string path = @"C:\Example.txt";
                //File.Create(path);

                if (Request.QueryString["IsDispute"] != null)
                {
                    if (Request.QueryString["IsDispute"] == "yes")
                    {
                        btnDispute.Style.Add("display", "none");
                    }

                }
                else
                {
                    btnDispute.Style.Add("display", "block");
                }


                ViewState.Add("LRNo", Request.QueryString["LRNo"].ToString());
                string LRNo = Request.QueryString["LRNo"].ToString();

                if (Session["UserType"].ToString() == "Transporter")
                {
                    h4Trans.Visible = true;
                }
                else
                {
                    h4Third.Visible = true;
                }
                BindGridview(LRNo);
                BindLRHeader(LRNo);
                if (Session["UserType"].ToString() == "Transporter" && Request.QueryString["IsDispute"] != "yes")
                {
                    Generate_Initial_Invoice(LRNo);
                }
                if (Session["UserType"].ToString() == "Third Party")
                {
                    btncalculate.Visible = false;
                }

                if (Session["UserType"].ToString() == "Transporter")
                {
                    rfvTransRemarks.Enabled = true;
                }
                else if (Session["UserType"].ToString() == "Third Party")
                {
                    rfvBillRemarks.Enabled = true;
                }
                BindLRFreightCalculation(LRNo);
                EnableDisableLRHeader();
                //   delayDeliveryDeduction(LRNo);
                //   EnableHeader_BasedOnUser();
                //  EnableDisableInvoiceButton(LRNo);
                CalendarExtender1.EndDate = DateTime.Now;
                CalendarExtender2.EndDate = DateTime.Now;
                //CalendarExtender3.EndDate = DateTime.Now;
                CalendarExtender4.EndDate = DateTime.Now;
                CalendarExtender5.EndDate = DateTime.Now;
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
    protected void btncalculate_Click(object sender, EventArgs e)
    {
        
        if (lbldistance.Text.Trim() == "")
        {
            lblerr.Text = "Distance master can not be empty.Please update the distance master.";
            lblerr.ForeColor = System.Drawing.Color.Red;
            lblerr.Visible = true;
            return;
        }
        else
        {
            lblerr.Visible = false;
        }
        //IsbtnCalclick = true;
        Isbtnclicked = "Calculate";
        IsbtnCalclicked = true;
        Generate_Initial_Invoice(ViewState["LRNo"].ToString());  //Added by Jyothi
        BindLRFreightCalculation(ViewState["LRNo"].ToString());

        if (Session["UserType"].ToString() == "Transporter")
        {
            //string a = txtGateOutDate.Text;

            string a = txtLoadingCharge.Text;
            //txtGateInDate.Text;
            Update_LR_Line_By_Transporter(); // here data gets pushed in nav which you have to stop
            string b = txtLoadingCharge.Text;
            string det = txtDetentionCharges.Text;
            Update_detention();
            //Update_Freight_By_Transporter();
            string c = txtLoadingCharge.Text;
            delayDeliveryDeduction(ViewState["LRNo"].ToString());
            TransitLoss();
            AccidentalLoss();
            billableAmount(ViewState["LRNo"].ToString());
            Update_Freight_By_Transporter();
            BindLRFreightCalculation(lblLRNo.Text.Trim());

            // redirect 
            DataTable dt = new DataTable();
            dt = GetData1("select [Approval Status] from TMS_LR_Header where [LR No_]='" + lblLRNo.Text.Trim() + "'  ");
            // string st = dt.Rows[0][0].ToString();
            string status = dt.Rows[0][0].ToString();



            if (status == "Pending at Bill Processing Team")
            {
                //Response.Redirect("LRReadyForUploadingDetails.aspx?LRNo=" + lblLRNo.Text.Trim());
                txtBillProcessingRemark.Enabled = false;
                txtTransporterRemark.Enabled = false;
                txtBreweryGateOutwardNo.Enabled = false;
                txtGateInDate.Enabled = false;
                TimeSelector1.Enabled = false;
                txtGateOutDate.Enabled = false;
                TimeSelector2.Enabled = false;
                txtLoadingCharge.Enabled = false;
                txtOthercharges.Enabled = false;
                txtTruckReportedDate.Enabled = false;
                txtTruckReportedTime.Enabled = false;
                txtTruckUnloadDate.Enabled = false;
                txtTruckUnloadedTime.Enabled = false;
                txtUnLoadingCharge.Enabled = false;

                grdLR.Enabled = false;

            }


        }
        if (Session["UserType"].ToString() == "Third Party")
        {
            Update_LR_Line_By_ThirdParty();
            Update_Freight_By_Transporter();



        }

        if (Session["UserType"].ToString() == "Admin")
        {
            billableAmount(ViewState["LRNo"].ToString());
            Update_Freight_By_Transporter();
            BindLRFreightCalculation(lblLRNo.Text.Trim());
        }
    }


    #endregion
    // calculate Delay penalty  orr delay delivery deduction
    protected void delayDeliveryDeduction(string Lrno)
    {
        //DateTime? gateOutDate = null;

        //DateTime ? truckReportedDate = null;
        //if (txtGateOutDate.Text != "")
        //{
        //    gateOutDate = DateTime.Parse(txtGateOutDate.Text);
        //}
        //if (txtTruckReportedDate.Text != "")
        //{
        //     truckReportedDate = DateTime.Parse(txtTruckReportedDate.Text);
        //}
        if (txtTruckReportedDate.Text != "" && txtGateOutDate.Text != "")
        {
            TimeSpan diff = DateTime.Parse(txtTruckReportedDate.Text) - DateTime.Parse(txtGateOutDate.Text);
            var NrOfDays = diff.TotalDays;



            DataTable dt = new DataTable();

            dt = GetData("select [Transit Days] from TMS_In_Transit_Duration where  [Brewery Location]=(select Location  from TMS_LR_Header where [LR No_]= '" + lblLRNo.Text.Trim() + "') and [Post Code] =(select [To post code]  from TMS_LR_Header where [LR No_]='" + lblLRNo.Text.Trim() + "')");
            string transdays = dt.Rows[0][0].ToString();
            double x = NrOfDays - double.Parse(transdays);
            lblDelaydeliverydays.Text = x.ToString() + " days";

            //--var NrOfDays = 1.0;
            decimal isAllrowVerified = 0;
            isAllrowVerified = GetSQLData_delaydelivery("select  [dbo].[TMS_fn_read_delayDeliveryDeduction]('" + lblLRNo.Text.Trim() + "','" + NrOfDays + "')");
            lbldelayPenalty.Text = isAllrowVerified.ToString();
            //edited by ishan
            if (Convert.ToDateTime(txtTruckReportedDate.Text) <= Convert.ToDateTime(lblExpDate.Text))
            {
                lbldelayPenalty.Text = "0";
            }

        }
    }

    protected void billableAmount(string Lrno)
    {
        decimal sumAllexpense = 0;
        sumAllexpense = decimal.Parse(lblFreightvalue.Text) + decimal.Parse(txtLoadingCharge.Text) + decimal.Parse(txtUnLoadingCharge.Text) + decimal.Parse(txtDetentionCharges.Text) + decimal.Parse(txtOthercharges.Text);
        decimal billableAmnt = 0;
        billableAmnt = GetSQLData_delaydelivery("select  [dbo].[TMS_fn_calculate_BillableAmount]('" + lblLRNo.Text.Trim() + "','" + sumAllexpense + "','" + lbldelayPenalty.Text.Trim() + "')");

        lblBillableamnt.Text = billableAmnt.ToString();
    }


    protected void TransitLoss()
    {
        decimal transitLoss = 0;
        transitLoss = GetSQLData_delaydelivery("select  [dbo].[TMS_fn_TransitLoss_InsertedIn _freigtTable]('" + lblLRNo.Text.Trim() + "')");
        lblTransitLossValue.Value = transitLoss.ToString();

    }

    protected void AccidentalLoss()
    {
        decimal accidentalloss = 0;
        accidentalloss = GetSQLData_delaydelivery("select  [dbo].[TMS_fn_AccidentalLoss_InsertedIn _freigtTable]('" + lblLRNo.Text.Trim() + "')");
        lblAccidentalloss.Value = accidentalloss.ToString();

    }


    private decimal GetSQLData_delaydelivery(string query)
    {
        decimal result = 0;
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


    public void Update_detention()
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
            con.Open();
            SqlCommand cmd = new SqlCommand("TMS_sp_Getfreightanddetention");
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UPDATE");
            cmd.Connection = con;
            cmd.Transaction = transaction;
            // #region Transaction
            try
            {
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pLRNo", lblLRNo.Text.Trim());
                cmd.Parameters.AddWithValue("@pTransporterType", lblTransporterType.Text == "Dedicated" ? 0 : 1);
                cmd.ExecuteNonQuery();
                transaction.Commit();

            }
            catch (Exception ex1)
            {
                con.Close();
                lblerr.Text = ex1.Message;
                lblerr.ForeColor = System.Drawing.Color.Red;
                lblerr.Visible = true;
            }
        }

        //BindLRFreightCalculation(lblLRNo.Text.Trim());

        DataTable dt = new DataTable();
        dt = GetData1("select [Last Entered Amt By Transporter] from TMS_Portal_Freight_Invoice where [LR No_]='" + lblLRNo.Text.Trim() + "' and  Head ='Detention' ");
        // string st = dt.Rows[0][0].ToString();
        txtDetentionCharges.Text = dt.Rows[0][0].ToString();

    }



    #region Update_Invoice_By_Transporter
    protected void Update_Freight_By_Transporter()
    {

        //Generate_Initial_Invoice(Request.QueryString["LRNo"].ToString());
        //BindLRFreightCalculation(Request.QueryString["LRNo"].ToString());
        //  txtDetentionCharges.Text=
        //Unloading charges
        //Loading charges
        //Detention
        //Others
        //Transit Losses
        //Accidental Losses
        //Discount
        //Delay Penalty
        //Total Invoice Value
        int recordupdated = 0;
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("ID", typeof(int)),
                                                    new DataColumn("LRNo",  typeof(string)),
                                                    new DataColumn("Head", typeof(string)),         
                                                    new DataColumn("LastEnteredAmtByTransporter", typeof(decimal)),   
                                                    new DataColumn("VerifiedByBillProcessingTeam", typeof(int))});

            var myFourRowArray = new List<FreightClass>();

            myFourRowArray.Add(new FreightClass(int.Parse(IDFreight.Value), ViewState["LRNo"].ToString(), "Freight", lblFreightvalue.Text.ToString()));
            myFourRowArray.Add(new FreightClass(int.Parse(IDUnLoadingCharge.Value), ViewState["LRNo"].ToString(), "Unloading charges", txtUnLoadingCharge.Text.ToString()));
            myFourRowArray.Add(new FreightClass(int.Parse(IDLoadingCharge.Value), ViewState["LRNo"].ToString(), "Loading charges", txtLoadingCharge.Text.ToString()));
            myFourRowArray.Add(new FreightClass(int.Parse(IdDetCharge.Value), ViewState["LRNo"].ToString(), "Detention", txtDetentionCharges.Text.ToString()));
            myFourRowArray.Add(new FreightClass(int.Parse(IDothers.Value), ViewState["LRNo"].ToString(), "Others", txtOthercharges.Text.ToString()));

            //
            myFourRowArray.Add(new FreightClass(int.Parse(IDTransitLoss.Value), ViewState["LRNo"].ToString(), "Transit Losses", lblTransitLossValue.Value.ToString()));
            myFourRowArray.Add(new FreightClass(int.Parse(IDAccidentalLosss.Value), ViewState["LRNo"].ToString(), "Accidental Losses", lblAccidentalloss.Value.ToString()));
            myFourRowArray.Add(new FreightClass(int.Parse(IDDiscount.Value), ViewState["LRNo"].ToString(), "Express Payment Term", "0.0"));

            myFourRowArray.Add(new FreightClass(int.Parse(iDdelaypenalty.Value), ViewState["LRNo"].ToString(), "Delay Penalty", lbldelayPenalty.Text.ToString()));

            myFourRowArray.Add(new FreightClass(int.Parse(IDTotalInvoiceValue.Value), ViewState["LRNo"].ToString(), "Total Invoice Value", lblBillableamnt.Text.ToString()));


            //




            foreach (FreightClass list1 in myFourRowArray)
            {
                DataRow dr1 = dt.NewRow();
                dr1["ID"] = list1.ID;
                dr1["LRNo"] = ViewState["LRNo"].ToString();
                dr1["Head"] = list1.Head;
                dr1["LastEnteredAmtByTransporter"] = list1.Amount;
                string case1 = list1.Head;
                switch (case1)
                {
                    case "Freight":
                        dr1["VerifiedByBillProcessingTeam"] = (chkfrght.Checked) ? 1 : 0;
                        break;
                    case "Unloading charges":
                        dr1["VerifiedByBillProcessingTeam"] = (chkUnLoadingCharge.Checked) ? 1 : 0;
                        break;
                    case "Loading charges":
                        dr1["VerifiedByBillProcessingTeam"] = (chkLoadingCharge.Checked) ? 1 : 0;
                        break;
                    case "Detention":
                        dr1["VerifiedByBillProcessingTeam"] = (chkDetentionCharges.Checked) ? 1 : 0;
                        break;
                    case "Others":
                        dr1["VerifiedByBillProcessingTeam"] = (chkOtherCharges.Checked) ? 1 : 0;
                        break;

                    case "Delay Penalty":
                        dr1["VerifiedByBillProcessingTeam"] = (chkDelayPenalty.Checked) ? 1 : 0;
                        break;
                    case "Total Invoice Value":
                        dr1["VerifiedByBillProcessingTeam"] = (Session["UserType"].ToString() == "Transporter") ? 0 : 1;
                        break;

                    case "Express Payment Term":
                        dr1["VerifiedByBillProcessingTeam"] = (Session["UserType"].ToString() == "Transporter") ? 0 : 1;
                        break;

                    case "Transit Losses":
                        dr1["VerifiedByBillProcessingTeam"] = (Session["UserType"].ToString() == "Transporter") ? 0 : 1;
                        break;

                    case "Accidental Losses":
                        dr1["VerifiedByBillProcessingTeam"] = (Session["UserType"].ToString() == "Transporter") ? 0 : 1;
                        break;



                }

                dt.Rows.Add(dr1);// CIPL can update                 
            }

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                con.Open();
                SqlCommand cmd = new SqlCommand("TMS_sp_submit_ByTransporter");
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
                    cmd.Parameters.AddWithValue("@Payment_Discount_Scheme_ID", "");
                    cmd.Parameters.AddWithValue("@Penalty_Reason_Code_ID", "");

                    if (Session["UserType"].ToString() == "Transporter")
                    {
                        cmd.Parameters.AddWithValue("@ApprovalStatus", 5);  // set status 
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ApprovalStatus", 10);//Set status as 'Pending for Bill processing Team'
                    }


                    cmd.Parameters.AddWithValue("@TransporterRemarks", txtTransporterRemark.Text);
                    cmd.Parameters.AddWithValue("@BillProcessingTeamRemark", txtBillProcessingRemark.Text);

                    cmd.Parameters.AddWithValue("@pDelayPenaltyText", "");
                    cmd.Parameters.AddWithValue("@pTransporterText", "");
                    cmd.Parameters.AddWithValue("@pChangeDelayPenalty", 0);
                    cmd.Parameters.AddWithValue("@pChangeSchemeCode", 0);
                    cmd.Parameters.AddWithValue("@RejectionReasonCode", "");


                    //int isPendingForInvoiced = GetSQLData("select sum([Transit Loss]),sum([Accidental Loss]) from TMS_LR_Lines where [LR No_] ='" + lblLRNo.Text.Trim() + "' ");
                    //  string query = GetSQLData("select sum([Transit Loss]),sum([Accidental Loss]) from TMS_LR_Lines  where [LR No_]='"+ lblLRNo.Text.Trim()+"' ") 




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

        // BindLRFreightCalculation(ViewState["LRNo"].ToString());
        // BindLRHeader(ViewState["LRNo"].ToString());
        //EnableDisableLRHeader();
    }
    #endregion


    #region Update_Invoice_By_THirdParty
    protected void Update_Freight_by_thirdparty()
    {

    }
    #endregion
    //    #region commented code
    //    string BreweryGateOutwardNo = txtBreweryGateOutwardNo.Text;

    //    string GateOutDate = txtGateOutDate.SelectedDate.ToShortDateString();

    //    string GOTime = TimeSelector2.Hour.ToString() + ":" + TimeSelector2.Minute.ToString() + ":" + TimeSelector2.Second.ToString() + " " + TimeSelector2.AmPm;
    //    DateTime dt1 = DateTime.Parse(GOTime); // No error check
    //    string GateOutTime = dt1.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);


    //    string ReachedDestinationDate = txtReachedDestinationDate.SelectedDate.ToShortDateString();
    //    string RDTime = txtReachedDestinationTime.Hour.ToString() + ":" + txtReachedDestinationTime.Minute.ToString() + ":" + txtReachedDestinationTime.Second.ToString() + " " + txtReachedDestinationTime.AmPm;
    //    DateTime dt2 = DateTime.Parse(RDTime); // No error check
    //    string ReachedDestinationTime = dt2.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

    //    string TruckReceivedDate = txtTruckReceivedDate.SelectedDate.ToShortDateString();
    //    string TRTime = txtTruckReceivedTime.Hour.ToString() + ":" + txtTruckReceivedTime.Minute.ToString() + ":" + txtTruckReceivedTime.Second.ToString() + " " + txtTruckReceivedTime.AmPm;
    //    DateTime dt3 = DateTime.Parse(TRTime); // No error check
    //    string TruckReceivedTime = dt3.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

    //    string TruckReleaseDate = txtTruckReleaseDate.SelectedDate.ToShortDateString();
    //    string TRLTime = txtTruckReleaseTime.Hour.ToString() + ":" + txtTruckReleaseTime.Minute.ToString() + ":" + txtTruckReleaseTime.Second.ToString() + " " + txtTruckReleaseTime.AmPm;
    //    DateTime dt4 = DateTime.Parse(TRLTime); // No error check
    //    string TruckReleaseTime = dt4.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);


    //    if (GateOutDate == "01-01-0001" || GateOutTime == "" || ReachedDestinationDate == "01-01-0001" || ReachedDestinationTime == "" || TruckReceivedDate == "01-01-0001" || TruckReceivedTime == "" || TruckReleaseDate == "01-01-0001" || TruckReleaseTime == "")
    //    {
    //        lblerr.Text = "Gate Out Date/Time, Reached Destination Date/Time ,Truck Received Date/Time or Truck Release Date/time is not filled by Transporter." + "\n" + Environment.NewLine +
    //             "Entries cannot be submitted before Transporter response";
    //        lblerr.Visible = true;
    //        lblerr.ForeColor = System.Drawing.Color.Red;
    //        return;
    //    }

    //    UpdateFlagValue = true;
    //    int recordUpdated = 0;
    //    bool isDataSendtoNav = false;
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        dt.Columns.AddRange(new DataColumn[4] { new DataColumn("ID", typeof(int)),
    //                                            new DataColumn("TransitLossQty",  typeof(int)),
    //                                            new DataColumn("AccidentalLossQty", typeof(int)),                                              
    //                                            new DataColumn("VerifiedByBillProcessingTeam", typeof(string))});
    //        int a = grdLR.PageIndex;
    //        for (int i = 0; i < grdLR.PageCount; i++)
    //        {
    //            grdLR.SetPageIndex(i);
    //            foreach (GridViewRow row in grdLR.Rows)
    //            {
    //                string key = grdLR.DataKeys[row.RowIndex].Value.ToString();

    //                TextBox txtTransitLossQty = row.FindControl("txtTransitLossQty") as TextBox;
    //                TextBox txtAccidentalLossQty = row.FindControl("txtAccidentalLossQty") as TextBox;
    //                CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;

    //                DataRow dr = dt.NewRow();
    //                dr["ID"] = int.Parse(key);
    //                if (txtTransitLossQty.Text != "")
    //                {
    //                    dr["TransitLossQty"] = int.Parse(txtTransitLossQty.Text);
    //                }
    //                if (txtAccidentalLossQty.Text != "")
    //                {
    //                    dr["AccidentalLossQty"] = int.Parse(txtAccidentalLossQty.Text);
    //                }
    //                dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? "1" : "0";

    //                if (Session["UserType"].ToString() == "Transporter")
    //                {
    //                    if (txtTransitLossQty.Text != "" || txtAccidentalLossQty.Text != "")
    //                        dt.Rows.Add(dr); // if no breakage value then dont add in DT
    //                }
    //                if (Session["UserType"].ToString() == "CIPL")
    //                {
    //                    dt.Rows.Add(dr);// CIPL can update
    //                }
    //            }
    //        }
    //        grdLR.SetPageIndex(a);

    //        UpdateFlagValue = GetUpdateFlagValueFromGrid(dt);
    //        using (SqlConnection con = new SqlConnection())
    //        {
    //            con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
    //            con.Open();
    //            SqlCommand cmd = new SqlCommand("TMS_sp_update_LRLineEntry");
    //            SqlTransaction transaction;
    //            transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted);
    //            cmd.Connection = con;
    //            cmd.Transaction = transaction;
    //            #region Transaction
    //            try
    //            {
    //                cmd.CommandTimeout = 0;
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMSRequestUpdateTable", dt);
    //                sqlParameter.SqlDbType = SqlDbType.Structured;
    //                cmd.Parameters.AddWithValue("@UserType", Session["UserType"].ToString());
    //                cmd.Parameters.AddWithValue("@LRNo", lblLRNo.Text.Trim());
    //                if (Session["UserType"].ToString() == "Transporter")
    //                {
    //                    cmd.Parameters.AddWithValue("@ApprovalStatus", 3); //Set Pending for Bill processing Team
    //                }
    //                if (Session["UserType"].ToString() == "CIPL") // Check If all rows are verified
    //                {
    //                    bool isAllrowVerified = false;
    //                    foreach (GridViewRow row in grdLR.Rows)
    //                    {
    //                        CheckBox chkVerifiedbyBill = row.FindControl("chkVerifiedbyBill") as CheckBox;
    //                        if (chkVerifiedbyBill.Checked)
    //                        {
    //                            isAllrowVerified = true;
    //                        }
    //                        else
    //                        {
    //                            isAllrowVerified = false;
    //                        }
    //                    }
    //                    if (isAllrowVerified)
    //                        cmd.Parameters.AddWithValue("@ApprovalStatus", 4); //Set Accepted                      
    //                    else
    //                        cmd.Parameters.AddWithValue("@ApprovalStatus", 3);
    //                }
    //                cmd.Parameters.AddWithValue("@TransporterRemark", txtTransporterRemark.Text.Trim());
    //                cmd.Parameters.AddWithValue("@BillProcessingRemark", txtBillProcessingRemark.Text.Trim());
    //                //added new parameters on 27July2016
    //                cmd.Parameters.AddWithValue("@BreweryGateOutwardNo", BreweryGateOutwardNo);
    //                cmd.Parameters.AddWithValue("@GateOutDate", GateOutDate);
    //                cmd.Parameters.AddWithValue("@GateOutTime", GateOutTime);
    //                cmd.Parameters.AddWithValue("@ReachedDestinationDate", ReachedDestinationDate);
    //                cmd.Parameters.AddWithValue("@ReachedDestinationTime", ReachedDestinationTime);
    //                cmd.Parameters.AddWithValue("@TruckReceivedDate", TruckReceivedDate);
    //                cmd.Parameters.AddWithValue("@TruckReceivedTime", TruckReceivedTime);
    //                cmd.Parameters.AddWithValue("@TruckReleaseDate", TruckReleaseDate);
    //                cmd.Parameters.AddWithValue("@TruckReleaseTime", TruckReleaseTime);

    //                recordUpdated = cmd.ExecuteNonQuery(); //saved in database
    //                if (recordUpdated > 0)
    //                {
    //                    Int32 isAllrowVerified = 0;
    //                    isAllrowVerified = GetSQLData("select [dbo].[isAllLRLineVerified]('" + lblLRNo.Text.Trim() + "')");

    //                    if (isAllrowVerified == 1)
    //                    {
    //                        //Send updated Data to Nav-- Called Web service iof Navision
    //                        SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
    //                        isDataSendtoNav = objsenddatatonav.SendLRtoNAV(lblLRNo.Text.Trim());
    //                    }
    //                    else
    //                    {
    //                        transaction.Commit();
    //                        con.Close();
    //                        lblerr.Text = "Entries submitted successfully";
    //                        lblerr.ForeColor = System.Drawing.Color.Green;
    //                        lblerr.Visible = true;
    //                        BindGridview(ViewState["LRNo"].ToString());
    //                        BindLRHeader(ViewState["LRNo"].ToString());
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    lblerr.Text = "Entries cannot be submitted at this time, Please contact adminstrator ";
    //                    lblerr.Visible = true;
    //                }
    //            }
    //            catch (Exception ex1)
    //            {
    //                try
    //                {
    //                    transaction.Rollback(); //manual                            
    //                    con.Close();
    //                    lblerr.Text = ex1.Message;
    //                    lblerr.ForeColor = System.Drawing.Color.Red;
    //                    lblerr.Visible = true;
    //                }
    //                catch (Exception ex2)
    //                {
    //                    lblerr.Text = ex2.Message;
    //                    lblerr.ForeColor = System.Drawing.Color.Red;
    //                    lblerr.Visible = true;
    //                }
    //            }
    //            #endregion

    //            #region Commit Transaction
    //            if (recordUpdated > 0 && isDataSendtoNav == true)
    //            {
    //                transaction.Commit();
    //                con.Close();
    //                lblerr.Text = "Entries submitted successfully";
    //                lblerr.ForeColor = System.Drawing.Color.Green;
    //                lblerr.Visible = true;
    //            }
    //            else
    //            {
    //                transaction.Rollback();
    //                con.Close();
    //                lblerr.Text = "Entries cannot be submit at this time, Please contact adminstrator ";
    //                lblerr.ForeColor = System.Drawing.Color.Red;
    //                lblerr.Visible = true;
    //            }
    //            #endregion
    //        }
    //        BindGridview(ViewState["LRNo"].ToString());
    //        BindLRHeader(ViewState["LRNo"].ToString());
    //        DisableInvoiceButton(ViewState["LRNo"].ToString());
    //    }
    //    catch (Exception es)
    //    {

    //    }
    //    #endregion
    //}


    //#region btnInvoice_Click  - Redirect to Invoice Page
    ///// <summary>
    ///// Button Invoice Event 
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btnInvoice_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("FreightInvoice.aspx?LRNo=" + ViewState["LRNo"].ToString());
    //}
    //#endregion




    private void BindGridview1(string LRno)
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

    #region Model Pop up - btnOk_Click
    //protected void btnOK_Click(object sender, EventArgs e)
    //{
    //    mpe.Hide();
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        dt.Columns.AddRange(new DataColumn[4] { new DataColumn("ID", typeof(int)),
    //                                            new DataColumn("TransitLossQty",  typeof(int)),
    //                                            new DataColumn("AccidentalLossQty", typeof(int)),                                              
    //                                            new DataColumn("VerifiedByBillProcessingTeam", typeof(string))});
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
    //                dr["TransitLossQty"] = decimal.Parse(txtTransitLossQty.Text);
    //            }
    //            if (txtAccidentalLossQty.Text != "")
    //            {
    //                dr["AccidentalLossQty"] = decimal.Parse(txtAccidentalLossQty.Text);
    //            }
    //            dr["VerifiedByBillProcessingTeam"] = (chkVerifiedbyBill.Checked) ? "1" : "0";
    //            dt.Rows.Add(dr);
    //        }
    //        UpdateLR_byCIPL_and_SendDatatoNAV(dt);
    //    }
    //    catch (Exception es)
    //    {
    //    }
    //}
    #endregion

    #region Model Pop up - btnCancel_Click
    //protected void btnCancel_Click(object sender, EventArgs e)
    //{
    //    mpe.Hide();
    //    //do nothing
    //}

    //private void UpdateLR_byCIPL_and_SendDatatoNAV(DataTable dt)
    //{
    //    int recordUpdated = 0;
    //    bool isDataSendtoNav = false;

    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection())
    //        {
    //            con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
    //            con.Open();
    //            SqlCommand cmd = new SqlCommand("TMS_sp_update_LRLine_By_CIPL");
    //            SqlTransaction transaction;
    //            transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted);
    //            cmd.Connection = con;
    //            cmd.Transaction = transaction;
    //            #region Transaction
    //            try
    //            {
    //                cmd.CommandTimeout = 0;
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMSRequestUpdateTable", dt);
    //                sqlParameter.SqlDbType = SqlDbType.Structured;
    //                cmd.Parameters.AddWithValue("@LRNo", lblLRNo.Text.Trim());
    //                bool isAllrowVerified = false;
    //                //int b = grdLR.PageIndex; //code to get all rows data including all paging
    //                //for (int j = 0; j < grdLR.PageCount; j++)
    //                //{
    //                //    grdLR.SetPageIndex(j);
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
    //                //}
    //                //grdLR.SetPageIndex(b);

    //                if (isAllrowVerified)
    //                    cmd.Parameters.AddWithValue("@ApprovalStatus", 4); //Set Accepted                      
    //                else
    //                {
    //                    if (isTransitAccidentialValue_Grid(dt)) //if all rows has transit/accidential loss value by transporter
    //                    {
    //                        cmd.Parameters.AddWithValue("@ApprovalStatus", 2); //Dispute
    //                        //SendMailOfDisputeToTransporter(lblLRNo.Text.ToString());
    //                    }
    //                    else
    //                        cmd.Parameters.AddWithValue("@ApprovalStatus", 5); //Set 'Pending for Transporter Action'  
    //                }

    //                cmd.Parameters.AddWithValue("@BillProcessingRemark", txtBillProcessingRemark.Text.Trim());

    //                recordUpdated = cmd.ExecuteNonQuery(); //saved in database
    //                if (recordUpdated > 0)
    //                {
    //                    Int32 isAll_rowVerified = 0;
    //                    isAll_rowVerified = GetSQLData("select [dbo].[isAllLRLineVerified]('" + lblLRNo.Text.Trim() + "')"); //call SQL function

    //                    if (isAll_rowVerified == 1)
    //                    {
    //                        //Send updated Data to Nav-- Called Web service of Navision
    //                        SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
    //                        isDataSendtoNav = objsenddatatonav.SendLRtoNAV(lblLRNo.Text.Trim());

    //                        if (isDataSendtoNav)
    //                        {
    //                            cmd = new SqlCommand();
    //                            cmd.CommandText = "Update TMS_LR_Header set [Approval Status] = 'Pending for Invoice' where [LR No_] = '" + lblLRNo.Text.Trim() + "'";
    //                            cmd.Connection = con;
    //                            cmd.Transaction = transaction;
    //                            int LrStatusUpdate = cmd.ExecuteNonQuery();
    //                            SendMailToTransporter(lblLRNo.Text);
    //                        }

    //                    }
    //                    else
    //                    {
    //                        transaction.Commit();
    //                        con.Close();
    //                        lblerr.Text = "Entries submitted successfully";
    //                        lblerr.ForeColor = System.Drawing.Color.Green;
    //                        lblerr.Visible = true;
    //                        BindGridview(ViewState["LRNo"].ToString());
    //                        BindLRHeader(ViewState["LRNo"].ToString());
    //                        EnableDisableInvoiceButton(ViewState["LRNo"].ToString());
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    lblerr.Text = "Entries cannot be submitted at this time, Please contact adminstrator ";
    //                    lblerr.Visible = true;
    //                }
    //            }
    //            catch (Exception ex1)
    //            {
    //                try
    //                {
    //                    transaction.Rollback(); //manual                            
    //                    con.Close();
    //                    lblerr.Text = ex1.Message;
    //                    lblerr.ForeColor = System.Drawing.Color.Red;
    //                    lblerr.Visible = true;

    //                }
    //                catch (Exception ex2)
    //                {
    //                    lblerr.Text = ex2.Message;
    //                    lblerr.ForeColor = System.Drawing.Color.Red;
    //                    lblerr.Visible = true;

    //                }
    //            }
    //            #endregiont

    //            #region Commit Transaction
    //            if (recordUpdated > 0 && isDataSendtoNav == true)
    //            {
    //                transaction.Commit();
    //                con.Close();
    //                lblerr.Text = "Entries updated and submitted to Nav system successfully";
    //                lblerr.ForeColor = System.Drawing.Color.Green;
    //                lblerr.Visible = true;
    //            }
    //            else
    //            {
    //                transaction.Rollback();
    //                con.Close();
    //                lblerr.Text = "Entries cannot be submit at this time, Please contact adminstrator ";
    //                lblerr.ForeColor = System.Drawing.Color.Red;
    //                lblerr.Visible = true;
    //            }
    //            #endregion
    //        }
    //        BindGridview(ViewState["LRNo"].ToString());
    //        BindLRHeader(ViewState["LRNo"].ToString());
    //        EnableDisableInvoiceButton(ViewState["LRNo"].ToString());
    //    }
    //    catch (Exception es)
    //    {

    //    }

    //}
    #endregion

    #region EnableHeader_BasedOnUser -  Enable- Disable controls as per the logged-in user
    //protected void EnableHeader_BasedOnUser()
    //{
    //    if (UpdateFlagValue) // i.e. All rows are verified by BillProcessing Team -> Then disable textbox
    //    {
    //        //txtBillProcessingRemark.Enabled = false;
    //        //txtTransporterRemark.Enabled = false;

    //        //txtBreweryGateOutwardNo.Enabled = false;
    //        //txtGateOutDate.Enabled = false;
    //        //TimeSelector2.Enabled = false;
    //        //TimeSelector2.ReadOnly = true;
    //        //txtReachedDestinationDate.Enabled = false;
    //        //txtReachedDestinationTime.Enabled = false;
    //        //txtReachedDestinationTime.ReadOnly = true;
    //        //txtTruckReceivedDate.Enabled = false;
    //        //txtTruckReceivedTime.Enabled = false;
    //        //txtTruckReceivedTime.ReadOnly = true;
    //        //txtTruckReleaseDate.Enabled = false;
    //        //txtTruckReleaseTime.Enabled = false;
    //        //txtTruckReleaseTime.ReadOnly = true;
    //        //btnSubmit.Enabled = false;


    //        foreach (Control ct in Page.Controls)
    //        {
    //            DisableControls(ct);
    //            TimeSelector2.Enabled = false;
    //            TimeSelector2.ReadOnly = true;
    //            txtReachedDestinationTime.Enabled = false;
    //            txtReachedDestinationTime.ReadOnly = true;
    //            txtTruckReceivedTime.Enabled = false;
    //            txtTruckReceivedTime.ReadOnly = true;
    //            txtTruckReleaseTime.Enabled = false;
    //            txtTruckReleaseTime.ReadOnly = true;
    //        }


    //    }

    //    else
    //    {
    //        if (Session["UserType"].ToString() == "Transporter")
    //        {
    //            txtTransporterRemark.Enabled = true;
    //        }
    //        else if (Session["UserType"].ToString() == "Third Party")
    //        {
    //            txtBillProcessingRemark.Enabled = true;
    //            EnableDisableSubmitButton(ViewState["LRNo"].ToString());

    //            txtBreweryGateOutwardNo.Enabled = false;
    //            txtGateOutDate.Enabled = false;
    //            TimeSelector2.Enabled = false;
    //            TimeSelector2.ReadOnly = true;
    //            txtReachedDestinationDate.Enabled = false;
    //            txtReachedDestinationTime.Enabled = false;
    //            txtReachedDestinationTime.ReadOnly = true;
    //            txtTruckReceivedDate.Enabled = false;
    //            txtTruckReceivedTime.Enabled = false;
    //            txtTruckReceivedTime.ReadOnly = true;
    //            txtTruckReleaseDate.Enabled = false;
    //            txtTruckReleaseTime.Enabled = false;
    //            txtTruckReleaseTime.ReadOnly = true;
    //        }
    //        else
    //        {
    //            foreach (Control ct in Page.Controls)
    //            {
    //                DisableControls(ct);

    //                TimeSelector2.Enabled = false;
    //                TimeSelector2.ReadOnly = true;
    //                txtReachedDestinationTime.Enabled = false;
    //                txtReachedDestinationTime.ReadOnly = true;
    //                txtTruckReceivedTime.Enabled = false;
    //                txtTruckReceivedTime.ReadOnly = true;
    //                txtTruckReleaseTime.Enabled = false;
    //                txtTruckReleaseTime.ReadOnly = true;
    //            }

    //        }
    //    }
    //}
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
    //private void EnableDisableInvoiceButton(string LRNo)
    //{
    //    Int32 result = GetSQLData("select count(TL.[Verified by Bill Processing]) from TMS_LR_Lines TL  where [LR No_] ='" + LRNo + "' and TL.[Verified by Bill Processing]=0");
    //    if (result > 0)
    //    {
    //        btnInvoice.Enabled = false;
    //    }
    //    else
    //    {
    //        btnInvoice.Enabled = true;
    //    }

    //    int isPendingForInvoiced = GetSQLData("select count(*) from TMS_LR_Header where [LR No_] ='" + LRNo + "' and [Approval Status]='Pending for Invoice'");
    //    if (isPendingForInvoiced > 0)
    //    {
    //        btnSubmit.Enabled = false;
    //    }
    //}
    #endregion

    #region EnableDisableSubmitButton  -  Disable Submit Button if any of the row does  not have transit/Accidential last entered amount
    //private void EnableDisableSubmitButton(string LRNo)
    //{
    //    Int32 hasAll_row_TransitAccidentalLoss = 0;
    //    hasAll_row_TransitAccidentalLoss = GetSQLData("Select  [dbo].[isAllLR_transitAccidential_EnteredByTransporter]('" + ViewState["LRNo"] + "')");      //call SQL function

    //    if (hasAll_row_TransitAccidentalLoss == 1)
    //    {
    //        btnSubmit.Enabled = true;
    //    }
    //    else
    //    {
    //        btnSubmit.Enabled = false;
    //    }
    //}
    #endregion

    /// <summary>
    /// Update LR by Transporter
    /// </summary>
    protected void Update_LR_Line_By_Transporter()
    {

        //------------------------------------Update LR Header --------------------------------------------------------------------

        string BreweryGateOutwardNo = txtBreweryGateOutwardNo.Text;
        // gate in Date and  time

        string GateInDate = Convert.ToDateTime(txtGateInDate.Text).ToShortDateString();
        string GOGateInTime = TimeSelector1.Hour.ToString() + ":" + TimeSelector1.Minute.ToString() + ":" + TimeSelector1.Second.ToString() + " " + TimeSelector1.AmPm;

        //  string test = TimeSelector1.Hour.ToString() + ":" + TimeSelector1.Minute.ToString() + ":" + TimeSelector1.Second.ToString() + "" + TimeSelector1.AmPm;
        // DateTime dtGateIn = DateTime.Parse(GOGateInTime); // No error check //15-12-2016 02:02:00
        DateTime dtGateInTime = Convert.ToDateTime(GateInDate + " " + GOGateInTime); //14-12-2016 02:02:00
        //string GateOutTime = dtGateIn.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);


        //string one = "13/02/09";
        string one = Convert.ToDateTime(txtGateInDate.Text).ToString("dd/MM/yy").Replace('-', '/');
        string two = "2:35:10 PM";  //4:4:0 PM
        // string ttt = g.Replace('-','/');
        //string w = datetime.now.ToString("dd/MM/yy");
        string az = "4:4:0 PM";



        DateTime dtt = Convert.ToDateTime(GateInDate + " " + GOGateInTime); //14-12-2016 02:02:00
        DateTime dtt1 = DateTime.ParseExact(one + " " + two, "dd/MM/yy h:mm:ss tt", CultureInfo.InvariantCulture); //13-02-2009 14:35:10
        // DateTime.ParseExact(



        //  gateOutDate and time
        string GateOutDate = Convert.ToDateTime(txtGateOutDate.Text).ToShortDateString();
        string GOTime = TimeSelector2.Hour.ToString() + ":" + TimeSelector2.Minute.ToString() + ":" + TimeSelector2.Second.ToString() + " " + TimeSelector2.AmPm;
        DateTime dt1 = DateTime.Parse(GOTime); // No error check
        DateTime dtGateOutTime = Convert.ToDateTime(GateOutDate + " " + GOTime); //14-12-2016 02:02:00

        string GateOutTime = dt1.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
        // truck reported date
        string ReachedDestinationDate = Convert.ToDateTime(txtTruckReportedDate.Text).ToShortDateString();
        string RDTime = txtTruckReportedTime.Hour.ToString() + ":" + txtTruckReportedTime.Minute.ToString() + ":" + txtTruckReportedTime.Second.ToString() + " " + txtTruckReportedTime.AmPm;
        DateTime dt2 = DateTime.Parse(RDTime); // No error check
        string ReachedDestinationTime = dt2.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
        // truck unloaded date  and time
        string TruckReleaseDate = Convert.ToDateTime(txtTruckUnloadDate.Text).ToShortDateString();
        string TRLTime = txtTruckUnloadedTime.Hour.ToString() + ":" + txtTruckUnloadedTime.Minute.ToString() + ":" + txtTruckUnloadedTime.Second.ToString() + " " + txtTruckUnloadedTime.AmPm;
        DateTime dt4 = DateTime.Parse(TRLTime); // No error check
        string TruckReleaseTime = dt4.ToString("HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

        if (GateOutDate == "01-01-0001" || GateOutTime == "" || ReachedDestinationDate == "01-01-0001" || ReachedDestinationTime == "" || TruckReleaseDate == "01-01-0001" || TruckReleaseTime == "")
        {
            lblerr.Text = "Gate Out Date/Time, Reached Destination Date/Time ,Truck Received Date/Time or Truck Release Date/time is not filled by Transporter." + "\n" + Environment.NewLine +
                 "Entries cannot be submitted before Transporter response";
            lblerr.Visible = true;
            lblerr.ForeColor = System.Drawing.Color.Red;
            return;
        }
        // -------------------------------------------------update Lr line--------------------------------------------------------
        UpdateFlagValue = true;
        int recordUpdated = 0;
        // bool isDataSendtoNav = false;
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

                    if (isTransitAccidentialValue_Grid(dt) && Isbtnclicked == "Submit")  //if all row has transit/accedential Loss - then status = Pending for Bill processing Team(3)     else - Pending for Transporter Action(5)
                        cmd.Parameters.AddWithValue("@ApprovalStatus", 3); //Set 'Pending for Bill processing Team'
                    else
                        cmd.Parameters.AddWithValue("@ApprovalStatus", ddlApprovalStatus.SelectedIndex); 
                        //cmd.Parameters.AddWithValue("@ApprovalStatus", 5); //Set 'Pending for Transporter Action'                   
                    cmd.Parameters.AddWithValue("@TransporterRemark", txtTransporterRemark.Text.Trim());
                    cmd.Parameters.AddWithValue("@BillProcessingRemark", txtBillProcessingRemark.Text.Trim());
                    cmd.Parameters.AddWithValue("@BreweryGateOutwardNo", BreweryGateOutwardNo);
                    cmd.Parameters.AddWithValue("@GateOutDate", GateOutDate);
                    cmd.Parameters.AddWithValue("@GateOutTime", dtGateOutTime);

                    cmd.Parameters.AddWithValue("@GateInDate", GateInDate);
                    cmd.Parameters.AddWithValue("@GateinTime", dtGateInTime);
                    cmd.Parameters.AddWithValue("@ReachedDestinationDate", ReachedDestinationDate);
                    cmd.Parameters.AddWithValue("@ReachedDestinationTime", ReachedDestinationTime);
                    //cmd.Parameters.AddWithValue("@TruckReceivedDate", );
                    //cmd.Parameters.AddWithValue("@TruckReceivedTime", '');
                    cmd.Parameters.AddWithValue("@TruckReleaseDate", TruckReleaseDate);
                    cmd.Parameters.AddWithValue("@TruckReleaseTime", TruckReleaseTime);

                    recordUpdated = cmd.ExecuteNonQuery(); //saved in database
                    if (recordUpdated > 0)
                    {
                        Int32 isAllrowVerified = 0;
                        isAllrowVerified = GetSQLData("select  [dbo].[isAllLRLineVerified]('" + lblLRNo.Text.Trim() + "')");

                        if (isAllrowVerified == 1)
                        {
                            //Send updated Data to Nav-- Called Web service iof Navision
                            //SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
                            //isDataSendtoNav = objsenddatatonav.SendLRtoNAV(lblLRNo.Text.Trim());
                        }
                        else
                        {
                            transaction.Commit();
                            con.Close();
                            lblerr.Text = "Entries submitted successfully";
                            lblerr.ForeColor = System.Drawing.Color.Green;
                            lblerr.Visible = true;
                            calculateChargeableQty(ViewState["LRNo"].ToString(), dt);
                            calculateTransitLossAmount(ViewState["LRNo"].ToString(), dt);
                            BindGridview(ViewState["LRNo"].ToString());
                            BindLRHeader(ViewState["LRNo"].ToString());
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
                //                if (recordUpdated > 0 && isDataSendtoNav == true)
                if (recordUpdated > 0)
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
            // EnableDisableInvoiceButton(ViewState["LRNo"].ToString());
        }
        catch (Exception es)
        {

        }



    }




    public void calculateChargeableQty(string LRNo, DataTable dt)
    {
        //[TMS_sp_calculateChargeableQty]
        // string value = "HI";
        //System.IO.File.WriteAllText(@"c:\Test\chargeqty.txt", value);

        int recordUpdated = 0;
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
            con.Open();
            SqlCommand cmd = new SqlCommand("TMS_sp_calculateChargeableQty");
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
                recordUpdated = cmd.ExecuteNonQuery(); //saved in database
                if (recordUpdated > 0)
                {
                    transaction.Commit();
                    con.Close();
                }
                else
                {
                    transaction.Rollback();
                    con.Close();
                }

            }
            catch (Exception msg)
            {

            }
            #endregion

        }
    }

    public void calculateTransitLossAmount(string LRNo, DataTable dt)
    {
        //[TMS_sp_calculateChargeableQty]
        //  string value = "Hallo";
        //System.IO.File.WriteAllText(@"c:\Test\TransitAmount.txt", value);
        int recordUpdated = 0;
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
            con.Open();
            SqlCommand cmd = new SqlCommand("TMS_sp_calculateTransitLossAmount");
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
                recordUpdated = cmd.ExecuteNonQuery(); //saved in database
                if (recordUpdated > 0)
                {
                    transaction.Commit();
                    con.Close();
                }
                else
                {
                    transaction.Rollback();
                    con.Close();
                }

            }
            catch (Exception msg)
            {

            }
            #endregion

        }
    }

    /// <summary>
    /// Update LR by CIPL user
    /// </summary>
    protected void Update_LR_Line_By_ThirdParty()
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

            // UpdateFlagValue = GetUpdateFlagValueFromGrid(dt);


            UpdateLR_byCIPL_and_SendDatatoNAV(dt); //update LR and send data to nav

        }
        catch (Exception es)
        {

        }

        EnableDisableLRHeader();
    }



    private void UpdateLR_byCIPL_and_SendDatatoNAV(DataTable dt)
    {
        int recordUpdated = 0;
        //bool isDataSendtoNav = false;

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
                    bool isallrowverifiedForFreightCalc = false;
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


                    if (chkfrght.Checked == true && chkLoadingCharge.Checked == true && chkUnLoadingCharge.Checked == true && chkDetentionCharges.Checked == true && chkOtherCharges.Checked == true && chkDelayPenalty.Checked == true)
                    {
                        isallrowverifiedForFreightCalc = true;
                    }
                    else
                    {
                        isallrowverifiedForFreightCalc = false;
                    }

                    //}
                    //grdLR.SetPageIndex(b);

                    //if (isAllrowVerified==true && isallrowverifiedForFreightCalc==false)
                    //    cmd.Parameters.AddWithValue("@ApprovalStatus", 2); // disputed lr
                    //if (isAllrowVerified == false && isallrowverifiedForFreightCalc == true)
                    //    cmd.Parameters.AddWithValue("@ApprovalStatus", 2);
                    //if (isAllrowVerified == false && isallrowverifiedForFreightCalc == false)
                    //    cmd.Parameters.AddWithValue("@ApprovalStatus", 2);



                    if (isAllrowVerified == true && isallrowverifiedForFreightCalc == true)
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

                        if (isAll_rowVerified == 1 && isallrowverifiedForFreightCalc == true)
                        {
                            //Send updated Data to Nav-- Called Web service of Navision
                            //SendDatatoNAV objsenddatatonav = new SendDatatoNAV();
                            //isDataSendtoNav = objsenddatatonav.SendLRtoNAV(lblLRNo.Text.Trim());

                            //if (isDataSendtoNav)
                            //{
                            cmd = new SqlCommand();
                            cmd.CommandText = "Update TMS_LR_Header set [Approval Status] = 'Pending for Invoice' where [LR No_] = '" + lblLRNo.Text.Trim() + "'";
                            cmd.Connection = con;
                            cmd.Transaction = transaction;
                            int LrStatusUpdate = cmd.ExecuteNonQuery();
                            // SendMailToTransporter(lblLRNo.Text);
                            //}

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
                            //EnableDisableInvoiceButton(ViewState["LRNo"].ToString());
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
                //if (recordUpdated > 0 && isDataSendtoNav == true)
                //{
                //    transaction.Commit();
                //    con.Close();
                //    lblerr.Text = "Entries updated and submitted to Nav system successfully";
                //    lblerr.ForeColor = System.Drawing.Color.Green;
                //    lblerr.Visible = true;
                //}
                if (recordUpdated > 0)
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
            //EnableDisableInvoiceButton(ViewState["LRNo"].ToString());
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
                    // mpe.Show();

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

        try
        {

            LRHeader LRobj = (new LRHeader()).GetLREntryDetails(LRno);

            //edited by vijay
            lblExpDate.Text = LRobj.ExpDateofDelivery.ToShortDateString() != "01-01-0001" ? LRobj.ExpDateofDelivery.ToShortDateString() : "";

            lblLRNo.Text = LRobj.LRNo;
            lblLRDate.Text = LRobj.LRDate.ToShortDateString() != "01-01-0001" ? LRobj.LRDate.ToShortDateString() : "";
            lblSize.Text = LRobj.Size;
            lblTransporterCode.Text = LRobj.TransporterCode;
            lblTruckNo.Text = LRobj.TruckNo;
            lblTransportName.Text = LRobj.TransporterName;
            lblDetGateInGateOut.Text = LRobj.DetGateInGateOut;
            lblDetTruckRepTruckUn.Text = LRobj.DetTruckRepTruckUnDate;
            lblTransporterType.Text = LRobj.TransporterType;
            lblFrom.Text = LRobj.From;
            lblTo.Text = LRobj.To;
            txtTransporterRemark.Text = LRobj.TransporterRemark;
            txtBillProcessingRemark.Text = LRobj.BillProcessingTeamRemarks;
            txtBreweryGateOutwardNo.Text = LRobj.BreweryGateOutwardNo;
            DataTable dtdistance = GetData("select top 1 [Distance (KM)] from TMS_Placement_Setup where [Brewery Location]='" + LRobj.Location + "' and [Post Code]='" + LRobj.ToPostCode + "' ");
            // Bind GateInDate
            if(dtdistance.Rows.Count>0)
            lbldistance.Text = String.Format("{0:0.00}", dtdistance.Rows[0].ItemArray[0]) + " " + "KM"; 
            #region

            if (LRobj.GateInDate != "")    // 15-12-2016
            {
                if (Convert.ToDateTime(LRobj.GateInDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-') != "01-01-1753 00:00:00" && Convert.ToDateTime(LRobj.GateInDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-') != "01-01-1900 00:00:00")
                {
                    txtGateInDate.Text = Convert.ToDateTime(LRobj.GateInDate).ToString("dd/MM/yyyy").Replace('/', '-');
                    //   System.IO.File.WriteAllText(@"c:\TMSServiceLogs\GateInDate.txt", Convert.ToDateTime(LRobj.GateInDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-'));
                }
            }
            else
            {
            }
            string gateintime = DateTime.Parse(LRobj.GateInTime).ToString("HH:mm:ss"); //	//LRobj.GateInTime=   01:05:00

            ////System.IO.File.WriteAllText(@"c:\TMSServiceLogs1\LRobj_gateintime.txt", gateintime);
            //System.IO.File.WriteAllText(@"c:\TMSServiceLogs1\LRobj_gateintime.txt", LRobj.GateInTime);
            //System.IO.File.WriteAllText(@"c:\TMSServiceLogs1\gateintime.txt", gateintime);
            //System.IO.File.WriteAllText(@"c:\TMSServiceLogs1\LRobj_GateInTimetest.txt", LRobj.GateInTime.Substring(11, 2));// 1:
             string gateintime12HRformat = Convert24HrTo12Hr(Convert.ToInt16(gateintime.Substring(0, 2)), Convert.ToInt16(gateintime.Substring(3, 2))); //02:00 AM
            //string gateintime12HRformat = Convert24HrTo12Hr(Convert.ToInt16(LRobj.GateInTime.Substring(11, 2)), Convert.ToInt16(LRobj.GateInTime.Substring(14, 2))); // 01:05 AM

           

            //System.IO.File.WriteAllText(@"c:\TMSServiceLogs1\LRobj_GateInTime1.txt", LRobj.GateInTime.Substring(11, 2));
            //System.IO.File.WriteAllText(@"c:\TMSServiceLogs1\LRobj_GateInTime2.txt", LRobj.GateInTime.Substring(14, 2));
            
            //System.IO.File.WriteAllText(@"c:\TMSServiceLogs1\gateintime12HRformat.txt", gateintime12HRformat);
           


            TimeSelector1.Hour = (Convert.ToInt16(gateintime12HRformat.Substring(0, 2)));  ////1
            TimeSelector1.Minute = (Convert.ToInt16(gateintime12HRformat.Substring(3, 2))); //5
            if (gateintime12HRformat.Substring(6, 2) == "AM")
            {
                TimeSelector1.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                TimeSelector1.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            #endregion

            //Bind GateOutDate
            #region
            if ((LRobj.GateOutDate) != "")
            {
                if (Convert.ToDateTime(LRobj.GateOutDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-') != "01-01-1753 00:00:00" && Convert.ToDateTime(LRobj.GateOutDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-') != "01-01-1900 00:00:00")
                {
                    txtGateOutDate.Text = Convert.ToDateTime(LRobj.GateOutDate).ToString("dd/MM/yyyy").Replace('/', '-');
                }
                else
                {
                }

            }
            else
            {
            }
            string _Gateouttime = DateTime.Parse(LRobj.GateOutTime).ToString("HH:mm:ss"); //	12:55:00
            string _Gateouttime12HRformat = Convert24HrTo12Hr(Convert.ToInt16(_Gateouttime.Substring(0, 2)), Convert.ToInt16(_Gateouttime.Substring(3, 2))); //02:00 AM
            //string _Gateouttime12HRformat = Convert24HrTo12Hr(Convert.ToInt16(LRobj.GateOutTime.Substring(11, 2)), Convert.ToInt16(LRobj.GateOutTime.Substring(14, 2))); //12:55 AM
            TimeSelector2.Hour = (Convert.ToInt16(_Gateouttime12HRformat.Substring(0, 2)));
            TimeSelector2.Minute = (Convert.ToInt16(_Gateouttime12HRformat.Substring(3, 2)));
            if (_Gateouttime12HRformat.Substring(6, 2) == "AM")
            {
                TimeSelector2.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                TimeSelector2.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            #endregion

            #region

            if (LRobj.ReachedDestinationDate != "")
            {
                if (Convert.ToDateTime(LRobj.ReachedDestinationDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-') != "01-01-1753 00:00:00" && Convert.ToDateTime(LRobj.ReachedDestinationDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-') != "01-01-1900 00:00:00")
                {
                    txtTruckReportedDate.Text = Convert.ToDateTime(LRobj.ReachedDestinationDate).ToString("dd/MM/yyyy").Replace('/', '-');
                }
                else
                {
                }
            }
            else
            {

            }
            //Bind truck reported time txtTruckReportedTime  
            string _Reportdtimetime = DateTime.Parse(LRobj.ReachedDestinationTime).ToString("HH:mm:ss"); ; //	"12:41:00"	string 
            string _Reportdtime12HRformat = Convert24HrTo12Hr(Convert.ToInt16(_Reportdtimetime.Substring(0, 2)), Convert.ToInt16(_Reportdtimetime.Substring(3, 2))); //02:36 AM
          //  string _Reportdtime12HRformat = Convert24HrTo12Hr(Convert.ToInt16(LRobj.ReachedDestinationTime.Substring(11, 2)), Convert.ToInt16(LRobj.ReachedDestinationTime.Substring(14, 2))); //02:36 AM

            txtTruckReportedTime.Hour = (Convert.ToInt16(_Reportdtime12HRformat.Substring(0, 2)));
            txtTruckReportedTime.Minute = (Convert.ToInt16(_Reportdtime12HRformat.Substring(3, 2)));

            if (_Reportdtime12HRformat.Substring(6, 2) == "AM")
                txtTruckReportedTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            else
                txtTruckReportedTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;

            #endregion

            // bind truck unloaded date
            #region
            if (LRobj.TruckReleaseDate != "")
            {
                if (Convert.ToDateTime(LRobj.TruckReleaseDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-') != "01-01-1753 00:00:00" && Convert.ToDateTime(LRobj.TruckReleaseDate).ToString("dd/MM/yyyy HH:mm:ss").Replace('/', '-') != "01-01-1900 00:00:00")
                {
                    txtTruckUnloadDate.Text = Convert.ToDateTime(LRobj.TruckReleaseDate).ToString("dd/MM/yyyy").Replace('/', '-');
                }
            }
            else
            {
            }
            string _Unloadingtime = LRobj.TruckReleaseTime != "" ? DateTime.Parse(LRobj.TruckReleaseTime).ToString("HH:mm:ss") : ""; //	 "00:41:00"    
            string _Unlodingtime12HRformat = Convert24HrTo12Hr(Convert.ToInt16(_Unloadingtime.Substring(0, 2)), Convert.ToInt16(_Unloadingtime.Substring(3, 2))); //02:36 AM
            //string _Unlodingtime12HRformat = Convert24HrTo12Hr(Convert.ToInt16(LRobj.TruckReleaseTime.Substring(11, 2)), Convert.ToInt16(LRobj.TruckReleaseTime.Substring(14, 2))); //02:36 AM
            txtTruckUnloadedTime.Hour = (Convert.ToInt16(_Unlodingtime12HRformat.Substring(0, 2)));
            txtTruckUnloadedTime.Minute = (Convert.ToInt16(_Unlodingtime12HRformat.Substring(3, 2)));
            if (_Unlodingtime12HRformat.Substring(6, 2) == "AM")
                txtTruckUnloadedTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            else
                txtTruckUnloadedTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            #endregion



            DataTable dt = GetOperationalStatus(ViewState["LRNo"].ToString());
            ddlApprovalStatus.DataSource = dt;
            ddlApprovalStatus.DataTextField = "Option Name";
            ddlApprovalStatus.DataValueField = "Option Id";
            ddlApprovalStatus.DataBind();
            SetOperationalStatus(ViewState["LRNo"].ToString());
        }
        catch (Exception msg)
        {
            System.IO.File.WriteAllText(@"c:\TMSServiceLogs\error.txt", msg.Message.ToString());

        }

    }


    private DataTable GetData1(string query)
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
    private void BindLRFreightCalculation(string LRNo)
    {
        IList<FreightCalculation> objcalculatedfreight = (new FreightCalculation()).GetCalculatedFreight(LRNo);

        string head;
        foreach (var val in objcalculatedfreight)
        {
            head = val.Head.ToString();
            if (IsbtnCalclicked)
            {
                switch (head)
                {
                    case "Freight":
                        lblFreightvalue.Text = val.lastEnteredAmount.ToString();
                        chkfrght.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                        IDFreight.Value = val.ID.ToString();

                        break;

                    case "Unloading charges":
                        if (txtUnLoadingCharge.Text == "")
                        {
                            txtUnLoadingCharge.Text = val.lastEnteredAmount.ToString();
                            chkUnLoadingCharge.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                            IDUnLoadingCharge.Value = val.ID.ToString();
                        }
                        break;
                       
                    case "Loading charges":
                        if (txtLoadingCharge.Text == "")
                        {
                            txtLoadingCharge.Text = val.lastEnteredAmount.ToString();
                            chkLoadingCharge.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                            IDLoadingCharge.Value = val.ID.ToString();
                        }
                        break;
                    case "Detention":
                        if (txtDetentionCharges.Text == "")
                        {
                            txtDetentionCharges.Text = val.lastEnteredAmount.ToString();
                            chkDetentionCharges.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                            IdDetCharge.Value = val.ID.ToString();
                        }
                        break;

                    case "Others":
                        if (txtOthercharges.Text == "")
                        {
                            txtOthercharges.Text = val.lastEnteredAmount.ToString();
                            chkOtherCharges.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                            IDothers.Value = val.ID.ToString();
                        }
                        break;

                    case "Transit Losses":
                        IDTransitLoss.Value = val.ID.ToString();
                        lblTransitLossValue.Value = val.lastEnteredAmount.ToString();
                        break;

                    case "Accidental Losses":
                        IDAccidentalLosss.Value = val.ID.ToString();
                        lblAccidentalloss.Value = val.lastEnteredAmount.ToString();
                        break;

                    case "Express Payment Term":
                        IDDiscount.Value = val.ID.ToString();
                        break;


                    case "Delay Penalty":
                        lbldelayPenalty.Text = val.lastEnteredAmount.ToString();
                        chkDelayPenalty.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                        iDdelaypenalty.Value = val.ID.ToString();
                        break;

                    case "Total Invoice Value":
                        lblBillableamnt.Text = val.lastEnteredAmount.ToString();
                        IDTotalInvoiceValue.Value = val.ID.ToString();
                        break;

                }
            }
            else
            {
                switch (head)
                {
                    case "Freight":
                        lblFreightvalue.Text = val.lastEnteredAmount.ToString();
                        chkfrght.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                        IDFreight.Value = val.ID.ToString();

                        break;
                    case "Unloading charges":
                        txtUnLoadingCharge.Text = val.lastEnteredAmount.ToString();
                        chkUnLoadingCharge.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                        IDUnLoadingCharge.Value = val.ID.ToString();
                        break;
                    case "Loading charges":
                        txtLoadingCharge.Text = val.lastEnteredAmount.ToString();
                        chkLoadingCharge.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                        IDLoadingCharge.Value = val.ID.ToString();
                        break;
                    case "Detention":

                        txtDetentionCharges.Text = val.lastEnteredAmount.ToString();
                        chkDetentionCharges.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                        IdDetCharge.Value = val.ID.ToString();
                        break;

                    case "Others":
                        txtOthercharges.Text = val.lastEnteredAmount.ToString();
                        chkOtherCharges.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                        IDothers.Value = val.ID.ToString();
                        break;

                    case "Transit Losses":
                        IDTransitLoss.Value = val.ID.ToString();
                        lblTransitLossValue.Value = val.lastEnteredAmount.ToString();
                        break;

                    case "Accidental Losses":
                        IDAccidentalLosss.Value = val.ID.ToString();
                        lblAccidentalloss.Value = val.lastEnteredAmount.ToString();
                        break;

                    case "Express Payment Term":
                        IDDiscount.Value = val.ID.ToString();
                        break;


                    case "Delay Penalty":
                        lbldelayPenalty.Text = val.lastEnteredAmount.ToString();
                        chkDelayPenalty.Checked = val.VerifiedbyBillProcessingTeam == 1 ? true : false;
                        iDdelaypenalty.Value = val.ID.ToString();
                        break;

                    case "Total Invoice Value":
                        lblBillableamnt.Text = val.lastEnteredAmount.ToString();
                        IDTotalInvoiceValue.Value = val.ID.ToString();
                        break;









                }
            }
        }




        //lblFreightvalue.


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
    /// 
    private void EnableDisableLRHeader()
    {
        if (Session["UserType"].ToString() == "Transporter")
        {
            txtLoadingCharge.ReadOnly = chkLoadingCharge.Checked ? true : false;
            txtUnLoadingCharge.ReadOnly = chkUnLoadingCharge.Checked ? true : false;
            txtOthercharges.ReadOnly = chkOtherCharges.Checked ? true : false;
            txtDetentionCharges.ReadOnly = true;

            txtGateInDate.ReadOnly = true;
            //TimeSelector1.ReadOnly = true;

            // txtGateOutDate.ReadOnly = true;
            //TimeSelector2.ReadOnly = true;


            // txtTruckReportedDate.ReadOnly = true;
            // txtTruckReportedTime.ReadOnly = true;

            //txtTruckUnloadDate.ReadOnly = true;
            // txtTruckUnloadedTime.ReadOnly = true;

            chkLoadingCharge.Enabled = false;
            chkUnLoadingCharge.Enabled = false;
            chkDetentionCharges.Enabled = false;
            chkfrght.Enabled = false;
            chkOtherCharges.Enabled = false;
            chkDelayPenalty.Enabled = false;
        }

        if (Session["UserType"].ToString() == "Third Party")
        {
            txtLoadingCharge.ReadOnly = true;
            txtUnLoadingCharge.ReadOnly = true;
            txtOthercharges.ReadOnly = true;
            txtDetentionCharges.ReadOnly = true;
            txtGateInDate.ReadOnly = true;
            TimeSelector1.ReadOnly = true;
            txtGateOutDate.ReadOnly = true;
            TimeSelector2.ReadOnly = true;
            txtBreweryGateOutwardNo.ReadOnly = true;
            txtTruckReportedDate.ReadOnly = true;
            txtTruckReportedTime.ReadOnly = true;
            txtTruckUnloadDate.ReadOnly = true;
            txtTruckUnloadedTime.ReadOnly = true;
            txtTransporterRemark.ReadOnly = true;
            //txtBillProcessingRemark.ReadOnly = true;




            chkLoadingCharge.Enabled = chkLoadingCharge.Checked ? false : true;
            chkUnLoadingCharge.Enabled = chkUnLoadingCharge.Checked ? false : true;
            chkDetentionCharges.Enabled = chkDetentionCharges.Checked ? false : true;
            chkfrght.Enabled = chkfrght.Checked ? false : true;
            chkOtherCharges.Enabled = chkOtherCharges.Checked ? false : true;
            chkDelayPenalty.Enabled = chkDelayPenalty.Checked ? false : true;

            //  if(chkLoadingCharge.Enabled)


        }
        DataTable dt;
        dt = GetData("select [Portal User Id] from TMS_Portal_User_Setup where [User Type]='Admin'");
        foreach (DataRow dr in dt.Rows)
        {
            if (Session["UserName"].ToString().ToUpper() == dr.ItemArray[0].ToString())
            {
                lbldelayPenalty.ReadOnly = false;
            }
        }

    }
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
    #region Generate Initial Invoice (First time calculation when logged in by Transporter only)
    private void Generate_Initial_Invoice(string LRno)
    {

        string query = "Select count(ID) from [TMS_Portal_Freight_Invoice] " +
                         "Where [Last Entered Amt By Transporter] is not null and [Verified By Bill Processing Team] is not null and Approval_Status_Invoice is not null " +
                         "and [LR No_] =  '" + LRno + "'";
        DataTable dt = GetData(query);
        if ((dt.Rows[0].ItemArray[0].ToString() == "0")||(dt.Rows[0].ItemArray[0].ToString() == "10"))
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

                            //cmd.Parameters.AddWithValue("@pDelayPenaltyCode", ddlDelayPenalty.SelectedIndex == 0 ? "" : ddlDelayPenalty.SelectedItem.Text);
                            //cmd.Parameters.AddWithValue("@pDiscountShemeCode", ddlSpecialDiscount.SelectedIndex == 0 ? "" : ddlSpecialDiscount.SelectedItem.Text);
                            //cmd.Parameters.AddWithValue("@pTransporterRemarks", txttransporterRemark.Text);
                            //cmd.Parameters.AddWithValue("@pBillProcessingTeamRemarks", txtBillProcessTeamRemarks.Text);

                            cmd.Parameters.AddWithValue("@pDelayPenaltyCode", "");
                            cmd.Parameters.AddWithValue("@pDiscountShemeCode", "");
                            cmd.Parameters.AddWithValue("@pTransporterRemarks", "");
                            cmd.Parameters.AddWithValue("@pBillProcessingTeamRemarks", "");
                            cmd.Parameters.AddWithValue("@pTransporterType", lblTransporterType.Text);

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
                // time = "0" + h + ":" + "0" + MM + " PM";

                time = h + ":" + "0" + MM + " PM";  //lmaru8748
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
                          "SGC Team, <br/> " +
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

    //#region SEND EMAIL TO TRANSPORTER ON DISPUTE CASE
    //private void SendMailOfDisputeToTransporter(string LRNo)
    //{

    //    DataTable dTable = GetData("select PUS.ID, UTM.company_Name_FK , PUS.[Portal User Id], PUS.[E-mail ID]  from TMS_Portal_User_Setup PUS " +
    //                                "INNER JOIN TMS_User_Transporter_Mapping  UTM on PUS.[Portal User Id]= UTM.[Portal User ID] " +
    //                                "and UTM.[Transporter Code] = (Select LR.[Transporter Code] from TMS_LR_Header LR where [LR No_] = '" + LRNo + "') " +
    //                                "and UTM.company_Name_FK =  (Select LR.company_Name_FK  from TMS_LR_Header LR where [LR No_] = '" + LRNo + "') " +
    //                                "Where PUS.[User Type] = 'Transporter'");

    //    foreach (DataRow dr in dTable.Rows)
    //    {
    //        string to = dr[3].ToString(); //Email ID of Receiver
    //        string username = dr[2].ToString(); //User Name of Receiver
    //        MailMessage objMailMessage = new MailMessage();
    //        SmtpClient objSmtpClient = new SmtpClient("smtp.office365.com");
    //        try
    //        {
    //            DataTable dt = GetData("SELECT top 1 [E-Mail Address to Send Mails] as [FromEmailID] ,[Service Account] as [UserName] ,[Service Password] as [Password] FROM [dbo].[TMS_Setup] where company_Name_FK = (select t.company_Name_FK from TMS_LR_Header t where t.[LR No_] = '" + LRNo + "')");
    //            objMailMessage.From = new MailAddress(dt.Rows[0][0].ToString()); //Email ID of Sender
    //            objMailMessage.To.Add(new MailAddress(to));
    //            objMailMessage.Subject = "TMS : LR DISPUTE DETAILS - " + DateTime.Now.ToShortDateString();

    //            DataTable QueryResultingData = GetData("SELECT 1 as [SR No.] , LRH.Location as [Brewery Code], UPPER(LRH.[From City]) as [From], UPPER(LRH.[To City]) as [To], CAST(SUM(LRL.Quantity) as decimal(38,2)) as [No. Of Cases] , LRH.[Invoice No_] as [Invoice/STN No.] " +
    //                                                    ", LRH.[Date of Dispatch] as [Date] , LRH.[LR No_] as [LR No.] FROM TMS_LR_Header LRH inner join TMS_LR_Lines LRL on LRH.[LR No_] = LRL.[LR No_] " +
    //                                                    "WHERE LRH.[LR No_] = '" + LRNo + "' GROUP BY Location , [From City], [To City] , LRH.[Invoice No_] , LRH.[Date of Dispatch] , LRH.[LR No_] ");

    //            StringBuilder sb = new StringBuilder();
    //            sb.Append("DEAR, <br/> " +
    //                      "SGC Team, <br/> " +
    //                      "SUB: LR Dispute - " + DateTime.Now.ToShortDateString() + "<br/><br/>" +
    //                      "There is a dispute on below LR. Please review the LR again ! <br/><br/>");

    //            StringWriter sw = new StringWriter(sb);
    //            HtmlTextWriter hw = new HtmlTextWriter(sw);
    //            GridView gr = new GridView();
    //            gr.DataSource = QueryResultingData;
    //            gr.DataBind();
    //            gr.RenderControl(hw);

    //            sb.Append("<br/><br/> Thanks and Regards, <br/><br/>Logistics Team,<br/>CIPL-DHD");

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
    //#endregion
    protected void grdLR_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {

    }
    protected void BtnSubmit1_Click(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int lineNumber = ValidateCharge();
        if (lineNumber!=0)
        {
            lblerr.Text = "Transit loss(Amt.) can not be zero for a non-zero Chargable(Qty). ( At Line Number " + lineNumber+" )";
            lblerr.ForeColor = System.Drawing.Color.Red;
            lblerr.Visible = true;
            return;
        }

        if(lbldistance.Text.Trim()=="")
        {
            lblerr.Text = "Distance master can not be empty.Please update the distance master.";
            lblerr.ForeColor = System.Drawing.Color.Red;
            lblerr.Visible = true;
            return;
        }
        else
        {
            lblerr.Visible = false;
        }

        Isbtnclicked = "Submit";
        //submitApprovalStatus();
        //using (SqlConnection con = new SqlConnection())
        //{
        //    con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("update TMS_Portal_Freight_Invoice set [Approval_Status_Invoice]= (SELECT [Option Name] FROM [TMS_LR_Header_Option_Approval_Status] A where [Option Id] = 3 ) where [LR No_]='"+lblLRNo.Text+"'");
        //    SqlTransaction transaction;
        //    transaction = con.BeginTransaction("UPDATE");
        //    cmd.Connection = con;
        //    cmd.Transaction = transaction;
        //    #region Transaction
        //    try
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        cmd.ExecuteNonQuery();
        //        transaction.Commit();

        //    }
        //    catch(Exception msg)
        //    {

        //    }
        //    #endregion
        //}

        //BindLRHeader(lblLRNo.Text);


        if (Session["UserType"].ToString() == "Transporter")
        {
            //string a = txtGateOutDate.Text;

            string a = txtLoadingCharge.Text;
            Update_LR_Line_By_Transporter(); // here data gets pushed in nav which you have to stop
            string b = txtLoadingCharge.Text;
            Update_detention();
            //Update_Freight_By_Transporter();
            string c = txtLoadingCharge.Text;
            delayDeliveryDeduction(ViewState["LRNo"].ToString());
            TransitLoss();
            AccidentalLoss();
            billableAmount(ViewState["LRNo"].ToString());
            Update_Freight_By_Transporter();
            BindLRFreightCalculation(lblLRNo.Text.Trim());

            // redirect 
            DataTable dt = new DataTable();
            dt = GetData1("select [Approval Status] from TMS_LR_Header where [LR No_]='" + lblLRNo.Text.Trim() + "'  ");
            // string st = dt.Rows[0][0].ToString();
            string status = dt.Rows[0][0].ToString();



            if (status == "Pending at Bill Processing Team")
            {
                //Response.Redirect("LRReadyForUploadingDetails.aspx?LRNo=" + lblLRNo.Text.Trim());
                txtBillProcessingRemark.Enabled = false;
                txtTransporterRemark.Enabled = false;
                txtBreweryGateOutwardNo.Enabled = false;
                txtGateInDate.Enabled = false;
                TimeSelector1.Enabled = false;
                txtGateOutDate.Enabled = false;
                TimeSelector2.Enabled = false;
                txtLoadingCharge.Enabled = false;
                txtOthercharges.Enabled = false;
                txtTruckReportedDate.Enabled = false;
                txtTruckReportedTime.Enabled = false;
                txtTruckUnloadDate.Enabled = false;
                txtTruckUnloadedTime.Enabled = false;
                txtUnLoadingCharge.Enabled = false;

                grdLR.Enabled = false;

            }


        }
        if (Session["UserType"].ToString() == "Third Party")
        {
            Update_LR_Line_By_ThirdParty();
            Update_Freight_By_Transporter();



        }

        if (Session["UserType"].ToString() == "Admin")
        {
            billableAmount(ViewState["LRNo"].ToString());
            Update_Freight_By_Transporter();
            BindLRFreightCalculation(lblLRNo.Text.Trim());
        }

    }

    public int ValidateCharge()
    {
        int lineNumber = 0;
        foreach (GridViewRow row in grdLR.Rows)
        {
            string transitLoss = grdLR.Rows[row.RowIndex].Cells[3].Text;
            string chargableQuantity = grdLR.Rows[row.RowIndex].Cells[6].Text;
            if(chargableQuantity!="0.00" && transitLoss =="0.00")
            {
                lineNumber = row.RowIndex + 1;
                return lineNumber;
            }
        }
        return lineNumber;
    }
    public void submitApprovalStatus()
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE TMS_LR_Header Set TMS_LR_Header.[Approval Status]= (SELECT A.[Option Name] from TMS_LR_Header_Option_Approval_Status A WHERE A.[Option Id]= 2) WHERE [TMS_LR_Header].[LR No_] = '" + lblLRNo.Text + "' ");
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UPDATE");
            cmd.Connection = con;
            cmd.Transaction = transaction;
            #region Transaction
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                transaction.Commit();

            }
            catch (Exception msg)
            {

            }
            #endregion
        }
    }



    protected void btnDispute_Click(object sender, EventArgs e)
    {
       // IsDispute = true;
        submitApprovalStatus();
        DataTable dt = GetOperationalStatus(ViewState["LRNo"].ToString());
        ddlApprovalStatus.DataSource = dt;
        ddlApprovalStatus.DataTextField = "Option Name";
        ddlApprovalStatus.DataValueField = "Option Id";
        ddlApprovalStatus.DataBind();
        SetOperationalStatus(ViewState["LRNo"].ToString());
    }


}

