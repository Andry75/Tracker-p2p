using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace p2p_Server
{
    class Encript
    {
        public static byte[] Encode(long input)
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
        public static long Decoding (byte[] input)
        {
            //bool isOk=false;
            long output;
            byte[] not_hashed_long= new byte[16];
            byte [] temp = new byte[16];
            byte[] long_ = new byte[8];
            MD5 coder = MD5.Create();
            for (int i = 0; i < 16; i++)
            {
                for (int j =0;j<16;j++)
                {
                    temp[j] = input[i*16+j];
                }
                for (int a = 0; a < 256; a++)
                {
                    bool correct = true;
                    byte[] ab = new byte[1]; ab[0]=(byte)a;
                    byte[] hash = coder.ComputeHash(ab);
                    for (int t = 0; t < 16; t++)
                        if (temp[t] != hash[t])
                        { correct = false; break; }
                    if(correct)
                    {
                        not_hashed_long[i] = (byte)a;
                        break;
                    }
                }
            }

             long_ [0] = not_hashed_long[0];
             long_[1] = not_hashed_long[3];
             long_[2] = not_hashed_long[5];
             long_[3] = not_hashed_long[6];
             long_[4] = not_hashed_long[11];
             long_[5] = not_hashed_long[13];
             long_[6] = not_hashed_long[14];
             long_[7] = not_hashed_long[15];
                output=BitConverter.ToInt64(long_,0);
            
            /*1 - нужная информация
             * 2 - не нужная
             * |1|2|2|1|2|1|1|2|2|2|2|1|2|1|1|1|
             * 
             */

           

            return output;
        }
    }
}
