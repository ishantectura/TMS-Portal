using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;
using System.Data;
using System.Data.SqlClient;
//using NAVService;
using TMSPortalService.App_Code;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

public partial class ReadyForPlacement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindData("","","");
    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    protected void BindData(string city,string requestNo,string transporterCode)
    {
        grdReadyForPlacement.DataSource = (new ReadyForPlacementRequests()).GetReadyForPlacementRequests(Session["UserName"].ToString(), city,requestNo,transporterCode);
        grdReadyForPlacement.DataBind();

        //Added by Jyothi
        grdReadyForPlacementExcel.DataSource = (new ReadyForPlacementRequests()).GetReadyForPlacementRequests(Session["UserName"].ToString(), city,requestNo,transporterCode);
        grdReadyForPlacementExcel.DataBind();
    }

    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        BindData(txtcity.Text, txtrequestno.Text, txttransportercode.Text);
        grdReadyForPlacement.PageIndex = e.NewPageIndex;
        grdReadyForPlacement.DataBind();
    }


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

    private void SubmitData()
    {
        // edited by vijay
        bool IsTruckNoExist;

        string errormsgforVehicleNo = "";
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ID"));
        dt.Columns.Add(new DataColumn("Vehicle No_"));
        dt.Columns.Add(new DataColumn("Expected Placement Date"));
        dt.Columns.Add(new DataColumn("DriverMobileNo"));
        dt.Columns.Add(new DataColumn("DriverName"));
        dt.Columns.Add(new DataColumn("TransporterType"));
        // dt.Columns.Add(new DataColumn("ExpectedPlacementDate"));
        foreach (GridViewRow row in grdReadyForPlacement.Rows)
        {
            string key = grdReadyForPlacement.DataKeys[row.RowIndex].Value.ToString();
            Label req = row.FindControl("lblReq") as Label;
            CheckBox chkPlaced = row.FindControl("chkPlaced") as CheckBox;
            TextBox txtTruckno = row.FindControl("txtTruckNo") as TextBox;
            TextBox txtDriverMobNo = row.FindControl("txtDriverMobNo") as TextBox;
            TextBox txtDriverName = row.FindControl("txtDriverName") as TextBox;
            TextBox txtExpectedPlacementDate = row.FindControl("txtExpectedPlacementDate") as TextBox;

            //edit by vijay
            Label lblvehicle = (Label)row.FindControl("lbltrucknovalidation");
            // Label ddlTransporterType = row.FindControl("ddlTransporterType") as Label;
            //Label label = row.FindControl("lblerrVehicle") as Label;

            Label lblTransporterType = row.FindControl("lblTransporterType") as Label;

            if (chkPlaced.Checked)
            {
                int returnvalue=0;
                if (txtTruckno.Text.Length > 3)
                {
                    returnvalue = validatetruckNo(txtTruckno.Text.ToString());
                }
                if (returnvalue == 1)
                {
                    string changeinexpexcteddate;
                    DataRow dr = dt.NewRow();
                    dr["ID"] = key;
                    dr["Vehicle No_"] = txtTruckno.Text;
                    changeinexpexcteddate = Convert.ToDateTime(txtExpectedPlacementDate.Text).ToShortDateString();
                    dr["Expected Placement Date"] = changeinexpexcteddate;
                    dr["DriverMobileNo"] = txtDriverMobNo.Text;
                    dr["DriverName"] = txtDriverName.Text;
                    dr["TransporterType"] = lblTransporterType.Text;
                    dt.Rows.Add(dr);
                }
                else
                {
                    //CustomValidator cutomevalidatorfortruckno = row.FindControl("truckNo") as CustomValidator;
                    //cutomevalidatorfortruckno.ErrorMessage = "Invalid truck no";
                    errormsgforVehicleNo = errormsgforVehicleNo + req.Text.ToString() + "  ";
                }
            }

            //Edited by vijay
            IsTruckNoExist = CheckTruckNumber(txtTruckno.Text.ToString());
            if (IsTruckNoExist == true)
            {
                lblvehicle.Text = "This Truck no request is in process";
                lblvehicle.ForeColor = Color.Red;
                lblvehicle.Visible = true;
                return;
            }

        }
       // lblerrVehicle.Text = "Incorrect Vehicle no for :" + errormsgforVehicleNo;

        if (dt.Rows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("TMS_sp_update_Placed_Request", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMS_PlacedRequestUpdateTable", dt);
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    BindData("","","");
                }
            }
        }
    }

    // edit by vijay
    public bool CheckTruckNumber(string vehicleNo)
    {
        bool TruckNoExistsorNot;
        using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand("TMS_sp_TruckNo_ExistsOr_Not", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TmsVehicleNumber", vehicleNo);
                cmd.Parameters.Add("@TruckNoexist", SqlDbType.Bit);
                cmd.Parameters["@TruckNoexist"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                TruckNoExistsorNot = (bool)cmd.Parameters["@TruckNoexist"].Value;
                con.Close();
            }
        }
        return TruckNoExistsorNot;

    }


    public int validatevehicleNo(string vehicleNo)
    {

        String reg1 = @"^[a-z]{2}[0-9]{2}[a-z]{1,2}[0-9]{4}$";
        Regex regexVeh = new Regex(reg1, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        Match match = regexVeh.Match(vehicleNo.Trim());
        if (match.Success)
        {
            // MessageBox.Show(match.Value);

            return 1;
        }
        else
        {
            // MessageBox.Show("Enter Valid VechicleNo" + Environment.NewLine + "(Ex: TN46AM1234)", "Transport Details");
            return 0;
        }
    }

    public static bool HasSpecialCharacters(string str)
    {
        string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
        char[] specialCharactersArray = specialCharacters.ToCharArray();
        int index = str.IndexOfAny(specialCharactersArray);
        if (index == -1)
            return false;
        else
            return true;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SubmitData();
        //SubmitDataToNav();
    }


    //private void SubmitDataToNav()
    //{

    //    //foreach (GridViewRow row in grdReadyForPlacement.Rows)
    //    //{
    //    //    string RequestNo = row.Cells[0].ToString();

    //    //    NavWebService_PortClient NavWebService_PortClient = new NavWebService_PortClient();
    //    //    RQT RQT = new RQT();
    //    //    TruckIndentRequest TruckIndentRequest = new TruckIndentRequest();
    //    //    RQT.Text[0] = TruckIndentRequest.GetRequestXML(RequestNo).ToString();
    //    //    NavWebService_PortClient.SendRequestToNav(RQT);
    //    //}
    //}

    public int validatetruckNo(string vehicleno)
    {
        string[] stringSeparators = new string[] { vehicleno };
        int returnval;
        string x = vehicleno.Remove(vehicleno.Length - 4);
        string f = vehicleno.Substring(vehicleno.Length - 4);
        int n;
        bool isNumeric = int.TryParse(f, out n);
        if (!HasSpecialCharacters(x) && int.TryParse(f, out n) && !Regex.IsMatch(x, @"\s"))
        {
            returnval = 1;
        }
        else
        {
            returnval = 0;
        }

        return returnval;
    }

    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)grdReadyForPlacement.HeaderRow.FindControl("chkboxSelectAll");
        foreach (GridViewRow row in grdReadyForPlacement.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkPlaced");
            if (ChkBoxHeader.Checked == true && ChkBoxRows.Enabled == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }
    

    //protected void chkPlaced_CheckedChanged(object sender, EventArgs e)
    //{
    //    foreach (GridViewRow row in grdReadyForPlacement.Rows)
    //    {
    //        string key = grdReadyForPlacement.DataKeys[row.RowIndex].Value.ToString();

    //        CheckBox chkPlaced = row.FindControl("chkPlaced") as CheckBox;
    //        TextBox txtTruckno = row.FindControl("txtTruckNo") as TextBox;
    //        CustomValidator cutomevalidatorfortruckno = row.FindControl("truckNo") as CustomValidator;

    //        if(chkPlaced.Checked)
    //        {

    //            int returnvalue = validatevehicleNo(txtTruckno.Text.ToString());
    //            if (returnvalue == 1)
    //            { }
    //            else
    //            {
    //                cutomevalidatorfortruckno.ErrorMessage = "Invalid truck no";
    //            }
    //        }
    //    }

    //    //CustomValidator cutomevalidatorfortruckno = e.FindControl("truckNo") as CustomValidator;
    //    //validatevehicleNo();
    //}

    //protected void grdReadyForPlacement_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //if (e.Row.RowType == DataControlRowType.DataRow)
    //    //{
    //    //    Label lblTransportertype = e.Row.FindControl("lblTransporterType") as Label;
    //    //    DropDownList ddlTransporterType = (e.Row.FindControl("ddlTransporterType") as DropDownList);
    //    //    if (lblTransportertype.Text != "")
    //    //    {
    //    //        ddlTransporterType.Items.FindByText(lblTransportertype.Text).Selected = true;
    //    //        lblTransportertype.Visible = false;
    //    //    }
    //    //    else
    //    //    {
    //    //        ddlTransporterType.Items.Insert(0, new ListItem(String.Empty, String.Empty));
    //    //        ddlTransporterType.SelectedIndex = 0;
    //    //    }
    //    //}
    //}
    //protected void chkPlaced_CheckedChanged(object sender, EventArgs e)
    //{

    //    // string firstCellValue = grdReadyForPlacement.SelectedRow[0].Cells[0].Value;
    //    //foreach ( gvr in grdReadyForPlacement.Rows)
    //    //{

    //    //    if (((CheckBox)gvr.findcontrol("CheckBox1")).Checked == true)
    //    //    {

    //    //        int uPrimaryid = gvr.cells["uPrimaryID"];
    //    //    }
    //    //}

    //    // validatetruckNo();
    //}

    protected void txtTruckNo_TextChanged(object sender, EventArgs e)
    {

        TextBox ddl = (TextBox)sender;
        GridViewRow row = (GridViewRow)ddl.NamingContainer;
        TextBox txtTruckno = (TextBox)row.FindControl("txtTruckNo");
        Label lblvehicle = (Label)row.FindControl("lbltrucknovalidation");
        CheckBox chkPlaced = (CheckBox)row.FindControl("chkPlaced");
        int returnvalue = 0;
        if (txtTruckno.Text.Length > 3)
        {
            returnvalue = validatetruckNo(txtTruckno.Text.ToString());
        }
        if (returnvalue == 1)
        {
            chkPlaced.Enabled = true;
            lblvehicle.Text = "";
        }
        else
        {
            lblvehicle.Text = "incorrect";
            lblvehicle.ForeColor = Color.Red;
            lblvehicle.Visible = true;
            chkPlaced.Enabled = false;
        }


        // txtName.Text = "*****";
    }

    //Added by Jyothi
    #region Export To excel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdReadyForPlacementExcel.Visible = true;
        ExportGridToExcel(grdReadyForPlacementExcel);
        grdReadyForPlacementExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "ReadyForPlacement" + DateTime.Now + ".xls";
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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (txtcity.Text != "" || txtrequestno.Text != "" || txttransportercode.Text != "")
            BindData(txtcity.Text.ToString(), txtrequestno.Text.ToString(), txttransportercode.Text.ToString());
        else
            BindData("", "", "");
    }
}