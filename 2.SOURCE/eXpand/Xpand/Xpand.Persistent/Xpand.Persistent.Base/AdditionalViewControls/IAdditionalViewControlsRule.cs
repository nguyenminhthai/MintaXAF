﻿using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.Persistent.Base;
using Xpand.Persistent.Base.Logic;

namespace Xpand.Persistent.Base.AdditionalViewControls {
    public interface ISupportAppeareance {
        [Category("Appearance")]
        Color? BackColor { get; set; }

        [Category("Appearance")]
        Color? ForeColor { get; set; }

        [Category("Appearance")]
        FontStyle? FontStyle { get; set; }

        [Category("Appearance")]
        int? Height { get; set; }
        [Category("Appearance")]
        int? FontSize { get; set; }

        [Editor("DevExpress.ExpressApp.Win.Core.ModelEditor.ImageGalleryModelEditorControl, DevExpress.ExpressApp.Win"+AssemblyInfo.VSuffix, typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Appearance")]
        string ImageName { get; set; }
    }

    public interface IAdditionalViewControlsRule : ILogicRule, ISupportAppeareance {
        [Category("Data")]
        [Description("The type of the control to be added to the view")]
        [TypeConverter(typeof(StringToTypeConverter))]
        [Required]
        [DataSourceProperty("ControlTypes")]
        Type ControlType { get; set; }


        [Category("Data")]
        [Description("The type of the control that will be used to decorate the inserted control")]
        [TypeConverter(typeof(StringToTypeConverter))]
        [Required]
        [DataSourceProperty("DecoratorTypes")]
        Type DecoratorType { get; set; }


        [Category("Data")]
        [Description("The type of the control that will be used to decorate the inserted control")]
        string MessageProperty { get; set; }


        [Category("Data")]
        [Description("The type of the control that will be used to decorate the inserted control")]
        [Localizable(true)]
        string Message { get; set; }

        [Category("Behavior")]
        [Description("Specifies the position at which the control is to be inserted")]
        Position Position { get; set; }

    }
    public enum Position {
        Top,
        Bottom,
        DetailViewItem
    }

    
}