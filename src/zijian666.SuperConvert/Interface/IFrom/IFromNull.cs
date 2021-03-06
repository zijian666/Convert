﻿using System;
using zijian666.SuperConvert.Core;

namespace zijian666.SuperConvert.Interface
{
    /// <summary>
    /// 处理 <seealso cref="null"/> 和 <seealso cref="DBNull"/> 转换的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromNull<T> : IFrom<DBNull, T>
    {
        /// <summary>
        /// 将 <seealso cref="null"/> 转为指定类型
        /// </summary>
        /// <param name="context">转换上下文</param>
        ConvertResult<T> FromNull(IConvertContext context);
    }
}
