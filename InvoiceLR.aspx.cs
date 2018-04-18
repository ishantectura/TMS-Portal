using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;
using System.Drawing;
using System.IO;
using Ionic.Zip;
using System.Text.RegularExpressions;

public partial class InvoiceLR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindGridview();
            if(Session["UserType"].ToString()=="CIPL")
            {
                //CheckBox chkcheckbox = ((CheckBox)(grdInvoicedLR.FindControl("chkSelectRow")));
                //chkcheckbox.Enabled = false;
            }
        }
    }
    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    public void BindGridview()
    {
        grdInvoicedLR.DataSource = (new AcceptedLRs()).GetInvoicedLRs(Session["UserName"].ToString(),txtFillterTrcukNo.Text,txtTransporterCode.Text,txtFilterLrno.Text,txtFromDate.Text,txtToDate.Text);
        grdInvoicedLR.DataBind();
        //Added by Jyothi
        grdInvoicedLRExcel.DataSource = (new AcceptedLRs()).GetInvoicedLRs(Session["UserName"].ToString(), txtFillterTrcukNo.Text, txtTransporterCode.Text, txtFilterLrno.Text, txtFromDate.Text, txtToDate.Text);
        grdInvoicedLRExcel.DataBind();
    }

    protected void chkSelectRow_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gv = (GridViewRow)chk.NamingContainer;
        int rownumber = gv.RowIndex;

        if (chk.Checked)
        {
            int i;
            for (i = 0; i <= grdInvoicedLR.Rows.Count - 1; i++)
            {
                if (i != rownumber)
                {
                    CheckBox chkcheckbox = ((CheckBox)(grdInvoicedLR.Rows[i].FindControl("chkSelectRow")));
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
        foreach (GridViewRow row in grdInvoicedLR.Rows)
        {
            string key = grdInvoicedLR.DataKeys[row.RowIndex].Value.ToString();
            CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
            if (chkSelectRow.Checked == true)
            {
                if (key != "")
                   // Response.Redirect("LREntry.aspx?LRNo=" + key);
                Response.Redirect("FreightInvoice.aspx?LRNo=" + key); //added by anu on 3 oct
            } 
        }
    }
    protected void grdInvoicedLR_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindGridview();
        grdInvoicedLR.PageIndex = e.NewPageIndex;
        grdInvoicedLR.DataBind();
    }
    protected void grdInvoicedLR_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckBox1 = (e.Row.FindControl("chkSelectRow") as CheckBox);

            if (Session["UserType"].ToString() == "CIPL")
            {
                CheckBox1.Enabled = false;
            }
            else
                CheckBox1.Enabled = true;
            
        }
    }

    //Added by Jyothi
    protected void grdInvoicedLR_RowDataBoundExcel(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckBox1 = (e.Row.FindControl("chkSelectRow") as CheckBox);

            if (Session["UserType"].ToString() == "CIPL")
            {
                CheckBox1.Enabled = false;
            }
            else
                CheckBox1.Enabled = true;

        }
    }

    protected void btnshowreport_Click(object sender, EventArgs e)
    {
        Boolean chk = false;

        foreach (GridViewRow row in grdInvoicedLR.Rows)
        {
            string key = grdInvoicedLR.DataKeys[row.RowIndex].Value.ToString();

            CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
            string load = "TPTInvoiceReport";
            if (chkSelectRow.Checked == true)
            {
                Session["LRNO"] = key;
                // Response.Redirect("Report.aspx?Report=" + "TPTInvoiceReport");
                Response.Write("<script>window.open ('Report.aspx?Report=" + load + "','_blank');</script>");
                chk = true;
            }
            else
            {
                //lblmsg.Text = "Select LR !";
                //lblmsg.ForeColor = System.Drawing.Color.Red;
                //lblmsg.Visible = true;

            }
        }

        if (chk == false)
        {
            lblmsg.Text = "Select LR !";
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Visible = true;
        }
    }




    //edit by vijay
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        uploadFileDiv.Visible = false;
        int flag = 0;
        var features = new List<string>();
        //change grdname
        foreach (GridViewRow row in grdInvoicedLR.Rows)
        {
            string key = grdInvoicedLR.DataKeys[row.RowIndex].Value.ToString();
            CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
            if (chkSelectRow.Checked == true)
            {
                //  string sourceDir = "C:\\inetpub\\wwwroot\\PublishTMS_test\\LRDatafiles\\";
                string sourceDir = Server.MapPath("LRDatafiles");
                string[] fileEntries = Directory.GetFiles(sourceDir);



                if (fileEntries.Count() > 0)
                {
                    foreach (string fileName in fileEntries)
                    {
                        //string s = "C:\\inetpub\\wwwroot\\PublishTMS_test\\LRDatafiles\\";
                        string s = Server.MapPath("LRDatafiles");  // Modifed by Jyothi
                        if (key.Contains('/'))
                        {
                            key = Regex.Replace(key, @"[^\w\d]", "");
                        }
                        int len = key.Length;
                        // string s = "abc_php.docx";
                        string[] stringSeparators = new string[] { s };
                        string[] result = fileName.Split(stringSeparators, StringSplitOptions.None);  // "abcd_3212.docx"   0,9-4   abc_php.docx 
                        //   string fileNameNew = result[1].Remove(result[1].LastIndexOf('_'), ((result[1].Length) - result[1].LastIndexOf('_')));

                        string fileNameNew = result[1].Remove(key.Length + 1); // Modifed by Jyothi
                        //   string a= result[1]..ToString();

                        // string fileNameNew = s.Remove(result[1].LastIndexOf('_'), ((result[1].Length) - result[1].LastIndexOf('_')));
                        string filenamenew1 = fileNameNew.Remove(0, 1); // Modifed by Jyothi
                        if (filenamenew1 == key) // Modifed by Jyothi
                        {
                            features.Add(result[1]);
                        }
                        else
                        {
                            if (features.Count == 0)
                            {
                                lblmsg1.Text = "No file exist for this LR no.";
                                lblmsg1.ForeColor = System.Drawing.Color.Red;
                                lblmsg1.Visible = true;
                            }
                        }
                    }
                    if (features.Count > 0)
                    {
                        DownloadFiles1(features, key);
                    }
                }
                else
                {

                    lblmsg1.Text = "No file exist";
                    lblmsg1.ForeColor = System.Drawing.Color.Red;
                    lblmsg1.Visible = true;

                }



            }
            else
            {
                lblmsg.Text = "Select LR No ";
            }
        }

    }


    //edit by vijay
    protected void DownloadFiles1(List<string> file, string lrno)
    {
        if (file.Count > 0)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");
                foreach (var row in file)
                {
                    //string filePath = "C:\\inetpub\\wwwroot\\PublishTMS_test\\LRDatafiles\\" + row;
                    string filePath = Server.MapPath("LRDatafiles") + "\\" + row;   // Modifed by Jyothi
                    zip.AddFile(filePath, "Files");
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format(lrno + "_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }





    //Added by Jyothi
    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdInvoicedLRExcel.Visible = true;
        ExportGridToExcel(grdInvoicedLRExcel);
        grdInvoicedLRExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "InvoicedLR" + DateTime.Now + ".xls";
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