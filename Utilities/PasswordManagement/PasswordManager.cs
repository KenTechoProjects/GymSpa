using System.Text.RegularExpressions;

namespace Utilities.PasswordManagement
{
    public class ValidatePassword
    {
        //public bool ValidatePassword(string password, out string ErrorMessage)
        //{
        //    var input = password;
        //    ErrorMessage = string.Empty;

        //    if (string.IsNullOrWhiteSpace(input))
        //    {
        //        throw new Exception("Password should not be empty");
        //    }

        //    var hasNumber = new Regex(@"[0-9]+");
        //    var hasUpperChar = new Regex(@"[A-Z]+");
        //    var hasMiniMaxChars = new Regex(@".{8,15}");
        //    var hasLowerChar = new Regex(@"[a-z]+");
        //    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        //    if (!hasLowerChar.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should contain at least one lower case letter.";
        //        return false;
        //    }
        //    else if (!hasUpperChar.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should contain at least one upper case letter.";
        //        return false;
        //    }
        //    else if (!hasMiniMaxChars.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should not be lesser than 8 or greater than 15 characters.";
        //        return false;
        //    }
        //    else if (!hasNumber.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should contain at least one numeric value.";
        //        return false;
        //    }

        //    else if (!hasSymbols.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should contain at least one special case character.";
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        public class PasswordAdvisor
        {
            public static PasswordScore CheckStrength(string password)
            {
                int score = 0;

                if (password.Length < 1)
                    return PasswordScore.Blank;
                if (password.Length < 4)
                    return PasswordScore.VeryWeak;

                if (password.Length >= 8)
                    score++;
                if (password.Length >= 12)
                    score++;
                if (Regex.Match(password, @"/\d+/", RegexOptions.ECMAScript).Success)
                    score++;
                if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success &&
                  Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                    score++;
                if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
                    score++;

                return (PasswordScore)score;
            }
        }
    }
}