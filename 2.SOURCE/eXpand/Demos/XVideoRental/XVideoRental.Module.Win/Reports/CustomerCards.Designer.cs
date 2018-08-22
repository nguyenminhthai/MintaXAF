﻿namespace XVideoRental.Module.Win.Reports {
    partial class CustomerCards {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.xrTableCellAddress = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictBoxPhoto = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrTableCellPhone = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabelFullName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCellPhoneLabel = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRowAddressInfo = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCellBirthday = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRowBirthdayInfo = new DevExpress.XtraReports.UI.XRTableRow();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrBarCodeCustomerId = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrTableCellBirthdayLabel = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellAddressLabel = new DevExpress.XtraReports.UI.XRTableCell();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrTableRowPhoneInfo = new DevExpress.XtraReports.UI.XRTableRow();
            this.pageFooterBand1 = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPanelCustomerInfoCard = new DevExpress.XtraReports.UI.XRPanel();
            this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrTableCustomerInfo = new DevExpress.XtraReports.UI.XRTable();
            this.pageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabelDiscountLevel = new DevExpress.XtraReports.UI.XRLabel();
            this.collectionDataSource1 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableCustomerInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // xrTableCellAddress
            // 
            this.xrTableCellAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(240)))), ((int)(((byte)(206)))));
            this.xrTableCellAddress.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(240)))), ((int)(((byte)(206)))));
            this.xrTableCellAddress.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCellAddress.CanGrow = false;
            this.xrTableCellAddress.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Phone")});
            this.xrTableCellAddress.Dpi = 100F;
            this.xrTableCellAddress.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.xrTableCellAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(72)))), ((int)(((byte)(83)))));
            this.xrTableCellAddress.Multiline = true;
            this.xrTableCellAddress.Name = "xrTableCellAddress";
            this.xrTableCellAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 15, 0, 0, 100F);
            this.xrTableCellAddress.StylePriority.UseBackColor = false;
            this.xrTableCellAddress.StylePriority.UseBorderColor = false;
            this.xrTableCellAddress.StylePriority.UseBorders = false;
            this.xrTableCellAddress.StylePriority.UseFont = false;
            this.xrTableCellAddress.StylePriority.UseForeColor = false;
            this.xrTableCellAddress.StylePriority.UsePadding = false;
            this.xrTableCellAddress.StylePriority.UseTextAlignment = false;
            this.xrTableCellAddress.Text = "xrTableCellAddress";
            this.xrTableCellAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCellAddress.Weight = 2.2847959812285721D;
            // 
            // xrPictBoxPhoto
            // 
            this.xrPictBoxPhoto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(240)))), ((int)(((byte)(196)))));
            this.xrPictBoxPhoto.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(209)))), ((int)(((byte)(144)))));
            this.xrPictBoxPhoto.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPictBoxPhoto.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "Photo")});
            this.xrPictBoxPhoto.Dpi = 100F;
            this.xrPictBoxPhoto.LocationFloat = new DevExpress.Utils.PointFloat(15F, 15F);
            this.xrPictBoxPhoto.Name = "xrPictBoxPhoto";
            this.xrPictBoxPhoto.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 8, 8, 8, 100F);
            this.xrPictBoxPhoto.SizeF = new System.Drawing.SizeF(140F, 180F);
            this.xrPictBoxPhoto.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.xrPictBoxPhoto.StylePriority.UseBackColor = false;
            this.xrPictBoxPhoto.StylePriority.UseBorderColor = false;
            this.xrPictBoxPhoto.StylePriority.UseBorders = false;
            this.xrPictBoxPhoto.StylePriority.UsePadding = false;
            // 
            // PageInfo
            // 
            this.PageInfo.BackColor = System.Drawing.Color.Transparent;
            this.PageInfo.BorderColor = System.Drawing.Color.Black;
            this.PageInfo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PageInfo.BorderWidth = 1F;
            this.PageInfo.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.Black;
            this.PageInfo.Name = "PageInfo";
            // 
            // DataField
            // 
            this.DataField.BackColor = System.Drawing.Color.Transparent;
            this.DataField.BorderColor = System.Drawing.Color.Black;
            this.DataField.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DataField.BorderWidth = 1F;
            this.DataField.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.DataField.ForeColor = System.Drawing.Color.Black;
            this.DataField.Name = "DataField";
            this.DataField.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold);
            this.Title.ForeColor = System.Drawing.Color.Maroon;
            this.Title.Name = "Title";
            // 
            // xrTableCellPhone
            // 
            this.xrTableCellPhone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(222)))), ((int)(((byte)(182)))));
            this.xrTableCellPhone.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(240)))), ((int)(((byte)(206)))));
            this.xrTableCellPhone.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCellPhone.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Address")});
            this.xrTableCellPhone.Dpi = 100F;
            this.xrTableCellPhone.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.xrTableCellPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(72)))), ((int)(((byte)(83)))));
            this.xrTableCellPhone.Multiline = true;
            this.xrTableCellPhone.Name = "xrTableCellPhone";
            this.xrTableCellPhone.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 15, 0, 0, 100F);
            this.xrTableCellPhone.StylePriority.UseBackColor = false;
            this.xrTableCellPhone.StylePriority.UseBorderColor = false;
            this.xrTableCellPhone.StylePriority.UseBorders = false;
            this.xrTableCellPhone.StylePriority.UseFont = false;
            this.xrTableCellPhone.StylePriority.UseForeColor = false;
            this.xrTableCellPhone.StylePriority.UsePadding = false;
            this.xrTableCellPhone.StylePriority.UseTextAlignment = false;
            this.xrTableCellPhone.Text = "xrTableCellPhone";
            this.xrTableCellPhone.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCellPhone.Weight = 2.2847959812285721D;
            // 
            // xrLabelFullName
            // 
            this.xrLabelFullName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabelFullName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FullName")});
            this.xrLabelFullName.Dpi = 100F;
            this.xrLabelFullName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.xrLabelFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(72)))), ((int)(((byte)(83)))));
            this.xrLabelFullName.LocationFloat = new DevExpress.Utils.PointFloat(166F, 27F);
            this.xrLabelFullName.Name = "xrLabelFullName";
            this.xrLabelFullName.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0, 100F);
            this.xrLabelFullName.SizeF = new System.Drawing.SizeF(182.2916F, 25.70834F);
            this.xrLabelFullName.StylePriority.UseBorders = false;
            this.xrLabelFullName.StylePriority.UseFont = false;
            this.xrLabelFullName.StylePriority.UseForeColor = false;
            this.xrLabelFullName.StylePriority.UsePadding = false;
            this.xrLabelFullName.StylePriority.UseTextAlignment = false;
            this.xrLabelFullName.Text = "xrLabelFullName";
            this.xrLabelFullName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableCellPhoneLabel
            // 
            this.xrTableCellPhoneLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(222)))), ((int)(((byte)(182)))));
            this.xrTableCellPhoneLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCellPhoneLabel.Dpi = 100F;
            this.xrTableCellPhoneLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.xrTableCellPhoneLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(95)))));
            this.xrTableCellPhoneLabel.Name = "xrTableCellPhoneLabel";
            this.xrTableCellPhoneLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(11, 0, 5, 0, 100F);
            this.xrTableCellPhoneLabel.StylePriority.UseBackColor = false;
            this.xrTableCellPhoneLabel.StylePriority.UseBorders = false;
            this.xrTableCellPhoneLabel.StylePriority.UseFont = false;
            this.xrTableCellPhoneLabel.StylePriority.UseForeColor = false;
            this.xrTableCellPhoneLabel.StylePriority.UsePadding = false;
            this.xrTableCellPhoneLabel.StylePriority.UseTextAlignment = false;
            this.xrTableCellPhoneLabel.Text = "Address:";
            this.xrTableCellPhoneLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCellPhoneLabel.Weight = 0.7152040187714277D;
            // 
            // xrTableRowAddressInfo
            // 
            this.xrTableRowAddressInfo.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellAddressLabel,
            this.xrTableCellAddress});
            this.xrTableRowAddressInfo.Dpi = 100F;
            this.xrTableRowAddressInfo.Name = "xrTableRowAddressInfo";
            this.xrTableRowAddressInfo.Weight = 0.55990991683711044D;
            // 
            // xrTableCellBirthday
            // 
            this.xrTableCellBirthday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(222)))), ((int)(((byte)(182)))));
            this.xrTableCellBirthday.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(240)))), ((int)(((byte)(206)))));
            this.xrTableCellBirthday.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCellBirthday.CanGrow = false;
            this.xrTableCellBirthday.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Birthday", "{0:d}")});
            this.xrTableCellBirthday.Dpi = 100F;
            this.xrTableCellBirthday.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.xrTableCellBirthday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(72)))), ((int)(((byte)(83)))));
            this.xrTableCellBirthday.Multiline = true;
            this.xrTableCellBirthday.Name = "xrTableCellBirthday";
            this.xrTableCellBirthday.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 15, 0, 0, 100F);
            this.xrTableCellBirthday.StylePriority.UseBackColor = false;
            this.xrTableCellBirthday.StylePriority.UseBorderColor = false;
            this.xrTableCellBirthday.StylePriority.UseBorders = false;
            this.xrTableCellBirthday.StylePriority.UseFont = false;
            this.xrTableCellBirthday.StylePriority.UseForeColor = false;
            this.xrTableCellBirthday.StylePriority.UsePadding = false;
            this.xrTableCellBirthday.StylePriority.UseTextAlignment = false;
            this.xrTableCellBirthday.Text = "xrTableCellBirthday";
            this.xrTableCellBirthday.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCellBirthday.Weight = 2.2847959812285721D;
            // 
            // xrTableRowBirthdayInfo
            // 
            this.xrTableRowBirthdayInfo.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellBirthdayLabel,
            this.xrTableCellBirthday});
            this.xrTableRowBirthdayInfo.Dpi = 100F;
            this.xrTableRowBirthdayInfo.Name = "xrTableRowBirthdayInfo";
            this.xrTableRowBirthdayInfo.Weight = 0.55990991683711044D;
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Dpi = 100F;
            this.topMarginBand1.HeightF = 22.91667F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanelCustomerInfoCard});
            this.detailBand1.Dpi = 100F;
            this.detailBand1.HeightF = 224.1667F;
            this.detailBand1.Name = "detailBand1";
            this.detailBand1.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("FullName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // FieldCaption
            // 
            this.FieldCaption.BackColor = System.Drawing.Color.Transparent;
            this.FieldCaption.BorderColor = System.Drawing.Color.Black;
            this.FieldCaption.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.FieldCaption.BorderWidth = 1F;
            this.FieldCaption.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.FieldCaption.ForeColor = System.Drawing.Color.Maroon;
            this.FieldCaption.Name = "FieldCaption";
            // 
            // xrBarCodeCustomerId
            // 
            this.xrBarCodeCustomerId.AutoModule = true;
            this.xrBarCodeCustomerId.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCodeCustomerId.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CardNumber", "{0:d6}")});
            this.xrBarCodeCustomerId.Dpi = 100F;
            this.xrBarCodeCustomerId.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.xrBarCodeCustomerId.LocationFloat = new DevExpress.Utils.PointFloat(360F, 14.99999F);
            this.xrBarCodeCustomerId.Name = "xrBarCodeCustomerId";
            this.xrBarCodeCustomerId.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrBarCodeCustomerId.SizeF = new System.Drawing.SizeF(127F, 52F);
            this.xrBarCodeCustomerId.StylePriority.UseBorders = false;
            this.xrBarCodeCustomerId.StylePriority.UseFont = false;
            this.xrBarCodeCustomerId.StylePriority.UsePadding = false;
            this.xrBarCodeCustomerId.StylePriority.UseTextAlignment = false;
            this.xrBarCodeCustomerId.Symbology = code128Generator1;
            this.xrBarCodeCustomerId.Text = "xrBarCodeCustomerId";
            this.xrBarCodeCustomerId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTableCellBirthdayLabel
            // 
            this.xrTableCellBirthdayLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(222)))), ((int)(((byte)(182)))));
            this.xrTableCellBirthdayLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCellBirthdayLabel.CanGrow = false;
            this.xrTableCellBirthdayLabel.Dpi = 100F;
            this.xrTableCellBirthdayLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.xrTableCellBirthdayLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(95)))));
            this.xrTableCellBirthdayLabel.Name = "xrTableCellBirthdayLabel";
            this.xrTableCellBirthdayLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(11, 0, 0, 0, 100F);
            this.xrTableCellBirthdayLabel.StylePriority.UseBackColor = false;
            this.xrTableCellBirthdayLabel.StylePriority.UseBorders = false;
            this.xrTableCellBirthdayLabel.StylePriority.UseFont = false;
            this.xrTableCellBirthdayLabel.StylePriority.UseForeColor = false;
            this.xrTableCellBirthdayLabel.StylePriority.UsePadding = false;
            this.xrTableCellBirthdayLabel.StylePriority.UseTextAlignment = false;
            this.xrTableCellBirthdayLabel.Text = "Birthdate:";
            this.xrTableCellBirthdayLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCellBirthdayLabel.Weight = 0.7152040187714277D;
            // 
            // xrTableCellAddressLabel
            // 
            this.xrTableCellAddressLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(240)))), ((int)(((byte)(206)))));
            this.xrTableCellAddressLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCellAddressLabel.CanGrow = false;
            this.xrTableCellAddressLabel.Dpi = 100F;
            this.xrTableCellAddressLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.xrTableCellAddressLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(95)))));
            this.xrTableCellAddressLabel.Name = "xrTableCellAddressLabel";
            this.xrTableCellAddressLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(11, 0, 0, 0, 100F);
            this.xrTableCellAddressLabel.StylePriority.UseBackColor = false;
            this.xrTableCellAddressLabel.StylePriority.UseBorders = false;
            this.xrTableCellAddressLabel.StylePriority.UseFont = false;
            this.xrTableCellAddressLabel.StylePriority.UseForeColor = false;
            this.xrTableCellAddressLabel.StylePriority.UsePadding = false;
            this.xrTableCellAddressLabel.StylePriority.UseTextAlignment = false;
            this.xrTableCellAddressLabel.Text = "Phone";
            this.xrTableCellAddressLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCellAddressLabel.Weight = 0.7152040187714277D;
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Dpi = 100F;
            this.bottomMarginBand1.HeightF = 18.9167F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // xrTableRowPhoneInfo
            // 
            this.xrTableRowPhoneInfo.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellPhoneLabel,
            this.xrTableCellPhone});
            this.xrTableRowPhoneInfo.Dpi = 100F;
            this.xrTableRowPhoneInfo.Name = "xrTableRowPhoneInfo";
            this.xrTableRowPhoneInfo.Weight = 1.1198198336742209D;
            // 
            // pageFooterBand1
            // 
            this.pageFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pageInfo1,
            this.pageInfo2});
            this.pageFooterBand1.Dpi = 100F;
            this.pageFooterBand1.HeightF = 29F;
            this.pageFooterBand1.Name = "pageFooterBand1";
            // 
            // xrPanelCustomerInfoCard
            // 
            this.xrPanelCustomerInfoCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(240)))), ((int)(((byte)(206)))));
            this.xrPanelCustomerInfoCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(209)))), ((int)(((byte)(144)))));
            this.xrPanelCustomerInfoCard.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanelCustomerInfoCard.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelDiscountLevel,
            this.xrLabelFullName,
            this.xrPictBoxPhoto,
            this.xrTableCustomerInfo,
            this.xrBarCodeCustomerId});
            this.xrPanelCustomerInfoCard.Dpi = 100F;
            this.xrPanelCustomerInfoCard.LocationFloat = new DevExpress.Utils.PointFloat(76.04166F, 0F);
            this.xrPanelCustomerInfoCard.Name = "xrPanelCustomerInfoCard";
            this.xrPanelCustomerInfoCard.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0, 100F);
            this.xrPanelCustomerInfoCard.SizeF = new System.Drawing.SizeF(500F, 210F);
            this.xrPanelCustomerInfoCard.StylePriority.UseBackColor = false;
            this.xrPanelCustomerInfoCard.StylePriority.UseBorderColor = false;
            this.xrPanelCustomerInfoCard.StylePriority.UseBorders = false;
            this.xrPanelCustomerInfoCard.StylePriority.UsePadding = false;
            // 
            // pageInfo2
            // 
            this.pageInfo2.Dpi = 100F;
            this.pageInfo2.Format = "Page {0} of {1}";
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(331F, 6F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.pageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTableCustomerInfo
            // 
            this.xrTableCustomerInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(240)))), ((int)(((byte)(206)))));
            this.xrTableCustomerInfo.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCustomerInfo.Dpi = 100F;
            this.xrTableCustomerInfo.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.xrTableCustomerInfo.LocationFloat = new DevExpress.Utils.PointFloat(155F, 79.37502F);
            this.xrTableCustomerInfo.Name = "xrTableCustomerInfo";
            this.xrTableCustomerInfo.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRowBirthdayInfo,
            this.xrTableRowAddressInfo,
            this.xrTableRowPhoneInfo});
            this.xrTableCustomerInfo.SizeF = new System.Drawing.SizeF(343F, 100F);
            this.xrTableCustomerInfo.StylePriority.UseBackColor = false;
            this.xrTableCustomerInfo.StylePriority.UseBorders = false;
            this.xrTableCustomerInfo.StylePriority.UseFont = false;
            // 
            // pageInfo1
            // 
            this.pageInfo1.Dpi = 100F;
            this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(6F, 6F);
            this.pageInfo1.Name = "pageInfo1";
            this.pageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.pageInfo1.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.pageInfo1.StyleName = "PageInfo";
            // 
            // xrLabelDiscountLevel
            // 
            this.xrLabelDiscountLevel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabelDiscountLevel.CanGrow = false;
            this.xrLabelDiscountLevel.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DiscountLevel")});
            this.xrLabelDiscountLevel.Dpi = 100F;
            this.xrLabelDiscountLevel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.xrLabelDiscountLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(72)))), ((int)(((byte)(83)))));
            this.xrLabelDiscountLevel.LocationFloat = new DevExpress.Utils.PointFloat(166F, 53.0001F);
            this.xrLabelDiscountLevel.Name = "xrLabelDiscountLevel";
            this.xrLabelDiscountLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0, 100F);
            this.xrLabelDiscountLevel.SizeF = new System.Drawing.SizeF(130F, 18F);
            this.xrLabelDiscountLevel.StylePriority.UseBorders = false;
            this.xrLabelDiscountLevel.StylePriority.UseFont = false;
            this.xrLabelDiscountLevel.StylePriority.UseForeColor = false;
            this.xrLabelDiscountLevel.StylePriority.UsePadding = false;
            this.xrLabelDiscountLevel.Text = "xrLabelDiscountLevel";
            // 
            // collectionDataSource1
            // 
            this.collectionDataSource1.Name = "collectionDataSource1";
            this.collectionDataSource1.ObjectTypeName = "XVideoRental.Module.Win.BusinessObjects.Customer";
            this.collectionDataSource1.TopReturnedRecords = 0;
            // 
            // MovieInvetory
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.detailBand1,
            this.pageFooterBand1,
            this.topMarginBand1,
            this.bottomMarginBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.collectionDataSource1});
            this.DataSource = this.collectionDataSource1;
            this.DisplayName = "Customer Cards";
            this.Extensions.Add("DataSerializationExtension", "XafReport");
            this.Extensions.Add("DataEditorExtension", "XafReport");
            this.Extensions.Add("ParameterEditorExtension", "XafReport");
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 23, 19);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTableCustomerInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTableCell xrTableCellAddress;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictBoxPhoto;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfo;
        private DevExpress.XtraReports.UI.XRControlStyle DataField;
        private DevExpress.XtraReports.UI.XRControlStyle Title;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellPhone;
        private DevExpress.XtraReports.UI.XRLabel xrLabelFullName;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellPhoneLabel;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowAddressInfo;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellAddressLabel;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellBirthday;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowBirthdayInfo;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellBirthdayLabel;
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.DetailBand detailBand1;
        private DevExpress.XtraReports.UI.XRPanel xrPanelCustomerInfoCard;
        private DevExpress.XtraReports.UI.XRLabel xrLabelDiscountLevel;
        private DevExpress.XtraReports.UI.XRTable xrTableCustomerInfo;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowPhoneInfo;
        private DevExpress.XtraReports.UI.XRBarCode xrBarCodeCustomerId;
        private DevExpress.XtraReports.UI.XRControlStyle FieldCaption;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
        private DevExpress.XtraReports.UI.PageFooterBand pageFooterBand1;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo1;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo2;
        protected DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource1;
    }
}
