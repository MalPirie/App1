using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace App1.Utilities
{
    public static class RedirectToWhen
    {
        private static readonly MethodInfo InternalPreserveStackTraceMethod = 
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

        private static class Cache
        {
            private static readonly IDictionary<Type, IDictionary<Type, MethodInfo>> _dict =
                new Dictionary<Type, IDictionary<Type, MethodInfo>>();

            public static IDictionary<Type, MethodInfo> GetDictionaryForType(Type type)
            {
                if (_dict.ContainsKey(type))
                {
                    return _dict[type];
                }
                var dict = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(m => m.Name == "When")
                    .Where(m => m.GetParameters().Length == 1)
                    .ToDictionary(m => m.GetParameters().First().ParameterType, m => m);
                _dict.Add(type, dict);
                return dict;
            }
        }

//        [DebuggerNonUserCode]
        public static void InvokeEventOptional<T>(T instance, object command)
        {
            MethodInfo info;
            var type = command.GetType();
            var dict = Cache.GetDictionaryForType(instance.GetType());
            if (!dict.TryGetValue(type, out info))
            {
                return;
            }
            try
            {
                info.Invoke(instance, new[] { command });
            }
            catch (TargetInvocationException ex)
            {
                if (InternalPreserveStackTraceMethod != null)
                {
                    InternalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
                }
                throw ex.InnerException;
            }
        }
    }
}