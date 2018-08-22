using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.ConditionalAppearance;
using System.Drawing;

namespace MintaXAF.Module.BusinessObjects.Base
{
    [NonPersistent(), OptimisticLocking(true)] // Thực hiện theo cơ chế OptimisticLocking   
    [ListViewFilter("All","",Index =0)]
    [DefaultProperty("Title")]
    [ListViewFilter("Available","[UseFlag]=true",true,Index =1)]
    [ListViewFilter("Unavailable", "[UseFlag]=false", Index = 2)]
    [Appearance("UseFlagColoredInListView",AppearanceItemType ="ViewItem",TargetItems ="*",
        Criteria ="UseFlag=false",Context ="ListView", FontStyle =FontStyle.Strikeout,FontColor ="ForestGreen",Priority =1)]
    public abstract class MintaBaseObject : BaseObject
    {
        protected const string EmailRegularExpression = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";

        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MintaBaseObject(Session session)
            : base(session)
        {
        }
        
        [Index(-1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public virtual string Title {
            get => GetPropertyValue<string>("Title");
            set => SetPropertyValue("Title", value);
        }

        [Index(99)]
        [CaptionsForBoolValues("Available","Unavailable")]
        [ImagesForBoolValues("State_Validation_Valid","State_Validation_Invalid")]
        [Browsable(false)]        
        public bool UseFlag {
            get => GetPropertyValue<bool>("UseFlag");
            set => SetPropertyValue("UseFlag", value);
        }

        // Audit change log
        private XPCollection<AuditDataItemPersistent> changeHistory;
        public XPCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                    changeHistory = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                return changeHistory;
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            UseFlag = true;
        }
        protected override void OnSaving()
        {            
            base.OnSaving();
          
        }
        protected override void OnSaved()
        {
            base.OnSaved();
            Session.Reload(this);
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
       
          
        }

        #region Action
        [Action(Caption ="Unuse",TargetObjectsCriteria ="UseFlag = true",ImageName ="State_Validation_Invalid",AutoCommit =true)]
        public void ActionAvaiable()
        {
            UseFlag = false;
        }
        [Action(Caption = "Use", TargetObjectsCriteria = "UseFlag = false", ImageName = "State_Validation_Valid", AutoCommit = true)]
        public void ActionUnvaiable()
        {
            UseFlag = true;
        }
        #endregion

    }
}