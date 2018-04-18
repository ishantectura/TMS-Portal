using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class PlacedRequest : System.Web.UI.Page
{
    DataTable dtCancellationReasonCode = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            //Edited by vijay
            BindGrid("","","");
            //BindGrid();
    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    private void BindGrid(string city,string requestNo , string transportedCode)
    {
        //Edited by vijay
        grdPlacedRequest.DataSource = (new PlacedRequests()).GetPlacedRequests(Session["UserName"].ToString(), city,requestNo,transportedCode);  //Take all the request data 	 Where [Ready for Sending]= 1 and date of Acceptance != blank/null
        grdPlacedRequest.DataBind();

        //Added by Jyothi
        grdPlacedRequestExcel.DataSource = (new PlacedRequests()).GetPlacedRequests(Session["UserName"].ToString(), city,requestNo,transportedCode);  //Take all the request data 	 Where [Ready for Sending]= 1 and date of Acceptance != blank/null
        grdPlacedRequestExcel.DataBind();

        if (Session["UserType"].ToString() == "Transporter") // Disable ReasonCode dropdown for Transporter user
        {
            btnCancelRequest.Visible = false;
        }


    }

    // To be delete from Database : SP: TMS_sp_update_PlacedRequest_ByCIPL , TYPE: TMSRequest_PlacedRequest_ByCIPL , if below method is in no use anymore
    protected void btnCancelRequest_Click(object sender, EventArgs e)
    {
        try
        {
            bool tobeCancel = false;
            foreach (GridViewRow row in grdPlacedRequest.Rows)
            {
                string key = grdPlacedRequest.DataKeys[row.RowIndex].Value.ToString();
                CheckBox chkPlaced = row.FindControl("chkSelectedRow") as CheckBox;

                if (chkPlaced.Checked)
                {    
                    DropDownList ddlReasonCode = row.FindControl("ddlReasonCode") as DropDownList;
                    Label lblmandatoryReasonCode = row.FindControl("lblmandatoryReasonCode") as Label;
                    if (ddlReasonCode != null)
                    {
                        if (ddlReasonCode.SelectedItem.Text == "") // nothing selected
                        {
                            lblmandatoryReasonCode.Text = "Please select Cancellation Reason code";
                            lblmandatoryReasonCode.Visible = true;
                            tobeCancel = false;
                        }
                        else
                        {
                            tobeCancel = true;
                        }
                    }
                }
            }
            if(tobeCancel)
            SubmitCIPLRequest();


            //Code to cancel request one by one
            //using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            //{
            //    using (SqlCommand cmd = new SqlCommand("TMS_sp_update_PlacedRequest_ByCIPL", con))  // set [Date of Acceptance] = today date
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMS_PlacedRequestUpdateTable", dt);
            //        sqlParameter.SqlDbType = SqlDbType.Structured;
            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //        BindGrid();
            //    }
            //}
        }
        catch (Exception ee)
        {

        }
    }

    protected void grdPlacedRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //edited by vijay
        BindGrid("",txtrequestno.Text,txttransportercode.Text);
        grdPlacedRequest.PageIndex = e.NewPageIndex;
        grdPlacedRequest.DataBind();
    }

    //edited by vijay 13-06-2017
    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)grdPlacedRequest.HeaderRow.FindControl("chkboxSelectAll");
        foreach (GridViewRow row in grdPlacedRequest.Rows)
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

    #region Grid_Event : OnRowCommand
    //protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "CancelRequest") //GridView Button- btnCancel click Event
    //    {
    //        int rowIndex = Convert.ToInt32(e.CommandArgument);
    //        GridViewRow gvRow = grdPlacedRequest.Rows[rowIndex];       
    //        string key = grdPlacedRequest.DataKeys[rowIndex].Values[0].ToString();

    //        DropDownList ddlReasonCode = gvRow.FindControl("ddlReasonCode") as DropDownList;
    //        Label lblmandatoryReasonCode = gvRow.FindControl("lblmandatoryReasonCode") as Label;
    //        if (ddlReasonCode != null)
    //        {
    //            if (ddlReasonCode.SelectedItem.Text == "") // nothing selected
    //            {
    //                lblmandatoryReasonCode.Text = "Please select Cancellation Reason code";
    //                lblmandatoryReasonCode.Visible = true;
    //            }
    //            else
    //            {
    //                SubmitCIPLRequest(key, gvRow);
    //            }
    //        }
    //    }        
    //}
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

                TextBox txtcancelRemark = (e.Row.FindControl("txtCancellationRemark") as TextBox);
                CheckBox chkSelect = (e.Row.FindControl("chkSelectedRow") as CheckBox);

                int datakey = 0;
                object objTemp = grdPlacedRequest.DataKeys[e.Row.RowIndex].Value as object;
                if (objTemp != null)
                {
                    datakey = Convert.ToInt32(objTemp);                    
                }

                DropDownList ddlReasonCode = (e.Row.FindControl("ddlReasonCode") as DropDownList);
                DataTable dt1 = new DataTable();
                dt1 = GetCancellationReasonCode(datakey);
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
              
                #endregion

                if (Session["UserType"].ToString() == "Transporter") // Disable ReasonCode dropdown for Transporter user
                {
                    ddlReasonCode.Enabled = false;
                    txtcancelRemark.Enabled = false;
                    chkSelect.Enabled = false;
                    ddlReasonCode.BackColor = System.Drawing.Color.Cornsilk;
                    txtcancelRemark.BackColor = System.Drawing.Color.Cornsilk;
                    chkSelect.BackColor = System.Drawing.Color.Cornsilk;
                }

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

                TextBox txtcancelRemark = (e.Row.FindControl("txtCancellationRemark") as TextBox);
                CheckBox chkSelect = (e.Row.FindControl("chkSelectedRow") as CheckBox);

                int datakey = 0;
                object objTemp = grdPlacedRequestExcel.DataKeys[e.Row.RowIndex].Value as object;
                if (objTemp != null)
                {
                    datakey = Convert.ToInt32(objTemp);
                }

                DropDownList ddlReasonCode = (e.Row.FindControl("ddlReasonCode") as DropDownList);
                DataTable dt1 = new DataTable();
                dt1 = GetCancellationReasonCode(datakey);
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

                #endregion

                if (Session["UserType"].ToString() == "Transporter") // Disable ReasonCode dropdown for Transporter user
                {
                    ddlReasonCode.Enabled = false;
                    txtcancelRemark.Enabled = false;
                    chkSelect.Enabled = false;
                    ddlReasonCode.BackColor = System.Drawing.Color.Cornsilk;
                    txtcancelRemark.BackColor = System.Drawing.Color.Cornsilk;
                    chkSelect.BackColor = System.Drawing.Color.Cornsilk;
                }

            }
        }
    }
    #endregion

    #region GridView Dropdown - ddlReasonCode selected index changed Event
    protected void ddlReasonCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        DropDownList ddlReasonCode = gvRow.FindControl("ddlReasonCode") as DropDownList;
        Label lblmandatoryReasonCode = gvRow.FindControl("lblmandatoryReasonCode") as Label;
        if (ddlReasonCode.SelectedIndex != 0) // nothing selected
        {
            lblmandatoryReasonCode.Text = "";
            lblmandatoryReasonCode.Visible = false;
        }
    }
    #endregion


    #region Get Cancellation ReasonCode from Master table
    private DataTable GetCancellationReasonCode(int ReqID)
    {
            string sqlStatement = "SELECT [Code],[Description] FROM [dbo].[TMS_Cancellation_Reason_Code] where company_Name_FK = (select t.company_Name_FK from TMS_Request t where t.ID = "+  ReqID +")";
            using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                {
                    dtCancellationReasonCode = new DataTable();
                    con.Open();
                    dtCancellationReasonCode.Load(cmd.ExecuteReader());                 
                    return dtCancellationReasonCode;
                }
            }
       
    }
    #endregion

    #region Submit request by CIPL User
    private void SubmitCIPLRequest()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ID"));
        dt.Columns.Add(new DataColumn("ReqNo"));  //commented by anu - As status updation no longer needed
        dt.Columns.Add(new DataColumn("Transporter Response"));
        dt.Columns.Add(new DataColumn("Cancellation Reason Code"));
        dt.Columns.Add(new DataColumn("Ready for Sending"));
        dt.Columns.Add(new DataColumn("Cancellation Remarks"));
               
        foreach (GridViewRow row in grdPlacedRequest.Rows)
        {
            string RowDatakey = grdPlacedRequest.DataKeys[row.RowIndex].Value.ToString();
            DropDownList ddlReasonCode = row.FindControl("ddlReasonCode") as DropDownList;
            TextBox txtTransporterRemark = row.FindControl("txtTransporterRemark") as TextBox;
            TextBox txtcancelRemark = row.FindControl("txtCancellationRemark") as TextBox;
            CheckBox chkSelect = row.FindControl("chkSelectedRow") as CheckBox;

            if (chkSelect.Checked)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = RowDatakey;
                dr["ReqNo"] = grdPlacedRequest.Rows[row.RowIndex].Cells[0].Text;
                dr["Transporter Response"] = ""; //No use in SP
                dr["Cancellation Reason Code"] = ddlReasonCode.SelectedItem.Value;
                dr["Ready for Sending"] = 1;  // to be discuss for CIPL //rigt now not used in SP - No updation performed
                dr["Cancellation Remarks"] = txtcancelRemark.Text;
                dt.Rows.Add(dr);
            }
        }
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
                    int rowaffected = cmd.ExecuteNonQuery();
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        SendDatatoNAV objSendDatatoNAV = new SendDatatoNAV(dr1[1].ToString()); //commented on 06 sep by recommendation of Brijesh 
                    }
                    //SEND EMAIL
                    transaction.Commit();
                    con.Close();
                    //Edited by vijay
                    BindGrid("",txtrequestno.Text,txttransportercode.Text);                   
                    lblErr.Text = "Canceled Successfully";
                    lblErr.Visible = true;
                    lblErr.ForeColor = System.Drawing.Color.Green;
                }
                catch (Exception ex1)
                {
                    try
                    {
                        transaction.Rollback(); //manual                             
                        con.Close();
                        lblErr.Text = ex1.Message;
                        lblErr.Visible = true;
                    }
                    catch (Exception ex2)
                    {
                        lblErr.Text = ex2.Message;
                        lblErr.Visible = true;
                    }
                }
                #endregion
            }
        }
    }
    #endregion

    //Added by Jyothi
    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdPlacedRequestExcel.Visible = true;
        ExportGridToExcel(grdPlacedRequestExcel);
        grdPlacedRequestExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "PlacedRequest" + DateTime.Now + ".xls";
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
        BindGrid("", txtrequestno.Text, txttransportercode.Text);
    }
}