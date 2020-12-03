using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.Core.Helpers
{
    class TypeHelpers
    {
        /// <summary>
        /// Gets type from full name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns>Type or null</returns>
        public static Type GetType(string typeName)
        {
            if(string.IsNullOrWhiteSpace(typeName))
                return null;

            var type = Type.GetType(typeName);
            if(type != null)
                return type;
            foreach(var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName, false, true);
                if(type != null)
                    return type;
            }
            return null;
        }

        /// <summary>
        /// Gets all instantiatable types that derive from <typeparamref name="T"/>, from all assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetChildTypesOf<T>()
        {
            return GetChildTypesOf<T>(AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Gets all instantiatable types that derive from <typeparamref name="T"/>, from given assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetChildTypesOf<T>(params Assembly[] assemblies)
        {
            return GetChildTypesOf<T>(assemblies?.SelectMany(x => x.GetTypes()));
        }

        /// <summary>
        /// Gets all instantiatable types that derive from <typeparamref name="T"/>, from a IEnumerable of types
        /// </summary>
        /// <typeparam name="T">Type to search for children of</typeparam>
        /// <param name="types">Types to search in</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetChildTypesOf<T>(IEnumerable<Type> types)
        {
            return types?.Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract) ?? null;
        }

        public static Dictionary<Type, TAttribute> GetChildTypesOfWithAttribute<TParent, TAttribute>(
            IEnumerable<Type> types) where TAttribute : Attribute
        {
            Dictionary<Type, TAttribute> dict = new Dictionary<Type, TAttribute>();

            var tp = types?.Where(x => typeof(TParent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract) ?? null;
            foreach(var t in tp)
                dict[t] = t.GetCustomAttribute<TAttribute>();

            return dict;
        }

        /// <summary>
        /// Gets all instantiatable types that derive from <paramref name="type"/>, from all assemblies
        /// </summary>
        /// <param name="type">Type to search for children of</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetChildTypesOf(Type type)
        {
            return GetChildTypesOf(type, AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Gets all instantiatable types that derive from T, from given assemblies
        /// </summary>
        /// <param name="type">Type to search for children of</param>
        /// <param name="assemblies">Assemblies to search in</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetChildTypesOf(Type type, params Assembly[] assemblies)
        {
            return GetChildTypesOf(type, assemblies?.SelectMany(x => x.GetTypes()));
        }

        /// <summary>
        /// Gets all instantiatable types that derive from <paramref name="type"/>, from <paramref name="types"/>
        /// </summary>
        /// <param name="type">Type to search for children of</param>
        /// <param name="types">Types to search in</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetChildTypesOf(Type type, IEnumerable<Type> types)
        {
            return types?.Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract) ?? null;
        }

        /// <summary>
        /// Returns all instantiatable types with attribute from assemblies
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesWithAttribute<T>(params Assembly[] assemblies) where T : Attribute
        {
            return assemblies.SelectMany(x => x.GetTypes())
            .Where(x => x.IsDefined(typeof(T)) && !x.IsInterface && !x.IsAbstract) ?? null;
        }

        /// <summary>
        /// Returns all instantiatable types with attribute from all assemblies
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesWithAttribute<T>() where T : Attribute
        {
            return GetTypesWithAttribute<T>(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()));
        }

        public static Dictionary<Type, T> GetTypesAndAttributesWithAttribute<T>() where T : Attribute
        {
            return GetTypesAndAttributesWithAttribute<T>(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static Dictionary<Type, T> GetTypesAndAttributesWithAttribute<T>(params Assembly[] assemblies) where T : Attribute
        {
            var types = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsDefined(typeof(T)) && !x.IsInterface && !x.IsAbstract) ?? null;
            return GetTypesAndAttributesWithAttribute<T>(types);
        }

        public static Dictionary<Type, T> GetTypesAndAttributesWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
        {
            var dict = new Dictionary<Type, T>();
            var ts = types?.Where(x => x.IsDefined(typeof(T)) && !x.IsInterface && !x.IsAbstract) ?? null;

            foreach(var t in ts)
                dict[t] = t.GetCustomAttribute<T>();

            return dict;
        }

        /// <summary>
        /// Returns all instantiatable types with attribute
        /// </summary>
        /// <param name="types">Types to search through</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
        {
            return types?.Where(x => x.IsDefined(typeof(T)) && !x.IsInterface && !x.IsAbstract) ?? null;
        }
    }
}