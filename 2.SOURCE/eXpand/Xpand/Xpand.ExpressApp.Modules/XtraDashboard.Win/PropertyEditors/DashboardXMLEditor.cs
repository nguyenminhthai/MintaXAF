using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Xpand.ExpressApp.Win.PropertyEditors.RichEdit;
using Xpand.ExpressApp.Win.SystemModule.ModelAdapters;
using EditorAliases = Xpand.Persistent.Base.General.EditorAliases;

namespace Xpand.ExpressApp.XtraDashboard.Win.PropertyEditors{
    [PropertyEditor(typeof(string), EditorAliases.DashboardXMLEditor, false)]
    [RichEditPropertyEditor("xml", false, true,"ControlText")]
    public class DashboardXMLEditor:RichEditWinPropertyEditor{
        public DashboardXMLEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override object CreateControlCore(){
            var controlCore = base.CreateControlCore();
            ApplyMinimalConfiiguration((RichEditContainerBase) controlCore);
            return controlCore;
        }
    }
}