using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Factory;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert
{
    public static class Converts
    {
        private static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.CompileLibraries;
            var libs = dependencies.Where(lib => !lib.Serviceable && lib.Type != "package");
            foreach (var library in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch
                {
                    //
                }
            }
            return assemblies.ToArray();
        }

        static ConvertSettings _settings;

        public static ConvertSettings Settings => _settings;

        static Converts()
        {
            var assemblies = GetAssemblies();
            var factories = new List<IConvertorFactory>();
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
            _settings = new ConvertSettings(builder)
            {
                Trace = new TextWriterTraceListener(Console.Out),
                StringSeparator = ",",
                Encoding = Encoding.UTF8,
                CultureInfo = CultureInfo.CurrentUICulture,
                NumberFormatInfo = NumberFormatInfo.CurrentInfo,
                StringSplitOptions = StringSplitOptions.RemoveEmptyEntries,
            };
        }


        public static ConvertResult<T> Convert<T>(this object value, IConvertSettings settings = null)
        {
            using IConvertContext context = new ConvertContext(settings.Combin(_settings));
            return context.Convert<T>(value);
        }

        public static ConvertResult<object> Convert(this object value, Type type, IConvertSettings settings = null)
        {
            using IConvertContext context = new ConvertContext(settings.Combin(_settings));
            return context.Convert(type, value);
        }

        public static T To<T>(this object value, T defaultValue) => Convert<T>(value, null).GetValueOrDefalut(defaultValue);

        public static T To<T>(this object value) => Convert<T>(value, null).Value;

    }

}
