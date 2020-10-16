using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace ZeroDbs.Interfaces.Common
{
    public delegate void ZeroEntityPropertyEmitSetter(object instance, object value);

    public class EntityPropertyEmitSetter
    {
        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo Info { get; set; }
        public ZeroEntityPropertyEmitSetter Setter { get; set; }
        static readonly Dictionary<Type, EntityPropertyEmitSetter[]> Cache = new Dictionary<Type, EntityPropertyEmitSetter[]>();

        /// <summary>
        /// 获取一个类中的所有公开实例属性和它们的Set方法委托
        /// </summary>
        public static EntityPropertyEmitSetter[] GetProperties(Type type)
        {
            EntityPropertyEmitSetter[] arr;
            if (Cache.TryGetValue(type, out arr))
            {
                return arr;
            }
            PropertyInfo[] ps = type.GetProperties();
            arr = new EntityPropertyEmitSetter[ps.Length];
            Type delegateType = typeof(ZeroEntityPropertyEmitSetter);
            for (int i = 0; i < ps.Length; i++)
            {
                EntityPropertyEmitSetter op = new EntityPropertyEmitSetter();
                op.Info = ps[i];
                op.Setter = CreateSetter(op.Info, delegateType);
                arr[i] = op;
            }
            Cache.Add(type, arr);
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
                // 如果是值类型，拆箱
                il.Emit(OpCodes.Unbox_Any, property.PropertyType);
            }
            else
            {
                // 如果是引用类型，转换
                il.Emit(OpCodes.Castclass, property.PropertyType);
            }
            il.Emit(OpCodes.Callvirt, property.GetSetMethod());
            il.Emit(OpCodes.Ret);
            //=== IL ===
            return (ZeroEntityPropertyEmitSetter)dm.CreateDelegate(delegateType);
        }

    }


}
