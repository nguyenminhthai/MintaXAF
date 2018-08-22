﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using Machine.Specifications;
using TypesInfo = DevExpress.ExpressApp.DC.TypesInfo;

namespace Xpand.Tests.Xpand.WorldCreator {
    [Subject("Xaf TypesInfo")]
    public class When_loading_types_from_Dynamic_Assemblies 
    {
        static DcAssemblyInfo findAssemblyInfo;
        static TypesInfo typesInfo;

        Establish context = () => {
            AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("TestAssembly"), AssemblyBuilderAccess.Run);
                        typesInfo = ((TypesInfo)XafTypesInfo.Instance);
                        findAssemblyInfo = typesInfo.FindAssemblyInfo(builder);
        };
        
        Because of = () => typesInfo.LoadTypes(findAssemblyInfo);

        It should_not_load_all_types = () => findAssemblyInfo.AllTypesLoaded.ShouldBeTrue();
    }
}