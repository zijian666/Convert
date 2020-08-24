using System;
using System.Collections.Generic;
using System.Linq;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    /// <summary>
    /// 聚合转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// 如果转换失败,会继续尝试下一个转换器,直到成功或全部失败
    /// </remarks>
    internal sealed class AggregateConvertor<T> : Convertor<T>, IConvertor<T>
    {
        public AggregateConvertor(IEnumerable<IConvertor<T>> convertors)
        {

            if (convertors == null)
            {
                throw new ArgumentNullException(nameof(convertors));
            }

            Convertors = convertors.Where(x => !(x is AggregateConvertor<T>)).Union(
                            convertors.OfType<AggregateConvertor<T>>().SelectMany(x => x.Convertors)
                         ).ToList().AsReadOnly();
            if (Convertors.Count < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(convertors), "转换器必须大于1个");
            }
        }

        public AggregateConvertor(params IConvertor<T>[] convertors)
            : this((IEnumerable<IConvertor<T>>)convertors)
        {

        }

        public IReadOnlyCollection<IConvertor<T>> Convertors { get; }

        public override ConvertResult<T> Convert(IConvertContext context, object input)
        {
            ExceptionCollection exceptions = null;
            foreach (var convertor in Convertors)
            {
                var result = convertor.Convert(context, input);
                if (result.Success)
                {
                    return result;
                }
                exceptions += result.Exception;
            }

            var message = Exceptions.ConvertFailMessage(input, TypeFriendlyName, context.Settings.CultureInfo);
            return exceptions.ToException(message);
        }
    }
}
