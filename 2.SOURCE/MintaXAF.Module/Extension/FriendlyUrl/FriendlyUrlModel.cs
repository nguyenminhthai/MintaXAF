using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace MintaXAF.Module.Extension.FriendlyUrl
{
    #region Cấu hình Enable/Disable FriendlyUrl    
    public interface IModelOptionsFriendlyUrl
    {
        [Category("Minta")]
        [Description("Enable/Disable Chức năng FriendlyUrl")]
        bool EnableFriendlyUrl { get; set; }
    }
    #endregion

    #region Cấu hình FriendlyUrl cho ListView và DetailView
    public interface IFriendlyUrl
    {
        [Category("Minta.FriendlyUrl")]
        [ModelValueCalculator("Id")]
        [Required]
        [Description("Tên thay thế cho ListView hoặc DetailView.")]
        string FriendlyUrl { get; set; }
    }

    public class FriendlyUrlMemberValueCalculator : IModelValueCalculator
    {
        #region Implementation of IModelValueCalculator
        public object Calculate(ModelNode node, string propertyName)
        {
            var modelClass = ((IModelObjectView)node.Parent).ModelClass;
            var friendlyKeyProperty = modelClass.FriendlyKeyProperty;
            return friendlyKeyProperty != null
                       ? modelClass.FindMember(friendlyKeyProperty).Name
                       : (modelClass.KeyProperty != null ? modelClass.FindMember(modelClass.KeyProperty).Name : null);
        }
        #endregion
    }

    [ModelAbstractClass]
    public interface IModelViewFriendlyUrl : IModelView, IFriendlyUrl
    {
    }

    public interface IModelFriendlyUrl : IModelNode
    {
        [ModelValueCalculator(typeof(FriendlyUrlMemberValueCalculator))]
        [Required]
        [Category("Minta.FriendlyUrl")]
        [DataSourceProperty("AllMembers")]
        string ValueMemberName { get; set; }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<string> AllMembers { get; }
    }

    public interface IModelDetailViewFriendlyUrl : IModelDetailView, IModelViewFriendlyUrl
    {
        IModelFriendlyUrl Url { get; }
    }
    #endregion
}
