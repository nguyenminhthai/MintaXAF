using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base;
using MintaXAF.Module.Extension.FriendlyUrl;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MintaXAF.Module.Web.Extension.FriendlyUrl
{
    public class FriendlyUrlHttpRequestManager : DefaultHttpRequestManager
    {
        Dictionary<string, string> reductions;
        Dictionary<string, string> expansions;
        public FriendlyUrlHttpRequestManager()
        {
            reductions = new Dictionary<string, string>();
            expansions = new Dictionary<string, string>();
            RegisterShortcuts();
        }
        void RegisterShortcuts()
        {
            foreach (IModelView modelview in WebApplication.Instance.Model.Views)
            {
                Register((modelview as IModelViewFriendlyUrl).FriendlyUrl, new ViewShortcut(modelview.AsObjectView.ModelClass.TypeInfo.Type, IsListView(modelview.Id) ? null : "{0}", modelview.Id).ToString());
            }
        }

        private bool IsListView(string modeView) => modeView.EndsWith("_ListView");

        private bool IsDetailView(string modeView) => modeView.EndsWith("_DetailView");

        private bool IsNewObjectView(ViewShortcut currentShortcut) => currentShortcut.TryGetValue("NewObject", out string shortcutNewObject) && shortcutNewObject == true.ToString(CultureInfo.InvariantCulture).ToLower();

        string ObjectKey(ViewShortcut currentShortcut, IModelViewFriendlyUrl modelView)
        {
            if (!string.IsNullOrEmpty(currentShortcut.ObjectKey))
            {
                var objectByKey = GetObjectByKey(currentShortcut.ObjectKey, modelView.AsObjectView);
                return GetFriendlyObjectKey(modelView, objectByKey);
            }
            return null;
        }
        string GetFriendlyObjectKey(IModelViewFriendlyUrl modelView, object objectByKey)
        {
            if (modelView.AsObjectView != null && modelView.AsObjectView.ModelClass.TypeInfo.IsPersistent)
            {
                var friendlyKeyMember = ((IModelDetailViewFriendlyUrl)modelView).Url.ValueMemberName;
                var memberInfo = modelView.AsObjectView.ModelClass.FindMember(friendlyKeyMember).MemberInfo;
                if (objectByKey != null)
                {
                    var value = memberInfo.GetValue(objectByKey);
                    return value?.ToString();
                }
            }
            return null;
        }

        object GetObjectByKey(string objectKey, IModelObjectView modelObjectView)
        {
            if (modelObjectView != null && modelObjectView.ModelClass.TypeInfo.IsPersistent)
            {
                var typeInfo = modelObjectView.ModelClass.TypeInfo;
                var objectSpace = WebApplication.Instance.CreateObjectSpace(typeInfo.Type);
                var convert = ReflectionHelper.Convert(objectKey, typeInfo.KeyMember.MemberType);
                return objectSpace.GetObjectByKey(typeInfo.Type, convert);
            }
            return null;
        }

        string GetFriendlyObjectKey(string[] strings, string viewId) => WebApplication.Instance.Model.Views[viewId] is IModelDetailViewFriendlyUrl modelView ? GetObjectKey(modelView, strings) : null;

        private string GetObjectKey(IModelDetailViewFriendlyUrl modelView, string[] strings)
        {
            var objectSpace = WebApplication.Instance.CreateObjectSpace(modelView.ModelClass.TypeInfo.Type);
            var modelMember = modelView.ModelClass.FindMember(modelView.Url.ValueMemberName);
            var keyValue = GetValueQueryString(strings, modelMember.Name);
            var findObject = objectSpace.FindObject(modelView.ModelClass.TypeInfo.Type, CriteriaOperator.Parse(modelMember.Name + "=?", Convert.ChangeType(keyValue?.Value, modelMember.Type == typeof(Guid) ? typeof(String) : modelMember.Type)));
            return modelView.ModelClass.TypeInfo.KeyMember.GetValue(findObject).ToString();
        }

        dynamic GetValueQueryString(string[] strings, string key)
        {
            if (strings.Length == 1) return null;
            var query = strings[1].Split('&');
            if (query.Length > 0)
            {
                foreach (var q in query)
                {
                    if (q.Contains(key))
                    {
                        return new
                        {
                            Key = key,
                            Value = q.Split('=')[1]
                        };
                    }
                }
            }
            return null;
        }

        private void Register(string abbr, string shortcut)
        {
            reductions.Add(shortcut, abbr);
            expansions.Add(abbr, shortcut);
        }

        public override string GetQueryString(ViewShortcut viewShortcut)
        {
            if (viewShortcut != null && !IsNewObjectView(viewShortcut))
            {
                if (IsListView(viewShortcut.ViewId) && reductions.TryGetValue(viewShortcut.ToString(), out string str))
                    return str;
                if (IsDetailView(viewShortcut.ViewId))
                {
                    IModelViewFriendlyUrl modelView = (IModelViewFriendlyUrl)WebApplication.Instance.Model.Views[viewShortcut.ViewId];
                    var objectKey = ObjectKey(viewShortcut, modelView);
                    string strmode = viewShortcut.TryGetValue("mode", out string mode) ? string.Format("&mode={0}", mode) : "";
                    string strView = string.Empty;
                    if (objectKey != null)
                        strView = strmode == "" ? viewShortcut.ToString().Replace(objectKey, "{0}") : viewShortcut.ToString().Replace(objectKey, "{0}").Replace(strmode, "");
                    else
                        strView = strmode == "" ? viewShortcut.ToString() : viewShortcut.ToString().Replace(strmode, "");

                    if (reductions.TryGetValue(strView, out str))
                        return string.Format("{0}?{1}={2}{3}", str, ((IModelDetailViewFriendlyUrl)modelView).Url.ValueMemberName, objectKey, strmode);
                }
            }

            return base.GetQueryString(viewShortcut);
        }

        public override ViewShortcut GetViewShortcut(string queryString)
        {
            string shortcut;
            if (queryString != null && expansions.TryGetValue(queryString, out shortcut))
            {
                return ViewShortcut.FromString(shortcut);
            }
            return base.GetViewShortcut(queryString);
        }
    }
}
