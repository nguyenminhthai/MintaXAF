﻿using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using Xpand.Persistent.Base.Logic;
using Xpand.Persistent.Base.Logic.Model;

namespace Xpand.Persistent.Base.ModelArtifact {
    public interface IObjectViewRule : ILogicRule {
        [DataSourceProperty("ObjectViews")]
        [Category("Data")]
        [Required]
        IModelObjectView ObjectView { get; set; }
    }

    public interface IContextObjectViewRule:IContextLogicRule,IObjectViewRule {
         
    }
}
