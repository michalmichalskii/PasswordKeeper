using PasswordKeeper.App.Common;
using PasswordKeeper.Domain;
using PasswordKeeper.Domain.Entity;

namespace PasswordKeeper.App.Concrete
{
    public class UserDataService : BaseService<User>
    {
        public string GenerateRandomPassword()
        {
            char[] alphabet = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] numbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] specialChars = new[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', '{', ']', '}', ';', ':', '\'', '\"', ',', '.', '<', '>', '/', '?', '\\' };
            bool hasUpperLetter = false, hasLowerLetter = false, hasSpecial = false, hasNumber = false;

            string randomPassword = "";

            while (!hasLowerLetter && !hasUpperLetter && !hasNumber && !hasSpecial)
            {
                randomPassword = "";

                var random = new Random();
                var randomLengthOfPassword = random.Next(16, 20);

                for (int i = 0; i < randomLengthOfPassword; i++)
                {
                    var randomNumbers = random.Next(numbers.Length);
                    var randomLetter = random.Next(alphabet.Length);
                    var randomSpecial = random.Next(specialChars.Length);
                    var randomUpperOrNot = random.Next(2);

                    var randomNumberOrLetterOrSpecial = random.Next(1, 4);

                    switch (randomNumberOrLetterOrSpecial)
                    {
                        case 1:
                            if (randomUpperOrNot == 1)
                            {
                                randomPassword += char.ToUpper(alphabet[randomLetter]);
                                hasUpperLetter = true;
                            }
                            else
                            {
                                randomPassword += alphabet[randomLetter];
                                hasLowerLetter = false;
                            }
                            break;
                        case 2:
                            randomPassword += numbers[randomNumbers];
                            hasNumber = true;
                            break;
                        case 3:
                            randomPassword += specialChars[randomSpecial];
                            hasSpecial = true;
                            break;
                    }
                }
            }
            return randomPassword;
        }
    }
}