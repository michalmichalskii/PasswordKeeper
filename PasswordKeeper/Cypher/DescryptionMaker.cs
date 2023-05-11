using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.Cypher
{
    public class DescryptionMaker //maybe static class?
    {
        public static string MakeCesarDescryption(string clearInput, int key = 10)
        {
            int descryption = 0;
            string descryptedText = "";

            for (int i = 0; i < clearInput.Length; i++)
            {
                int textUser = clearInput[i];
                descryption = textUser + key;
                descryptedText += char.ConvertFromUtf32(descryption);
            }
            return descryptedText;
        }
    }
}
