using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeCharm.Utility
{
    public static class SystemReflectionHelper
    {
        public static List<Type> GetAllLoadedClassesInAppDomainSubclassedFrom(Type baseType)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Module module in assembly.GetModules(false))
                {
                    try
                    {
                        Type[] classTypes = module.FindTypes(BaseTypeFilter, baseType);
                        types.AddRange(classTypes);
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        // ignore unloadable types
                    }
                }
            }
            return types;
        }

        public static bool BaseTypeFilter(Type t, object filterCriteria)
        {
            Type baseType = (Type) filterCriteria;
            try
            {
                return baseType.IsAssignableFrom(t);
            }
            catch
            {
                return false;
            }
        }
    }
}
