using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BusinessLayerClasses;
using System.IO;
//using System.IO.Compression;
using System.Net;


using System.IO;
using Ionic.Zip;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class LRReadyForUploading : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindData("", "","","","","");
            BindDropDown();
            //btnShowDetails.Enabled = false;
            lblmsg.Text = "";
            txtFilterLrno.Text = "";
            txtFillterTrcukNo.Text = "";
            lblmsg1.Text = "";
        }

    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    private void BindData(string truckno, string lrno,string transportercode,string city,string fromdate,string todate)
    {
        string loginAs = "";
        if (Session["UserType"].ToString() == "Transporter")
        {
            loginAs = "Transporter";
            h4Trans.Visible = true;
        }
        else
        {
            loginAs = "Third Party";
            h4Third.Visible = true;
            btnUpload.Visible = false;
        }
        //string username,string truckno, string lrno,string logiinAs,string transportercode,string city,string fromdate,string todate
        if(transportercode=="--Select Transporter--")
        {
            transportercode = "";
        }

        grdPendingLR.DataSource = (new PendingLRs()).GetPendingLRs(Session["UserName"].ToString(), truckno, lrno, loginAs,transportercode,city,fromdate,todate); 
        grdPendingLR.DataBind();

        //Added by Jyothi
        grdPendingLRExcel.DataSource = (new PendingLRs()).GetPendingLRs(Session["UserName"].ToString(), truckno, lrno, loginAs, transportercode, city, fromdate, todate);
        grdPendingLRExcel.DataBind();

    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
      
        bool IsSelect = false; // Added by Jyothi
        foreach (GridViewRow row in grdPendingLR.Rows)
        {
            string key = grdPendingLR.DataKeys[row.RowIndex].Value.ToString();
            CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
            DropDownList ddlTransitStatus = row.FindControl("ddlTransitStatus") as DropDownList;
            if (chkSelectRow.Checked == true)
            {
                if (key != "")
                {
                    // UpdateLRHeader(ddlTransitStatus.SelectedItem.Text, key);

                    #region Added by Jyothi
                    IsSelect = true;
                    if (key != "")
                    {
                        // UpdateLRHeader(ddlTransitStatus.SelectedItem.Text, key);
                        string SaveLocation = Server.MapPath("LRDatafiles");
                        DirectoryInfo fileDirectory = new DirectoryInfo(SaveLocation);
                        FileInfo[] Files = fileDirectory.GetFiles(key + "*" + ".*");
                        if (h4Third.Visible == true)
                        {
                            Response.Redirect("LRReadyForUploadingDetails.aspx?LRNo=" + key);
                        }
                        else
                        {
                            if (Files.Count() == 0)
                            {
                                lblmsg.Text = " Please upload the document";
                                lblmsg.ForeColor = System.Drawing.Color.Red;
                                lblmsg.Visible = true;
                                break;
                            }
                            else
                                Response.Redirect("LRReadyForUploadingDetails.aspx?LRNo=" + key);
                        }
                    }
                    #endregion
                   // Response.Redirect("LRReadyForUploadingDetails.aspx?LRNo=" + key);
                }
            }
        }
        if (!IsSelect)
        {
            lblmsg.Text = " Please Select any row to get LR Details";
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Visible = true;
        }
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

    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        BindData("", "","","","","");
        grdPendingLR.PageIndex = e.NewPageIndex;
        grdPendingLR.DataBind();
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        if (ddlTransporterNo.SelectedIndex == 0)
        {
            BindData(txtFillterTrcukNo.Text, txtFilterLrno.Text, ddlTransporterNo.SelectedItem.Text, txtCity.Text, txtFromDate.Text, txtToDate.Text); //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
        }
        else
            BindData(txtFillterTrcukNo.Text, txtFilterLrno.Text, ddlTransporterNo.SelectedItem.Text, txtCity.Text, txtFromDate.Text, txtToDate.Text); //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
    }

    protected void btnCalculateInvoice_Click(object sender, EventArgs e)
    {

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        int flag = 0;
        foreach (GridViewRow row in grdPendingLR.Rows)
        {
            string key = grdPendingLR.DataKeys[row.RowIndex].Value.ToString();
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

    //protected void uploadFile_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (UploadImages.HasFiles)
    //        {
    //            if (ViewState["LRName"] != null)
    //            {
    //                foreach (HttpPostedFile uploadedFile in UploadImages.PostedFiles)
    //                {
    //                    //string myFile = @"C:\TMS_Document\";
    //                    uploadedFile.SaveAs(Server.MapPath("Data")+"\\"+ViewState["LRName"].ToString() + "_" + uploadedFile.FileName);
    //                    //   uploadedFile.SaveAs(@"c:\TMS_Document\" + ViewState["LRName"].ToString() + "_" + uploadedFile.FileName);

    //                    listofuploadedfiles.Text += String.Format("{0}<br />", uploadedFile.FileName);
    //                }
    //                uploadFileDiv.Visible = false;
    //                lblmsg.Text = listofuploadedfiles.Text + "File(s) Uploaded successfully !";
    //                lblmsg.ForeColor = System.Drawing.Color.Green;
    //                lblmsg.Visible = true;


    //                foreach (GridViewRow row in grdPendingLR.Rows)
    //                {
    //                    CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
    //                    if (chkSelectRow.Checked == true)
    //                    {
    //                        chkSelectRow.Checked = false;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                lblmsg.Text = "Please Select LR Row for which you want to upload Documents";
    //                lblmsg.ForeColor = System.Drawing.Color.Red;
    //                lblmsg.Visible = true;
    //                uploadFileDiv.Visible = false;
    //            }
    //        }
    //    }
    //    catch (Exception ed)
    //    {
    //        lblmsg.Text = "Error: Folder does not exist.<br/> Error Detail: " + ed.Message;
    //        lblmsg.ForeColor = System.Drawing.Color.Red;
    //        lblmsg.Visible = true;
    //    }




    //}




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


                foreach (GridViewRow row in grdPendingLR.Rows)
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


    //private void SaveTToWeb()
    //{
    //    try
    //    {
    //        //create WebClient object
    //        WebClient client = new WebClient();
    //        string myFile = @"C:\file.txt";
    //        client.Credentials = CredentialCache.DefaultCredentials;
    //        client.UploadFile(@"http://myweb.com/projects/idl/Draft Results/RK/myFile", "PUT", myFile);
    //        client.Dispose();
    //    }
    //    catch (Exception err)
    //    {
    //        MessageBox.Show(err.Message);
    //    }
    //}

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gv = (GridViewRow)chk.NamingContainer;
        int rownumber = gv.RowIndex;

        if (chk.Checked)
        {
            int i;
            for (i = 0; i <= grdPendingLR.Rows.Count - 1; i++)
            {
                if (i != rownumber)
                {
                    CheckBox chkcheckbox = ((CheckBox)(grdPendingLR.Rows[i].FindControl("chkSelectRow")));
                    chkcheckbox.Checked = false;
                }
                lblmsg.Text = "";
                lblmsg.Visible = false;
                uploadFileDiv.Visible = false;
                
                //ChkLRFileUploadedOrnot(grdPendingLR.Rows[gv.RowIndex].Cells[1].Text.ToString());


            }
        }
        else
        {
            ViewState["LRName"] = null;
        }

    }


    //protected int ChkLRFileUploadedOrnot(string LRNO)
    //{
    //    string key = LRNO;
    //    int count = 0;
    //    string sourceDir = "C:\\inetpub\\TMSPortal_live\\LRDatafiles\\";
    //    string[] fileEntries = Directory.GetFiles(sourceDir);
    //    if (fileEntries.Count() > 0)
    //    {
    //        foreach (string fileName in fileEntries)
    //        {
    //            string s = "C:\\inetpub\\TMSPortal_live\\LRDatafiles\\";
    //            if (key.Contains('/'))
    //            {
    //                key = Regex.Replace(key, @"[^\w\d]", "");
    //            }
    //            int len = key.Length;
    //            string[] stringSeparators = new string[] { s };
    //            string[] result = fileName.Split(stringSeparators, StringSplitOptions.None);  // "abcd_3212.docx"   0,9-4   abc_php.docx 
    //            // string fileNameNew = result[1].Remove(result[1].LastIndexOf('_'), ((result[1].Length) - result[1].LastIndexOf('_')));

    //            string fileNameNew = result[1].Remove(key.Length);

    //            if (fileNameNew == key)
    //            {
    //                count++;
    //            }

    //        }

    //    }

    //    return count;
    //}

    protected void ddlTransitStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        DropDownList ddlTransitStatus = gvRow.FindControl("ddlTransitStatus") as DropDownList;
        string myDataKey = "";
        object objTemp = grdPendingLR.DataKeys[gvRow.RowIndex].Value as object;
        if (objTemp != null)
        {
            myDataKey = objTemp.ToString();
        }
        // UpdateLRHeader(ddlTransitStatus.SelectedItem.Text, myDataKey);
        // BindData();
    }

    protected void grdPendingLR_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //int index = Convert.ToInt32(e.Row.DataItemIndex.ToString());
        //if (index != -1)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        string myDataKey = "";
        //        object objTemp = grdPendingLR.DataKeys[e.Row.RowIndex].Value as object;
        //        if (objTemp != null)
        //        {
        //             myDataKey = objTemp.ToString();                   
        //        }
        //        DropDownList ddlTransitStatus = (e.Row.FindControl("ddlTransitStatus") as DropDownList);
        //        DataTable dt1 = new DataTable();
        //        dt1 = GetData("select [Transit Status] FROM  [dbo].[TMS_LR_Header] where [LR No_] = '" + myDataKey + "'");
        //        if(dt1.Rows[0][0].ToString() != "")
        //        ddlTransitStatus.Items.FindByText(dt1.Rows[0][0].ToString()).Selected = true;
        //        if (dt1.Rows[0][0].ToString() == "LR Copy Submitted")
        //        {
        //            CheckBox chkSelectRow = e.Row.FindControl("chkSelectRow") as CheckBox;
        //            chkSelectRow.Enabled = true;
        //            chkSelectRow.BackColor = System.Drawing.Color.Green;
        //            chkSelectRow.BorderColor = System.Drawing.Color.DarkBlue;
        //            chkSelectRow.Width = System.Web.UI.WebControls.Unit.Pixel(20);
        //            chkSelectRow.Height = System.Web.UI.WebControls.Unit.Pixel(20);
        //        }
        //    }
        //}
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

    protected void txtFilterLrno_TextChanged(object sender, EventArgs e)
    {
        BindData(txtFillterTrcukNo.Text, txtFilterLrno.Text, ddlTransporterNo.SelectedItem.Text, txtCity.Text, txtFromDate.Text,txtToDate.Text ); //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
        lblmsg.Text = "";
    }
    protected void txtFillterTrcukNo_TextChanged(object sender, EventArgs e)
    {
        BindData(txtFillterTrcukNo.Text, txtFilterLrno.Text, ddlTransporterNo.SelectedItem.Text, txtCity.Text, txtFromDate.Text, txtToDate.Text); //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
        lblmsg.Text = ""; //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
    }
    protected void testbtn_Click(object sender, EventArgs e)
    {

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
            //BindData("", txtFromDate.Text, txtToDate.Text, "", "", "", "");
            BindData(txtFillterTrcukNo.Text, txtFilterLrno.Text, "", txtCity.Text, txtFromDate.Text, txtToDate.Text); //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
        }
        else
            BindData(txtFillterTrcukNo.Text, txtFilterLrno.Text, ddlTransporterNo.SelectedItem.Text, txtCity.Text, txtFromDate.Text, txtToDate.Text); //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
            //BindData(ddlTransporterNo.SelectedItem.Text, txtFromDate.Text, txtToDate.Text, "", "", "", "");
    }

    #region
    //protected void btnDownload_Click(object sender, EventArgs e)
    //{
    //    uploadFileDiv.Visible = false;
    //    int flag = 0;
    //    var features = new List<string>();
    //    foreach (GridViewRow row in grdPendingLR.Rows)
    //    {
    //        string key = grdPendingLR.DataKeys[row.RowIndex].Value.ToString();
    //        CheckBox chkSelectRow = row.FindControl("chkSelectRow") as CheckBox;
    //        if (chkSelectRow.Checked == true)
    //        {
    //            string sourceDir = "C:\\inetpub\\wwwroot\\PublishTMS_test\\LRDatafiles\\"; 
    //            string[] fileEntries = Directory.GetFiles(sourceDir);



    //            if (fileEntries.Count() > 0)
    //            {
    //                foreach (string fileName in fileEntries)
    //                {
    //                    string s = "C:\\inetpub\\wwwroot\\PublishTMS_test\\LRDatafiles\\";
    //                    if (key.Contains('/'))
    //                    {
    //                        key = Regex.Replace(key, @"[^\w\d]", "");
    //                    }
    //                    int len = key.Length;
    //                    // string s = "abc_php.docx";
    //                    string[] stringSeparators = new string[] { s };
    //                    string[] result = fileName.Split(stringSeparators, StringSplitOptions.None);  // "abcd_3212.docx"   0,9-4   abc_php.docx 
    //                    //   string fileNameNew = result[1].Remove(result[1].LastIndexOf('_'), ((result[1].Length) - result[1].LastIndexOf('_')));

    //                    string fileNameNew = result[1].Remove(key.Length);
    //                    //   string a= result[1]..ToString();

    //                    // string fileNameNew = s.Remove(result[1].LastIndexOf('_'), ((result[1].Length) - result[1].LastIndexOf('_')));
    //                    if (fileNameNew == key)
    //                    {
    //                        features.Add(result[1]);
    //                    }
    //                    else
    //                    {
    //                        if (features.Count == 0)
    //                        {
    //                            lblmsg1.Text = "No file exist for this LR no.";
    //                            lblmsg1.ForeColor = System.Drawing.Color.Red;
    //                            lblmsg1.Visible = true;
    //                        }
    //                    }
    //                }
    //                if (features.Count > 0)
    //                {
    //                    DownloadFiles1(features, key);
    //                }
    //            }
    //            else
    //            {

    //                lblmsg1.Text = "No file exist";
    //                lblmsg1.ForeColor = System.Drawing.Color.Red;
    //                lblmsg1.Visible = true;

    //            }



    //        }
    //        else
    //        {
    //            lblmsg.Text = "Select LR No ";
    //        }
    //    }

    //}
    //protected void DownloadFiles1(List<string> file, string lrno)
    //{
    //    if (file.Count > 0)
    //    {
    //        using (ZipFile zip = new ZipFile())
    //        {
    //            zip.AlternateEncodingUsage = ZipOption.AsNecessary;
    //            zip.AddDirectoryByName("Files");
    //            foreach (var row in file)
    //            {
    //                string filePath = "C:\\inetpub\\wwwroot\\PublishTMS_test\\LRDatafiles\\" + row;
    //                zip.AddFile(filePath, "Files");
    //            }
    //            Response.Clear();
    //            Response.BufferOutput = false;
    //            string zipName = String.Format(lrno + "_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
    //            Response.ContentType = "application/zip";
    //            Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
    //            zip.Save(Response.OutputStream);
    //            Response.End();
    //        }
    //    }
    //}
    #endregion


    protected void btnDownload_Click(object sender, EventArgs e)
    {
        uploadFileDiv.Visible = false;
        int flag = 0;
        var features = new List<string>();
        foreach (GridViewRow row in grdPendingLR.Rows)
        {
            string key = grdPendingLR.DataKeys[row.RowIndex].Value.ToString();
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


    protected void txtTransportercode_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtCity_TextChanged(object sender, EventArgs e)
    {
        BindData(txtFillterTrcukNo.Text, txtFilterLrno.Text, ddlTransporterNo.SelectedItem.Text, txtCity.Text, txtFromDate.Text, txtToDate.Text); //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
        lblmsg.Text = ""; //string truckno, string lrno,string transportercode,string city,string fromdate,string todate
    }

    //Added by Jyothi
    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdPendingLRExcel.Visible = true;
        ExportGridToExcel(grdPendingLRExcel);
        grdPendingLRExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "LRReadyForUploading" + DateTime.Now + ".xls";
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