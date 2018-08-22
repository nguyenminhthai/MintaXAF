﻿using System;
using System.Diagnostics;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Xpand.ExpressApp;
using Xpand.ExpressApp.WorldCreator;
using Xpand.ExpressApp.WorldCreator.Core;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.BaseImpl.PersistentMetaData;
using Xpand.Persistent.BaseImpl.PersistentMetaData.PersistentAttributeInfos;

using Machine.Specifications;
using TypeMock.ArrangeActAssert;
using System.Linq;
using Xpand.Persistent.Base.PersistentMetaData;

namespace Xpand.Tests.Xpand.WorldCreator {
    //    public class With_Isolations
    //    {
    //        
    //
    //        public static string GetUniqueAssemblyName()
    //        {
    //            return "a"+Guid.NewGuid().ToString().Replace("-", "");
    //        }
    //        Establish context = () => {
    //            XafTypesInfo.Reset();
    //            XafTypesInfo.HardReset();
    //            var type1 = typeof(PersistentClassInfo);
    //            foreach (var type in type1.Assembly.GetTypes()) {
    //                if (type.Namespace.StartsWith(type1.Namespace)) {
    //                    XafTypesInfo.Instance.RegisterEntity(type);
    //                }
    //            }
    //        };
    //    }

    public abstract class with_classInfo_with_interfaceInfos<InterfaceType> : With_In_Memory_DataStore {
        protected static PersistentClassInfo _persistentClassInfo;

        Establish context = () => {
            _persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            _persistentClassInfo.PersistentAssemblyInfo = new PersistentAssemblyInfo(_persistentClassInfo.Session);
            _persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
            var interfaceInfos = _persistentClassInfo.Interfaces;
            var interfaceInfo = new InterfaceInfo(_persistentClassInfo.Session);
            Isolate.WhenCalled(() => interfaceInfo.Type).WillReturn(typeof(InterfaceType));
            interfaceInfos.Add(interfaceInfo);
        };

    }
    //    public abstract class with_TypesInfo
    //    {
    //        Establish context = () =>
    //        {
    //            Isolate.CleanUp();
    //            typesInfo = Isolate.Fake.Instance<TypesInfo>();
    //            Isolate.Swap.NextInstance<TypesInfo>().With(typesInfo);
    //            Isolate.WhenCalled(() => typesInfo.ExtendedCollectionMemberInfoType).WillReturn(typeof(ExtendedCollectionMemberInfo));
    //            Isolate.WhenCalled(() => typesInfo.ExtendedCoreMemberInfoType).WillReturn(typeof(ExtendedCoreTypeMemberInfo));
    //            Isolate.WhenCalled(() => typesInfo.ExtendedReferenceMemberInfoType).WillReturn(typeof(ExtendedReferenceMemberInfo));
    //            Isolate.WhenCalled(() => typesInfo.IntefaceInfoType).WillReturn(typeof(InterfaceInfo));
    //            Isolate.WhenCalled(() => typesInfo.PersistentTypesInfoType).WillReturn(typeof(PersistentClassInfo));
    //        };
    //
    //        protected static TypesInfo typesInfo;
    //    }

    //    public abstract class With_Types 
    //    {
    //        protected static TypesInfo TypesInfo;
    //
    //        Establish context = () =>
    //        {
    //            TypesInfo = Isolate.Fake.Instance<TypesInfo>();
    //            XafTypesInfo.Instance.RegisterEntity(typeof(PersistentRuleRequiredFieldAttribute));
    //            var typeInfos = XafTypesInfo.Instance.FindTypeInfo(typeof(IPersistentRuleRequiredFieldAttribute)).Implementors;
    //            Isolate.WhenCalled(() => TypesInfo.ExtendedCoreMemberInfoType).WillReturn(typeof(ExtendedCoreTypeMemberInfo));
    //            Isolate.WhenCalled(() => TypesInfo.ExtendedCoreMemberInfoType).WillReturn(typeof(ExtendedCoreTypeMemberInfo));
    //            Isolate.WhenCalled(() => TypesInfo.ExtendedCoreMemberInfoType).WillReturn(typeof(ExtendedCoreTypeMemberInfo));
    //            Isolate.WhenCalled(() => TypesInfo.ExtendedReferenceMemberInfoType).WillReturn(typeof(ExtendedReferenceMemberInfo));
    //            Isolate.WhenCalled(() => TypesInfo.ExtendedCollectionMemberInfoType).WillReturn(typeof(ExtendedCollectionMemberInfo));
    //        };
    //    }

    public abstract class With_DynamicCore_Property {
        protected static Type Type;
        protected static PropertyInfo PropertyInfo;
        protected static PersistentClassInfo ClassInfo;
        Establish context = () => {
            ClassInfo = new PersistentClassInfo(Session.DefaultSession) { Name = "TestClass" };
            ClassInfo.OwnMembers.Add(new PersistentCoreTypeMemberInfo(Session.DefaultSession) { Name = "TestProperty", DataType = DBColumnType.Boolean });
            //            Type = TypeDefineBuilder.Define(ClassInfo);
            PropertyInfo = Type.GetProperty("TestProperty");
        };


    }

    public abstract class With_In_Memory_DataStore : With_WC_types {
        protected static XPObjectSpace XPObjectSpace;
        protected static UnitOfWork UnitOfWork;
        Establish context = () => {
            XPObjectSpace = (XPObjectSpace)ObjectSpaceInMemory.CreateNew();
            UnitOfWork = (UnitOfWork)XPObjectSpace.Session;
        };

    }

    public abstract class With_Types_info {
        Establish context = () => {
            ReflectionHelper.Reset();
            XafTypesInfo.Reset();
            XafTypesInfo.HardReset();
            if (XafTypesInfo.PersistentEntityStore != null)
                ((XpoTypeInfoSource)XafTypesInfo.PersistentEntityStore).Reset();
            else {
                XafTypesInfo.SetPersistentEntityStore(new XpandXpoTypeInfoSource((TypesInfo)XafTypesInfo.Instance));
            }

//            XpandModuleBase.Dictiorary = ((XpoTypeInfoSource)XafTypesInfo.PersistentEntityStore).XPDictionary;
            foreach (var type in typeof(User).Assembly.GetTypes()) {
                XafTypesInfo.Instance.RegisterEntity(type);
            }

        };
    }
    public abstract class With_WC_types : With_Types_info {
        Establish context = () => {
            WCTypesInfo.Instance.Reset();
            var type1 = typeof(PersistentClassInfo);
            var types = type1.Assembly.GetTypes().Where(type => type.Namespace.StartsWith(type1.Namespace));
            foreach (var type in types) {
                WCTypesInfo.Instance.Register(type);
                XafTypesInfo.Instance.RegisterEntity(type);
            }
        };
    }

    //    public interface IXafTypesInfo {
    //        IIsolationAppNodeWrapper InitXafTypesInfo(Func<Assembly[]> assemblies);
    //    }

    public interface IIsolationView {
        IIsolationControllers CreateDetailView();
        IIsolationControllers CreateListView(Nesting nesting, Action<ListView> viewCreated);
    }

    //    public interface IIsolationAppNodeWrapper {
    //        IIsolationView WithApplicationNodeWrapper(Action<ApplicationNodeWrapper> withCreatedNodes);
    //    }

    public interface IIsolationControllers {
        void InitControllers(Action<ViewController> activated);
    }

    //    public class IsolationFactory<TObject> : IXafTypesInfo, IIsolationView, IIsolationAppNodeWrapper, IIsolationControllers where TObject : class
    //    {
    //        XPObjectSpace _XPObjectSpace;
    //        TObject _currentObject;
    //        XafApplication _xafApplication;
    //        Frame _frame;
    //        ApplicationNodeWrapper _applicationNodeWrapper;
    //        View _view;
    //
    //
    //        public IXafTypesInfo InitDataStore(Func<XPObjectSpaceProvider> func)
    //        {
    //            Isolate.CleanUp();
    //
    //            ReflectionHelper.Reset();
    //            XafTypesInfo.Reset(true);
    //
    //            var XPObjectSpaceProvider = func.Invoke()?? new XPObjectSpaceProvider(new MemoryDataStoreProvider());
    //            _XPObjectSpace =XPObjectSpaceProvider.CreateObjectSpace();
    //            return this;
    //        }
    //
    //    
    //
    //        IIsolationAppNodeWrapper IXafTypesInfo.InitXafTypesInfo(Func<Assembly[]> assemblies)
    //        {
    //            _xafApplication = Isolate.Fake.Instance<XafApplication>();
    //            XafTypesInfo.Instance.RegisterEntity(typeof(TObject));
    //
    //            XafTypesInfo.Instance.LoadTypes(typeof(SystemModule).Assembly);
    //            XafTypesInfo.Instance.LoadTypes(typeof(eXpandSystemModule).Assembly);
    //            foreach (var assembly in assemblies.Invoke()) {
    //                XafTypesInfo.Instance.LoadTypes(assembly);    
    //            }
    //            return this;
    //        }
    //
    //        IIsolationControllers IIsolationView.CreateDetailView()
    //        {
    //            _currentObject = (TObject)_XPObjectSpace.CreateObject(typeof(TObject));
    //            var detailView = new DetailView(_XPObjectSpace, CurrentObject, _xafApplication, true);
    //            Isolate.WhenCalled(detailView.SynchronizeWithInfo).IgnoreCall();
    //            detailView.SetInfo(_applicationNodeWrapper.Views.GetDetailViews(typeof(TObject))[0].Node);
    //
    //            return this;
    //        }
    //
    //        IIsolationView IIsolationAppNodeWrapper.WithApplicationNodeWrapper(Action<ApplicationNodeWrapper> withCreatedNodes) {
    //            _applicationNodeWrapper = new ApplicationNodeWrapper(new Dictionary(new DictionaryNode(ApplicationNodeWrapper.NodeName), Schema.GetCommonSchema()));
    //
    //            _applicationNodeWrapper.Load(typeof(TObject));
    //            withCreatedNodes.Invoke(_applicationNodeWrapper);
    //            return this;
    //        }
    //
    //        public IIsolationControllers CreateListView(Nesting nesting,Action<ListView> viewCreated)
    //        {
    //            ListViewInfoNodeWrapper listViewInfoNodeWrapper = _applicationNodeWrapper.Views.GetListViews(typeof(TObject))[0];
    //            var listEditor = Isolate.Fake.Instance<ListEditor>();
    //            Isolate.WhenCalled(() => listEditor.RequiredProperties).WillReturn(new string[0]);
    //            Isolate.WhenCalled(() => listEditor.Model).WillReturn(listViewInfoNodeWrapper);
    //            _view = new ListView(new CollectionSource(_XPObjectSpace, typeof(TObject)), listEditor, true, _xafApplication);
    //            Isolate.WhenCalled(() => _view.SynchronizeWithInfo()).IgnoreCall();
    //            _view.SetInfo(listViewInfoNodeWrapper.Node);
    //            viewCreated.Invoke((ListView) _view);
    //            return this;
    //        }
    //
    //        public void InitControllers(Action<ViewController> activated)
    //        {
    //            IList<Controller> collectControllers = new ControllersManager().CollectControllers(info => true);
    //            _frame = new Frame(_xafApplication, TemplateContext.View, collectControllers);
    //            Frame.SetView(_view);
    //
    //            IEnumerable<Controller> controllers = collectControllers.Where(controller => typeof(ViewController).IsAssignableFrom(controller.GetType()));
    //            foreach (ViewController controller in controllers)
    //            {
    //                ViewController controller1 = controller;
    //                Isolate.WhenCalled(() => controller1.Frame).WillReturn(Frame);
    //
    //                controller.Application = _xafApplication;
    //                if (!controller.Active) {
    //                    controller.SetView(_view);
    //                    activated.Invoke(controller);
    //                }
    //            }
    //        }
    //
    //        protected Frame Frame {
    //            get {
    //                return _frame;
    //            }
    //        }
    //
    //        protected TObject CurrentObject {
    //            get {
    //                return _currentObject;
    //            }
    //        }
    //    }

    //    public class MyClass {
    //        Establish context = () => {
    //            
    //            var isolationFactory = new IsolationFactory<InterfaceInfo>();
    //            RunControllers.ForListView(Nesting.Nested).WithArtifacts().WithXPObjectSpaceProvider()
    //            RunControllers.ForListView(Nesting.Nested).WithXPObjectSpaceProvider()
    //            RunControllers.ForListView(Nesting.Nested).WithArtifacts().WithXPObjectSpaceProvider()
    //            RunControllers.ForListView(Nesting.Nested).WithXPObjectSpaceProvider()
    //            isolationFactory.CreateListView(Nesting.Any, null);
    //            isolationFactory.InitControllers(null);            
    //            isolationFactory.InitDataStore(null).InitXafTypesInfo(null).WithApplicationNodeWrapper(null).CreateListView(Nesting.Any,
    //                                                                                                                        null).InitControllers(null);            
    //        };
    //        Because of = () => { };
    //        It should_should = () => { };
    //    }
    //    public abstract class With_In_Memory_DataStore1<TObject,TView>:With_In_Memory_DataStore where TObject:class where TView:View {
    //        protected static Frame Frame;
    //        protected static TObject CurrentObject;
    //
    //        Establish context = () =>
    //        {
    //            
    //            Type objectType = typeof(TObject);
    //            Type moduleType = typeof(WorldCreatorModule);
    //
    //            XafTypesInfo.Instance.RegisterEntity(typeof(TObject));
    //
    //            XafTypesInfo.Instance.LoadTypes(typeof(SystemModule).Assembly);
    //            XafTypesInfo.Instance.LoadTypes(typeof(eXpandSystemModule).Assembly);
    //            XafTypesInfo.Instance.LoadTypes(moduleType.Assembly);
    //            
    //            var applicationNodeWrapper = new ApplicationNodeWrapper(new Dictionary(new DictionaryNode(ApplicationNodeWrapper.NodeName), Schema.GetCommonSchema()));
    //            
    //            applicationNodeWrapper.Load(objectType);
    //
    //            var xafApplication = Isolate.Fake.Instance<XafApplication>();
    //
    //            View view;
    //            if (typeof(TView) == typeof(DetailView)) {
    //                CurrentObject = (TObject) XPObjectSpace.CreateObject(objectType);
    //                view = new DetailView(XPObjectSpace, CurrentObject, xafApplication, true);
    //                Isolate.WhenCalled(() => view.SynchronizeWithInfo()).IgnoreCall();
    //                view.SetInfo(applicationNodeWrapper.Views.GetDetailViews(objectType)[0].Node);
    //            }
    //            else {
    //                ListViewInfoNodeWrapper listViewInfoNodeWrapper = applicationNodeWrapper.Views.GetListViews(objectType)[0];
    //                var listEditor = Isolate.Fake.Instance<ListEditor>();
    //                Isolate.WhenCalled(() => listEditor.RequiredProperties).WillReturn(new string[0]);
    //                Isolate.WhenCalled(() => listEditor.Model).WillReturn(listViewInfoNodeWrapper);
    //                view = new ListView(new CollectionSource(XPObjectSpace, objectType),listEditor,  true,xafApplication);
    //                Isolate.WhenCalled(() => view.SynchronizeWithInfo()).IgnoreCall();
    //                view.SetInfo(listViewInfoNodeWrapper.Node);
    //            }
    //            IList<Controller> collectControllers = new ControllersManager().CollectControllers(info => true);
    //            Frame = new Frame(xafApplication,TemplateContext.View,collectControllers);
    //            Frame.SetView(view);
    //
    //            IEnumerable<Controller> controllers = collectControllers.Where(controller => typeof(ViewController).IsAssignableFrom(controller.GetType()));
    //            foreach (ViewController controller in controllers){
    //                ViewController controller1 = controller;
    //                Isolate.WhenCalled(() => controller1.Frame).WillReturn(Frame);
    //                
    //                controller.Application=xafApplication;
    //                if (!controller.Active)
    //                    controller.SetView(view);
    //            }
    //        };
    //
    //        Establish context = () => {
    //            Isolate.CleanUp();
    //            UnitOfWork.DefaultSession.Disconnect();
    //            ReflectionHelper.Reset();
    //            XafTypesInfo.Reset(true);
    //            var dataStore = new InMemoryDataStore( AutoCreateOption.DatabaseAndSchema);
    //
    //            XpoDefault.DataLayer = new SimpleDataLayer(XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary, dataStore);
    //            UnitOfWork = new UnitOfWork(UnitOfWork.DefaultSession.DataLayer);
    //            XPObjectSpace = new XPObjectSpace(UnitOfWork, XafTypesInfo.Instance);
    //        };
    //    }
    public abstract class With_DynamicReference_Property {
        protected static Type Type;

        protected static PropertyInfo PropertyInfo;

        Establish context = () => {
            var persistentClassInfo = new PersistentClassInfo(Session.DefaultSession) { Name = "TestClass" };
            persistentClassInfo.OwnMembers.Add(new PersistentReferenceMemberInfo(Session.DefaultSession) { Name = "TestProperty", ReferenceType = typeof(User) });

            //            Type = TypeDefineBuilder.Define(persistentClassInfo);
            PropertyInfo = Type.GetProperty("TestProperty");
        };
    }
    public abstract class With_DynamicCollection_Property {
        public static Type Type;
        protected static PropertyInfo PropertyInfo;

        Establish context = () => {
            var persistentClassInfo = new PersistentClassInfo(Session.DefaultSession) { Name = "TestClass" };
            persistentClassInfo.OwnMembers.Add(new PersistentCollectionMemberInfo(Session.DefaultSession) { Name = "TestProperty" });

            //            Type = TypeDefineBuilder.Define(persistentClassInfo);
            PropertyInfo = Type.GetProperty("TestProperty");
        };
    }


}
