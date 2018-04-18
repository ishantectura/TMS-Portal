using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayerClasses;
using System.IO;




public partial class MasterData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTable();
            BindCompanyInDropdown();

            txtCity.Visible = false;
            txtCode.Visible = false;
            txtName.Visible = false;
            lblCity.Visible = false;
            lblCode.Visible = false;
            lblName.Visible = false;
            btnGo.Visible = false;
            //Added by Jyothi
            btnExport.Visible = false;
        }




    }

    //Added by Jyothi
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    public void BindTable()
    {
        ListItemCollection TableSPCollection = new ListItemCollection();

        TableSPCollection.Add(new ListItem("", "0"));


        TableSPCollection.Add(new ListItem("Brand Master", "1"));
        TableSPCollection.Add(new ListItem("Cancellation Reason Code", "2"));
        TableSPCollection.Add(new ListItem("In-Transit Duration", "3"));

        TableSPCollection.Add(new ListItem("Customer Master", "4"));
        TableSPCollection.Add(new ListItem("Detention Setup", "5"));
        TableSPCollection.Add(new ListItem("Item Master", "6"));

        TableSPCollection.Add(new ListItem("Location Master", "7"));
        TableSPCollection.Add(new ListItem("Payment Discount Scheme", "8"));
        TableSPCollection.Add(new ListItem("Penalty Reason Code", "9"));

        TableSPCollection.Add(new ListItem("Placement Setup", "10"));
        TableSPCollection.Add(new ListItem("Rejection Reason Code", "11"));
        TableSPCollection.Add(new ListItem("Freight Master", "12"));


        TableSPCollection.Add(new ListItem("SKU Master", "13"));
        TableSPCollection.Add(new ListItem("Transit Loss Charges", "14"));
        TableSPCollection.Add(new ListItem("Unit of Measure", "15"));

        TableSPCollection.Add(new ListItem("User Transporter Mapping", "16"));
        TableSPCollection.Add(new ListItem("Vendor", "17"));
        TableSPCollection.Add(new ListItem("Whse Shipping Truck IN", "18"));
        TableSPCollection.Add(new ListItem("User", "19"));


        drpMastertable.DataSource = TableSPCollection;
        drpMastertable.DataBind();
    }

    protected void drpMastertable_TextChanged(object sender, EventArgs e)
    {

    }

    protected void drpMastertable_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtCity.Text = "";
        txtCode.Text = "";
        txtName.Text = "";
        txtCity.Visible = false;
        txtName.Visible = false;
        txtCode.Visible = false;
        lblCity.Visible = false;
        lblCode.Visible = false;
        lblName.Visible = false;
        btnGo.Visible = false;
        //Added by Jyothi
        btnExport.Visible = false;
        // string  str = drpMastertable.SelectedItem.ToString();
        BindCompanyInDropdown();
        ShowDataInGrid();//comment
    }

    protected void grdMasterTable_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ShowDataInGrid();
        grdMasterTable.PageIndex = e.NewPageIndex;

        ////rebind your gridview - GetSource(),Datasource of your GirdView
        ////grdMasterTable.DataSource = GetSource();
        grdMasterTable.DataBind();
    }

    protected void grdMasterTable_DataBound(object sender, EventArgs e)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        for (int i = 0; i < grdMasterTable.Columns.Count; i++)
        {
            if (i != 1 && i != 2)
            {
                TableHeaderCell cell = new TableHeaderCell();
                TextBox txtSearch = new TextBox();
                txtSearch.Attributes["placeholder"] = grdMasterTable.Columns[i].HeaderText;
                txtSearch.CssClass = "search_textbox";
                cell.Controls.Add(txtSearch);
                row.Controls.Add(cell);
            }
        }
        grdMasterTable.HeaderRow.Parent.Controls.AddAt(1, row);
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        ShowDataInGrid();
    }
    public void ShowDataInGrid()
    {
        DataTable dt;
        dt = new DataTable();
        switch (drpMastertable.SelectedItem.ToString())
        {
            case "Brand Master":

                Brand_Masters Brand_Masters = new Brand_Masters();
                
                IList<Brand_Master> ilistbrandMaster = Brand_Masters.GetBrand_Masters(txtCode.Text, txtName.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Brand_Master>(ilistbrandMaster);
                bindBrandMaster(dt);
                // grdMasterTable.DataSource = Brand_Masters.GetBrand_Masters(txtCode.Text, txtName.Text, drpSelectCompany.SelectedItem.Text);
                //grdMasterTable.DataBind();
                txtCode.Visible = true;
                txtName.Visible = true;
                lblName.Visible = true;
                lblCode.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Cancellation Reason Code":
                Cancellation_Reason_Codes Cancellation_Reason_Codes = new Cancellation_Reason_Codes();
                //DataTable dt = new DataTable();
                IList<Cancellation_Reason_Code> ilistCancelReasonCode = Cancellation_Reason_Codes.GetCancellation_Reason_Codes(txtCode.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Cancellation_Reason_Code>(ilistCancelReasonCode);
                bindCancellationReasonCode(dt);
                txtCode.Visible = true;
                lblCode.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Customer Master":
                Customers Customer = new Customers();
                IList<Customer> ilistCustomer = Customer.GetCustomers(txtName.Text, txtCity.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Customer>(ilistCustomer);
                bindCustomerMaster(dt);
                txtName.Visible = true;
                txtCity.Visible = true;
                lblName.Visible = true;
                lblCity.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Detention Setup":
                Detention_Setups DetentionSetup = new Detention_Setups();
                IList<Detention_Setup> ilistDetentionSetup = DetentionSetup.GetDetention_Setups(drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Detention_Setup>(ilistDetentionSetup);
                bindDetentionSetup(dt);
                break;
            case "In-Transit Duration":
                Transit_Durations Transit_Duration = new Transit_Durations();
                IList<Transit_Duration> ilistTransitDuration = Transit_Duration.GetTransit_Durations(drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Transit_Duration>(ilistTransitDuration);
                bindInTransitDuration(dt);

                break;


            case "Item Master":
                Items item = new Items();
                IList<Item> ilistItem = item.GetItems(drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Item>(ilistItem);
                bindItemMaster(dt);
                break;
            case "Location Master":
                Locations location = new Locations();
                IList<Location> ilistLocation = location.GetLocations(txtName.Text, txtCode.Text, txtCity.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Location>(ilistLocation);
                txtName.Visible = true;
                txtCode.Visible = true;
                txtCity.Visible = true;
                lblCity.Visible = true;
                lblCode.Visible = true;
                lblName.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Payment Discount Scheme":
                Payment_Discount_Schemes paymentDiscountScheme = new Payment_Discount_Schemes();
                IList<Payment_Discount_Scheme> ilistPaymentDisScheme = paymentDiscountScheme.GetPayment_Discount_Schemes(txtCode.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Payment_Discount_Scheme>(ilistPaymentDisScheme);
                bindPaymentDiscountScheme(dt);
                txtCode.Visible = true;
                lblCode.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Penalty Reason Code":
                Penalty_Reason_Codes penaltyReasonCode = new Penalty_Reason_Codes();
                IList<Penalty_Reason_Code> ilistPenaltyreasonCode = penaltyReasonCode.GetPenalty_Reason_Codes(txtCode.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Penalty_Reason_Code>(ilistPenaltyreasonCode);
                bindPaneltyReasonCode(dt);
                txtCode.Visible = true;
                lblCode.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Placement Setup":
                Placement_Setups placement = new Placement_Setups();
                IList<Placement_Setup> ilistPlacementSetup = placement.GetPlacement_Setups(drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Placement_Setup>(ilistPlacementSetup);
                bindPlacementSetup(dt);
                break;
            case "Rejection Reason Code":
                Rejection_Reason_Codes rejectionReasonCode = new Rejection_Reason_Codes();
                IList<Rejection_Reason_Code> ilistRejectionReasonCode = rejectionReasonCode.GetRejection_Reason_Codes(txtCode.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Rejection_Reason_Code>(ilistRejectionReasonCode);
                bindRejectionReasonCode(dt);
                txtCode.Visible = true;
                lblCode.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Freight Master":
                Shipping_Agent_Purch_PriceINs shippingAgent = new Shipping_Agent_Purch_PriceINs();
                IList<Shipping_Agent_Purch_PriceIN> ilistShipping_Agent_Purch_PriceIN= shippingAgent.GetShipping_Agent_Purch_PriceINs(drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Shipping_Agent_Purch_PriceIN>(ilistShipping_Agent_Purch_PriceIN);
                bindFreightMaster(dt);
                break;
            case "SKU Master":
                SKU_Masters skuMaster = new SKU_Masters();

                IList<SKU_Master> ilistSKU_Master = skuMaster.GetSKU_Masters(txtName.Text, txtCode.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<SKU_Master>(ilistSKU_Master);
                bindSKUMaster(dt);
                txtCode.Visible = true;
                txtName.Visible = true;
                lblCode.Visible = true;
                lblName.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Transit Loss Charges":
                Transit_Loss_Chargess transitLossCharge = new Transit_Loss_Chargess();
                IList<Transit_Loss_Charges> ilistTransit_Loss_Chargess = transitLossCharge.GetTransit_Loss_Chargess(drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Transit_Loss_Charges>(ilistTransit_Loss_Chargess);
                bindTransitLossCharge(dt);
                break;
            case "Unit of Measure":
                Unit_of_Measures unitMeasure = new Unit_of_Measures();
                IList<Unit_of_Measure> ilistUnit_of_Measure = unitMeasure.GetUnit_of_Measures(txtCode.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Unit_of_Measure>(ilistUnit_of_Measure);
                bindUnitOfMeasure(dt);
                txtCode.Visible = true;
                lblCode.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "User Transporter Mapping":
                User_Transporter_Mappings userTransporterMapping = new User_Transporter_Mappings();
                IList<User_Transporter_Mapping> ilistUser_Transporter_Mapping = userTransporterMapping.GetUser_Transporter_Mappings(drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<User_Transporter_Mapping>(ilistUser_Transporter_Mapping);
                bindUserTransporterMapping(dt);
                break;
            case "Vendor":
                Vendors Vendor = new Vendors();
                IList<Vendor> ilistVendor = Vendor.GetVendors(txtName.Text, txtCity.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Vendor>(ilistVendor);
                bindVendor(dt);
                txtName.Visible = true;
                txtCity.Visible = true;
                lblName.Visible = true;
                lblCity.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "Whse Shipping Truck IN":
                Whse_Shipping_Truck_INs whseShippingTruck = new Whse_Shipping_Truck_INs();
                IList<Whse_Shipping_Truck_IN> ilistWhseShippingTruck_IN = whseShippingTruck.GetWhse_Shipping_Truck_INs(txtCode.Text, drpSelectCompany.SelectedItem.Text);
                dt = ConvertTable.ToDataTable<Whse_Shipping_Truck_IN>(ilistWhseShippingTruck_IN);
                bindWhseShippingTruckIN(dt);
                txtCode.Visible = true;
                lblCode.Visible = true;
                btnGo.Visible = true;
                //Added by Jyothi
                btnExport.Visible = true;
                break;
            case "User":
                Users user = new Users();
                IList<User> ilistUsers = user.GetUsers();
                dt=ConvertTable.ToDataTable<User>(ilistUsers);
                bindUser(dt);
                break;
            default:
                break;
        }
    }

    

    //  Customise the  field Name on the masterdata form for master data  
    private void bindCancellationReasonCode (DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["Description"] != null)
        {
            dtValue.Columns["Description"].ColumnName = "Description";
        }
        if (dtValue.Columns["ChargeToTransporter"] != null)
        {
            dtValue.Columns["ChargeToTransporter"].ColumnName = "Charge to Transporter";
        }
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindBrandMaster(DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["Name"] != null)
        {
            dtValue.Columns["Name"].ColumnName = "Name";
        }
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindCustomerMaster(DataTable dtValue)
    {

        if (dtValue.Columns["Name"] != null)
        {
            dtValue.Columns["Name"].ColumnName = "Code";
        }
        if (dtValue.Columns["Name2"] != null)
        {
            dtValue.Columns["Name2"].ColumnName = "Name 2";
        }
        if (dtValue.Columns["No_"] != null)
        {
            dtValue.Columns["No_"].ColumnName = "No";
        }
        if (dtValue.Columns["Address"] != null)
        {
            dtValue.Columns["Address"].ColumnName = "Address";
        }
        if (dtValue.Columns["Address2"] != null)
        {
            dtValue.Columns["Address2"].ColumnName = "Address 2";
        }
        if (dtValue.Columns["City"] != null)
        {
            dtValue.Columns["City"].ColumnName = "City";
        }
        if (dtValue.Columns["Contact"] != null)
        {
            dtValue.Columns["Contact"].ColumnName = "Contact";
        }
        if (dtValue.Columns["PhoneNo_"] != null)
        {
            dtValue.Columns["PhoneNo_"].ColumnName = "Phone No";
        }
        if (dtValue.Columns["TelexNo_"] != null)
        {
            dtValue.Columns["TelexNo_"].ColumnName = "Telex No";
        }
        if (dtValue.Columns["LocationCode"] != null)
        {
            dtValue.Columns["LocationCode"].ColumnName = "Location Code";
        }
        if (dtValue.Columns["PostCode"] != null)
        {
            dtValue.Columns["PostCode"].ColumnName = "Post Code";
        }
        if (dtValue.Columns["County"] != null)
        {
            dtValue.Columns["County"].ColumnName = "County";
        }
        if (dtValue.Columns["EMail"] != null)
        {
            dtValue.Columns["EMail"].ColumnName = "EMail";
        }
        if (dtValue.Columns["State_Code"] != null)
        {
            dtValue.Columns["State_Code"].ColumnName = "State Code";
        }
        if (dtValue.Columns["Country_RegionCode"] != null)
        {
            dtValue.Columns["Country_RegionCode"].ColumnName = "Country Region Code";
        }
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindDetentionSetup(DataTable dtValue)
    {

        if (dtValue.Columns["BreweryLocation"] != null)
        {
            dtValue.Columns["BreweryLocation"].ColumnName = "Brewery Location";
        }
        if (dtValue.Columns["BreweryName"] != null)
        {
            dtValue.Columns["BreweryName"].ColumnName = "Brewery Name";
        }
        if (dtValue.Columns["DetentionType"] != null)
        {
            dtValue.Columns["DetentionType"].ColumnName = "Detention Type";
        }
        if (dtValue.Columns["TransporterCode"] != null)
        {
            dtValue.Columns["TransporterCode"].ColumnName = "Transporter Code";
        }
        if (dtValue.Columns["TruckSize"] != null)
        {
            dtValue.Columns["TruckSize"].ColumnName = "Truck Size";
        }
        if (dtValue.Columns["PostCodeType"] != null)
        {
            dtValue.Columns["PostCodeType"].ColumnName = "Post Code Type";
        }
        if (dtValue.Columns["TransporterName"] != null)
        {
            dtValue.Columns["TransporterName"].ColumnName = "Transporter Name";
        }
        if (dtValue.Columns["ValidFromDate"] != null)
        {
            dtValue.Columns["ValidFromDate"].ColumnName = "Valid From Date";
        }
        if (dtValue.Columns["PostCode"] != null)
        {
            dtValue.Columns["PostCode"].ColumnName = "Post Code";
        }
        if (dtValue.Columns["ValidToDate"] != null)
        {
            dtValue.Columns["ValidToDate"].ColumnName = "Valid To Date";
        }
        if (dtValue.Columns["Days"] != null)
        {
            dtValue.Columns["Days"].ColumnName = "Days";
        }
        if (dtValue.Columns["Charge"] != null)
        {
            dtValue.Columns["Charge"].ColumnName = "Charge";
        }
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindInTransitDuration(DataTable dtValue)
    {

        if (dtValue.Columns["BreweryLocation"] != null)
        {
            dtValue.Columns["BreweryLocation"].ColumnName = "Brewery Location";
        }
        if (dtValue.Columns["BreweryName"] != null)
        {
            dtValue.Columns["BreweryName"].ColumnName = "Brewery Name";
        }
        if (dtValue.Columns["Postcode"] != null)
        {
            dtValue.Columns["Postcode"].ColumnName = "Post Code";
        }
        if (dtValue.Columns["TransitDays"] != null)
        {
            dtValue.Columns["TransitDays"].ColumnName = "Transit Days";
        }
        
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindItemMaster(DataTable dtValue)
    {

        if (dtValue.Columns["No_"] != null)
        {
            dtValue.Columns["No_"].ColumnName = "No";
        }
        if (dtValue.Columns["No_2"] != null)
        {
            dtValue.Columns["No_2"].ColumnName = "No 2";
        }
        if (dtValue.Columns["Description"] != null)
        {
            dtValue.Columns["Description"].ColumnName = "Description";
        }
        if (dtValue.Columns["Description2"] != null)
        {
            dtValue.Columns["Description2"].ColumnName = "Description 2";
        }
        if (dtValue.Columns["BaseUnitofMeasure"] != null)
        {
            dtValue.Columns["BaseUnitofMeasure"].ColumnName = "Base Unit of Measure";
        }
        if (dtValue.Columns["UnitPrice"] != null)
        {
            dtValue.Columns["UnitPrice"].ColumnName = "Unit Price";
        }
        if (dtValue.Columns["UnitCost"] != null)
        {
            dtValue.Columns["UnitCost"].ColumnName = "Unit Cost";
        }
        if (dtValue.Columns["StandardCost"] != null)
        {
            dtValue.Columns["StandardCost"].ColumnName = "Standard Cost";
        }

        if (dtValue.Columns["LastDirectCost"] != null)
        {
            dtValue.Columns["LastDirectCost"].ColumnName = "Last Direct Cost";
        }
        if (dtValue.Columns["IndirectCost_"] != null)
        {
            dtValue.Columns["IndirectCost_"].ColumnName = "Indirect Cost";
        }
        if (dtValue.Columns["FGItem"] != null)
        {
            dtValue.Columns["FGItem"].ColumnName = "FG Item";
        }
        if (dtValue.Columns["ItemType"] != null)
        {
            dtValue.Columns["ItemType"].ColumnName = "Item Type";
        }
        if (dtValue.Columns["PackType"] != null)
        {
            dtValue.Columns["PackType"].ColumnName = "Pack Type";
        }

        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindLocationMaster(DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["Name"] != null)
        {
            dtValue.Columns["Name"].ColumnName = "Name";
        }
        if (dtValue.Columns["Name2"] != null)
        {
            dtValue.Columns["Name2"].ColumnName = "Name2";
        }
        if (dtValue.Columns["Address"] != null)
        {
            dtValue.Columns["Address"].ColumnName = "Address";
        }
        if (dtValue.Columns["Address2"] != null)
        {
            dtValue.Columns["Address2"].ColumnName = "Address 2";
        }
        if (dtValue.Columns["City"] != null)
        {
            dtValue.Columns["City"].ColumnName = "City";
        }
        if (dtValue.Columns["PhoneNo_"] != null)
        {
            dtValue.Columns["PhoneNo_"].ColumnName = "Phone No";
        }
        if (dtValue.Columns["PhoneNo_2"] != null)
        {
            dtValue.Columns["PhoneNo_2"].ColumnName = "Phone No 2";
        }

        if (dtValue.Columns["TelexNo_"] != null)
        {
            dtValue.Columns["TelexNo_"].ColumnName = "Telex No";
        }
        if (dtValue.Columns["FaxNo_"] != null)
        {
            dtValue.Columns["FaxNo_"].ColumnName = "Fax No";
        }
        if (dtValue.Columns["Contact"] != null)
        {
            dtValue.Columns["Contact"].ColumnName = "Contact";
        }
        if (dtValue.Columns["PostCode"] != null)
        {
            dtValue.Columns["PostCode"].ColumnName = "Post Code";
        }
        if (dtValue.Columns["County"] != null)
        {
            dtValue.Columns["County"].ColumnName = "County";
        }
        if (dtValue.Columns["EMail"] != null)
        {
            dtValue.Columns["EMail"].ColumnName = "EMail";
        }
        if (dtValue.Columns["HomePage"] != null)
        {
            dtValue.Columns["HomePage"].ColumnName = "Home Page";
        }
        if (dtValue.Columns["Country_RegionCode"] != null)
        {
            dtValue.Columns["Country_RegionCode"].ColumnName = "Country Region Code";
        }
        if (dtValue.Columns["LocationType"] != null)
        {
            dtValue.Columns["LocationType"].ColumnName = "LocationType";
        }
        if (dtValue.Columns["StateCode"] != null)
        {
            dtValue.Columns["StateCode"].ColumnName = "StateCode";
        }
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindPaymentDiscountScheme(DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["Type"] != null)
        {
            dtValue.Columns["Type"].ColumnName = "Type";
        }
        if (dtValue.Columns["TransporterCode"] != null)
        {
            dtValue.Columns["TransporterCode"].ColumnName = "Transporter Code";
        }
        if (dtValue.Columns["TransporterName"] != null)
        {
            dtValue.Columns["TransporterName"].ColumnName = "Transporter Name";
        }
        if (dtValue.Columns["ValidFrom"] != null)
        {
            dtValue.Columns["ValidFrom"].ColumnName = "Valid From";
        }
        if (dtValue.Columns["ValidTo"] != null)
        {
            dtValue.Columns["ValidTo"].ColumnName = "Valid To";
        }
        if (dtValue.Columns["NoOfDaystoAdvance"] != null)
        {
            dtValue.Columns["NoOfDaystoAdvance"].ColumnName = "No of Days to Advance";
        }
        if (dtValue.Columns["Deduction"] != null)
        {
            dtValue.Columns["Deduction"].ColumnName = "Deduction";
        }

        if (dtValue.Columns["SchemeDescription"] != null)
        {
            dtValue.Columns["SchemeDescription"].ColumnName = "Scheme Description";
        }
        
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindPaneltyReasonCode(DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["PenaltyAmount"] != null)
        {
            dtValue.Columns["PenaltyAmount"].ColumnName = "Penalty Amount";
        }
        

        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindPlacementSetup(DataTable dtValue)
    {

        if (dtValue.Columns["BreweryLocation"] != null)
        {
            dtValue.Columns["BreweryLocation"].ColumnName = "Brewery Location";
        }
        if (dtValue.Columns["Name"] != null)
        {
            dtValue.Columns["Name"].ColumnName = "Name";
        }
        if (dtValue.Columns["PostCode"] != null)
        {
            dtValue.Columns["PostCode"].ColumnName = "Post Code";
        }
        if (dtValue.Columns["City"] != null)
        {
            dtValue.Columns["City"].ColumnName = "City";
        }
        if (dtValue.Columns["Distance"] != null)
        {
            dtValue.Columns["Distance"].ColumnName = "Distance";
        }
        if (dtValue.Columns["PlacementTime"] != null)
        {
            dtValue.Columns["PlacementTime"].ColumnName = "Placement Time";
        }
        if (dtValue.Columns["PenaltyReasonCode"] != null)
        {
            dtValue.Columns["PenaltyReasonCode"].ColumnName = "Penalty Reason Code";
        }
       

        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindFreightMaster(DataTable dtValue)
    {

        if (dtValue.Columns["ShippingAgentCode"] != null)
        {
            dtValue.Columns["ShippingAgentCode"].ColumnName = "Shipping Agent Code";
        }
        if (dtValue.Columns["ShippingAgentServiceCode"] != null)
        {
            dtValue.Columns["ShippingAgentServiceCode"].ColumnName = "Shipping Agent Service Code";
        }
        if (dtValue.Columns["ShippingChargeType"] != null)
        {
            dtValue.Columns["ShippingChargeType"].ColumnName = "Shipping Charge Type";
        }
        if (dtValue.Columns["ShippingChargeNo_"] != null)
        {
            dtValue.Columns["ShippingChargeNo_"].ColumnName = "Shipping Charge No";
        }
        if (dtValue.Columns["ShippingChargePer"] != null)
        {
            dtValue.Columns["ShippingChargePer"].ColumnName = "Shipping Charge Per";
        }
        if (dtValue.Columns["StartingDate"] != null)
        {
            dtValue.Columns["StartingDate"].ColumnName = "Starting Date";
        }
        if (dtValue.Columns["CurrencyCode"] != null)
        {
            dtValue.Columns["CurrencyCode"].ColumnName = "Currency Code";
        }
        if (dtValue.Columns["EndingDate"] != null)
        {
            dtValue.Columns["EndingDate"].ColumnName = "Ending Date";
        }
        if (dtValue.Columns["MultiplybyDistance"] != null)
        {
            dtValue.Columns["MultiplybyDistance"].ColumnName = "Multiply by Distance";
        }
        if (dtValue.Columns["UnitCost"] != null)
        {
            dtValue.Columns["UnitCost"].ColumnName = "Unit Cost";
        }


        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindSKUMaster(DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["Name"] != null)
        {
            dtValue.Columns["Name"].ColumnName = "Name";
        }
        


        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindTransitLossCharge(DataTable dtValue)
    {

        if (dtValue.Columns["Type"] != null)
        {
            dtValue.Columns["Type"].ColumnName = "Type";
        }
        if (dtValue.Columns["TransporterCode"] != null)
        {
            dtValue.Columns["TransporterCode"].ColumnName = "Transporter Code";
        }
        if (dtValue.Columns["TransporterName"] != null)
        {
            dtValue.Columns["TransporterName"].ColumnName = "Transporter Name";
        }
        if (dtValue.Columns["SKU"] != null)
        {
            dtValue.Columns["SKU"].ColumnName = "SKU";
        }

        if (dtValue.Columns["PackDescription"] != null)
        {
            dtValue.Columns["PackDescription"].ColumnName = "Pack Description";
        }
        if (dtValue.Columns["ValidFromDate"] != null)
        {
            dtValue.Columns["ValidFromDate"].ColumnName = "Valid From Date";
        }

        if (dtValue.Columns["UnitofMeasure"] != null)
        {
            dtValue.Columns["UnitofMeasure"].ColumnName = "Unit of Measure";
        }
        if (dtValue.Columns["FromKM"] != null)
        {
            dtValue.Columns["FromKM"].ColumnName = "From KM";
        }

        if (dtValue.Columns["ToKM"] != null)
        {
            dtValue.Columns["ToKM"].ColumnName = "To KM";
        }
        if (dtValue.Columns["ValidToDate"] != null)
        {
            dtValue.Columns["ValidToDate"].ColumnName = "Valid To Date";
        }
        if (dtValue.Columns["Quantity"] != null)
        {
            dtValue.Columns["Quantity"].ColumnName = "Quantity";
        }
        if (dtValue.Columns["AllowanceQuantity"] != null)
        {
            dtValue.Columns["AllowanceQuantity"].ColumnName = "Allowance Quantity";
        }




        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindRejectionReasonCode(DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["Description"] != null)
        {
            dtValue.Columns["Description"].ColumnName = "Description";
        }
        
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindUnitOfMeasure(DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["Description"] != null)
        {
            dtValue.Columns["Description"].ColumnName = "Description";
        }

        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindUserTransporterMapping(DataTable dtValue)
    {

        if (dtValue.Columns["PortalUserID"] != null)
        {
            dtValue.Columns["PortalUserID"].ColumnName = "Portal User ID";
        }
        if (dtValue.Columns["TransporterCode"] != null)
        {
            dtValue.Columns["TransporterCode"].ColumnName = "Transporter Code";
        }

        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindUser(DataTable dtValue)
    {

        if (dtValue.Columns["PortalUserID"] != null)
        {
            dtValue.Columns["PortalUserID"].ColumnName = "Portal User ID";
        }
        if (dtValue.Columns["LicTermAggrementAccepted"] != null)
        {
            dtValue.Columns["LicTermAggrementAccepted"].ColumnName = "Lic Term Aggrement Accepted";
        }
        if (dtValue.Columns["AccessControll"] != null)
        {
            dtValue.Columns["AccessControll"].ColumnName = "Access Controll";
        }
        if (dtValue.Columns["UserType"] != null)
        {
            dtValue.Columns["UserType"].ColumnName = "User Type";
        }
        if (dtValue.Columns["EmailID"] != null)
        {
            dtValue.Columns["EmailID"].ColumnName = "Email ID";
        }
        //if (dtValue.Columns["Password"] != null)
        //{
        //    dtValue.Columns["Password"].ColumnName = "Password";
        //}
        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }
    private void bindVendor(DataTable dtValue)
    {

        if (dtValue.Columns["No_"] != null)
        {
            dtValue.Columns["No_"].ColumnName = "No";
        }
        if (dtValue.Columns["Name"] != null)
        {
            dtValue.Columns["Name"].ColumnName = "Name";
        }
        if (dtValue.Columns["Name2"] != null)
        {
            dtValue.Columns["Name2"].ColumnName = "Name 2";
        }
        if (dtValue.Columns["Address"] != null)
        {
            dtValue.Columns["Address"].ColumnName = "Address";
        }
        if (dtValue.Columns["Address2"] != null)
        {
            dtValue.Columns["Address2"].ColumnName = "Address 2";
        }
        if (dtValue.Columns["City"] != null)
        {
            dtValue.Columns["City"].ColumnName = "City";
        }
        if (dtValue.Columns["PhoneNo_"] != null)
        {
            dtValue.Columns["PhoneNo_"].ColumnName = "Phone No";
        }
        if (dtValue.Columns["PaymentTermsCode"] != null)
        {
            dtValue.Columns["PaymentTermsCode"].ColumnName = "Payment Terms Code";
        }
        if (dtValue.Columns["ShippingAgentCode"] != null)
        {
            dtValue.Columns["ShippingAgentCode"].ColumnName = "Shipping Agent Code";
        }
        if (dtValue.Columns["Country_RegionCode"] != null)
        {
            dtValue.Columns["Country_RegionCode"].ColumnName = "Country Region Code";
        }
        if (dtValue.Columns["PaytoVendorNo_"] != null)
        {
            dtValue.Columns["PaytoVendorNo_"].ColumnName = "Pay to Vendor No";
        }
        if (dtValue.Columns["PaymentMethodCode"] != null)
        {
            dtValue.Columns["PaymentMethodCode"].ColumnName = "Payment Method Code";
        }
        if (dtValue.Columns["FaxNo_"] != null)
        {
            dtValue.Columns["FaxNo_"].ColumnName = "Fax No";
        }
        if (dtValue.Columns["PostCode"] != null)
        {
            dtValue.Columns["PostCode"].ColumnName = "Post Code";
        }

        if (dtValue.Columns["County"] != null)
        {
            dtValue.Columns["County"].ColumnName = "County";
        }
        if (dtValue.Columns["EMail"] != null)
        {
            dtValue.Columns["EMail"].ColumnName = "EMail";
        }
        if (dtValue.Columns["HomePage"] != null)
        {
            dtValue.Columns["HomePage"].ColumnName = "Home Page";
        }
        if (dtValue.Columns["StateCode"] != null)
        {
            dtValue.Columns["StateCode"].ColumnName = "State Code";
        }
        if (dtValue.Columns["Transporter"] != null)
        {
            dtValue.Columns["Transporter"].ColumnName = "Transporter";
        }

        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }

    private void bindWhseShippingTruckIN(DataTable dtValue)
    {

        if (dtValue.Columns["Code"] != null)
        {
            dtValue.Columns["Code"].ColumnName = "Code";
        }
        if (dtValue.Columns["Description"] != null)
        {
            dtValue.Columns["Description"].ColumnName = "Description";
        }
        if (dtValue.Columns["TruckSize"] != null)
        {
            dtValue.Columns["TruckSize"].ColumnName = "Truck Size";
        }
        if (dtValue.Columns["MaximumWeight"] != null)
        {
            dtValue.Columns["MaximumWeight"].ColumnName = "Maximum Weight";
        }
        if (dtValue.Columns["MaximumCubage"] != null)
        {
            dtValue.Columns["MaximumCubage"].ColumnName = "Maximum Cubage";
        }
        

        grdMasterTable.DataSource = dtValue;
        grdMasterTable.DataBind();

        //Added by Jyothi
        grdMasterTableExcel.DataSource = dtValue;
        grdMasterTableExcel.DataBind();

    }

    protected void drpSelectCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowDataInGrid();
    }
    public void BindCompanyInDropdown()
    {
        DataTable dt = new DataTable();
        dt = GetData("SELECT '' as ID ,'' as NAME FROM [dbo].[TMS_Company] UNION SELECT  [ID] as ID  ,[Name] as NAME  FROM [dbo].[TMS_Company]");
        drpSelectCompany.DataSource = dt;
        drpSelectCompany.DataTextField = "NAME";
        drpSelectCompany.DataValueField = "ID";
        drpSelectCompany.DataBind();

    }

    
    /// <summary>
    /// Execute SQL Query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    private DataTable GetData(string query)
    {
        using (SqlConnection con = new SqlConnection(BusinessLayerClasses.ConnectionString.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                DataTable dt = new DataTable();
                con.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
    }

    //Added by Jyothi
    #region ExportToExcel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdMasterTableExcel.Visible = true;
        ExportGridToExcel(grdMasterTableExcel);
        grdMasterTableExcel.Visible = false;
    }

    private void ExportGridToExcel(GridView gridview)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "MasterData" + DateTime.Now + ".xls";
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