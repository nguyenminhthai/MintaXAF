﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Xpand.Utils.Helpers;

namespace Xpand.Persistent.Base.General{
    public static class MonoCecilExtensions{
        public static Assembly FindAssembly(this AppDomain appDomain,AssemblyDefinition assemblyDefinition){
            return appDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.FullName == assemblyDefinition.FullName);
        }

        public static Type FindType(this AppDomain appDomain, TypeDefinition typeDefinition){
            return appDomain.FindAssembly(typeDefinition).GetType(typeDefinition.FullName);
        }

        public static Assembly FindAssembly(this AppDomain appDomain,TypeDefinition typeDefinition){
            return appDomain.FindAssembly(typeDefinition.Module.Assembly);

        }
        static readonly ConcurrentDictionary<Assembly,AssemblyDefinition> AssemblyDefinitions=new ConcurrentDictionary<Assembly, AssemblyDefinition>();
        public static IEnumerable<AssemblyDefinition> ToAssemblyDefinition(this IEnumerable<Assembly> assemblies){
            return assemblies
                .Where(assembly =>!assembly.IsDynamic()&& File.Exists(assembly.Location))
                .Select(assembly => {
                    if (!AssemblyDefinitions.ContainsKey(assembly))
                        AssemblyDefinitions.TryAdd(assembly, AssemblyDefinition.ReadAssembly(assembly.Location));
                    return AssemblyDefinitions[assembly];
                });
        }

        /// <summary>
        ///     Is childTypeDef a subclass of parentTypeDef. Does not test interface inheritance
        /// </summary>
        /// <param name="childTypeDef"></param>
        /// <param name="parentTypeDef"></param>
        /// <returns></returns>
        public static bool IsSubclassOf(this TypeDefinition childTypeDef, TypeDefinition parentTypeDef){
            return childTypeDef.MetadataToken
                   != parentTypeDef.MetadataToken
                   && childTypeDef
                       .EnumerateBaseClasses()
                       .Any(b => b.MetadataToken == parentTypeDef.MetadataToken);
        }

        /// <summary>
        ///     Does childType inherit from parentInterface
        /// </summary>
        /// <param name="childType"></param>
        /// <param name="parentInterfaceDef"></param>
        /// <returns></returns>
        public static bool DoesAnySubTypeImplementInterface(this TypeDefinition childType,
            TypeDefinition parentInterfaceDef){
            Debug.Assert(parentInterfaceDef.IsInterface);
            return childType
                .EnumerateBaseClasses()
                .Any(typeDefinition => typeDefinition.DoesSpecificTypeImplementInterface(parentInterfaceDef));
        }

        /// <summary>
        ///     Does the childType directly inherit from parentInterface. Base
        ///     classes of childType are not tested
        /// </summary>
        /// <param name="childTypeDef"></param>
        /// <param name="parentInterfaceDef"></param>
        /// <returns></returns>
        public static bool DoesSpecificTypeImplementInterface(this TypeDefinition childTypeDef,
            TypeDefinition parentInterfaceDef){
            Debug.Assert(parentInterfaceDef.IsInterface);
            return childTypeDef
                .Interfaces
                .Any(ifaceDef => DoesSpecificInterfaceImplementInterface(ifaceDef.Resolve(), parentInterfaceDef));
        }

        /// <summary>
        ///     Does interface iface0 equal or implement interface iface1
        /// </summary>
        /// <param name="iface0"></param>
        /// <param name="iface1"></param>
        /// <returns></returns>
        public static bool DoesSpecificInterfaceImplementInterface(TypeDefinition iface0, TypeDefinition iface1){
            Debug.Assert(iface1.IsInterface);
            Debug.Assert(iface0.IsInterface);
            return iface0.MetadataToken == iface1.MetadataToken || iface0.DoesAnySubTypeImplementInterface(iface1);
        }

        /// <summary>
        ///     Is source type assignable to target type
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsAssignableFrom(this TypeDefinition target, TypeDefinition source){
            return target == source
                   || target.MetadataToken == source.MetadataToken
                   || source.IsSubclassOf(target)
                   || target.IsInterface && source.DoesAnySubTypeImplementInterface(target);
        }

        /// <summary>
        ///     Enumerate the current type, it's parent and all the way to the top type
        /// </summary>
        /// <param name="klassType"></param>
        /// <returns></returns>
        public static IEnumerable<TypeDefinition> EnumerateBaseClasses(this TypeDefinition klassType){
            for (var typeDefinition = klassType;
                typeDefinition != null;
                typeDefinition = typeDefinition.BaseType?.Resolve()) yield return typeDefinition;
        }
    }
}