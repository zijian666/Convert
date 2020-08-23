using System.Globalization;

namespace zijian666.SuperConvert.Core
{
    public static class ResourceStringManager
    {
        public readonly static ResourceStrings ZH_CN = new ResourceStrings();

        public static ResourceStrings GetResource(CultureInfo cultureInfo) => ZH_CN;

        public static ResourceStrings GetResource() => GetResource(CultureInfo.CurrentUICulture);
    }
}
