using System.Text.RegularExpressions;

namespace UserModule.Utilities
{
    public partial class RegexValidations
    {
        [GeneratedRegex("^[a-zA-Z]+$")]
        private static partial Regex NameRegex();
        [GeneratedRegex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
        private static partial Regex EmailRegex();
        [GeneratedRegex("^[0-9]{11}$")]
        private static partial Regex PhoneRegex();
        public static bool IsValidMail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return EmailRegex().IsMatch(email);
        }

        public static bool IsValidName(string firstName, string lastName, string middleName = "")
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                return false;

            if (!string.IsNullOrWhiteSpace(middleName) && !NameRegex().IsMatch(middleName))
                return false;

            if (!NameRegex().IsMatch(firstName) || !NameRegex().IsMatch(lastName))
                return false;

            return true;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            return PhoneRegex().IsMatch(phoneNumber);
        }
    }
}
