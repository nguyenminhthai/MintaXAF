﻿using System;
using System.Drawing;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.Logic;

namespace Xpand.ExpressApp.AdditionalViewControlsProvider.Logic {
    public class AdditionalViewControlsRule : LogicRule, IAdditionalViewControlsRule {
        public AdditionalViewControlsRule(IContextAdditionalViewControlsRule additionalViewControlsRule)
            : base(additionalViewControlsRule) {
                Message = additionalViewControlsRule.Message;
                ControlType = additionalViewControlsRule.ControlType;
                DecoratorType = additionalViewControlsRule.DecoratorType;
                MessageProperty = additionalViewControlsRule.MessageProperty;
                Position = additionalViewControlsRule.Position;
                BackColor = additionalViewControlsRule.BackColor;
                ForeColor = additionalViewControlsRule.ForeColor;
                FontStyle = additionalViewControlsRule.FontStyle;
                Height = additionalViewControlsRule.Height;
                FontSize=additionalViewControlsRule.FontSize;
                ImageName = additionalViewControlsRule.ImageName;
            }
        #region IAdditionalViewControlsRule Members
        public string Message { get; set; }

        public Type ControlType { get; set; }

        public Type DecoratorType { get; set; }

        public string MessageProperty { get; set; }

        public Position Position { get; set; }

        public Color? BackColor { get; set; }

        public Color? ForeColor { get; set; }

        public FontStyle? FontStyle { get; set; }

        public int? Height { get; set; }

        public int? FontSize { get; set; }

        public string ImageName { get; set; }

        #endregion
    }
}