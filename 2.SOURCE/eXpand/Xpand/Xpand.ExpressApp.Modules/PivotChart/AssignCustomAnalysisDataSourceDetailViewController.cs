﻿using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.PivotChart;
using DevExpress.Persistent.Base;
using AnalysisViewControllerBase = Xpand.ExpressApp.PivotChart.Core.AnalysisViewControllerBase;

namespace Xpand.ExpressApp.PivotChart {
    public class AssignCustomAnalysisDataSourceDetailViewController : AnalysisViewControllerBase {
        readonly CriteriaOperator _criteriaOperator;
        public event EventHandler<AnalysisEditorArgs> DataSourceCreated;
        public event EventHandler<CriteriaOperatorArgs> ApplyingCollectionCriteria;
        public event EventHandler<AnalysisEditorArgs> DatasourceCreating;

        protected virtual void InvokeDatasourceCreating(AnalysisEditorArgs e){
            EventHandler<AnalysisEditorArgs> handler = DatasourceCreating;
            handler?.Invoke(this, e);
        }


        protected virtual void InvokeApplyingCollectionCriteria(CriteriaOperatorArgs e) {
            EventHandler<CriteriaOperatorArgs> handler = ApplyingCollectionCriteria;
            handler?.Invoke(this, e);
        }

        protected virtual void InvokeDataSourceCreated(AnalysisEditorArgs e){
            EventHandler<AnalysisEditorArgs> handler = DataSourceCreated;
            handler?.Invoke(this, e);
        }


        public AssignCustomAnalysisDataSourceDetailViewController() {
        }

        public AssignCustomAnalysisDataSourceDetailViewController(CriteriaOperator criteriaOperator) {
            _criteriaOperator = criteriaOperator;
        }

        protected override void OnActivated() {
            base.OnActivated();
            foreach (var analysisEditor in AnalysisEditors) {
                analysisEditor.DataSourceCreating += analysisEditor_DataSourceCreating;    
            }
        }

        void analysisEditor_DataSourceCreating(object sender, DataSourceCreatingEventArgs e) {
            CriteriaOperator userCriteria = null;
            
            if (e.AnalysisInfo != null) {
                if (!string.IsNullOrEmpty(e.AnalysisInfo.Criteria)) {
                    using (ObjectSpace.CreateParseCriteriaScope()) {
                        userCriteria = CriteriaWrapper.ParseCriteriaWithReadOnlyParameters(e.AnalysisInfo.Criteria, e.AnalysisInfo.DataType);
                    }
                }

                var analysisEditorBase = (AnalysisEditorBase)sender;
                var criteriaOperatorArgs = new CriteriaOperatorArgs(analysisEditorBase,e.AnalysisInfo);
                var analysisEditorArgs = new AnalysisEditorArgs(analysisEditorBase, e.AnalysisInfo);
                InvokeDatasourceCreating(analysisEditorArgs);
                InvokeApplyingCollectionCriteria(criteriaOperatorArgs);
                e.DataSource = analysisEditorArgs.Handled
                                   ? analysisEditorArgs.DataSource
                                   : ObjectSpace.CreateCollection(e.AnalysisInfo.DataType,
                                                                       userCriteria & criteriaOperatorArgs.Criteria &_criteriaOperator);
                e.Handled = true;
                InvokeDataSourceCreated(analysisEditorArgs);
            }            
        }


    }

    public class AnalysisEditorArgs : HandledEventArgs{
        public AnalysisEditorArgs(AnalysisEditorBase analysisEditorBase, IAnalysisInfo analysisInfo) {
            AnalysisEditorBase = analysisEditorBase;
            AnalysisInfo = analysisInfo;
        }

        public AnalysisEditorBase AnalysisEditorBase{ get; }

        public IAnalysisInfo AnalysisInfo{ get; }

        public object DataSource { get; set; }
    }

    public class CriteriaOperatorArgs : AnalysisEditorArgs {
        public CriteriaOperatorArgs(AnalysisEditorBase analysisEditorBase,IAnalysisInfo analysisInfo) : base(analysisEditorBase,analysisInfo) {
        }

        public CriteriaOperator Criteria { get; set; }
    }



}