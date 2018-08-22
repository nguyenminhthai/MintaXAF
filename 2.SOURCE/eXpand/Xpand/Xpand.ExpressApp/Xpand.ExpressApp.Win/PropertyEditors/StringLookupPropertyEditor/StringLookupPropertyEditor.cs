﻿using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace eXpand.ExpressApp.Win.PropertyEditors.StringLookupPropertyEditor
{
    public class StringLookupPropertyEditor : DXPropertyEditor ,IComplexPropertyEditor
    {
        private LookupEditorHelper helper;
        private List<ComboBoxItem> comboBoxItems;

        public StringLookupPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override void SetupRepositoryItem(RepositoryItem item)
        {
            ((RepositoryItemComboBox) item).Items.AddRange(ComboBoxItems);
            base.SetupRepositoryItem(item);
        }

        private List<ComboBoxItem> ComboBoxItems
        {
            get
            {
                if (comboBoxItems== null)
                {
                    var xpView = new XPView(helper.ObjectSpace.Session, ObjectType);
                    xpView.AddProperty(PropertyName, PropertyName, true);
                    comboBoxItems = new List<ComboBoxItem>();
                    foreach (ViewRecord record in xpView)
                        comboBoxItems.Add(new ComboBoxItem(record[0]));
                }
                return comboBoxItems;
            }
        }

        protected override object CreateControlCore()
        {
            return new ComboBoxEdit();
        }

        protected override RepositoryItem CreateRepositoryItem()
        {
            var repositoryItemComboBox = new RepositoryItemComboBox();
            repositoryItemComboBox.Items.AddRange(ComboBoxItems);
            return repositoryItemComboBox;
        }

        public void Setup(ObjectSpace objectSpace, XafApplication application)
        {
            if (helper == null)
                helper = new LookupEditorHelper(application, objectSpace, ObjectTypeInfo, Model);
            
            helper.SetObjectSpace(objectSpace);
        }
    }
}