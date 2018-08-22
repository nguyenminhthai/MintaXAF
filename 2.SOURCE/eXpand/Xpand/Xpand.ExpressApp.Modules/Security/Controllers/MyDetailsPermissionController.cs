﻿using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Xpand.ExpressApp.Security.Permissions;
using Xpand.Persistent.Base.Security;

namespace Xpand.ExpressApp.Security.Controllers {
    public class MyDetailsPermissionController : WindowController{
        MyDetailsController _myDetailsController;
        ShowNavigationItemController _showNavigationItemController;
        ChoiceActionItem _myDetailsItem;
        const string KeyDisable = "MyDetailsPermissionController";

        public MyDetailsPermissionController(){
            TargetWindowType=WindowType.Main;
        }

        protected override void OnActivated() {
            base.OnActivated();
            if (Application.Security.IsRemoteClient())
                return ;
            if (!SecuritySystem.IsGranted(new IsAdministratorPermissionRequest())) {
                var isGranted = !SecuritySystem.IsGranted(new MyDetailsOperationRequest(new MyDetailsPermission(Modifier.Deny)));
                _myDetailsController = Frame.GetController<MyDetailsController>();
                _myDetailsController?.Active.SetItemValue(KeyDisable, isGranted);
                _showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
                if (_showNavigationItemController != null) {
                    _myDetailsItem = FindMyDetailsItem(_showNavigationItemController.ShowNavigationItemAction.Items);
                    _myDetailsItem?.Active.SetItemValue(KeyDisable, isGranted);
                }
            }
            else {
                Active["IsAdmin"] = false;
            }
        }
        protected override void OnDeactivated() {
            _myDetailsController?.Active.RemoveItem(KeyDisable);
            _myDetailsItem?.Active.RemoveItem(KeyDisable);
            base.OnDeactivated();
        }
        private ChoiceActionItem FindMyDetailsItem(IEnumerable<ChoiceActionItem> items) {
            foreach (ChoiceActionItem item in items) {
                if (item.Id == DevExpress.ExpressApp.Security.MyDetailsController.MyDetailsNavigationItemId)
                    return item;
                ChoiceActionItem t = FindMyDetailsItem(item.Items);
                if (t != null)
                    return t;
            }
            return null;
        }

    }

}
