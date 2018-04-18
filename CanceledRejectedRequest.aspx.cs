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

public partial class CanceledRejectedRequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindGrid("","","","");
    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    private void BindGrid(string OperationStatus, string TransporterResponse,string requestNo , string transporterCode)
    {
        grdCancelRejectRequest.DataSource = (new PlacedRequests()).GetCanceledRejectedRequests(OperationStatus, TransporterResponse, Session["UserName"].ToString(),requestNo,transporterCode);  //Take all the request data 	 Where [Ready for Sending]= 1 and date of Acceptance != blank/null
        grdCancelRejectRequest.DataBind();

        //Added by Jyothi
        grdCancelRejectRequestExcel.DataSource = (new PlacedRequests()).GetCanceledRejectedRequests(OperationStatus, TransporterResponse, Session["UserName"].ToString(),requestNo,transporterCode);  //Take all the request data 	 Where [Ready for Sending]= 1 and date of Acceptance != blank/null
        grdCancelRejectRequestExcel.DataBind();
    }

    // To be delete from Database : SP: TMS_sp_update_PlacedRequest_ByCIPL , TYPE: TMSRequest_PlacedRequest_ByCIPL , if below method is in no use anymore
    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt.Columns.AddRange(new DataColumn[1] { new DataColumn("ID", typeof(int)) });

    //        foreach (GridViewRow row in grdPlacedRequest.Rows)
    //        {
    //            string key = grdPlacedRequest.DataKeys[row.RowIndex].Value.ToString();
    //            CheckBox chkPlaced = row.FindControl("chkPlaced") as CheckBox;

    //            if (chkPlaced.Checked)
    //            {
    //                DataRow dr = dt.NewRow();
    //                dr["ID"] = int.Parse(key);
    //                dt.Rows.Add(dr);
    //            }
    //        }

    //        using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
    //        {
    //            using (SqlCommand cmd = new SqlCommand("TMS_sp_update_PlacedRequest_ByCIPL", con))  // set [Date of Acceptance] = today date
    //            {
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMS_PlacedRequestUpdateTable", dt);
    //                sqlParameter.SqlDbType = SqlDbType.Structured;
    //                con.Open();
    //                cmd.ExecuteNonQuery();
    //                con.Close();
    //                BindGrid();
    //            }
    //        }
    //    }
    //    catch (Exception ee)
    //    {

    //    }
    //}

    protected void grdCancelRejectRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindGrid(ddlOperationalStatus.SelectedItem.Text, ddlTransporterResponse.SelectedItem.Text,txtrequestno.Text,txttransportercode.Text);
        grdCancelRejectRequest.PageIndex = e.NewPageIndex;
        grdCancelRejectRequest.DataBind();
    }
    
    
    //Added by Jyothi
    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdCancelRejectRequestExcel.Visible = true;
        ExportGridToExcel(grdCancelRejectRequestExcel);
        grdCancelRejectRequestExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "CancelRejectedRequest" + DateTime.Now + ".xls";
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
        BindGrid(ddlOperationalStatus.SelectedItem.Text, ddlTransporterResponse.SelectedItem.Text,txtrequestno.Text,txttransportercode.Text);
    }
}