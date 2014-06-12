using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace P2P_client
{
    public class Encript
    {
        static public byte[] Encode(long input)
        {
            byte[] output = new byte[256];

            byte[] value = BitConverter.GetBytes(input);
            byte[] temp_not_hashed = new byte[16];
            /*1 - нужная информация
             * 2 - не нужная
             * |1|2|2|1|2|1|1|2|2|2|2|1|2|1|1|1|
             * 
             */
            
             new Random(42).NextBytes(temp_not_hashed);
             temp_not_hashed[0] = value[0];
             temp_not_hashed[3] = value[1];
             temp_not_hashed[5] = value[2];
             temp_not_hashed[6] = value[3];
             temp_not_hashed[11] = value[4];
             temp_not_hashed[13] = value[5];
             temp_not_hashed[14] = value[6];
             temp_not_hashed[15] = value[7];
            
             for (int i = 0; i < 16; i++)
             {
                 MD5 md5 = System.Security.Cryptography.MD5.Create();

                 byte[] t = new byte[1];
                 t[0] = temp_not_hashed[i];
                 byte[] hashBytes = md5.ComputeHash(t);

                 for (int a = 0; a<16; a++)
                 {
                     output[i * 16 + a] = hashBytes[a];
                 }
             }

            return output;
        }
    }
}
