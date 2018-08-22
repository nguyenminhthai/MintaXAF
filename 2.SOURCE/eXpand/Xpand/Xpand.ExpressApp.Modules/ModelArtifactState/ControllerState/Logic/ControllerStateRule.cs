﻿using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model.Core;
using Xpand.ExpressApp.ModelArtifactState.ArtifactState.Logic;
using Xpand.Persistent.Base.ModelArtifact;

namespace Xpand.ExpressApp.ModelArtifactState.ControllerState.Logic{
    public class ControllerStateRule : ArtifactStateRule,IControllerStateRule{
        public ControllerStateRule(IContextControllerStateRule controllerStateRule): base(controllerStateRule){
            ControllerType = controllerStateRule.ControllerType;    
            ControllerState=controllerStateRule.ControllerState;    
        }

        [Category("Data"), TypeConverter(typeof(StringToTypeConverter))]
        public Type ControllerType { get; set; }

        [Category("Behavior")]
        public Persistent.Base.ModelArtifact.ControllerState ControllerState { get; set; }
    }
}
