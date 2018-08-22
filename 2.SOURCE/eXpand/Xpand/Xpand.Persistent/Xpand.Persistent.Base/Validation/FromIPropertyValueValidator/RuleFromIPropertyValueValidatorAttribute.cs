﻿using System;
using DevExpress.Persistent.Validation;

namespace Xpand.Persistent.Base.Validation.FromIPropertyValueValidator{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RuleFromIPropertyValueValidatorAttribute : RuleBaseAttribute,
        IRuleFromIPropertyValueValidatorProperties{
        public RuleFromIPropertyValueValidatorAttribute(){
        }

        public RuleFromIPropertyValueValidatorAttribute(string id, string targetContextIDs)
            : base(id, targetContextIDs){
        }

        public RuleFromIPropertyValueValidatorAttribute(string id, DefaultContexts targetContexts)
            : base(id, targetContexts){
        }

        public RuleFromIPropertyValueValidatorAttribute(string id, string targetContextIDs, string messageTemplate)
            : base(id, targetContextIDs, messageTemplate){
        }

        public RuleFromIPropertyValueValidatorAttribute(string id, DefaultContexts targetContexts,
            string messageTemplate)
            : base(id, targetContexts, messageTemplate){
        }

        protected new RuleFromIPropertyValueValidatorProperties Properties
            => (RuleFromIPropertyValueValidatorProperties) base.Properties;

        protected override Type RuleType => typeof(RuleFromIPropertyValueValidator);

        protected override Type PropertiesType => typeof(RuleFromIPropertyValueValidatorProperties);

        #region IRuleFromIPropertyValueValidatorProperties Members

        string IRulePropertyValueProperties.TargetPropertyName{
            get { return Properties.TargetPropertyName; }

            set { Properties.TargetPropertyName = value; }
        }

        string IRuleFromIPropertyValueValidatorProperties.MessageTemplateInvalidPropertyValue{
            get { return Properties.MessageTemplateInvalidPropertyValue; }

            set { Properties.MessageTemplateInvalidPropertyValue = value; }
        }

        #endregion
    }
}