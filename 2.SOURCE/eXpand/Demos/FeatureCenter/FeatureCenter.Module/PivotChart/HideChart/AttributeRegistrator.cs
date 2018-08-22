﻿using System;
using System.Collections.Generic;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.General.Model;
using Xpand.Persistent.Base.ModelArtifact;

namespace FeatureCenter.Module.PivotChart.HideChart {
    public class AttributeRegistrator : Xpand.Persistent.Base.General.AttributeRegistrator {
        private const string DetailView = "HideChart_DetailView";
        public override IEnumerable<Attribute> GetAttributes(ITypeInfo typesInfo) {
            if (typesInfo.Type != typeof(Analysis)) yield break;
            yield return new CloneViewAttribute(CloneViewType.DetailView, DetailView);
            yield return new XpandNavigationItemAttribute("PivotChart/Hide Chart", DetailView) { ObjectKey = "Name='HideChart'" };
            yield return new DisplayFeatureModelAttribute(DetailView, new BinaryOperator("Name", "HideChart"));
            yield return new ActionStateRuleAttribute("Hide_save_and_close_for_" + DetailView, "SaveAndClose", "1=1", "1=1", ActionState.Hidden);
        }
    }
}
