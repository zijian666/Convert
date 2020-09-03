using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


namespace zijian666.SuperConvert
{
    public static partial class Converts
    {
        /// <summary>
        /// 使用MD5加密
        /// </summary>
        /// <param name="input"> 加密字符串 </param>
        /// <remarks> 周子鉴 2015.08.26 </remarks>
        public static Guid ToMD5_Fast(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Guid.Empty;
            }
            using var md5Provider = new MD5CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5Provider.ComputeHash(bytes);
            Swap(hash, 0, 3); //交换0,3的值
            Swap(hash, 1, 2); //交换1,2的值
            Swap(hash, 4, 5); //交换4,5的值
            Swap(hash, 6, 7); //交换6,7的值
            return new Guid(hash);
        }

        private static void Swap(byte[] arr, int a, int b)
        {
            var temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }

        /// <summary>
        /// 产生一个包含随机'盐'的的MD5
        /// </summary>
        /// <param name="input"> 输入内容 </param>
        /// <returns> </returns>
        /// <remarks> 周子鉴 2015.10.03 </remarks>
        public static Guid ToRandomMD5(string input)
        {
            if (input == null)
            {
                input = "";
            }
            using var md5Provider = new MD5CryptoServiceProvider();
            //获取一个随机数,用于充当 "盐"
            var salt = new object().GetHashCode();
            input += salt;
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5Provider.ComputeHash(bytes);
            var saltBytes = BitConverter.GetBytes(salt);
            var index = hash[0] % 12 + 1;
            hash[index] = saltBytes[0];
            hash[index + 1] = saltBytes[1];
            hash[index + 2] = saltBytes[2];
            hash[index + 3] = saltBytes[3];
            return new Guid(hash);
        }

        /// <summary>
        /// 对比使用 ToRandomMD5 产生的MD5和原信息是否匹配
        /// </summary>
        /// <param name="input"> 原信息 </param>
        /// <param name="rmd5"> 随机盐MD5 </param>
        /// <returns> </returns>
        /// <remarks> 周子鉴 2015.10.03 </remarks>
        public static bool EqualsRandomMD5(string input, Guid rmd5)
        {
            if (input == null)
            {
                input = "";
            }
            var arr = rmd5.ToByteArray();
            var index = arr[0] % 12 + 1;
            //将盐取出来
            var salt = BitConverter.ToInt32(arr, index);
            using var md5Provider = new MD5CryptoServiceProvider();
            input += salt;
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5Provider.ComputeHash(bytes);
            for (var i = 0; i < 16; i++)
            {
                if (i == index) //跳过盐的部分
                {
                    i += 4;
                    continue;
                }
                if (hash[i] != arr[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 使用16位MD5加密
        /// </summary>
        /// <param name="input"> 加密字符串 </param>
        /// <param name="count"> 加密次数 </param>
        public static string ToMD5x16(string input, int count = 1)
        {
            if (string.IsNullOrEmpty(input))
            {
                return _emptyMd5x16;
            }
            if (count <= 0)
            {
                return input;
            }
            for (var i = 0; i < count; i++)
            {
                input = ToMD5x16(input);
            }
            return input;
        }

        /// <summary>
        /// 使用16位MD5加密
        /// </summary>
        /// <param name="input"> 加密字符串 </param>
        public static string ToMD5x16(string input)
            => string.IsNullOrEmpty(input) ? _emptyMd5x16 : ToMD5x16(Encoding.UTF8.GetBytes(input));

        /// <summary>
        /// 使用MD5加密
        /// </summary>
        /// <param name="input"> 需要加密的字节 </param>
        public static string ToMD5x16(byte[] input)
        {
            if ((input == null) || (input.Length == 0))
            {
                return _emptyMd5;
            }
            var md5 = new MD5CryptoServiceProvider();
            var data = md5.ComputeHash(input);
            return ByteToString(data, 4, 8);
        }

        /// <summary>
        /// 使用MD5加密
        /// </summary>
        /// <param name="input"> 加密字符串 </param>
        /// <param name="count"> 加密次数 </param>
        public static string ToMD5(string input, int count = 1)
        {
            if (input == null)
            {
                return _emptyMd5;
            }
            if (count <= 0)
            {
                return input;
            }
            for (var i = 0; i < count; i++)
            {
                input = ToMD5(input);
            }
            return input;
        }

        /// <summary>
        /// 使用MD5加密
        /// </summary>
        /// <param name="input"> 加密字符串 </param>
        public static string ToMD5(string input)
            => ToMD5(Encoding.UTF8.GetBytes(input));

        private static readonly string _emptyMd5 = new string('0', 32);
        private static readonly string _emptyMd5x16 = new string('0', 16);

        /// <summary>
        /// 使用MD5加密
        /// </summary>
        /// <param name="input"> 需要加密的字节 </param>
        public static string ToMD5(byte[] input)
        {
            if (input == null)
            {
                return _emptyMd5;
            }
            using var md5 = new MD5CryptoServiceProvider();
            var data = md5.ComputeHash(input);
            return ByteToString(data);
        }

        /// <summary>
        /// 使用SHA1加密
        /// </summary>
        /// <param name="input"> 加密字符串 </param>
        /// <param name="count"> 加密次数 </param>
        public static string ToSHA1(string input, int count = 1)
        {
            if ((input == null) || (count <= 0))
            {
                return input;
            }
            using var sha1 = new SHA1CryptoServiceProvider();
            var data = Encoding.UTF8.GetBytes(input);
            for (var i = 0; i < count; i++)
            {
                data = sha1.ComputeHash(data);
            }
            return ByteToString(data);
        }

        /// <summary>
        /// 使用SHA1加密
        /// </summary>
        /// <param name="input"> 加密字符串 </param>
        public static string ToSHA1(string input)
            => input == null ? null : ToSHA1(Encoding.UTF8.GetBytes(input));

        /// <summary>
        /// 使用SHA1加密
        /// </summary>
        /// <param name="input"> 需要加密的字节 </param>
        public static string ToSHA1(byte[] input)
        {
            if (input == null)
            {
                return null;
            }
            using var sha1 = new SHA1CryptoServiceProvider();
            var data = sha1.ComputeHash(input);
            return ByteToString(data);
        }

        private static string ByteToString(IReadOnlyList<byte> data)
            => ByteToString(data, 0, data.Count);

        private static string ByteToString(IReadOnlyList<byte> data, int offset, int count)
        {
            var chArray = new char[count * 2];
            var end = offset + count;
            for (int i = offset, j = 0; i < end; i++)
            {
                var num2 = data[i];
                chArray[j++] = NibbleToHex((byte)(num2 >> 4));
                chArray[j++] = NibbleToHex((byte)(num2 & 15));
            }
            return new string(chArray);
        }

        private static char NibbleToHex(byte nibble)
            => nibble < 10 ? (char)(nibble + 0x30) : (char)(nibble - 10 + 'a');
    }
}
