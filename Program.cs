using static System.Console;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Ciphers
{
    class Program
    {
        static void Vizner(string orig)
        {
            string key = CreateKey("").ToUpper(), cipher = "", orig1 = orig.Replace(" ", string.Empty);
            CreateVizner(out char[,] vizner);
            int count = 0;
            for (int i = 0; i < orig1.Length; i++)
            {
                int j = i;
                if (j >= key.Length) 
                    if (count > 0) 
                    { 
                        j -= key.Length * count; 
                        if (j >= key.Length - 1) 
                            count++; 
                    } 
                    else 
                    { 
                        j -= key.Length; 
                        count++; 
                    } 
                cipher += vizner[key[j] - 65, orig1[i] - 65];
            }

            for (int i = 0; i < orig.Length; i++)
                if (orig[i] == ' ')
                    cipher = cipher.Insert(i, new string(' ', 1));

            WriteLine($"{orig}\n{key}\n{cipher}");
        }
        static void Vizner1(string cipher)
        {
            string key = CreateKey(""), orig = "", cipher1 = cipher.Replace(" ", string.Empty); ;
            CreateVizner(out char[,] vizner);
            int count = 0;
            for (int i = 0; i < cipher1.Length; i++)
            {
                int j = i; 
                if (j >= key.Length)
                {
                    if (count > 0)
                    { 
                        j -= key.Length * count; 
                        if (j == key.Length - 1)
                            count++;
                    } 
                    else 
                    { 
                        j -= key.Length; 
                        count++;
                    }
                }
                for (int c = 0; c < 26; c++)
                { 
                    if (vizner[key[j] - 65, c] == cipher1[i]) 
                    { 
                        orig += vizner[0, c]; 
                        break; 
                    } 
                }
            }

            for (int i = 0; i < cipher.Length; i++)
            {
                if (cipher[i] == ' ')
                    orig = orig.Insert(i, new string(' ', 1));
            }

            WriteLine($"{cipher}\n{key}\n{orig}");
        }
        static void CreateVizner(out char[,] vizner) 
        { 
            vizner = new char[26, 26]; 
            for (int i = 0; i < 26; i++) 
            { 
                for (int j = 65; j < 91; j++) 
                { 
                    int c = j + i;
                    if (c > 90) c -= 26;
                        vizner[i, j - 65] = (char)c;
                }
            }
        }

        static void Caesar(string orig)
        {
            char[] vs = orig.ToCharArray(); 
            string cipher = "";
            int key = CreateKey(0);
            foreach (var item in vs)
            {
                char c = item; 
                if (c >= 65 && c <= 90) 
                    En(ref c, key, 90, -26);          
                cipher += c; 
            }
            WriteLine($"{orig}\n{cipher}");       
        }
        static void Caesar1(string cipher)
        {
            cipher = cipher.ToUpper();
            char[] vs = cipher.ToCharArray(); 
            string orig = "";
            int key = CreateKey(0);
            foreach (var item in vs)
            {
                char c = item; 
                if (c >= 65 && c <= 90) 
                    En(ref c, -key, 65, 26);
                orig += c;           
            }            
            WriteLine(orig);
        }
        static void En(ref char c, int key, int border, int alp)
        {
            c = (char)(c + key);
            if (c > border)
                c = (char)(c + alp);
        }

        static void Transposition(string orig)
        {
            char[] cipher = orig.ToCharArray();
            string cipher1 = "";
            int key = CreateKey(0), count = 0;
            for (int i = 0; i < cipher.Length; i++)
            {
                count++;
                if (count == key)
                {
                    if (i == cipher.Length-1)
                    {
                        cipher1 += cipher[i];
                        break;
                    }
                    char temp = cipher[i];
                    cipher[i] = cipher[i + 1];
                    cipher[i + 1] = temp;
                    count = -1;
                }
                cipher1 += cipher[i];
            }

            WriteLine($"{orig}\n{cipher1}");
        }

        static void Xor(string orig)
        {
            string key = CreateKey(""), cipher = "";
            int count = 0;
            for (int i = 0; i < orig.Length; i++)
            {
                int j = i;
                if (j >= key.Length)
                {
                    if (count > 0)
                    {
                        j -= key.Length * count;
                        if (j >= key.Length - 1)
                            count++;
                    }
                    else
                    {
                        j -= key.Length;
                        count++;
                    }
                }
                int Ikey = key[j], Icipher = orig[i];
                cipher += (char)(Ikey ^ Icipher);
            }
            WriteLine($"{orig}\n{key}\n{cipher}");
        }

        static string Playfer(string orig)
        {
            string key = CreateKey("").ToUpper(), cipher = "";
            List<char> used = CreatePlayfer(new List<char>(), key);

            char[,] mas = new char[5,5];
            int count = 0;
            for (int i = 0; i < mas.GetLength(0); i++)
            {
                for (int j = 0; j < mas.GetLength(0); j++)
                {
                    mas[i, j] = used[(i * 4) + j + count];
                    Write($"{mas[i, j]}\t");
                }
                count++;
                WriteLine("");
            }
            WriteLine("");

            for (int i = 0; i < orig.Length; i += 2)
            {
                if (i == orig.Length - 1 && orig.Length % 2 == 1 || orig[i] == orig[i + 1])
                {
                    if (orig[i] != 'X')
                        orig = orig.Insert(i + 1, new string('X', 1));
                    else
                        orig = orig.Insert(i + 1, new string('Q', 1));
                } 
            }

            for (int c = 0; c < orig.Length; c += 2)
            {
                char a = orig[c], b = orig[c + 1];
                List<int> ij = new List<int> { -1, -1, -1, -1 };

                for (int i = 0; i < mas.GetLength(0); i++)
                {
                    for (int j = 0; j < mas.GetLength(0); j++)
                    {
                        if (ij[0] != -1 && ij[2] != -1)
                            break;

                        if (a == mas[i,j])
                        {
                            ij[0] = i;
                            ij[1] = j;
                        }
                        if (b == mas[i, j])
                        {
                            ij[2] = i;
                            ij[3] = j;
                        }
                    }
                }

                if (ij[1] == ij[3])
                {
                    if (ij[0] != 4)
                        a = mas[ij[0] + 1, ij[1]];
                    else
                        a = mas[ij[0] = 0, ij[1]];

                    if (ij[2] != 4)
                        b = mas[ij[2] + 1, ij[3]];
                    else
                        b = mas[ij[2] = 0, ij[3]];
                }
                else if (ij[0] == ij[2])
                {
                    if (ij[1] != 4)
                        a = mas[ij[0], ij[1] + 1];
                    else
                        a = mas[ij[0], ij[1] = 0];

                    if (ij[3] != 4)
                        b = mas[ij[2], ij[3] + 1];
                    else
                        b = mas[ij[2], ij[3] = 0];
                }
                else
                {
                    a = mas[ij[0], ij[3]];
                    b = mas[ij[2], ij[1]];
                }

                cipher += a;
                cipher += b;
            }

            WriteLine($"{orig}\n{key}\n{cipher}");
            return cipher;
        }
        static void Playfer1(string orig)
        {
            string key = CreateKey("").ToUpper(), cipher = "";
            List<char> used = CreatePlayfer(new List<char>(), key);

            char[,] mas = new char[5, 5];
            int count = 0;
            for (int i = 0; i < mas.GetLength(0); i++)
            {
                for (int j = 0; j < mas.GetLength(0); j++)
                {
                    mas[i, j] = used[(i * 4) + j + count];
                    Write($"{mas[i, j]}\t");
                }
                count++;
                WriteLine("");
            }
            WriteLine("");

            for (int c = 0; c < orig.Length; c += 2)
            {
                char a = orig[c], b = orig[c + 1];
                List<int> ij = new List<int> { -1, -1, -1, -1 };

                for (int i = 0; i < mas.GetLength(0); i++)
                {
                    for (int j = 0; j < mas.GetLength(0); j++)
                    {
                        if (ij[0] != -1 && ij[2] != -1)
                            break;

                        if (a == mas[i, j])
                        {
                            ij[0] = i;
                            ij[1] = j;
                        }
                        if (b == mas[i, j])
                        {
                            ij[2] = i;
                            ij[3] = j;
                        }
                    }
                }

                for (int i = 0; i < orig.Length; i += 2)
                {
                    if (i == orig.Length - 1 && orig.Length % 2 == 1 || orig[i] == orig[i + 1])
                    {
                        if (orig[i] != 'X')
                            orig = orig.Insert(i + 1, new string('X', 1));
                        else
                            orig = orig.Insert(i + 1, new string('Q', 1));
                    }
                }

                if (ij[1] == ij[3])
                {
                    if (ij[0] != 0)
                        a = mas[ij[0] - 1, ij[1]];
                    else
                        a = mas[ij[0] = 4, ij[1]];

                    if (ij[2] != 0)
                        b = mas[ij[2] - 1, ij[3]];
                    else
                        b = mas[ij[2] = 4, ij[3]];
                }
                else if (ij[0] == ij[2])
                {
                    if (ij[1] != 0)
                        a = mas[ij[0], ij[1] - 1];
                    else
                        a = mas[ij[0], ij[1] = 4];

                    if (ij[3] != 0)
                        b = mas[ij[2], ij[3] - 1];
                    else
                        b = mas[ij[2], ij[3] = 4];
                }
                else
                {
                    a = mas[ij[0], ij[3]];
                    b = mas[ij[2], ij[1]];
                }

                cipher += a;
                cipher += b;
            }

            WriteLine($"{orig}\n{key}\n{cipher}");
        }


        static List<char> CreatePlayfer(List<char> list, string str)
        {
            foreach (char i in str)
                if (list.IndexOf(i) == -1)
                    list.Add(i);

            for (int i = 65; i < 91; i++)
            {
                if (i == 74)
                    continue;

                if (list.IndexOf((char)i) == -1)
                    list.Add((char)i);
            }
            return list;
        }

        static string CreateKey(string key)
        {
            bool b = true;
            while (b)
            {
                Write("Введите ключ(только буквы английского алфавита): ");
                key = ReadLine();
                if (!key.Contains(' ') && Regex.IsMatch(key, @"[A-Za-z]", RegexOptions.IgnoreCase))
                    b = false;
            }
            Clear();
            return key;
        }
        static int CreateKey(int key)
        {
            bool b = true;
            while (b)
            {
                Write("Введите ключ(только цифры): ");
                if (int.TryParse(ReadLine(), out key))
                    b = false;
            }
            Clear();
            return key;
        }

        static void Main()
        {
            int on = 0;
            string str = "";
            while (on != 9)
            {
                if (on == 0)
                {
                    Write("Введите строку: ");
                    str = ReadLine();
                }
                bool b = true;
                while (b)
                {
                    Write("0. Другая строка для шифровки\n1. Шифр Виженера(шифровка)\n2. Шифр Виженера(дешифровка)\n3. Шифр Цезаря(шифровка)\n4. Шифр Цезаря(дешифровка)\n" +
                          "5. Транспозиция(шифровка\\дешифровка)\n6. Xor(шифровка\\дешифровка).\n7. Шифр Плейфера(шифровка)\n8. Шифр Плейфера(дешифровка)\n9. Выход\n");
                    Write("Введите пункт: ");
                    if (int.TryParse(ReadLine(), out on) && on >= 0 && on < 10)
                        b = false;
                    Clear();
                }

                switch (on)
                {
                    case 1: Vizner(str.ToUpper()); break;
                    case 2: Vizner1(str.ToUpper()); break;
                    case 3: Caesar(str.ToUpper()); break;
                    case 4: Caesar1(str.ToUpper()); break;
                    case 5: Transposition(str); break;
                    case 6: Xor(str); break;
                    case 7: Playfer(str.Replace(" ", string.Empty).ToUpper().Replace("J", "I")); break;
                    case 8: Playfer1(str.Replace(" ", string.Empty).ToUpper().Replace("J", "I")); break;
                }
            } 
        }
    }
}