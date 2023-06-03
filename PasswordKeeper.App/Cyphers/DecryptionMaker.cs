using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.Cypher
{
    public class DecryptionMaker
    {
        public static string MakeCesarDecryption(string clearInput, int key = 10)
        {
            StringBuilder decryptedText = new StringBuilder();

            for (int i = 0; i < clearInput.Length; i++)
            {
                int decryption = clearInput[i] + key;
                decryptedText.Append(char.ConvertFromUtf32(decryption));
            }
            return decryptedText.ToString();
        }
    }
}
