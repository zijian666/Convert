using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Extensions
{
    static class ConvertSettingsExtensions
    {
        public static IConvertSettings Combin(this IConvertSettings settings1, IConvertSettings settings2)
        {
            if (settings1 is null && settings2 is null)
            {
                return Converts.Settings;
            }
            if (settings1 is null)
            {
                return settings2;
            }
            if (settings2 is null || settings2 == settings1)
            {
                return settings1;
            }
            return new CombinConvertSettings(settings1, settings2);
        }
    }
}
