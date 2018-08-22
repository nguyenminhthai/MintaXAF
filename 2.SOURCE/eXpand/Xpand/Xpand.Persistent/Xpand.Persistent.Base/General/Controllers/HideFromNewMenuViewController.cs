using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;

namespace Xpand.Persistent.Base.General.Controllers {
    public class HideFromNewMenuViewController : ViewController<ObjectView> {
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            foreach (ITypeInfo typeInfo in GetHiddenTypes()) {
                var attribute = View.ObjectTypeInfo.FindAttribute<HideFromNewMenuAttribute>();
                if (attribute != null) {
                    Frame.GetController<NewObjectViewController>(controller => {
                        var choiceActionItem = controller.NewObjectAction.Items.FirstOrDefault(item => ReferenceEquals(item.Data, typeInfo.Type));
                        controller.NewObjectAction.Items.Remove(choiceActionItem);
                    });
                }
            }
        }

        public List<ITypeInfo> GetHiddenTypes() {
            var objects = new List<ITypeInfo>();
            if (View.ObjectTypeInfo.FindAttribute<HideFromNewMenuAttribute>() != null)
                objects.Add(View.ObjectTypeInfo);
            objects.AddRange(View.ObjectTypeInfo.Descendants.Where(typeInfo => typeInfo.FindAttribute<HideFromNewMenuAttribute>() != null));
            return objects;
        }
    }
}
