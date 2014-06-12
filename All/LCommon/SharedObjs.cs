using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using bc = System.BitConverter;
namespace P2P_client
{
    public class Config
    {
        /// <summary>
        /// Открытый порт для входящих подключений
        /// </summary>
        static public short Port = 8090;


        /// <summary>
        /// Таймаут обновления
        /// </summary>
        static public int UpdateTimeout=15*60*1000;

        /// <summary>
        /// Максимальное количество подключений
        /// </summary>
        static public int ConnectionsMax=100;


        /// <summary>
        /// Количество открытых подключений
        /// </summary>
        static public int Connections = 0;

        /// <summary>
        /// Объект службы CTE
        /// </summary>
        static public CTE CTE_Service = new CTE();
        static public bool idtmode=false;

        static public bool CTE_Enaibled=true;
        static public bool IDT_Enaibled=true;

        static public IDT IDT_Service = new IDT();



        static public void Save()
        {
            FileStream fs = new FileStream("config", FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(bc.GetBytes(Port),0, 2);
            fs.Write(bc.GetBytes(UpdateTimeout), 0, 4);
            fs.Write(bc.GetBytes(ConnectionsMax), 0, 4);
            byte[] cte = bc.GetBytes(CTE_Enaibled);
            byte[] idt = bc.GetBytes(IDT_Enaibled);
            byte[] mod = bc.GetBytes(idtmode);
            fs.Write(cte, 0, cte.Length);
            fs.Write(idt, 0, idt.Length);
            fs.Write(mod, 0, mod.Length);
            fs.Close();
        }
        static public void Open()
        {
            if (!System.IO.File.Exists("config")) return;

            FileStream fs = new FileStream("config", FileMode.Open, FileAccess.Read);
            byte[] temp = new byte[4];

            fs.Read(temp, 0, 2);
            Port = bc.ToInt16(temp, 0);

            fs.Read(temp, 0, 4);
            UpdateTimeout = bc.ToInt32(temp, 0);


            fs.Read(temp, 0, 4);
            ConnectionsMax = bc.ToInt32(temp, 0);

            CTE_Enaibled = Convert.ToBoolean(fs.ReadByte());
            IDT_Enaibled = Convert.ToBoolean(fs.ReadByte());
            idtmode = Convert.ToBoolean(fs.ReadByte());


            fs.Close();
        }
    }
    public class ProgramMaterials
    {
        public static List<Material> MaterialsArray;
        public static int cur_file;
        public static int cur_part;

    }
}
