using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCommandServer.Common
{
    public static class Extensions
    {
        public static string ToErrorString(this Exception ex) => ex.GetType().FullName + ": " + ex.Message +
            "\nInner: " + ex.InnerException?.Message + "\nStack: " + ex.StackTrace + "\n";

        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {

            first = list.Count > 0 ? list[0] : default; // or throw
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default; // or throw
            second = list.Count > 1 ? list[1] : default; // or throw
            rest = list.Skip(2).ToList();
        }

        public static string Join(this IEnumerable<string> list, string separator) => string.Join(separator, list);

        public static string Join<T>(this IEnumerable<T> list, string separator, Func<T, string> selector) => string.Join(separator, list.Select(selector));

        public static string Repeat(this string str, uint count) => string.Concat(Enumerable.Repeat(str, (int)count));

        public static void Rethrow(this Exception ex) => ExceptionDispatchInfo.Capture(ex.InnerException).Throw();

        // File Utils

        private static Encoding DefaultEncoding = Encoding.UTF8;

        public static Stream ToStream(this byte[] bytes)
        {
            var ms = new MemoryStream();
            ms.Write(bytes);
            return ms;
        }

        public static async Task<Stream> ToStreamAsync(this byte[] bytes)
        {
            var ms = new MemoryStream();
            await ms.WriteAsync(bytes);
            return ms;
        }

        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string ToEncodedString(this byte[] bytes, Encoding encoding = null)
        {
            encoding ??= DefaultEncoding;
            return encoding.GetString(bytes);
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
        {
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }

        public static string ToBase64(this Stream stream)
        {
            var bytes = stream.ToByteArray();
            return bytes.ToBase64();
        }

        public static async Task<string> ToBase64Async(this Stream stream)
        {
            var bytes = await stream.ToByteArrayAsync();
            return bytes.ToBase64();
        }

        public static string ToEncodedString(this Stream stream, Encoding encoding = null)
        {
            encoding ??= DefaultEncoding;
            var bytes = stream.ToByteArray();
            return bytes.ToEncodedString(encoding);
        }

        public static async Task<string> ToEncodedStringAsync(this Stream stream, Encoding encoding = null)
        {
            encoding ??= DefaultEncoding;
            var bytes = await stream.ToByteArrayAsync();
            return bytes.ToEncodedString(encoding);
        }

        public static byte[] ToByteArray(this string str, Encoding encoding = null)
        {
            encoding ??= DefaultEncoding;
            return encoding.GetBytes(str);
        }

        public static Stream ToStream(this string str, Encoding encoding = null)
        {
            encoding ??= DefaultEncoding;
            var bytes = str.ToByteArray(encoding);
            return bytes.ToStream();
        }

        public static Task<Stream> ToStreamAsync(this string str, Encoding encoding = null)
        {
            encoding ??= DefaultEncoding;
            var bytes = str.ToByteArray(encoding);
            return bytes.ToStreamAsync();
        }

        public static string ToBase64(this string str, Encoding encoding = null)
        {
            encoding ??= DefaultEncoding;
            var bytes = str.ToByteArray(encoding);
            return bytes.ToBase64();
        }

        public static byte[] Base64ToByteArray(this string base64)
        {
            return Convert.FromBase64String(base64);
        }

        public static Stream Base64ToStream(this string base64)
        {
            var bytes = base64.Base64ToByteArray();
            return bytes.ToStream();
        }

        public static Task<Stream> Base64ToStreamAsync(this string base64)
        {
            var bytes = base64.Base64ToByteArray();
            return bytes.ToStreamAsync();
        }

        public static string Base64ToString(this string base64, Encoding encoding = null)
        {
            encoding ??= DefaultEncoding;
            var bytes = base64.Base64ToByteArray();
            return bytes.ToEncodedString(encoding);
        }
    }
}
