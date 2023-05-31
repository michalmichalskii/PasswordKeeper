using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.Cypher
{
    public class DescryptionMaker
    {
        public static string MakeCesarDescryption(string clearInput, int key = 10)
        {
            StringBuilder descryptedText = new StringBuilder();

            for (int i = 0; i < clearInput.Length; i++)
            {
                int descryption = clearInput[i] + key;
                descryptedText.Append(char.ConvertFromUtf32(descryption));
            }
            return descryptedText.ToString();
        }
    }
}
