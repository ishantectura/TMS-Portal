using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GateEntry : System.Web.UI.Page
{
    static string UserName;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserName = Session["UserName"].ToString();
        if (!Page.IsPostBack)
        {
            //Bind_RequestNo();
            calGateInDate.StartDate = DateTime.Now;
            calGateInDate.EndDate = DateTime.Now;
            calGateInDate.SelectedDate = DateTime.Today.Date;
            ddlVehicle.ClearSelection();

           
            Bind_dropdownVehicle();

            SetcurrentTime();

         //   BindtransporterType();
        }
        
    }

    public void Bind_dropdownVehicle()
    {
        DataTable dt = new DataTable();
        dt = GetData("select  distinct([Vehicle No_] ) from TMS_Request where [Operational Status]='Sent' and [Transporter Code] in ((select distinct [Transporter Code] from [TMS_User_Transporter_Mapping] where [Portal User ID]='" + UserName + "'))");
        ddlVehicle.DataSource = dt;
        ddlVehicle.DataTextField = "Vehicle No_";
        ddlVehicle.DataValueField = "Vehicle No_";
        ddlVehicle.DataBind();
        ddlVehicle.Items.Insert(0, new ListItem(String.Empty, String.Empty));
    }



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


    private void Bind_RequestNo()
    {
        //DataTable dt = new DataTable();
        //dt = GetData("select distinct(Tr.ID) as ID, TR.[No_] as Requestno from TMS_Request TR Where  LEFT( YEAR([Date of Acceptance]),2) >19 and LOWER(TR.[Operational Status]) = 'sent' and [Transporter Response] = 'Accepted' and [Vehicle No_] <> '' and [Driver Name] <> '' and [Driver Mobile No_]<>'' ");
        //ddlreqno.DataSource = dt;
        //ddlreqno.DataTextField = "Requestno"; 
        //ddlreqno.DataValueField = "ID";
        //ddlreqno.DataBind();
        //ddlreqno.Items.Insert(0, new ListItem(String.Empty, String.Empty));
    }

    public void getcurrenttime()
    {
       // string x = DateTime.Now.TimeOfDay;
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
      



         try
         {

             //string gateInDate = Convert.ToDateTime(date).ToString("yyyy-MM-dd") != "0001-01-01" ? Convert.ToDateTime(txtGateInDate.Text).ToString("yyyy-MM-dd") : "";
             string gateInDate = DateTime.Today.ToString("yyyy-MM-dd");
             if (gateInDate != "")
                 gateInDate = Convert.ToDateTime(gateInDate).ToString("yyyy-MM-dd h:mm tt"); // Convert.ToDateTime(gateInDate).ToString("yyyy-mm-dd hh:mm:ss.mmm");
             else
             {
                 lblmsg.Text = " Please Enter Gate In Date";
                 lblmsg.Visible = true;
                 return;
             }
             if (ddlreqno.SelectedIndex != -1 && gateInDate != "" && txtGateInwardNo.Text != "" && lstHour.SelectedIndex != -1 && lstMin.SelectedIndex != -1 && (validatetruckNo(txtVehicle.Text.ToString())==1))
             {
                 using (SqlConnection con = new SqlConnection())
                 {
                     con.ConnectionString = BusinessLayerClasses.ConnectionString.GetConnectionString();
                     con.Open();
                     SqlCommand cmd = new SqlCommand("TMS_sp_update_Request_operationalStatus");
                     SqlTransaction transaction;
                     transaction = con.BeginTransaction(IsolationLevel.ReadUncommitted); // Start a local transaction.
                     cmd.Connection = con;
                     cmd.Transaction = transaction;
                     #region Transaction
                     try
                     {                         
                         cmd.CommandTimeout = 0;
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.AddWithValue("@ReqNo", ddlreqno.SelectedItem.Text);
                         cmd.Parameters.AddWithValue("@GateInDate", gateInDate);
                         cmd.Parameters.AddWithValue("@BreweryGateInwardNo", txtGateInwardNo.Text);
                         cmd.Parameters.AddWithValue("@GateInTimeHH", lstHour.SelectedItem.Text);
                         cmd.Parameters.AddWithValue("@GateInTimeMM", lstMin.SelectedItem.Text);
                         cmd.Parameters.AddWithValue("@VehicleNo", txtVehicle.Text.Trim());
                         cmd.Parameters.AddWithValue("@TransporterType", lblTransporterType.Text);

                         
                         cmd.ExecuteNonQuery();
                        // transaction.Commit();

                         SendDatatoNAV objSendDatatoNAV = new SendDatatoNAV(ddlreqno.SelectedItem.Text);                      
                         //ResetForm(); // Reset form                                          
                         transaction.Commit();
                         con.Close();
                         ResetForm();
                         lblerr.Text = "";
                         lblmsg.Text = "Request Sent successfully";
                         lblmsg.Visible = true;    
                     }
                     catch (Exception ex1)
                     {
                         try
                         {
                             transaction.Rollback(); //manual
                             ResetForm(); // Reset form 
                             con.Close();
                             lblerr.Text = ex1.Message;
                             lblerr.Visible = true;
                         }
                         catch (Exception ex2)
                         {
                             lblerr.Text = ex2.Message;
                             lblerr.Visible = true;
                         }
                     }
                     #endregion              
                 }
             }
         }
         catch (Exception ee)
         {
             lblerr.Text = "Please select a valid Date !! ";
             lblerr.Visible = true;
         }
         Bind_dropdownVehicle();
         ddlVehicle.SelectedIndex = 0;
         ddlreqno.Items.Clear();
         txtGateInwardNo.Text = "";
         txtVehicle.Text = "";
    }

    public void BindtransporterType()
    {
        if (ddlreqno.SelectedItem.Text != "")
        {
            DataTable dt = GetData("select top 1 [Transporter Type] from TMS_Request where No_= '" + ddlreqno.SelectedItem.Text + "'");
            DataTable dt1 = GetData("select top 1 [Transporter Name] from TMS_Request where No_= '" + ddlreqno.SelectedItem.Text + "'");
            lblTransporterType.Text = dt.Rows[0].ItemArray[0].ToString();
            lblTransporterName.Text = dt1.Rows[0].ItemArray[0].ToString();

            if (lblTransporterType.Text != "")
            {
               // ddlTransType.Items.FindByText(lblTransporterType.Text).Selected = true;
                //lblTransporterType.Visible = false;
            }
            else
            {
               // ddlTransType.Items.Insert(0, new ListItem(String.Empty, String.Empty));
               // ddlTransType.SelectedIndex = 0;
            }
        }
    }

    public void BindtransporterName()
    {
        //[Transporter Name]


    }

    public void SetcurrentTime()  // get current time 
    {
        var time = DateTime.Now;

        string hr = DateTime.Now.ToString("HH:mm");
        var hour = hr.Remove(2);
        var minute = hr.Substring(hr.Length - 2);

        txthr.Text = hr.Remove(2);
        txtmin.Text = hr.Substring(hr.Length - 2);

        lstHour.Items.FindByValue(hour.ToString()).Selected = true;
        lstMin.Items.FindByValue(minute.ToString()).Selected = true;
        lstHour.SelectionMode = ListSelectionMode.Single;
        lstMin.SelectionMode = ListSelectionMode.Single;
    }


     
    private void ResetForm()
    {
        //Bind_RequestNo();
        txtGateInwardNo.Text = "";
        calGateInDate.SelectedDate = DateTime.Today.Date;    
        lstHour.SelectedIndex = 0;
        lstMin.SelectedIndex = 0;
        txthr.Text = "00";
        txtmin.Text = "00";
        lblmsg.Text = "";
        lblmsg.Visible = false;
       // txtVehicle.Text = "";
    }

    private DataTable GetrequestLines(string ReqNo)
    {
        DataTable dt = new DataTable();
        dt = GetData("select * from TMS_Request_Permit_Details where [Request No_] = '" + ReqNo + "'");
        return dt;



    }
    
    protected void ddlreqno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DataTable dt = new DataTable();
        //dt = GetData("select [Vehicle No_] as VehicleNo from TMS_Request TR Where  LEFT( YEAR([Date of Acceptance]),2) >19 and LOWER(TR.[Operational Status]) = 'sent' and ID = '" + ddlreqno.SelectedItem.Value + "'");
        //txtVehicle.Text = dt.Rows.Count > 0 ? dt.Rows[0].ItemArray[0].ToString() : "";
    }

    protected void txtVehicle_TextChanged(object sender, EventArgs e)
    {
        txtVehicle.Text = ddlVehicle.SelectedValue;

        DataTable dt = new DataTable();
        var vehicle = txtVehicle.Text;
        var vehicleNo = "%" + vehicle + "%";
        //var vehicle =  "%UP16Y5432%";
        // dt = GetData("select  ID as ID, [No_] as Requestno from TMS_Request   where [Vehicle No_] like '" + vehicleNo + "' and [Operational Status]=''Sent'' and [PlacedFromTransporter]<> ''Placed''  ");

        dt = GetData("select  ID as ID, [No_] as Requestno from TMS_Request   where [Vehicle No_] like '" + vehicleNo + "' and [Operational Status]='Sent' and [Transporter Response]='Accepted'  ");
        //[Operational Status]='Sent'  and [Transporter Response]='Accepted'
        //var z = dt.Rows[0].ItemArray[0].ToString();

        ddlreqno.DataSource = dt;
        ddlreqno.DataTextField = "Requestno";
        ddlreqno.DataValueField = "ID";
        ddlreqno.DataBind();
        //ddlreqno.Items.Insert(0, new ListItem(String.Empty, String.Empty));

        if (ddlreqno.SelectedItem.Text != "")
        {
            DataTable dtnameandcity = new DataTable();
            DataTable dtshipmenttype = GetData("select  [Shipment Type] as ShipmentType,Destination as Destination from TMS_Request   where [No_] = '" + ddlreqno.SelectedItem.Text + "'  ");
            if (dtshipmenttype.Rows[0].ItemArray[0].ToString() == "Sales")
            {
                dtnameandcity = GetData("select Name as Name,City as City  from TMS_Customer   where [No_] = '" + dtshipmenttype.Rows[0].ItemArray[1].ToString() + "'  ");
            }
            if (dtshipmenttype.Rows[0].ItemArray[0].ToString() == "Stock Transfer")
            {
                dtnameandcity = GetData("select Name as Name,City as City  from TMS_Location   where [Code] = '" + dtshipmenttype.Rows[0].ItemArray[1].ToString() + "'  ");
            }

            lbldestination.Text = dtshipmenttype.Rows[0].ItemArray[1].ToString();
            lblName.Text = dtnameandcity.Rows[0].ItemArray[0].ToString();
            lblCity.Text = dtnameandcity.Rows[0].ItemArray[1].ToString();

        }

        BindtransporterType();
             
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string[] GetCompletionList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }
        DataTable dt = GetVehicleNumbers(prefixText);
        List<string> items = new List<string>(count);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strName = dt.Rows[i][0].ToString();
            items.Add(strName);
        }
        return items.ToArray();
    }

    public static DataTable GetVehicleNumbers(string VehicleNo)
    {
        List<string> listVehicleNumbers = new List<string>();
        DataTable dtVehNums = new DataTable();
        string query = "select  distinct([Vehicle No_] ) as VehicleNo from TMS_Request where [Operational Status]='Sent' and [Vehicle No_] like '%" + VehicleNo + "%' and [Transporter Code] in ((select distinct [Transporter Code] from [TMS_User_Transporter_Mapping] where [Portal User ID]='" + UserName + "'))";
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
       // dtVehNums = GetData("select  distinct([Vehicle No_] ) as VehicleNo from TMS_Request where [Operational Status]='Sent' and [Vehicle No_] like '%" + VehicleNo + "%' and [Transporter Code] in ((select distinct [Transporter Code] from [TMS_User_Transporter_Mapping] where [Portal User ID]='" + UserName + "'))");
      //  return dtVehNums;
    }
    //[WebMethod]
    //public List<string> GetVehicleNumbers(string prefixText, int count)
    //{
    //    List<string> listVehicleNumbers = new List<string>();
    //    DataTable dtVehNums = new DataTable();
    //    dtVehNums = GetData("select  distinct([Vehicle No_] ) as VehicleNo from TMS_Request where [Operational Status]='Sent' and [Vehicle No_] like '%" + VehicleNo + "%' and [Transporter Code] in ((select distinct [Transporter Code] from [TMS_User_Transporter_Mapping] where [Portal User ID]='" + Session["UserName"].ToString() + "'))");
    //    listVehicleNumbers = dtVehNums.AsEnumerable().Select(r => r.Field<string>("VehicleNo")).ToList();
    //    return listVehicleNumbers;
    //}
    protected void ddlVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtVehicle.Text = ddlVehicle.SelectedItem.Text;

        DataTable dt = new DataTable();
        var vehicle = ddlVehicle.SelectedItem.Text;
        var vehicleNo = "%" + vehicle + "%";
        //var vehicle =  "%UP16Y5432%";
        // dt = GetData("select  ID as ID, [No_] as Requestno from TMS_Request   where [Vehicle No_] like '" + vehicleNo + "' and [Operational Status]=''Sent'' and [PlacedFromTransporter]<> ''Placed''  ");

        dt = GetData("select  ID as ID, [No_] as Requestno from TMS_Request   where [Vehicle No_] like '" + vehicleNo + "' and [Operational Status]='Sent' and [Transporter Response]='Accepted'  ");
        //[Operational Status]='Sent'  and [Transporter Response]='Accepted'
        //var z = dt.Rows[0].ItemArray[0].ToString();

        ddlreqno.DataSource = dt;
        ddlreqno.DataTextField = "Requestno";
        ddlreqno.DataValueField = "ID";
        ddlreqno.DataBind();
        //ddlreqno.Items.Insert(0, new ListItem(String.Empty, String.Empty));

        if (ddlreqno.SelectedItem.Text != "")
        {
            DataTable dtnameandcity = new DataTable();
            DataTable dtshipmenttype = GetData("select  [Shipment Type] as ShipmentType,Destination as Destination from TMS_Request   where [No_] = '" + ddlreqno.SelectedItem.Text + "'  ");
            if (dtshipmenttype.Rows[0].ItemArray[0].ToString() == "Sales")
            {
                dtnameandcity = GetData("select Name as Name,City as City  from TMS_Customer   where [No_] = '" + dtshipmenttype.Rows[0].ItemArray[1].ToString() + "'  ");
            }
            if (dtshipmenttype.Rows[0].ItemArray[0].ToString() == "Stock Transfer")
            {
                dtnameandcity = GetData("select Name as Name,City as City  from TMS_Location   where [Code] = '" + dtshipmenttype.Rows[0].ItemArray[1].ToString() + "'  ");
            }

            lbldestination.Text = dtshipmenttype.Rows[0].ItemArray[1].ToString();
            lblName.Text = dtnameandcity.Rows[0].ItemArray[0].ToString();
            lblCity.Text = dtnameandcity.Rows[0].ItemArray[1].ToString();

        }

        BindtransporterType();

    }
    protected void txtVehicle_TextChanged1(object sender, EventArgs e)
    {

        if (validatetruckNo(txtVehicle.Text.ToString()) == 0)
        {
            lblvehicleerr.Text = "Incorrect vehicle no";
            //lblvehicleerr.

        }
        else
        {
            lblvehicleerr.Text = "";
        }
    }

    protected void txtVehicle1_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtdemo_TextChanged(object sender, EventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox != null)
        {
            string theText = textBox.Text;
            FillVehicalDropDown(theText);
        }
    }

    public void FillVehicalDropDown(string vehicleNo)
    {
        DataTable dt = new DataTable();
        dt = GetData("select  distinct([Vehicle No_] ) from TMS_Request where [Operational Status]='Sent' and [Transporter Code] in ((select distinct [Transporter Code] from [TMS_User_Transporter_Mapping] where [Portal User ID]='" + UserName + "'))" + "and [Vehicle No_] like '%" +vehicleNo.Trim() + "%'");
        ddlVehicle.Items.Clear();
        ddlVehicle.DataSource = dt;
        ddlVehicle.DataTextField = "Vehicle No_";
        ddlVehicle.DataValueField = "Vehicle No_";
        ddlVehicle.DataBind();
        ddlVehicle.Items.Insert(0, new ListItem(String.Empty, String.Empty));
    }
}