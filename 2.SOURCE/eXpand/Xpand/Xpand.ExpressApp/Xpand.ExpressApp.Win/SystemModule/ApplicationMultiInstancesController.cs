﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using Xpand.Persistent.Base.General.Model;

namespace Xpand.ExpressApp.Win.SystemModule {
    public interface IModelOptionsApplicationMultiInstances : IModelNode {
        [Category(AttributeCategoryNameProvider.Xpand)]
        [Description("If false only one application instance is allowed")]
        [DefaultValue(true)]
        bool ApplicationMultiInstances { get; set; }
    }
    public class ApplicationMultiInstancesController : WindowController, IModelExtender {
        public ApplicationMultiInstancesController() {
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated() {
            base.OnActivated();
            if (!((IModelOptionsApplicationMultiInstances)Application.Model.Options).ApplicationMultiInstances) {
                string processName = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 1) {
                    if (processes.FirstOrDefault(process => !process.Equals(Process.GetCurrentProcess())) != null) {
                        System.Windows.Forms.MessageBox.Show("Application is already running");
                        Environment.Exit(0);
                    }
                }
            }
        }

        #region IModelExtender Members

        void IModelExtender.ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            extenders.Add<IModelOptions, IModelOptionsApplicationMultiInstances>();
        }

        #endregion
    }
}