﻿using System;
using System.Drawing;
using System.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Xpand.ExpressApp.Logic.Security;
using Xpand.Persistent.Base.AdditionalViewControls;

namespace Xpand.ExpressApp.AdditionalViewControlsProvider.Security {
    [NonPersistent]
    public class AdditionalViewControlsPermission : LogicRulePermission, IAdditionalViewControlsRule {
        [RuleRequiredField]
        public Type ControlType { get; set; }
        [RuleRequiredField]
        public Type DecoratorType { get; set; }

        #region IAdditionalViewControlsRule Members

        public string Message { get; set; }
        public string MessageProperty { get; set; }
        public Position Position { get; set; }
        public Color? BackColor { get; set; }
        public Color? ForeColor { get; set; }
        public FontStyle? FontStyle { get; set; }
        public int? Height { get; set; }
        public int? FontSize { get; set; }
        public string ImageName { get; set; }
        #endregion
        public override IPermission Copy() {
            return new AdditionalViewControlsPermission();
        }
    }
}