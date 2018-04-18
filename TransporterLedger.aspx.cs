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

public partial class TransporterLedger : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindDropDown();
            BindData("","","");
            //btnShowDetails.Enabled = false;
        }
    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }


    #region Bind Gridview
    private void BindData(string transporterNo, string fromdate, string todate)
    {
        grdTransporterLedger.DataSource = (new TransporterLedgers()).GetTransporterLedger(transporterNo, fromdate, todate, Session["UserName"].ToString());
        grdTransporterLedger.DataBind();

        //Added by Jyothi
        grdTransporterLedgerExcel.DataSource = (new TransporterLedgers()).GetTransporterLedger(transporterNo, fromdate, todate, Session["UserName"].ToString());
        grdTransporterLedgerExcel.DataBind();

    }
    #endregion
    protected void grdTransporterLedger_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ddlTransporterNo.SelectedIndex == 0)
        {
            BindData("", txtFromDate.Text, txtToDate.Text);
        }
        else
            BindData(ddlTransporterNo.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
        
        grdTransporterLedger.PageIndex = e.NewPageIndex;
        grdTransporterLedger.DataBind();

    }
    
   

    private void BindDropDown()
    { 
        //ddlTransporterNo


        try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT distinct [Vendor No_] FROM TMS_Vendor_Ledger_Entry order by [Vendor No_] ", con))
                    {


                        cmd.CommandType = System.Data.CommandType.Text;
                      
                        con.Open();

                        ddlTransporterNo.DataSource = cmd.ExecuteReader();
                        ddlTransporterNo.DataTextField = "Vendor No_";
                        ddlTransporterNo.DataValueField = "Vendor No_";
                        ddlTransporterNo.DataBind();
                        con.Close();
                    }
                }
            ddlTransporterNo.Items.Insert(0, new ListItem("--Select Transporter--", "0"));

            }
            catch (Exception E)
            {
               
            }




    
    }
    protected void ddlTransporterNo_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtFromDate.Text = "";
        txtToDate.Text = "";
        if (ddlTransporterNo.SelectedIndex == 0)
        {
            BindData("", txtFromDate.Text, txtToDate.Text);
        }
        else
            BindData(ddlTransporterNo.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
    }
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        if (ddlTransporterNo.SelectedIndex == 0)
        {
        BindData("", txtFromDate.Text, txtToDate.Text);
        }
        else
        BindData(ddlTransporterNo.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        if (ddlTransporterNo.SelectedIndex == 0)
        {
        BindData("", txtFromDate.Text, txtToDate.Text);
        }
        else
        BindData(ddlTransporterNo.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        if (ddlTransporterNo.SelectedIndex == 0)
        {
            BindData("", txtFromDate.Text, txtToDate.Text);
        }
        else
            BindData(ddlTransporterNo.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
    }

    //Added by Jyothi
    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdTransporterLedgerExcel.Visible = true;
        ExportGridToExcel(grdTransporterLedgerExcel);
        grdTransporterLedgerExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "TrnasporterLedger" + DateTime.Now + ".xls";
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