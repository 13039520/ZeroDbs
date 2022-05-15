using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace ZeroDbs.Common
{
    public delegate void ZeroEntityPropertyEmitSetter(object instance, object value);
    public class EntityPropertyEmitSetter
    {
        public PropertyInfo Info { get; set; }
        public ZeroEntityPropertyEmitSetter Setter { get; set; }
        static object _lock = new object();
        static readonly Dictionary<Type, EntityPropertyEmitSetter[]> Cache = new Dictionary<Type, EntityPropertyEmitSetter[]>();

        public static EntityPropertyEmitSetter[] GetProperties(Type type)
        {
            EntityPropertyEmitSetter[] arr;
            if (Cache.TryGetValue(type, out arr))
            {
                return arr;
            }
            var ps = EntityPropertyInfoCache.GetPropertyInfoList(type);
            arr = new EntityPropertyEmitSetter[ps.Count];
            Type delegateType = typeof(ZeroEntityPropertyEmitSetter);
            for (int i = 0; i < ps.Count; i++)
            {
                EntityPropertyEmitSetter op = new EntityPropertyEmitSetter();
                op.Info = ps[i];
                op.Setter = CreateSetter(op.Info, delegateType);
                arr[i] = op;
            }
            lock (_lock)
            {
                if (Cache.ContainsKey(type))
                {
                    Cache[type] = arr;
                }
                else
                {
                    Cache.Add(type, arr);
                }
            }
            return arr;
        }
        static ZeroEntityPropertyEmitSetter CreateSetter(PropertyInfo property, Type delegateType)
        {
            var type = property.DeclaringType;
            var dm = new DynamicMethod("", null, new[] { typeof(object), typeof(object) }, type);
            //=== IL ===
            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            if (property.PropertyType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, property.PropertyType);
            }
            else
            {
                il.Emit(OpCodes.Castclass, property.PropertyType);
            }
            il.Emit(OpCodes.Callvirt, property.GetSetMethod());
            il.Emit(OpCodes.Ret);
            return (ZeroEntityPropertyEmitSetter)dm.CreateDelegate(delegateType);
        }

    }


}
