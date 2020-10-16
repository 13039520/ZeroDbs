using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace ZeroDbs.DataAccess.Common
{
    public delegate void EntityPropertySetter(object instance, object value);
    public delegate object EntityPropertyGetter(object instance, string name);
    public class EntityObjectProperty
    {
        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo Info { get; set; }
        /// <summary>
        /// Set方法委托
        /// </summary>
        public EntityPropertySetter Setter { get; set; }
        /// <summary>
        /// Get方法委托
        /// </summary>
        public EntityPropertyGetter Getter { get; set; }
        static readonly Dictionary<Type, EntityObjectProperty[]> Cache = new Dictionary<Type, EntityObjectProperty[]>();

        /// <summary>
        /// 获取一个类中的所有公开实例属性和它们的Set方法委托
        /// </summary>
        public static EntityObjectProperty[] GetProperties(Type type)
        {
            EntityObjectProperty[] arr;
            if (Cache.TryGetValue(type, out arr))
            {
                return arr;
            }
            PropertyInfo[] ps = type.GetProperties();
            arr = new EntityObjectProperty[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
                EntityObjectProperty op = new EntityObjectProperty();
                op.Info = ps[i];
                op.Setter = CreateSetter(op.Info);
                op.Getter = CreateGetter(op.Info);
                arr[i] = op;
            }
            Cache.Add(type, arr);
            return arr;
        }
        static EntityPropertySetter CreateSetter(PropertyInfo property)
        {
            var type = property.DeclaringType;
            var dm = new DynamicMethod("", null, new[] { typeof(object), typeof(object) }, type);
            //=== IL ===
            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            if (property.PropertyType.IsValueType)
            {
                // 如果是值类型，装箱
                il.Emit(OpCodes.Unbox, property.PropertyType);
            }
            else
            {
                // 如果是引用类型，转换
                il.Emit(OpCodes.Castclass, property.PropertyType);
            }
            il.Emit(OpCodes.Callvirt, property.GetSetMethod());
            il.Emit(OpCodes.Ret);
            //=== IL ===
            return (EntityPropertySetter)dm.CreateDelegate(typeof(EntityPropertySetter));
        }
        static EntityPropertyGetter CreateGetter(PropertyInfo property)
        {
            var type = property.DeclaringType;
            var dm = new DynamicMethod("get_" + property.Name, typeof(object), new[] { type }, type);
            var iLGenerator = dm.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);

            iLGenerator.Emit(OpCodes.Callvirt, property.GetMethod);
            if (property.PropertyType.IsValueType)
            {
                // 如果是值类型，装箱
                iLGenerator.Emit(OpCodes.Box, property.PropertyType);
            }
            else
            {
                // 如果是引用类型，转换
                iLGenerator.Emit(OpCodes.Castclass, property.PropertyType);
            }

            iLGenerator.Emit(OpCodes.Ret);

            return (EntityPropertyGetter)dm.CreateDelegate(typeof(EntityPropertyGetter));
        }

    }


}
