using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;
using System.Drawing;
using System.IO;

public partial class LRReadyForInvoice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["UserName"] != null)
            {
                BindGridview();
            }
        }

    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    protected void chkSelectRow_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gv = (GridViewRow)chk.NamingContainer;
        int rownumber = gv.RowIndex;

        if (chk.Checked)
        {
            int i;
            for (i = 0; i <= grdAcceptedLR.Rows.Count - 1; i++)
            {
                if (i != rownumber)
                {
                    CheckBox chkcheckbox = ((CheckBox)(grdAcceptedLR.Rows[i].FindControl("chkSelectRow")));
                    chkcheckbox.Checked = false;
                }
                lblmsg.Text = "";
                lblmsg.Visible = false;
                uploadFileDiv.Visible = false;

            }
        }
        else
        {
            ViewState["LRName"] = null;
        }
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdAcceptedLR.Rows)
        {
            string key = grdAcceptedLR.DataKeys[row.RowIndex].Value.ToString();
            CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
            if (chkSelectRow.Checked == true)
            {
                if (key != "")
                    // Response.Redirect("LREntry.aspx?LRNo=" + key);
                    Response.Redirect("FreightInvoice.aspx?LRNo=" + key); //added by anu on 3 oct
            }
        }
    }

    protected void grdAcceptedLR_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindGridview();
        grdAcceptedLR.PageIndex = e.NewPageIndex;
        grdAcceptedLR.DataBind();
    }

    public void BindGridview()
    {
        grdAcceptedLR.DataSource = (new AcceptedLRs()).GetAcceptedLRs(Session["UserName"].ToString(),txtFillterTrcukNo.Text,txtTransporterCode.Text,txtFilterLrno.Text,txtFromDate.Text,txtToDate.Text);
        grdAcceptedLR.DataBind();

        //Added by Jyothi
        grdAcceptedLRExcel.DataSource = (new AcceptedLRs()).GetAcceptedLRs(Session["UserName"].ToString(), txtFillterTrcukNo.Text, txtTransporterCode.Text, txtFilterLrno.Text, txtFromDate.Text, txtToDate.Text);
        grdAcceptedLRExcel.DataBind();
    }
    //protected void btnshowreport_Click(object sender, EventArgs e)
    //{
    //    Boolean chk=false;
        
    //    foreach (GridViewRow row in grdAcceptedLR.Rows)
    //     {
    //        string key = grdAcceptedLR.DataKeys[row.RowIndex].Value.ToString();
            
    //        CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
    //        string load = "TPTInvoiceReport";
    //        if (chkSelectRow.Checked == true)
    //        {
    //            Session["LRNO"] = key;
    //           // Response.Redirect("Report.aspx?Report=" + "TPTInvoiceReport");
    //            Response.Write("<script>window.open ('Report.aspx?Report=" + load + "','_blank');</script>");
    //            chk = true;
    //        }
    //        else
    //        {
    //            //lblmsg.Text = "Select LR !";
    //            //lblmsg.ForeColor = System.Drawing.Color.Red;
    //            //lblmsg.Visible = true;
            
    //        }
    //    }

    //    if (chk == false)
    //    {
    //        lblmsg.Text = "Select LR !";
    //        lblmsg.ForeColor = System.Drawing.Color.Red;
    //        lblmsg.Visible = true;
    //    }
    //}

    //Added by Jyothi
    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdAcceptedLRExcel.Visible = true;
        ExportGridToExcel(grdAcceptedLRExcel);
        grdAcceptedLRExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "LRReadyForInvoice" + DateTime.Now + ".xls";
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

    protected void btngo_Click(object sender, EventArgs e)
    {
        BindGridview();
    }
}