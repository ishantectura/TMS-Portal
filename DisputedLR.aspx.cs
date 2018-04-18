using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;
using System.IO;
using System.Text.RegularExpressions;
using Ionic.Zip;

public partial class DisputedLR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindData();
            //btnShowDetails.Enabled = false;
        }
    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    private void BindData()
    {
        grdDisputedLR.DataSource = (new DisputedLrs()).GetDisputedLrs(Session["UserName"].ToString(),txtFillterTrcukNo.Text,txtFilterLrno.Text,txtFromDate.Text,txtToDate.Text);
        grdDisputedLR.DataBind();

        //Added by Jyothi
        grdDisputedLRExcel.DataSource = (new DisputedLrs()).GetDisputedLrs(Session["UserName"].ToString(), txtFillterTrcukNo.Text, txtFilterLrno.Text, txtFromDate.Text, txtToDate.Text);
        grdDisputedLRExcel.DataBind();

    }
    protected void grdDisputedLR_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindData();
        grdDisputedLR.PageIndex = e.NewPageIndex;
        grdDisputedLR.DataBind();
    }
    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdDisputedLR.Rows)
        {
            string key = grdDisputedLR.DataKeys[row.RowIndex].Value.ToString();
            CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
            DropDownList ddlTransitStatus = row.FindControl("ddlTransitStatus") as DropDownList;
            if (chkSelectRow.Checked == true)
            {
                if (key != "")
                {
                    // UpdateLRHeader(ddlTransitStatus.SelectedItem.Text, key);
                    Response.Redirect("LRReadyForUploadingDetails.aspx?LRNo=" + key + "&IsDispute=yes");
                }
            }
        }
        lblmsg.Text = " Please Select any row to get LR Details";
        lblmsg.ForeColor = System.Drawing.Color.Red;
        lblmsg.Visible = true;
    }
    protected void chkSelectRow_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gv = (GridViewRow)chk.NamingContainer;
        int rownumber = gv.RowIndex;

        if (chk.Checked)
        {
            int i;
            for (i = 0; i <= grdDisputedLR.Rows.Count - 1; i++)
            {
                if (i != rownumber)
                {
                    CheckBox chkcheckbox = ((CheckBox)(grdDisputedLR.Rows[i].FindControl("chkSelectRow")));
                    chkcheckbox.Checked = false;
                }
                lblmsg.Text = "";
                lblmsg.Visible = false;
            }
        }
        else
        {
            ViewState["LRName"] = null;
        }
    }


    protected void btnUpload_Click(object sender, EventArgs e)
    {
        int flag = 0;
        foreach (GridViewRow row in grdDisputedLR.Rows)
        {
            string key = grdDisputedLR.DataKeys[row.RowIndex].Value.ToString();
            //string LRName = row.Cells[1].Text;
            CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
            if (chkSelectRow.Checked == true)
            {
                uploadFileDiv.Visible = true;
                listofuploadedfiles.Text = "";
                ViewState["LRName"] = key;
                flag = 1;
                lblmsg.Text = "";
                lblmsg.Visible = false;

            }
            else
            {
                if (flag != 1)
                {
                    lblmsg.Text = "Please Select LR Row for which you want to upload Documents";
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                    lblmsg.Visible = true;
                    uploadFileDiv.Visible = false;
                }
            }

        }

    }
    protected void uploadFile_Click(object sender, System.EventArgs e)
    {

        if ((UploadImages.PostedFile != null) && (UploadImages.PostedFile.ContentLength > 0) && UploadImages.HasFiles == true)
        {



            if (ViewState["LRName"] != null)
            {
                foreach (HttpPostedFile uploadedFile in UploadImages.PostedFiles)
                {
                    string fn = System.IO.Path.GetFileName(uploadedFile.FileName);
                    string newLRNo;
                    if (ViewState["LRName"].ToString().Contains('/'))
                    {
                        newLRNo = Regex.Replace(ViewState["LRName"].ToString(), @"[^\w\d]", "");
                    }
                    else
                    {
                        newLRNo = ViewState["LRName"].ToString();
                    }
                    string SaveLocation = Server.MapPath("LRDatafiles") + "\\" + newLRNo + "_" + fn;
                    try
                    {
                        // UploadImages.PostedFile.SaveAs(SaveLocation);
                        uploadedFile.SaveAs(SaveLocation);
                        //  Response.Write("The file has been uploaded.");
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Error: " + ex.Message);
                        //Note: Exception.Message returns detailed message that describes the current exception. 
                        //For security reasons, we do not recommend you return Exception.Message to end users in 
                        //production environments. It would be better just to put a generic error message. 
                    }
                }
                uploadFileDiv.Visible = false;
                lblmsg.Text = listofuploadedfiles.Text + "File(s) Uploaded successfully !";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                lblmsg.Visible = true;


                foreach (GridViewRow row in grdDisputedLR.Rows)
                {
                    CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
                    if (chkSelectRow.Checked == true)
                    {
                        chkSelectRow.Checked = false;
                    }
                }

            }
            else
            {
                lblmsg.Text = "Please Select LR Row for which you want to upload Documents";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                lblmsg.Visible = true;
                uploadFileDiv.Visible = false;
            }



        }
        else
        {
            Response.Write("Please select a file to upload.");
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
       // uploadFileDiv.Visible = false;
        int flag = 0;
        var features = new List<string>();
        foreach (GridViewRow row in grdDisputedLR.Rows)
        {
            string key = grdDisputedLR.DataKeys[row.RowIndex].Value.ToString();
            CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
            if (chkSelectRow.Checked == true)
            {
                //string sourceDir = "C:\\inetpub\\wwwroot\\PublishTMS_test\\LRDatafiles\\";
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

                        //string fileNameNew = result[1].Remove(key.Length);
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
                                lblmsg.Text = "No file exist for this LR no.";
                                lblmsg.ForeColor = System.Drawing.Color.Red;
                                lblmsg.Visible = true;
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

                    lblmsg.Text = "No file exist";
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                    lblmsg.Visible = true;

                }



            }
            else
            {
                lblmsg.Text = "Select LR No ";
            }
        }

    }

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

    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdDisputedLRExcel.Visible = true;
        ExportGridToExcel(grdDisputedLRExcel);
        grdDisputedLRExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "DisputedLR" + DateTime.Now + ".xls";
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
        BindData();
    }
}