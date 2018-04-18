using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class TrackVehicle : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindGridview("","");
            //txtVehicleNo.Text = "";
        }
        else
        {
            lblmsg.Text = "";
            //txtVehicleNo.Text = "";
        }
        
    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

     #region Bind Gridview
    private void BindGridview(string vehicle, string city)
    {
      
        

        grdTrackVehicle.DataSource = (new TrackVehicles()).GetTrackVehicles(Session["UserName"].ToString(),vehicle,city);
        grdTrackVehicle.DataBind();

        //Added by Jyothi
        grdTrackVehicleExcel.DataSource = (new TrackVehicles()).GetTrackVehicles(Session["UserName"].ToString(), vehicle, city);
        grdTrackVehicleExcel.DataBind();

        //IList<BusinessLayerClasses.TrackVehicle> track = (new TrackVehicles()).GetTrackVehicles(Session["UserName"].ToString());
        
        //foreach (var va in track)
        //{
        //    if (va.TransitStatus != "")
        //    {
        //    if (va.TransitStatus == "In-Transit")
        //    {
        //        //GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        //        //DropDownList ddlTransitStatus= gvRow.FindControl("ddlTransitStatus") as DropDownList
        //        //In-Transit
        //        //LR Copy Submitted
        //        //Reached at destination
        //        //Unloaded

        //        DropDownList ddlTransitStatus = grdTrackVehicle.FindControl("ddlTransitStatus") as DropDownList;
                
        //        ddlTransitStatus.SelectedIndex = 0;
                
        //        //ddlTransitStatus.SelectedIndex = 0;
        //    }
        //    //    if (va.TransitStatus == "LR Copy Submitted")
            //    {
            //        DropDownList ddlTransitStatus = grdTrackVehicle.FindControl("ddlTransitStatus") as DropDownList;
            //        ddlTransitStatus.SelectedIndex = 1;
            //    }
            //    if (va.TransitStatus == "Reached at destination")
            //    {
            //        DropDownList ddlTransitStatus = grdTrackVehicle.FindControl("ddlTransitStatus") as DropDownList;
            //        ddlTransitStatus.SelectedIndex=2;
            //    }
            //    if (va.TransitStatus == "Unloaded")
            //    {
            //        DropDownList ddlTransitStatus = grdTrackVehicle.FindControl("ddlTransitStatus") as DropDownList;
            //        ddlTransitStatus.SelectedIndex=3;
            //    }
        //    }

        //}
    }
     #endregion
    protected void ddlTransitStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        DropDownList ddlTransitStatus = gvRow.FindControl("ddlTransitStatus") as DropDownList;
        string myDataKey = "";
        object objTemp = grdTrackVehicle.DataKeys[gvRow.RowIndex].Value as object;
        if (objTemp != null)
        {
            myDataKey = objTemp.ToString();
        }

        UpdateLRHeader(ddlTransitStatus.SelectedItem.Text, myDataKey);
        //BindGridview();

//      ddlTransitStatus_SelectedIndexChanged

        //DateTime statuschangedate = DateTime.Now;
        //using (SqlConnection con = new SqlConnection())
        //{
        //         con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
        //            con.Open();
        //            SqlCommand cmd = new SqlCommand("update TMS_LR_Header set [Modified_date] = GETDATE() where [LR No_]='" + myDataKey + "'");
        //        SqlTransaction transaction;
        //        transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted); // Start a local transaction.
        //        cmd.Connection = con;
        //        cmd.Transaction = transaction;
                
        //        try
        //        {
        //        cmd.CommandTimeout = 0;
        //        //cmd.CommandType = CommandType;
        //       // cmd.Parameters.AddWithValue("@LRNo", LRNo);
        //       // cmd.Parameters.AddWithValue("@TransitStatus", TransitStatus);
        //        cmd.ExecuteNonQuery();
        //        transaction.Commit();
        //        con.Close();
        //        }
        //        catch(Exception ex2)
        //        {
        //            lblmsg.Text = ex2.Message;
        //            lblmsg.Visible = true;
        //            lblmsg.ForeColor = System.Drawing.Color.Red;
        //        }

        //   // SqlCommand cmd = new SqlCommand(" UPDATE Account  SET name = Aleesha, CID = 24 Where name =Areeba and CID =11 )";
        //     //   cmd.ExecuteNonQuery();
        //}
     //   BindGridview();


    }


    protected void UpdateLRHeader(string TransitStatus, string LRNo)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
            con.Open();
            SqlCommand cmd = new SqlCommand("TMS_sp_update_LRHeader_TransitStatus");
            SqlTransaction transaction;
            transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted); // Start a local transaction.
            cmd.Connection = con;
            cmd.Transaction = transaction;
            #region Transaction
            try
            {
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LRNo", LRNo);
                cmd.Parameters.AddWithValue("@TransitStatus", TransitStatus);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                con.Close();
            }
            catch (Exception ex1)
            {
                try
                {
                    transaction.Rollback(); //manual                  
                    con.Close();
                    lblmsg.Text = ex1.Message;
                    lblmsg.Visible = true;
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                }
                catch (Exception ex2)
                {
                    lblmsg.Text = ex2.Message;
                    lblmsg.Visible = true;
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            #endregion
        }
    }
    protected void grdTrackVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int index = Convert.ToInt32(e.Row.DataItemIndex.ToString());
        if (index != -1)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string myDataKey = "";
                object objTemp = grdTrackVehicle.DataKeys[e.Row.RowIndex].Value as object;
                if (objTemp != null)
                {
                    myDataKey = objTemp.ToString();
                }
                DropDownList ddlTransitStatus = (e.Row.FindControl("ddlTransitStatus") as DropDownList);
                DataTable dt1 = new DataTable();
                dt1 = GetData("select [Transit Status] FROM  [dbo].[TMS_LR_Header] where [LR No_] = '" + myDataKey + "'");
                if (dt1.Rows[0][0].ToString() != "")
                    ddlTransitStatus.Items.FindByText(dt1.Rows[0][0].ToString()).Selected = true;
                if (dt1.Rows[0][0].ToString() == "LR Copy Submitted")
                {
                    CheckBox chkSelectRow = e.Row.FindControl("chkSelectRow") as CheckBox;
                    chkSelectRow.Enabled = true;
                    chkSelectRow.BackColor = System.Drawing.Color.Green;
                    chkSelectRow.BorderColor = System.Drawing.Color.DarkBlue;
                    chkSelectRow.Width = System.Web.UI.WebControls.Unit.Pixel(20);
                    chkSelectRow.Height = System.Web.UI.WebControls.Unit.Pixel(20);
                }
            }
        }


       
        
    }

    //Added by Jyothi
    protected void grdTrackVehicle_RowDataBoundExcel(object sender, GridViewRowEventArgs e)
    {
        int index = Convert.ToInt32(e.Row.DataItemIndex.ToString());
        if (index != -1)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string myDataKey = "";
                object objTemp = grdTrackVehicleExcel.DataKeys[e.Row.RowIndex].Value as object;
                if (objTemp != null)
                {
                    myDataKey = objTemp.ToString();
                }
                DropDownList ddlTransitStatus = (e.Row.FindControl("ddlTransitStatus") as DropDownList);
                DataTable dt1 = new DataTable();
                dt1 = GetData("select [Transit Status] FROM  [dbo].[TMS_LR_Header] where [LR No_] = '" + myDataKey + "'");
                if (dt1.Rows[0][0].ToString() != "")
                    ddlTransitStatus.Items.FindByText(dt1.Rows[0][0].ToString()).Selected = true;
                if (dt1.Rows[0][0].ToString() == "LR Copy Submitted")
                {
                    CheckBox chkSelectRow = e.Row.FindControl("chkSelectRow") as CheckBox;
                    chkSelectRow.Enabled = true;
                    chkSelectRow.BackColor = System.Drawing.Color.Green;
                    chkSelectRow.BorderColor = System.Drawing.Color.DarkBlue;
                    chkSelectRow.Width = System.Web.UI.WebControls.Unit.Pixel(20);
                    chkSelectRow.Height = System.Web.UI.WebControls.Unit.Pixel(20);
                }
            }
        }




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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // SubmitData();
    }

    private void SubmitData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ID"));

        dt.Columns.Add((new DataColumn("Transit status")));

        dt.Columns.Add((new DataColumn("Status Remark")));
       


        foreach (GridViewRow row in grdTrackVehicle.Rows)
        {
            string key = grdTrackVehicle.DataKeys[row.RowIndex].Value.ToString();
            DropDownList ddlTransitStatus = row.FindControl("ddlTransitStatus") as DropDownList;
            TextBox txtStatusRemark = row.FindControl("txtStatusRemark") as TextBox;
            
            //CheckBox chkPlaced = row.FindControl("chkSelectedRow") as CheckBox;
            

           // if (chkPlaced.Checked)
            //{
                DataRow dr = dt.NewRow();
                dr["ID"] = key;
                dr["Transit status"] = ddlTransitStatus.SelectedItem.Text;
               dr["Status Remark"]= txtStatusRemark.Text;
                dt.Rows.Add(dr);
           // }
        }

        using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand("TMS_sp_update_PendingLR", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = cmd.Parameters.AddWithValue("@TMS_PendingLR", dt);
                sqlParameter.SqlDbType = SqlDbType.Structured;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                BindGridview("","");
            }
        }
    }
    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)grdTrackVehicle.HeaderRow.FindControl("chkboxSelectAll");
        foreach (GridViewRow row in grdTrackVehicle.Rows)
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
    protected void txtVehicleNo_TextChanged(object sender, EventArgs e)
    {
        BindGridview(txtVehicleNo.Text.ToString(),"");
        //txtVehicleNo.Text = "";
    }
   
    protected void btnSub_Click(object sender, EventArgs e)
    {
        SubmitData();
       // NavWebServiceCarlsberg.RQT.
    }
    protected void grdTrackVehicle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindGridview("","");
        grdTrackVehicle.PageIndex = e.NewPageIndex;
        grdTrackVehicle.DataBind();
    }
    protected void txtcity_TextChanged(object sender, EventArgs e)
    {
        BindGridview("",txtcity.Text);
    }

    //Added by Jyothi
    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdTrackVehicleExcel.Visible = true;
        ExportGridToExcel(grdTrackVehicleExcel);
        grdTrackVehicleExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "TrackVehicle" + DateTime.Now + ".xls";
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