﻿using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;

namespace Xpand.ExpressApp.Win.PropertyEditors.NullAble.DoublePropertyEditor {
    [PropertyEditor(typeof(double),true)]
    [PropertyEditor(typeof(double?),true)]
    public class XpandDoublePropertyEditor : DevExpress.ExpressApp.Win.Editors.DoublePropertyEditor {
        public XpandDoublePropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) {
        }

        protected override void SetupRepositoryItem(RepositoryItem item) {
            base.SetupRepositoryItem(item);

            var integerEdit = (RepositoryItemDoubleEdit)item;
            integerEdit.Init(EditMask, DisplayFormat);
            var repositoryItemIntegerEdit = (RepositoryItemDoubleEdit)item;
            if (View != null) {
                Type type = MemberInfo.MemberType;
                bool b = type == typeof(double?);
                repositoryItemIntegerEdit.AllowNullInput =
                    b
                        ? DefaultBoolean.True
                        : DefaultBoolean.Default;
            }
        }
    }
}