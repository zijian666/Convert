﻿using System;

namespace zijian666.SuperConvert.Interface
{
    /// <summary>
    /// 处理值类型的转换接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFormValueType<T>
        : IFrom<ValueType, T>
    {

    }
}
