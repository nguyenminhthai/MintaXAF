﻿using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using Fasterflect;
using Xpand.Persistent.Base.General;

namespace Xpand.ExpressApp.SystemModule {
    public interface IModelClassPreventDataLoading : IModelNode {
        [Category("eXpand.PreventDataLoading")]
        PreventDataLoading PreventDataLoading { get; set; }
        [Category("eXpand.PreventDataLoading")]
        bool PreventDataLoadingWhenFilterByText { get; set; }
    }

    public enum PreventDataLoading{
        Default,
        FiltersEmpty,
        FilterEmpty,
    }

    [ModelInterfaceImplementor(typeof(IModelClassPreventDataLoading), "ModelClass")]
    public interface IModelListViewPreventDataLoading : IModelClassPreventDataLoading {
    }

    public  class PreventDataLoadingController : ViewController<ListView>,IModelExtender {
        private FilterController _filterController;
        protected const string PreventDataLoadingKey = "PreventLoadingData";

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (_filterController != null){
                _filterController.FullTextFilterAction.Execute -= FullTextFilterAction_Execute;
                _filterController.SetFilterAction.Execute -= SetFilterActionOnExecute;
            }
        }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if (ShouldPreventLoading()) {
                PreventDataLoading();
            }
            Frame.GetController<FilterController>(controller => {
                _filterController = controller;
                _filterController.SetFilterAction.Execute += SetFilterActionOnExecute;
                _filterController.FullTextFilterAction.Execute += FullTextFilterAction_Execute;
            });
        }
        
        private void SetFilterActionOnExecute(object sender, SingleChoiceActionExecuteEventArgs e){
            if (ShouldPreventLoading()) 
                PreventDataLoading();
            else{
                DefaultLoading();
            }
        }

        public void PreventDataLoading(CriteriaOperator criteriaOperator){
            if (View != null && ((IModelListViewPreventDataLoading)View.Model).PreventDataLoading!=SystemModule.PreventDataLoading.Default) {
                if (ReferenceEquals(criteriaOperator, null)){
                    PreventDataLoading();
                }
                else{
                    DefaultLoading();
                }
            }
        }

        protected void PreventDataLoading(){
            View.CollectionSource.Criteria[PreventDataLoadingKey] = GetPreventLoadingDataCriteria();
        }

        bool ShouldPreventLoading(){
            var preventDataLoading = ((IModelListViewPreventDataLoading)View.Model);
            if (preventDataLoading.PreventDataLoading == SystemModule.PreventDataLoading.FilterEmpty)
                return string.IsNullOrEmpty(View.Model.Filter);
            if (preventDataLoading.PreventDataLoading == SystemModule.PreventDataLoading.FiltersEmpty)
                return ((IModelListViewFilter)View.Model).Filters.CurrentFilter == null;
            return false;
        }

        void FullTextFilterAction_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            if (((IModelListViewPreventDataLoading) View.Model).PreventDataLoadingWhenFilterByText)
                DefaultLoading();
        }

        private BinaryOperator GetPreventLoadingDataCriteria() {
            var memberInfo = View.ObjectTypeInfo.KeyMember;
            var memberType = memberInfo.MemberType;
            var o = memberType.IsValueType ? memberType.CreateInstance() : null;
            return new BinaryOperator(memberInfo.Name, o);
        }

        protected void DefaultLoading() {
            if (!ReferenceEquals(View.CollectionSource.Criteria[PreventDataLoadingKey], null))
                View.CollectionSource.Criteria[PreventDataLoadingKey] = null;
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders){
            extenders.Add<IModelClass, IModelClassPreventDataLoading>();
            extenders.Add<IModelListView, IModelListViewPreventDataLoading>();
        }
    }
}
