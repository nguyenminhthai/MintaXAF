﻿using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using Xpand.ExpressApp.Attributes;
using Xpand.ExpressApp.Core;
using Xpand.ExpressApp.Model;

namespace Xpand.ExpressApp.NodeUpdaters {
    public class ModelListViewLinqColumnsNodesGeneratorUpdater : ModelNodesGeneratorUpdater<ModelListViewColumnsNodesGenerator> {
        public override void UpdateNode(ModelNode node) {
            var linqViewInfo = node.Parent as IModelListViewLinq;
            if (linqViewInfo != null && !string.IsNullOrEmpty(linqViewInfo.XPQueryMethod)) {
                var listViewInfo = (IModelListView)linqViewInfo;
                IEnumerable<CustomQueryPropertyAttribute> customQueryPropertyAttributes = LinqCollectionSourceHelper.GetQueryProperties(listViewInfo.ModelClass.TypeInfo.Type, linqViewInfo.XPQueryMethod);
                if (customQueryPropertyAttributes != null) {
                    AddColumnsNode(listViewInfo);
                    RemoveDefaultColumns(listViewInfo, customQueryPropertyAttributes);
                    AddQueryColumns(listViewInfo, customQueryPropertyAttributes);
                }
            }
        }

        void AddQueryColumns(IModelListView listViewInfo, IEnumerable<CustomQueryPropertyAttribute> customQueryPropertyAttributes) {
            foreach (CustomQueryPropertyAttribute queryPropertyAttribute in customQueryPropertyAttributes) {
                var col = listViewInfo.Columns[queryPropertyAttribute.Name];
                if (col == null) {
                    col = listViewInfo.Columns.AddNode<IModelColumn>(queryPropertyAttribute.Name);
                    col.PropertyName = queryPropertyAttribute.Name;
                }
            }
        }

        void RemoveDefaultColumns(IModelListView listViewInfo, IEnumerable<CustomQueryPropertyAttribute> customQueryPropertyAttributes) {
            for (int i = listViewInfo.Columns.Count; i > 0; ) {
                i--;
                IModelColumn col = listViewInfo.Columns[i];
                var names = customQueryPropertyAttributes.Select(attribute => attribute.Name).ToArray();
                if (Array.IndexOf(names, col.Id) < 0) {
                    col.Remove();
                }
            }
        }

        void AddColumnsNode(IModelListView listViewInfo) {
            if (listViewInfo.Columns == null) {
                listViewInfo.AddNode<IModelColumns>("Columns");
            }
        }
    }
}