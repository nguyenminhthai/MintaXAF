﻿using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.PivotChart;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.XtraPivotGrid;

namespace Xpand.ExpressApp.PivotChart.Core {
    public class PivotGridFieldBuilder : DevExpress.ExpressApp.PivotChart.PivotGridFieldBuilder {
        IModelApplication _modelApplication;
        IModelMember _propertyModel;

        public PivotGridFieldBuilder(IAnalysisControl analysisControl)
            : base(analysisControl) {
        }

        public event EventHandler<SetupGridFieldArgs> SetupGridField;

        protected virtual void OnSetupGridField(SetupGridFieldArgs e) {
            EventHandler<SetupGridFieldArgs> handler = SetupGridField;
            if (handler != null) handler(this, e);
        }

        public new void SetModel(IModelApplication modelApplication) {
            _modelApplication = modelApplication;
            base.SetModel(modelApplication);
        }

        public override void SetupPivotGridField(IMemberInfo memberInfo) {
            _propertyModel = GetPropertyModel(memberInfo);
            base.SetupPivotGridField(memberInfo);
        }

        IModelMember GetPropertyModel(IMemberInfo memberInfo) {
            IModelMember result = null;
            if (_modelApplication != null && memberInfo != null) {
                var modelClass = _modelApplication.BOModel.GetClass(memberInfo.Owner.Type);
                if (modelClass != null) {
                    result = modelClass.FindOwnMember(memberInfo.Name);
                }
            }
            return result;
        }

        PivotGridFieldBase FindPivotGridField(string bindingPropertyName) {
            return Owner.Fields[bindingPropertyName];
        }

        public override void ApplySettings() {
            try {
                Owner.BeginUpdate();
                IAnalysisInfo analysisInfo = GetAnalysisInfo();
                if (analysisInfo != null) {
                    ITypeInfo objectTypeInfo = XafTypesInfo.Instance.FindTypeInfo(analysisInfo.DataType);
                    foreach (string propertyName in analysisInfo.DimensionProperties) {
                        IMemberInfo memberInfo = objectTypeInfo.FindMember(propertyName);
                        _propertyModel = GetPropertyModel(memberInfo);
                        if (memberInfo != null) {
                            PivotGridFieldBase field = FindPivotGridField(GetBindingName(memberInfo));
                            if (field != null) {
                                SetupPivotGridField(field, memberInfo.MemberType, GetMemberDisplayFormat(memberInfo));
                                field.Caption = CaptionHelper.GetFullMemberCaption(objectTypeInfo, propertyName);
                            }
                        }
                    }
                }
            }
            finally {
                Owner.EndUpdate();
            }
        }

        string GetMemberDisplayFormat(IMemberInfo memberInfo) {
            string result = "";
            IModelMember modelMember = GetPropertyModel(memberInfo);
            if (modelMember != null) {
                result = modelMember.DisplayFormat;
            }
            else {
                ModelDefaultAttribute displayFormatAttribute =
                    memberInfo.FindAttributes<ModelDefaultAttribute>().FirstOrDefault(
                        attribute => attribute.PropertyName == "DisplayFormat");
                if (displayFormatAttribute != null) {
                    result = displayFormatAttribute.PropertyValue;
                }
            }
            return result;
        }

        protected override void SetupPivotGridField(PivotGridFieldBase field, Type memberType, string displayFormat) {
            OnSetupGridField(new SetupGridFieldArgs(field, memberType, displayFormat));
            if (memberType == typeof (DateTime)) {
                if (_propertyModel != null)
                    field.GroupInterval = ((IModelMemberAnalysisDisplayDateTime) _propertyModel).PivotGroupInterval;
            }
            else
                base.SetupPivotGridField(field, memberType, displayFormat);
        }
    }
}