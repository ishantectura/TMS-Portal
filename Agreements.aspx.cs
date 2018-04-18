using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Agreements : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ddlYear.Items.Add(DateTime.Now.AddYears(-2).Year.ToString());
            ddlYear.Items.Add(DateTime.Now.AddYears(-1).Year.ToString());
            ddlYear.Items.Add(DateTime.Now.Year.ToString());
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            ShowGrid();
            BindTransporters();
            if (Session["UserType"].ToString() == "Admin")
            {
                uploadAgreement.Visible = true;
            }
        }
    }

    protected void uploadFile_Click(object sender, System.EventArgs e)
    {

        uploadFile();
    }

    public void uploadFile()
    {
        if ((flUploadAgreement.PostedFile != null) && (flUploadAgreement.PostedFile.ContentLength > 0) && flUploadAgreement.HasFiles == true)
        {
            foreach (HttpPostedFile uploadedFile in flUploadAgreement.PostedFiles)
            {
                string fn = System.IO.Path.GetFileName(uploadedFile.FileName);
                Boolean isSelected = false;
                foreach(ListItem li in cblTransporters.Items)
                {
                    if (li.Selected)
                    {
                        string CurrentFilePath = Server.MapPath("Agreements") + "\\" + ddlYear.SelectedItem.Text + "\\" + li.Value + "\\";
                        bool exists = System.IO.Directory.Exists(CurrentFilePath);
                        isSelected = true;
                        if (exists)
                        {
                            System.IO.DirectoryInfo di = new DirectoryInfo(CurrentFilePath);

                            foreach (FileInfo file in di.GetFiles())
                            {
                                file.Delete();
                            }
                            foreach (DirectoryInfo dir in di.GetDirectories())
                            {
                                dir.Delete(true);
                            }
                        }
                        if (!exists)
                            System.IO.Directory.CreateDirectory(CurrentFilePath);

                        uploadedFile.SaveAs(CurrentFilePath + fn);
                        SavePath(li.Value, ddlYear.SelectedItem.Text, CurrentFilePath + fn);
                    }
                }
                if (!isSelected)
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please select atleast one transporter.";
                    return;
                }
                else
                {
                    lblmsg.Visible = false;
                }
                    
                cblTransporters.ClearSelection();
                    ShowGrid();
               
                
            }
            //uploadFileDiv.Visible = false;
            lblmsg.Text = "File(s) Uploaded successfully !";
            lblmsg.ForeColor = System.Drawing.Color.Green;
            lblmsg.Visible = true;

        }
        else
        {
            lblmsg.Text = "Please select a file to upload.";
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Visible = true;
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
       
    }

    public void SavePath(string userName,string year,string path)
    {
        using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand("TMS_sp_InsertAgreements", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@path", path);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void ShowGrid()
    {
        //DataTable dt = GetData("SELECT Username , Year , path  FROM [dbo].[TMS_Agreements] where username ='" + Session["UserName"].ToString() + "'");
        DataTable dt = GetData("SELECT Name,Username , Year , path  FROM [dbo].[TMS_Agreements] TA inner join TMS_Vendor TV on TA.Username = TV.No_ where Username in (select[Transporter Code] from TMS_User_Transporter_Mapping where [Portal User ID]  ='" + Session["UserName"].ToString() + "')");

        if (dt.Rows.Count == 0)
            GridView2.Visible = false;
        else
            GridView2.Visible = true;
        GridView2.DataSource = dt;
        GridView2.DataBind();
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

    protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
    {if (e.CommandArgument.ToString().Length>5)
        {
            string strURL = e.CommandArgument.ToString();
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            int totalLength = strURL.Split('\\').Length;
            string year = strURL.Split('\\')[totalLength - 3];
            string transporterCode = strURL.Split('\\')[totalLength - 2];
            string lastName = strURL.Split('\\')[totalLength - 1];
            string filename = transporterCode + "_" + year + "_" + lastName;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            byte[] data = req.DownloadData(strURL);
            response.BinaryWrite(data);
            response.End();
        }
    }

    public void BindTransporters()
    {
        DataTable dt = GetData("select distinct ([Transporter Name] + ','+[Transporter Code])as Name,[Transporter Code] as Code  from TMS_User_Transporter_Mapping");
        cblTransporters.DataSource = dt; 
        cblTransporters.DataTextField = "Name";
        cblTransporters.DataValueField = "Code";
        cblTransporters.DataBind();
    }

    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ShowGrid();
        GridView2.PageIndex = e.NewPageIndex;
        GridView2.DataBind();
    }
}