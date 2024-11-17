using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SampleTest.Resources.Utils
{
    public static class FunctionsUtil
    {
        // Verifica se uma string possui apenas algarismos
        public static bool StringIsOnlyNumbers(string s) => s.All(char.IsDigit);

        // Remove acentuação de strings
        public static string RemoveAccents(this string text)
        {
            var sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (var letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string GenerateAppOfflineLoginSHA256Hash(string data)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(data));

            var stringBuilder = new StringBuilder();

            foreach (var t in hash)
            {
                stringBuilder.Append(t.ToString("x2")); // two hexadecimals uppercase
            }
            return stringBuilder.ToString();
        }

        public static string GenerateSHA256Hash(string data, IConfiguration configuration)
        {

            const string key = "Authentication:SaltKey";

            var salt = configuration.GetSection(key)?.Value ?? configuration.GetSection(key.Replace(":", "__"))?.Value;

            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(salt + data));
            var stringBuilder = new StringBuilder();

            foreach (var t in hash)
            {
                stringBuilder.Append(t.ToString("x2")); // two hexadecimals uppercase
            }
            return stringBuilder.ToString();
        }

        public static string GenerateSHA256Hash(string data)
        {

            const string key = "Authentication:SaltKey";

            var salt = Settings.Instance.AppSettings(key);

            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(salt + data));
            var stringBuilder = new StringBuilder();

            foreach (var t in hash)
            {
                stringBuilder.Append(t.ToString("x2")); // two hexadecimals uppercase
            }
            return stringBuilder.ToString();
        }

        public static string ToHex(string value)
        {
            byte[] ba = Encoding.Default.GetBytes(value);
            var hexString = BitConverter.ToString(ba).Replace("-", "");
            return hexString;
        }

        public static string FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return Encoding.ASCII.GetString(raw);
        }

        private static readonly Dictionary<string, byte[]> ImageMimeTypes = new()
        {
            {"image/jpeg", new byte[] {255, 216, 255}},
            {"image/jpg", new byte[] {255, 216, 255}},
            {"image/png", new byte[] {137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82}}
        };

        public static bool ValidarExtensaoArquivoImagem(IEnumerable<byte> file, string contentType)
        {
            var (_, value) = ImageMimeTypes.SingleOrDefault(x => x.Key.Equals(contentType));

            return file.Take(value.Length).SequenceEqual(value);
        }

        private static readonly Dictionary<string, string> DocsMimeTypes = new()
        {
            {"doc", "application/msword"},
            {"xls", "application/vnd.ms-excel"},
            {"docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {"pdf", "application/pdf"},
            {"jpeg", "image/jpeg"},
            {"jpg", "image/jpg"},
            {"png", "image/png"}
        };

        public static bool ValidarExtensaoArquivoDocs(string contentType)
        {
            return DocsMimeTypes.Any(x => x.Value.Equals(contentType));
        }

        public static string DataToString(DateTime? data)
        {
            if (data == null)
                return "";
            else return data?.ToString("dd/MM/yyyy");
        }
    }
}
