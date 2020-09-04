using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoneyBookWithDataset
{
    /// <summary>
    /// 확장메소드
    /// </summary>
    /// <remarks>
    /// 자주쓰는 명령을 확장으로 선언했습니다.
    /// 속성으로 접근하는것이 더 편해서 작성 함
    /// </remarks>
    public static class MethodExtensions
    {
        public static string MakeFilePath(this string value, params string[] param)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(value.Replace("/", "\\"));
            foreach (var item in param)
            {
                if (sb.Length > 0 && sb.ToString().EndsWith("\\") == false) sb.Append("\\");
                sb.Append(item.Replace("/", "\\"));
            }
            var retval = sb.ToString().Replace("/", "\\").Replace("\\\\", "\\");
            return retval.ToString();
        }


        public static string MakeFTPPath(this string value, params string[] param)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(value.Replace("\\", "/"));
            foreach (var item in param)
            {
                if (sb.Length > 0 && sb.ToString().EndsWith("/") == false) sb.Append("/");
                sb.Append(item.Replace("\\", "/"));
            }
            var retval = sb.ToString().Replace("//", "/");
            return retval.ToString();
        }

        /// <summary>
        /// Data Map
        /// </summary>
        /// <remarks>Arduino 의 Map 명령과 동일 합니다</remarks>
        /// <returns>Double</returns>
        public static double Map(this double x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
        public static string Base64Encode(this string src)
        {
            string base64enc = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(src));
            return base64enc;
        }
        public static string Base64Decode(this string src)
        {
            var base64dec = Convert.FromBase64String(src);
            return System.Text.Encoding.UTF8.GetString(base64dec);
        }
       



        //public static void SetBGColor(this System.Windows.Forms.Label ctl,System.Drawing.Color color1)
        //{
        //    ctl.BackColor = System.Drawing.Color.Red;
        //}

        /// <summary>
        /// Bitarray to BinString(1010101010)
        /// </summary>
        /// <remarks>BitArray 값을 100010101 형태의 문자로 변환 합니다</remarks>
        /// <param name="arr"></param>
        /// <returns>String</returns>
        public static string BitString(this System.Collections.BitArray arr)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = arr.Length; i > 0; i--)
                sb.Append(arr[i - 1] ? "1" : "0");
            return sb.ToString();
        }

        /// <summary>
        /// String to Int
        /// </summary>
        /// <remarks>BitArray 에서 정수(Int32)로 변경 합니다.</remarks>
        /// <param name="arr"></param>
        /// <returns>Int32</returns>
        public static int ValueI(this System.Collections.BitArray arr)
        {
            byte[] buf = new byte[4];
            arr.CopyTo(buf, 0);
            return BitConverter.ToInt32(buf, 0);
        }

        /// <summary>
        /// 입력문자열이 숫자인지 확인합니다
        /// </summary>
        /// <remarks>입력된 문자열이 Double 로 변환되는지를 체크 합니다</remarks>
        /// <param name="input"></param>
        /// <returns>True/False</returns>
        public static bool IsNumeric(this string input)
        {
            double data;
            return double.TryParse(input, out data);
            //return Regex.IsMatch(input, @"^\d+$");
        }

        public static DateTime GetDateValue(this string input)
        {
            DateTime data;
            if (input.Length == 8)
                input = string.Format("{0}-{1}-{2}",
                    input.Substring(0, 4),
                    input.Substring(4, 2),
                    input.Substring(6, 2));

            if (!DateTime.TryParse(input, out data))
                return DateTime.Parse("1982-11-23");
            else
                return data;
        }
        /// <summary>
        /// isNullofEmpty()
        /// </summary>
        /// <remarks>isNullOfempty() 함수를 속성형태로 변경</remarks>
        /// <param name="input"></param>
        /// <returns>True / False</returns>
        public static Boolean IsEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// byte[] to string
        /// </summary>
        /// <remarks>Byte[] 에서 문자열을 추출 합니다. 인코딩은 Default 를 사용 합니다</remarks>
        /// <param name="input"></param>
        /// <returns>문자열</returns>
        public static string GetString(this Byte[] input)
        {
            return System.Text.Encoding.Default.GetString(input);
        }

        /// <summary>
        /// 16진수 문자열 형태로 반환합니다. 각 HEX문사 사이에 공백이 포함됩니다.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHexString(this Byte[] input)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte b in input)
                sb.Append(" " + b.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// 문자를 HEX 문자로 변경합니다. 각 HEX문자 사이에는 공백이 없습니다.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHexStringNoSpace(this string input)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char b in input.ToCharArray())
                sb.Append(((byte)b).ToString("X2"));
            return sb.ToString();
        }
    }
}

