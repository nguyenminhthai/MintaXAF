﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Net.Mail;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Xpand.ExpressApp.Email.BusinessObjects;
using Xpand.Persistent.Base.Email;
using Xpand.Persistent.Base.Logic.Model;
using Xpand.Persistent.Base.Logic.NodeGenerators;

namespace Xpand.ExpressApp.Email.Model {
    public interface IModelLogicEmail : IModelLogicContexts{
        [DefaultValue(true)]
        bool CreateControllersOnLogon { get; set; }
        IModelEmailReceipients EmailReceipients { get; }
        IModelEmailTemplateContexts EmailTemplateContexts { get; }
        IModelEmailLogicRules Rules { get; }
        IModelEmailSmtpClientContexts SmtpClientContexts { get; }
    }

    [ModelNodesGenerator(typeof(LogicRulesNodesGenerator))]
    public interface IModelEmailLogicRules : IModelNode, IModelList<IModelEmailRule> {
    }

    [ModelInterfaceImplementor(typeof(IContextEmailRule), "Attribute")]
    public interface IModelEmailRule : IContextEmailRule, IModelConditionalLogicRule<IEmailRule>{
        [Browsable(false)]
        IEnumerable<string> SmtpClientContexts { get; }

        [Browsable(false)]
        IEnumerable<string> TemplateContexts { get; }
        
        [Browsable(false)]
        IEnumerable<string> EmailReceipientsContexts { get; }
    }

    public class ModelEmailRuleValidator : IModelNodeValidator<IModelEmailRule> {
        public bool IsValid(IModelEmailRule node, IModelApplication application) {
            return !string.IsNullOrEmpty(node.EmailReceipientsContext)||node.CurrentObjectEmailMember!=null;
        }
    }

    [DomainLogic(typeof(IModelEmailRule))]
    public class ModelEmailRuleDomainLogic {
        public static IEnumerable<string> Get_EmailReceipientsContexts(IModelEmailRule emailRule) {
            return (((IModelApplicationEmail) emailRule.Application).Email.EmailReceipients.Select(
                template => template.GetValue<string>("Id")));
        }
        public static IEnumerable<string> Get_SmtpClientContexts(IModelEmailRule emailRule) {
            return (((IModelApplicationEmail) emailRule.Application).Email.SmtpClientContexts.Select(
                template => template.GetValue<string>("Id")));
        }

        public static IEnumerable<string> Get_TemplateContexts(IModelEmailRule emailRule) {
            return (((IModelApplicationEmail) emailRule.Application).Email.EmailTemplateContexts.Select(
                template => template.GetValue<string>("Id")));
        }
    }

    [ModelNodesGenerator(typeof(ModelEmailSmtpClientContextsNodesGenerator))]
    public interface IModelEmailSmtpClientContexts : IModelList<IModelSmtpClientContext>,  IModelNode {
    }

    public class ModelEmailSmtpClientContextsNodesGenerator:ModelNodesGeneratorBase{
        protected override void GenerateNodesCore(ModelNode node){
            
        }
    }

    public interface IModelSmtpClientContext : IModelNode {
        [Category("SmtpClient"), ModelBrowsable(typeof(ModelSmtpClientDeliveryMethodVisibilityCalculator))]
        bool EnableSsl { get; set; }
        [Required(typeof(ModelSmtpClientDeliveryMethodRequiredCalculator)), Category("SmtpClient"),ModelBrowsable(typeof(ModelSmtpClientDeliveryMethodVisibilityCalculator))]
        string Host { get; set; }
        [Required(typeof(ModelSmtpClientDeliveryMethodRequiredCalculator)), Category("Credentials"), ModelBrowsable(typeof(ModelSmtpClientUseDefaultCredentialsVisibilityCalculator))]
        string Password { get; set; }

        [DefaultValue(25), Category("SmtpClient"),
         ModelBrowsable(typeof(ModelSmtpClientDeliveryMethodVisibilityCalculator))]
        int Port { get; set; }
        [RuleRegularExpression(null, DefaultContexts.Save, @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$"), Required]
        string SenderEmail { get; set; }
        [RuleRegularExpression(null, DefaultContexts.Save, @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$"), Required]
        [ModelValueCalculator("SenderEmail")]
        string ReplyToEmails { get; set; }
        [Category("Credentials"), ModelBrowsable(typeof(ModelSmtpClientDeliveryMethodVisibilityCalculator)), DefaultValue(true), RefreshProperties(RefreshProperties.All)]
        bool UseDefaultCredentials { get; set; }
        [Category("Credentials"), ModelBrowsable(typeof(ModelSmtpClientUseDefaultCredentialsVisibilityCalculator)), Required(typeof(ModelSmtpClientDeliveryMethodRequiredCalculator))]
        string UserName { get; set; }
        [Category("SmtpClient"),RefreshProperties(RefreshProperties.All)]
        SmtpDeliveryMethod DeliveryMethod { get; set; }
        [Category("SmtpClient")]
        string PickupDirectoryLocation { get; set; }
    }

    public class ModelSmtpClientDeliveryMethodRequiredCalculator:IModelIsRequired{
        public bool IsRequired(IModelNode node, string propertyName){
            return new ModelSmtpClientDeliveryMethodVisibilityCalculator().IsVisible(node, propertyName);
        }
    }

    public class ModelSmtpClientDeliveryMethodVisibilityCalculator:IModelIsVisible{
        public bool IsVisible(IModelNode node, string propertyName){
            return ((IModelSmtpClientContext) node).DeliveryMethod == SmtpDeliveryMethod.Network;
        }
    }

    public class ModelSmtpClientUseDefaultCredentialsVisibilityCalculator : IModelIsVisible {
        public bool IsVisible(IModelNode node, string propertyName) {
            return !((IModelSmtpClientContext) node).UseDefaultCredentials &&
                   new ModelSmtpClientDeliveryMethodVisibilityCalculator().IsVisible(node, propertyName);
        }
    }

    [ModelNodesGenerator(typeof(ModelEmailRecepientsNodesGenerator))]
    public interface IModelEmailReceipients : IModelList<IModelEmailReceipientGroup>, IModelNode {
    }

    public class ModelEmailRecepientsNodesGenerator:ModelNodesGeneratorBase{
        protected override void GenerateNodesCore(ModelNode node){
            
        }
    }

    public interface IModelEmailReceipientGroup : IModelNode, IModelList<IModelEmailReceipient>{
    }

    public interface IModelEmailReceipient : IModelNode {
        [Editor("DevExpress.ExpressApp.Win.Core.ModelEditor.CriteriaModelEditorControl, DevExpress.ExpressApp.Win"+AssemblyInfo.VSuffix
            , typeof(UITypeEditor)), CriteriaOptions("EmailReceipient.TypeInfo")]
        string Criteria { get; set; }
        [Required, DataSourceProperty("EmailMembers")]
        IModelMember EmailMember { get; set; }
        [Browsable(false)]
        IModelList<IModelMember> EmailMembers { get; }
        [DataSourceProperty("Application.BOModel"), Required]
        IModelClass EmailReceipient { get; set; }
        EmailType EmailType { get; set; }
    }

    public enum EmailType {
        Normal,
        CC,
        BCC
    }
    [DomainLogic(typeof(IModelEmailReceipient))]
    public class ModelEmailReceipientDomainLogic {
        public static IModelList<IModelMember> Get_EmailMembers(IModelEmailReceipient emailReceipient) {
            return ((emailReceipient.EmailReceipient != null) ? new CalculatedModelNodeList<IModelMember>(emailReceipient.EmailReceipient.AllMembers) : new CalculatedModelNodeList<IModelMember>());
        }
    }

    public interface IModelEmailTemplateContexts : IModelList<IModelEmailTemplate>, IModelNode {
    }

    public interface IModelEmailTemplate : IModelNode {
        [CriteriaOptions("EmailTemplate.TypeInfo"),
         Editor("DevExpress.ExpressApp.Win.Core.ModelEditor.CriteriaModelEditorControl, DevExpress.ExpressApp.Win"+AssemblyInfo.VSuffix
             , typeof (UITypeEditor))]
        string Criteria { get; set; }
        [DataSourceProperty("EmailTemplates"), Required]
        IModelClass EmailTemplate { get; set; }
        [Browsable(false)]
        IModelList<IModelClass> EmailTemplates { get; }
    }

    [DomainLogic(typeof(IModelEmailTemplate))]
    public class ModelEmailTemplateDomainLogic {
        public static IModelClass Get_EmailTemplate(IModelEmailTemplate modelEmailTemplate) {
            return modelEmailTemplate.Application.BOModel.First(@class => (@class.TypeInfo.Type == typeof(EmailTemplate)));
        }

        public static IModelList<IModelClass> Get_EmailTemplates(IModelEmailTemplate modelEmailTemplate) {
            return new CalculatedModelNodeList<IModelClass>(modelEmailTemplate.Application.BOModel.Where(
                    @class => typeof (IEmailTemplate).IsAssignableFrom(@class.TypeInfo.Type)));
        }
    }

}