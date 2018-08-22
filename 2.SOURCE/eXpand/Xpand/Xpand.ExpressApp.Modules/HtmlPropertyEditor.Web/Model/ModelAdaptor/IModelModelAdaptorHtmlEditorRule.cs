﻿using System;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using Xpand.Persistent.Base.ModelAdapter.Logic;

namespace Xpand.ExpressApp.HtmlPropertyEditor.Web.Model.ModelAdaptor {
    [ModelInterfaceImplementor(typeof(IModelAdaptorRule), "Attribute")]
    public interface IModelModelAdaptorHtmlEditorRule : IModelModelAdaptorRule, IModelHtmlEditor {
    }

    [DomainLogic(typeof(IModelModelAdaptorHtmlEditorRule))]
    public class ModelModelAdaptorHtmlEditorRuleDomainLogic {
        public static Type Get_RuleType(IModelModelAdaptorHtmlEditorRule modelListView) {
            return typeof(IModelAdaptorHtmlEditorRule);
        }
    }
}
