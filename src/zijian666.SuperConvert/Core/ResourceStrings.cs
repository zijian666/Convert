namespace zijian666.SuperConvert.Core
{
    public class ResourceStrings
    {
        /// <summary>
        /// `{0}` 无法转换为 {1}
        /// </summary>
        public virtual string CANT_CONVERT { get; set; } = "`{0}`({1}) 无法转换为 {2}";
        /// <summary>
        /// 无法生成 {0} 类型的转换器：泛型定义类型
        /// </summary>
        public virtual string CANT_BUILD_CONVERTOR_BECAUSE_GENERIC_DEFINITION_TYPE { get; set; } = "无法生成 {0} 类型的转换器：泛型定义类型";
        /// <summary>
        /// 无法生成 {0} 类型的转换器：静态类型
        /// </summary>
        public virtual string CANT_BUILD_CONVERTOR_BECAUSE_STATIC_TYPE { get; set; } = "无法生成 {0} 类型的转换器：静态类型";
        /// <summary>
        /// 无法生成 {0} 类型的转换器：未找到合适的转换器
        /// </summary>
        public virtual string CANT_BUILD_CONVERTOR_BECAUSE_NOTFOUND { get; set; } = "无法生成 {0} 类型的转换器：未找到合适的转换器";
        /// <summary>
        /// 转换器{0} 转换失败：{1}
        /// </summary>
        public virtual string CONVERTOR_CAST_FAIL { get; set; } = "转换失败：{1}；(转换器：{0})";

        /// <summary>
        /// 属性：{0}.{1} 转换失败
        /// </summary>
        public virtual string PROPERTY_CAST_FAIL { get; set; } = "{0} 属性{1} 转换失败";


        /// <summary>
        /// 属性：{0}.{1} 设置失败，值：{2}
        /// </summary>
        public virtual string PROPERTY_SET_FAIL { get; set; } = "{0} 属性{1} 设置失败，值：{2}";

        /// <summary>
        /// {0} 实例化失败
        /// </summary>
        public virtual string INSTANTIATION_FAIL { get; set; } = "{0} 实例化失败";

        /// <summary>
        /// 值超过限制：{0}
        /// </summary>
        public string VALUE_OVERFLOW { get; set; } = "值超过限制：{0}";

        /// <summary>
        /// 集合插值失败，{0}[{1}]={2}
        /// </summary>
        public virtual string COLLECTION_ADD_FAIL { get; set; } = "集合{0}插值失败，[{1}]={2}";

        /// <summary>
        /// 集合键转换失败：{0}，KEY={1}
        /// </summary>
        public virtual string COLLECTION_KEY_FAIL { get; set; } = "集合{0}键{1}转换失败";
    }
}
