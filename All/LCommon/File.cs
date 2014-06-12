using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace P2P_client
{
    
    [Serializable]
    public class File
    {
        private string Path;
        private string Path_real;
        public string Dirs;
        public string Name;
        private long Size;
        private int Count_Parts;
        public byte[][] Hashs;
        public int Part_size;
        private long Part_size_last;
        private byte[] Curent_BitMap;// 1- есть часть, 0 - нет части
        private byte[] Need_to_download_parts;// 0- нужно скачать, 1 - скачивается часть, 2 - скачаная часть 
        
        public string path_real
        {
            get { return Path_real; }
            set { Path_real = value; }
        }
        public int Count_Parts_
        {
            get { return Count_Parts; }

        }
        public string path
        {
            get { return Path; }

        }
        /// <summary>
        /// Загрузка информации из мета-файла
        /// </summary>
        /// <param name="Path_">Адресс к файлу</param>
        /// <param name="size_">Размер файла</param>
        /// <param name="Count_Parts_">Количество частей</param>
        /// <param name="Hashs_">Масив хешей частей</param>
        /// <param name="Part_Size_">Размер части</param>
        public File(string Path_, string Path_real, long size_, int Count_Parts_, byte[][] Hashs_, int Part_Size_)
        {
            Path = Path_;
            path_real = Path_real;
            Size = size_;
            Count_Parts = Count_Parts_;
            Hashs = Hashs_;
            Part_size = Part_Size_;
            Create_Curent_Bit_Map();
            Create_Download_Bit_Map();
            Parser();
            Create_dir();
            CheckHashes();


        }

        public void CheckHashes()
        {
            ProgramMaterials.cur_part = 0;
            if (!System.IO.File.Exists(Path_real))
            {
                Create_file();
                return;
            }
            FileStream fs = new FileStream(path_real, FileMode.Open);

            if (fs.Length != Size)
            {
                Create_file();
                fs.Close();
                return;
            }
            fs.Close();
            MD5 md5 = MD5.Create();

            for (int i = 0; i < Count_Parts; i++)
            {
                ProgramMaterials.cur_part = i;
                byte[] part = Read_Curent_Part(i);
                byte[] hashBytes = md5.ComputeHash(part);

                Curent_BitMap[i] = 1;

                for (int j = 0; j < 16; j++)
                {
                    if (hashBytes[j] != Hashs[i][j])
                    {
                        Curent_BitMap[i] = 0;

                        break;
                    }
                }
            }
            Create_Download_Bit_Map();

        }


        /// <summary>
        /// Метод проверяет весь ли файл скачан
        /// </summary>
        /// <returns>Возращяет True или False</returns>
        public bool IsFull()
        {
            for (int i = 0; i < Need_to_download_parts.Length; i++)
            {
                if (Need_to_download_parts[i] == 2)
                {

                }
                else
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Загрузка информации для докачки файла
        /// </summary>
        /// <param name="Path_">Адресс к файлу</param>
        /// <param name="size_">Размер файла</param>
        /// <param name="Count_Parts_">Количество частей</param>
        /// <param name="Hashs_">Масив хешей частей</param>
        /// <param name="Part_Size_">Размер части</param>
        /// <param name="Curent_BitMap_">Вид файла на данный момент</param>
        /// <param name="Need_to_download_parts_">Список частей которые надо скачать</param>
        public File(string Path_, long size_, int Count_Parts_, byte[][] Hashs_, int Part_Size_, byte[] Curent_BitMap_, byte[] Need_to_download_parts_)
        {
            Path = Path_;
            Size = size_;
            Count_Parts = Count_Parts_;
            Hashs = Hashs_;
            Part_size = Part_Size_;
            Curent_BitMap = Curent_BitMap_;
            Need_to_download_parts = Need_to_download_parts_;
            FileStream File_Stream;
            File_Stream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            File_Stream.Close();
            Parser();
        }
        /// <summary>
        /// Загрузка информации при создании раздачи 
        /// </summary>
        /// <param name="Path_">Адресс к файлу</param>
        /// <param name="Part_Size_">Размер части</param>
        public File(string path_real, string Path_, int Part_size_)
        {
            Path = Path_;
            Path_real = path_real;
            Part_size = Part_size_;
            Parser();

            Create_Size();
            Create_Count_Parts();
            Create_Hashs();

            Curent_BitMap = new byte[Count_Parts];
            Need_to_download_parts = new byte[Count_Parts];
            for (int i = 0; i < Count_Parts; i++)
            {
                Curent_BitMap[i] = 1;
                Need_to_download_parts[i] = 2;
            }

        }
        private void Create_Hashs()
        {
            int part_number = 0;
            Hashs = new byte[Count_Parts][];
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            for (int i = 0; i < Count_Parts; i++)
            {
                part_number = i;
                byte[] part = Read_Curent_Part(part_number);
                Hashs[i] = md5.ComputeHash(part);
            }

        }
        private byte[] Read_Curent_Part(int part_number)
        {
            FileStream File_Stream;
            File_Stream = new FileStream(Path_real, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            byte[] part = new byte[Part_size];
            int pos_start_to_read_part = Part_size * (part_number);



            File_Stream.Seek(pos_start_to_read_part, SeekOrigin.Begin);
            if (part_number < Count_Parts - 1)
            {
                File_Stream.Read(part, 0, Part_size);
            }
            else
            {
                int lastpart = (int)(Size - (long)(Part_size * part_number));
                File_Stream.Read(part, 0, lastpart);
            } File_Stream.Close();
            return part;//Если часть найдена считывает и возвращяет ёё




        }
        private void Create_Count_Parts()
        {
            int temp = Convert.ToInt32(Size / Part_size);
            long temp2 = Part_size * temp;
            temp2 = Size - temp2;
            if (temp2 > 0)
            {
                Part_size_last = temp2;
                temp++;
            }
            Count_Parts = temp;

        }
        private void Create_Size()
        {
            FileStream File_Stream;
            File_Stream = new FileStream(Path_real, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Size = File_Stream.Length;
            File_Stream.Close();
        }
        public void Create_Download_Bit_Map()
        {
            Need_to_download_parts = new byte[Count_Parts];
            for (int i = 0; i < Count_Parts; i++)
            {
                if (Curent_BitMap[i] == 1)
                {
                    Need_to_download_parts[i] = 2;
                }
                else
                {
                    Need_to_download_parts[i] = 0;
                }


            }

        }
        private void Create_Curent_Bit_Map()
        {
            Curent_BitMap = new byte[Count_Parts];
            for (int i = 0; i < Count_Parts; i++)
            {
                Curent_BitMap[i] = 0;
            }
        }
        private void Parser()
        {
            string filename = Path_real;
            string dirname = "";
            int i = filename.IndexOf('\\');

            while (i > -1)
            {
                dirname += filename.Substring(0, i + 1);
                filename = filename.Substring(i + 1);
                i = filename.IndexOf('\\');
            }

            Name = filename;
            Dirs = dirname;
        }
        /// <summary>
        /// Получения списка частей которіе нужно скачать
        /// </summary>
        /// <returns>Возвращяет масив где 0- нужно скачать, 1 - скачивается часть, 2 - скачаная часть - </returns>
        public byte[] Get_Download_Bit_Map()
        {
            return Need_to_download_parts;
        }
        /// <summary>
        /// Вносит изминения в БитМап
        /// </summary>
        /// <param name="number_part">Номер части которую нужно изменить</param>
        /// <param name="flag">Флаг изминения: 0- нужно скачать, 1 - скачивается часть, 2 - скачаная часть</param>
        private void Set_Download_Bit_Map(int number_part, byte flag)
        {
            Need_to_download_parts[number_part] = flag;
        }
        /// <summary>
        /// Вносит изминения в БитМап Файла
        /// </summary>
        /// <param name="number_part">Номер части которую нужно изменить</param>
        /// <param name="flag">Флаг изминения: 1- есть часть, 0 - нет части</param>
        private void Set_Curent_Bit_Map(int number_part, byte flag)
        {
            Curent_BitMap[number_part] = flag;
        }
        /// <summary>
        /// Проверка есть эта часть или нет
        /// </summary>
        /// <param name="number_part">Номер части</param>
        /// <returns>true - если часть есть false - если чати нет </returns>
        public bool IsPart(int number_part)
        {
            if (Curent_BitMap[number_part] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Получает чать 
        /// </summary>
        /// <param name="number_part">Номер части</param>
        /// <returns>Возвращяет часть если она есть, если нет то возвращяет null</returns>
        public byte[] Get_Part(int number_part)
        {
            if (IsPart(number_part))
            {
                FileStream File_Stream;
                File_Stream = new FileStream(Path_real, FileMode.Open, FileAccess.Read);
                byte[] part = new byte[Part_size];
                int pos_start_to_read_part = Part_size * (number_part);

                File_Stream.Seek(pos_start_to_read_part, SeekOrigin.Begin);
                if (number_part < Count_Parts - 1)
                {

                    File_Stream.Read(part, 0, Part_size);
                }
                else
                {
                    int lastpart = (int)(Size - (long)(Part_size * number_part));
                    File_Stream.Read(part, 0, lastpart);
                }
                File_Stream.Close();

                return part;
            }
            else
            {

                return null;
            }

        }
        /// <summary>
        /// Записывает часть
        /// </summary>
        /// <param name="number_part_">Номер части</param>
        /// <param name="part_">Часть</param>
        public void Set_Part(int number_part_, byte[] part_)
        {
            FileStream File_Stream;
            File_Stream = new FileStream(Path_real, FileMode.Open, FileAccess.Write);
            byte[] part = part_;
            int number_of_part = number_part_;
            int pos_start_to_write_part = 0;


            pos_start_to_write_part = Part_size * (number_of_part);
            File_Stream.Seek(pos_start_to_write_part, SeekOrigin.Begin);
            if (number_of_part < Count_Parts - 1)
                File_Stream.Write(part, 0, Part_size);
            else
            {
                int lastpart = (int)(Size - (long)(Part_size * number_part_));
                File_Stream.Write(part, 0, (int)lastpart);
            }
            Set_Curent_Bit_Map(number_part_, 1);
            Set_Download_Bit_Map(number_part_, 2);

            File_Stream.Close();
        }

        private void Create_file()
        {
            if (System.IO.File.Exists(Path_real)) return;
            FileStream File_Stream;
            File_Stream = new FileStream(Path_real, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (Size == File_Stream.Length) return;
            File_Stream.Seek(0, SeekOrigin.Begin);
            for (long i = 0; i < Size; i++)
            {
                File_Stream.WriteByte(0);
            }

            File_Stream.Close();
        }

        private void Create_dir()
        {
            Directory.CreateDirectory(Dirs); ;
        }
        /// <summary>
        /// Размер файла
        /// </summary>
        public long File_Size
        {
            get { return Size; }
        }
    }
}
