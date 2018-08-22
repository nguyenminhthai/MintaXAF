﻿using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;

namespace Xpand.ExpressApp.Win.PropertyEditors.NullAble.IntegerPropertyEditor {
    [PropertyEditor(typeof(int),true)]
    [PropertyEditor(typeof(int?),true)]
    public class XpandIntegerPropertyEditor : DevExpress.ExpressApp.Win.Editors.IntegerPropertyEditor {
        public XpandIntegerPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) {
        }

        protected override void SetupRepositoryItem(RepositoryItem item) {
            base.SetupRepositoryItem(item);

            var integerEdit = (RepositoryItemIntegerEdit)item;
            integerEdit.Init(EditMask, DisplayFormat);
            var repositoryItemIntegerEdit = (RepositoryItemIntegerEdit)item;
            if (View != null) {
                bool b = MemberInfo.MemberType == typeof(int?);
                repositoryItemIntegerEdit.AllowNullInput =
                    b
                        ? DefaultBoolean.True
                        : DefaultBoolean.Default;
            }
        }
    }
}