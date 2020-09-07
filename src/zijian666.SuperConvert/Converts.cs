using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Serialization;
using System.Text;
using zijian666.Core.Abstractions;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Factory;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert
{
    public static partial class Converts
    {
        private static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.CompileLibraries;
            var loadContext = AssemblyLoadContext.Default;
            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch
                {
                    //
                }
            }
            return assemblies.ToArray();
        }

        public static ConvertSettings Settings { get; }

        static Converts()
        {
            var assemblies = GetAssemblies();
            var factories = new List<IConvertorFactory>();
            var translators = new List<ITranslator>();
            var types = assemblies.SelectMany(x => x.SafeGetTypes())
                .Where(x => !x.Name.StartsWith("<") && x.Instantiable() && x.GetConstructor(Type.EmptyTypes) != null);
            foreach (var type in types)
            {
                if (typeof(IConvertorFactory).IsAssignableFrom(type))
                {
                    if (type.Instantiable() && type.GetConstructor(Type.EmptyTypes) != null)
                    {
                        factories.Add((IConvertorFactory)Activator.CreateInstance(type));
                    }
                }
                else if (typeof(ITranslator).IsAssignableFrom(type))
                {
                    if (type.Instantiable() && type.GetConstructor(Type.EmptyTypes) != null)
                    {
                        translators.Add((ITranslator)Activator.CreateInstance(type));
                    }
                }
                else
                {
                    var t = type.GetGenericArguments(typeof(IConvertor<>));
                    if (t != null && t.Length == 1)
                    {
                        var conv = Activator.CreateInstance(type);
                        factories.Add(new InstantiatedConvertorFactory(conv, t[0]));
                    }
                }
            }
            var builder = new ConvertorBuilder(factories.ToArray());
            Settings = new ConvertSettings(builder)
            {
                Trace = new TextWriterTraceListener(Console.Out),
                StringSeparator = ",",
                Encoding = Encoding.UTF8,
                CultureInfo = CultureInfo.CurrentUICulture,
                NumberFormatInfo = NumberFormatInfo.CurrentInfo,
                StringSplitOptions = StringSplitOptions.RemoveEmptyEntries,
            };
            Settings.Translators.AddRange(translators);
        }


        public static ConvertResult<T> Convert<T>(this object value, IConvertSettings settings = null)
        {
            using IConvertContext context = new ConvertContext(settings.Combin(Settings));
            return context.Convert<T>(value);
        }

        public static ConvertResult<object> Convert(this object value, Type type, IConvertSettings settings = null)
        {
            using IConvertContext context = new ConvertContext(settings.Combin(Settings));
            return context.Convert(type, value);
        }

        public static T To<T>(this object value, T defaultValue) => Convert<T>(value, null).GetValueOrDefalut(defaultValue);

        public static T To<T>(this object value) => Convert<T>(value, null).Value;

        /// <summary>
        /// 获取一个类型的默认值
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public static object GetDefault(this Type type)
        {
            if ((type == null)
                || (type.IsValueType == false) //不是值类型
                || (Nullable.GetUnderlyingType(type) != null)) //可空值类型
            {
                return null;
            }
            return type.Instantiable() ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// 判断一个对象的值是否为 null 或等效于null的值
        /// </summary>
        public static bool IsNull(this object value)
            => value switch
            {
                null => true,
                DBNull => true,
                IConvertible x => x.GetTypeCode() == TypeCode.DBNull || x.GetTypeCode() == TypeCode.Empty,
                _ => value.Equals(null) || value.Equals(DBNull.Value),
            };

        /// <summary>
        /// 判断一个对象的值是否为空,包括null,空集合,空字符串
        /// </summary>
        public static bool IsEmpty(this object value)
            => value switch
            {
                null => true,
                DBNull => true,
                string x => x.Length == 0,
                Array x => x.Length == 0,
                ICollection x => x.Count == 0,
                IEnumerable x => !x.Cast<object>().Any(),
                IConvertible x => x.GetTypeCode() switch
                {
                    TypeCode.DBNull => true,
                    TypeCode.Empty => true,
                    _ => false
                },
                _ => value.Equals(null) || value.Equals(DBNull.Value),
            };

        /// <summary>
        /// 判断一个对象的值是否为连续的空白,包括null,空集合,空字符串,空白字符串,全部为null,空字符串或空白字符串的集合
        /// </summary>
        public static bool IsSerialBlank(this object value)
            => value switch
            {
                null => true,
                DBNull => true,
                string s => string.IsNullOrWhiteSpace(s),
                IEnumerable<char> s => s.All(char.IsWhiteSpace),
                StringBuilder s => s.Length == 0 || string.IsNullOrWhiteSpace(s.ToString()),
                IEnumerable<string> s => s.All(string.IsNullOrWhiteSpace),
                IEnumerable<object> s => s.All(x => x == null),
                IEnumerable s => s.Cast<object>().All(x => x == null),
                IConvertible s => s.GetTypeCode() switch
                {
                    TypeCode.DBNull => true,
                    TypeCode.Empty => true,
                    TypeCode.String => string.IsNullOrWhiteSpace(s.ToString(null)),
                    _ => false
                },
                _ => value.Equals(null) || value.Equals(DBNull.Value),
            };


        public static void SetStringSerializer(IStringSerializer serializer)
            => ((ISlot<IStringSerializer>)Settings)?.Set(serializer);

        public static void SetReflectCompiler(IReflectCompiler compiler)
            => ((ISlot<IReflectCompiler>)Settings)?.Set(compiler);

        public static IFormatterConverter GetFormatterConverter() => FormatterConverter.Instance;

        public static void SetFormatterConverter(this ISlot<IFormatterConverter> slot)
            => slot.Set(FormatterConverter.Instance);
    }

}
