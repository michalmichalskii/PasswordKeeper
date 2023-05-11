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
            int encryption = 0;
            string encryptedText = "";

            for (int i = 0; i < clearInput.Length; i++)
            {
                int textUser = clearInput[i];
                encryption = textUser + key;
                encryptedText += char.ConvertFromUtf32(encryption);
            }
            return encryptedText;
        }
    }
}
