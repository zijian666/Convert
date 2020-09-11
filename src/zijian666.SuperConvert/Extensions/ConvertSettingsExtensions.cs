using System;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Extensions
{
    public static class ConvertSettingsExtensions
    {
        public static IConvertSettings Combin(this IConvertSettings settings1, IConvertSettings settings2)
        {
            if (settings1 is null && settings2 is null)
            {
                return Converts.Settings;
            }
            if (ReferenceEquals(settings1, settings2))
            {
                return settings1;
            }
            if (settings1 is null)
            {
                return settings2;
            }
            if (settings2 is null || settings2 == settings1)
            {
                return settings1;
            }
            return new AggregateConvertSettings(settings1, settings2);
        }

        public static IFormatProvider GetFormatProvider(this IConvertSettings settings, Type type)
        {
            if (settings.FormatProviders != null && settings.FormatProviders.TryGetValue(type, out var formatProvider))
            {
                return formatProvider;
            }
            if (settings.FormatProviders?.Count == 1)
            {
                foreach (var item in settings.FormatProviders)
                {
                    return item.Value;
                }
            }
            return null;
        }

        public static string GetFormatString(this IConvertSettings settings, Type type)
        {
            if (settings.FormatStrings != null && settings.FormatStrings.TryGetValue(type, out var format))
            {
                return format;
            }
            if (settings.FormatStrings?.Count == 1)
            {
                foreach (var item in settings.FormatStrings)
                {
                    return item.Value;
                }
            }
            return null;
        }

        public static string Format(this IConvertSettings settings, IFormattable input)
        {
            if (settings == null)
            {
                return input.ToString(null, null);
            }
            var inputType = input.GetType();
            return input.ToString(settings.GetFormatString(inputType), settings.GetFormatProvider(inputType));
        }

        public static ResourceStrings GetResourceStrings(this IConvertSettings settings)
            => ResourceStringManager.GetResource(settings?.CultureInfo);

        public static T CreateInstance<T>(this IConvertSettings settings)
        {
            var compiler = settings?.ReflectCompiler;
            if (compiler == null)
            {
                return Activator.CreateInstance<T>();
            }
            var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                return Activator.CreateInstance<T>();
            }
            var creator = compiler.CompileCreator<T>(constructor);
            return creator();
        }

        public static object CreateInstance(this IConvertSettings settings, Type type)
        {
            var compiler = settings?.ReflectCompiler;
            if (compiler == null)
            {
                return Activator.CreateInstance(type);
            }
            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                return Activator.CreateInstance(type);
            }
            var creator = compiler.CompileCreator<object>(constructor);
            return creator();
        }
    }
}
