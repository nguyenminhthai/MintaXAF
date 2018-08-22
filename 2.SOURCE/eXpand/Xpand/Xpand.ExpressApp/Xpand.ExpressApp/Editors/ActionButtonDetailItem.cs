﻿using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace Xpand.ExpressApp.Editors {
    
    public interface IModelActionButton : IModelViewItem
    {
        [DataSourceProperty("Application.ActionDesign.Actions")]
        [Required]
        IModelAction ActionId { get; set; }
        bool ShowInContainer { get; set; }
    }
    
    public abstract class ActionButtonDetailItem : ViewItem {
        private readonly IModelActionButton _model;

        protected ActionButtonDetailItem(Type objectType, string id) : base(objectType, id) {
        }

        protected ActionButtonDetailItem(IModelViewItem model, Type objectType)
            : base(objectType, model != null ? model.Id : string.Empty)
        {
            _model = (IModelActionButton) model;
        }

        public event EventHandler Executed;

        public IModelViewItem Model => _model;

        public override string Caption {
            get {
                return !(string.IsNullOrEmpty(_model.Caption)) ? _model.Caption : _model.ActionId.Caption;
            }
            set { throw new NotImplementedException(); }
        }

        protected void InvokeExecuted(EventArgs e) {
            EventHandler handler = Executed;
            handler?.Invoke(this, e);
        }
    }
}