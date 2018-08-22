using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Fasterflect;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.Xpo;
using Xpand.Utils.Helpers;

namespace Xpand.ExpressApp.WorldCreator.System{
    public class WorldCreatorTypeInfoSource: XpandXpoTypeInfoSource{
        static readonly ReflectionDictionary _reflectionDictionary = new ReflectionDictionary();
        private static readonly Dictionary<ITypesInfo, WorldCreatorTypeInfoSource> _instances=new Dictionary<ITypesInfo, WorldCreatorTypeInfoSource>();

        public WorldCreatorTypeInfoSource(TypesInfo typesInfo) : this(typesInfo,Type.EmptyTypes){
        }

        public WorldCreatorTypeInfoSource(TypesInfo typesInfo, params Type[] types) : base(typesInfo,types){
        }

        public WorldCreatorTypeInfoSource(TypesInfo typesInfo, XPDictionary dictionary) : base(typesInfo, dictionary){
        }

        public static WorldCreatorTypeInfoSource Instance{
            get{
                Init();
                return _instances[XafTypesInfo.Instance];
            }
        }

        static void Init(){
            if (!_instances.ContainsKey(XafTypesInfo.Instance)) {
                var typesInfo = (TypesInfo) XafTypesInfo.Instance;
                var types = XpandModuleBase.BaseImplAssembly.GetTypes().Where(IsWorldCreatorType).ToArray();
                var entityStore = new WorldCreatorTypeInfoSource(typesInfo,types);
                var classInfo = entityStore.XPDictionary.GetClassInfo(typeof(XPObjectType));
                classInfo.RemoveAttribute(typeof(PersistentAttribute));
                classInfo.AddAttribute(new PersistentAttribute("PersistentClasses_XPObjectType"));
                typesInfo.AddEntityStore(entityStore);
                entityStore.EntityTypes.Each(XafTypesInfo.Instance.RegisterEntity);
                _instances.Add(XafTypesInfo.Instance, entityStore);
            }
        }

        private static bool IsWorldCreatorType(Type type){
            return type.FullName.StartsWith(WorldCreatorModule.BaseImplNameSpace) &&
                   _reflectionDictionary.CanGetClassInfoByType(type)||type==typeof(XPObjectType);
        }

        public void ForceRegisterEntity(Type item){
            XPDictionary.QueryClassInfo(item);
            ((HashSet<Type>) this.GetFieldValue("entityTypes")).Add(item);
            Instance.RegisterEntity(item);
        }

        public static void UseDefaultObjectTypePersistance(){
            var classInfo = Instance.XPDictionary.GetClassInfo(typeof(XPObjectType));
            classInfo.RemoveAttribute(typeof(PersistentAttribute));
        }
    }
}