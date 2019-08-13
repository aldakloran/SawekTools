using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SawekTools {
    public static class StringExtensions {
        public static string RemoveDigits(this string key) {
            return Regex.Replace(key, @"\d", "");
        }

        public static string RemoveSpace(this string value) {
            var len = value.Length;
            var src = value.ToCharArray();
            int dstIdx = 0;

            for (int i = 0; i < len; i++) {
                var ch = src[i];

                switch (ch) {
                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':
                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':
                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':
                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':
                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':
                        continue;

                    default:
                        src[dstIdx++] = ch;
                        break;
                }
            }
            return new string(src, 0, dstIdx);
        }

        public static string RemoveInvalidFileNameChars(this string value) {
            return Path.GetInvalidFileNameChars()
                       .Aggregate(value, (current, forbiddenLetters) => current.Replace(forbiddenLetters, '_'));
        }

        public static string RemovePolishLetters(this string s) {
            char[] polishLetters = { 'ą', 'ć', 'ę', 'ł', 'ń', 'ó', 'ś', 'ż', 'ź' };
            char[] replacements = { 'a', 'c', 'e', 'l', 'n', 'o', 's', 'z', 'z' };

            string resolt = s;
            for (int i = 0; i < polishLetters.Length; i++) {
                resolt = resolt.Replace(polishLetters[i], replacements[i]);
                resolt = resolt.Replace(char.ToUpper(polishLetters[i]), char.ToUpper(replacements[i]));
            }

            return resolt;
        }

        public static string DigitsOnly(this string value) {
            if (!string.IsNullOrEmpty(value)) {
                var arr = value.ToCharArray();

                return arr.Where(item => char.IsNumber(item) || item == '.' || item == ',' || item == '-').Aggregate<char, string>(null, (current, item) => current + item);
            }

            return value;
        }

        public static int GetInt(this string value) {
            const int default_value = 0;

            if (!int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out int result) &&
                !int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = default_value;
            }

            return result;
        }

        public static short GetShort(this string value) {
            const short default_value = 0;

            if (!short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out short result) &&
                !short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = default_value;
            }

            return result;
        }

        public static float GetFloat(this string value) {
            const float default_value = 0;

            if (!float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out float result) &&
                !float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = default_value;
            }

            return result;
        }

        public static double GetDouble(this string value) {
            const double default_value = 0;

            if (!double.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out double result) &&
                !double.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !double.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = default_value;
            }

            return result;
        }

        public static decimal GetDecimal(this string value) {
            const decimal default_value = 0;

            if (!decimal.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal result) &&
                !decimal.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !decimal.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = default_value;
            }

            return result;
        }

        #region StringExtensions encryption

        public static string Encrypt(this string publicText) {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2    = new Rfc2898DeriveBytes(publicText, salt, 10000); // minimum is 1000
            var hash      = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool Decode(this string encryptedText, string publicText) {
            string savedPasswordHash = encryptedText;
            var    hashBytes         = Convert.FromBase64String(savedPasswordHash);

            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(publicText, salt, 10000); // minimum is 1000
            var hash   = pbkdf2.GetBytes(20);

            return hash.SequenceEqual(hashBytes.Skip(salt.Length));
        }

        #endregion

    }
}