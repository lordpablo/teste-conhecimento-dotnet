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

        public static bool IsEmpty(this string obj)
        {
            return string.IsNullOrWhiteSpace(obj);
        }

        public static bool IsCpf(this string numero)
        {
            if (numero.IsEmpty()) return false;

            var cpf = numero
                .Where(char.IsNumber)
                .Select(c => (int)char.GetNumericValue(c))
                .ToArray();

            if (cpf.Length != 11) return false;

            var v = new int[2];

            //Nota: Calcula o primeiro dígito de verificação.
            v[0] = 10 * cpf[0] + 9 * cpf[1] + 8 * cpf[2];
            v[0] += 7 * cpf[3] + 6 * cpf[4] + 5 * cpf[5];
            v[0] += 4 * cpf[6] + 3 * cpf[7] + 2 * cpf[8];
            v[0] = 11 - v[0] % 11;
            v[0] = v[0] >= 10 ? 0 : v[0];

            //Nota: Calcula o segundo dígito de verificação.
            v[1] = 11 * cpf[0] + 10 * cpf[1] + 9 * cpf[2];
            v[1] += 8 * cpf[3] + 7 * cpf[4] + 6 * cpf[5];
            v[1] += 5 * cpf[6] + 4 * cpf[7] + 3 * cpf[8];
            v[1] += 2 * v[0];
            v[1] = 11 - v[1] % 11;
            v[1] = v[1] >= 10 ? 0 : v[1];

            //Nota: Verdadeiro se os dígitos de verificação são os esperados.
            return (v[0] == cpf[9] && v[1] == cpf[10]);
        }

        public static bool IsCnpj(this string numero)
        {
            if (numero.IsEmpty()) return false;

            var cnpj = numero
                .Where(char.IsNumber)
                .Select(c => (int)char.GetNumericValue(c))
                .ToArray();

            if (cnpj.Length != 14) return false;

            //Nota: Dígitos de verificação.
            var v = new int[2];

            //Nota: Calcula o primeiro dígito de verificação.
            v[0] = 5 * cnpj[0] + 4 * cnpj[1] + 3 * cnpj[2] + 2 * cnpj[3];
            v[0] += 9 * cnpj[4] + 8 * cnpj[5] + 7 * cnpj[6] + 6 * cnpj[7];
            v[0] += 5 * cnpj[8] + 4 * cnpj[9] + 3 * cnpj[10] + 2 * cnpj[11];
            v[0] = 11 - v[0] % 11;
            v[0] = v[0] >= 10 ? 0 : v[0];

            //Nota: Calcula o segundo dígito de verificação.
            v[1] = 6 * cnpj[0] + 5 * cnpj[1] + 4 * cnpj[2] + 3 * cnpj[3];
            v[1] += 2 * cnpj[4] + 9 * cnpj[5] + 8 * cnpj[6] + 7 * cnpj[7];
            v[1] += 6 * cnpj[8] + 5 * cnpj[9] + 4 * cnpj[10] + 3 * cnpj[11];
            v[1] += 2 * v[0];
            v[1] = 11 - v[1] % 11;
            v[1] = v[1] >= 10 ? 0 : v[1];

            //Nota: Verdadeiro se os dígitos de verificação são os esperados.
            return (v[0] == cnpj[12] && v[1] == cnpj[13]);
        }
        public static bool IsCpfCnpj(this string numero)
        {
            return IsCpf(numero) || IsCnpj(numero);
        }

        public static bool IsOlderThan18(DateTime birthDate)
        {
            // Obtém a data atual
            DateTime today = DateTime.Today;

            // Calcula a idade
            int age = today.Year - birthDate.Year;

            // Ajusta se o aniversário ainda não ocorreu neste ano
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            // Verifica se é maior ou igual a 18 anos
            return age >= 18;
        }
    }
}
