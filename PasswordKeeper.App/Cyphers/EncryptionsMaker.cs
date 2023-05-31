using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.Cypher
{
    public class EncryptionsMaker //maybe static class?
    {
        public static string MakeCesarEncryption(string clearInput, int key = 10)
        {
            StringBuilder encryptedText = new StringBuilder();

            for (int i = 0; i < clearInput.Length; i++)
            {
                int encryption = clearInput[i] + key;
                encryptedText.Append(char.ConvertFromUtf32(encryption));
            }
            return encryptedText.ToString();
        }
    }
}
