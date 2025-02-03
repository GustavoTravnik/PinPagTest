namespace BankServices.Utils
{
    public class CpfValidator
    {
        public static string? GetValidCpf(string? cpf)
        {
            if (cpf == null) return null;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
                return null;

            if (!IsValidCpfDigit(cpf, 9, [10, 9, 8, 7, 6, 5, 4, 3, 2]) ||
                !IsValidCpfDigit(cpf, 10, [11, 10, 9, 8, 7, 6, 5, 4, 3, 2]))
            {
                return null;
            }

            return cpf;
        }

        private static bool IsValidCpfDigit(string cpf, int length, int[] weights)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
                sum += int.Parse(cpf[i].ToString()) * weights[i];

            int digit = 11 - (sum % 11);
            if (digit >= 10) digit = 0;

            return digit == int.Parse(cpf[length].ToString());
        }
    }
}
