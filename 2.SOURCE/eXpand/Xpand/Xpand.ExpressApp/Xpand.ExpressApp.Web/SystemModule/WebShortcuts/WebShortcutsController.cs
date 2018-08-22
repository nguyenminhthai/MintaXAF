﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using Xpand.Persistent.Base.General;
using Xpand.Utils.Helpers;
using System.Text;

namespace Xpand.ExpressApp.Web.SystemModule.WebShortcuts {
    public interface IModelOptionsWebShortcut {
        IModelWebShortcut WebShortcut { get; }
    }

    public interface IModelWebShortcut:IModelNode {
        [DefaultValue(true)]
        bool Enabled { get; set; }
        [DefaultValue("Control+Alt+Shift+N")]
        string CtrlNReplacement { get; set; }
        [DefaultValue("Control+Alt+Shift+T")]
        string CtrlTReplacement { get; set; }
    }

    public class WebShortcutsController : WindowController, IXafCallbackHandler, IModelExtender {
        private const string KeybShortCutsScriptName = "KeybShortCuts";
        protected override void OnActivated(){
            base.OnActivated();
            var url = WebWindow.CurrentRequestPage.ClientScript.GetWebResourceUrl(GetType(), ResourceNames.jwerty);
            ((WebWindow)Frame).RegisterClientScriptInclude("Jwerty",url);
            var script = GetScript();
            if (!string.IsNullOrEmpty(script)) {
                ((WebWindow)Frame).RegisterStartupScript("ActionKeybShortCuts", script, true);
            }
        }

        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
            if (((IModelOptionsWebShortcut)Application.Model.Options).WebShortcut.Enabled) {
                Frame.TemplateChanged += FrameOnTemplateChanged;
                Frame.Disposing+=FrameOnDisposing;
            }
        }

        void FrameOnDisposing(object sender, EventArgs eventArgs) {
            Frame.Disposing-=FrameOnDisposing;
            Frame.TemplateChanged-=FrameOnTemplateChanged;
        }

        void FrameOnTemplateChanged(object sender, EventArgs eventArgs) {
            var callbackManagerHolder = Frame.Template as ICallbackManagerHolder;
            if (callbackManagerHolder?.CallbackManager != null) {
                callbackManagerHolder.CallbackManager.RegisterHandler(KeybShortCutsScriptName, this);
                ((WebWindow)Frame).PagePreRender += CallbackManagerOnPreRender;
            }
        }

        void CallbackManagerOnPreRender(object sender, EventArgs e) {
            ((WebWindow)Frame).PagePreRender -= CallbackManagerOnPreRender;
            
        }

        string GetScript() {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("if (!window.AttachedShortcuts) window.AttachedShortcuts = { }");
            var actions = Frame.Controllers.Cast<Controller>().SelectMany(controller => controller.Actions).Where(@base => @base.Enabled && @base.Active && !string.IsNullOrEmpty(@base.Shortcut));
            var script = actions.Select(ReplaceUnsupportedShortcuts).Select(GetScriptCore);
            sb.AppendLine(string.Join(Environment.NewLine, script));
            return sb.ToString();
        }

        Keys ShortcutToKeys(string str) {
            if (!string.IsNullOrEmpty(str)) {
                object value;
                if (typeof(Shortcut).EnumTryParse(str, out value))
                    return (Keys)value;
                if (typeof(Keys).EnumTryParse(str, out value))
                    return (Keys)value;
                if (str.Contains("+")) {
                    return str.Split('+')
                              .Aggregate(Keys.None, (current, item) => current | (Keys)Enum.Parse(typeof(Keys), item));
                }
            }
            return Keys.None;
        }

        KeyValuePair<ActionBase, string> ReplaceUnsupportedShortcuts(ActionBase actionBase) {
            var shortcutToKeys = ShortcutToKeys(actionBase.Shortcut);
            if (((shortcutToKeys & Keys.Control) == Keys.Control)) {
                var modelWebShortcut = ((IModelOptionsWebShortcut)Application.Model.Options).WebShortcut;
                if (((shortcutToKeys & Keys.N) == Keys.N)) {
                    shortcutToKeys = ShortcutToKeys(modelWebShortcut.CtrlNReplacement);
                } else if (((shortcutToKeys & Keys.T) == Keys.T)) {
                    shortcutToKeys = ShortcutToKeys(modelWebShortcut.CtrlTReplacement);
                }
            }
            return new KeyValuePair<ActionBase, string>(actionBase, KeyShortcut.GetKeyDisplayText(shortcutToKeys).Replace(KeyShortcut.ControlKeyName, "ctrl"));
        }

        string GetScriptCore(KeyValuePair<ActionBase, string> keyValuePair) {
            var xafCallbackManager = ((ICallbackManagerHolder)Frame.Template).CallbackManager;
            var script = xafCallbackManager.GetScript(KeybShortCutsScriptName, "'" + keyValuePair.Key.Id + "'");
            var scriptCore = string.Format(
                @"if (!window.AttachedShortcuts['{0}']) {{
                    jwerty.key('{0}', {1});
                    window.AttachedShortcuts['{0}'] = true;
                }}", 
                keyValuePair.Value, "function () { " + script + ";return false; }");
            return scriptCore;
        }

        public void ProcessAction(string parameter) {
            var actionBase = Frame.Controllers.Cast<Controller>().SelectMany(controller => controller.Actions).First(@base => @base.Id == parameter);
            actionBase.DoExecute();
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            extenders.Add<IModelOptions, IModelOptionsWebShortcut>();
        }
    }
}
