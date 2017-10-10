using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab02_DataEncription
{
    class Utilities
    {
       
        /// <summary>
        /// This method validates that the user writes all the commands correctly.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <param name="mod"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Validation(string entry, ref string filePath, ref string type, ref int mod, ref int key)
        {
            string[] current = entry.Split(' ');
            if (current[0].ToLower() == "x")
            {
                type = "X";
                return true;
            }
            else
            {
                if (current.Length == 2)
                {
                    if (!(current[0] == "-c"))
                        return false;
                    type = current[0];
                    string[] path = current[1].Split('"');
                    if (!(path.Length > 3))
                    {
                        if (!(path[0] == "-f" && File.Exists(path[1])))
                            return false;
                        filePath = path[1];
                    }
                    else
                        return false;

                    return true;

                }
                else if (current.Length == 4)
                {
                    if (!(current[0] == "-d"))
                        return false;
                    type = current[0];
                    string[] path = current[1].Split('"');
                    if (!(path.Length > 3))
                    {
                        if (!(path[0] == "-f" && File.Exists(path[1])))
                            return false;
                        filePath = path[1];
                    }
                    else
                        return false;
                    if (!(current[2] == "-k"))
                        return false;
                    var privateKey = current[3].Split(',');
                    if (privateKey.Length == 2)
                    {
                        return int.TryParse(privateKey[0], out mod) && int.TryParse(privateKey[1], out key);
                    }
                    else
                        return false;

                }
                else
                    return false;
            }

        }
        /// <summary>
        /// This methods show if the message was encrypted or decrypted correctly
        /// </summary>
        public static void MessageC()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The document was successfully encrypted.\n\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void MessageD()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Document was successfully decrypted.\n\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// This method gets the highest value of the file to encrypt or decrypt
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns></returns>
        public static int MaxValueOfText(string path)
        {
            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    byte[] values = reader.ReadBytes((int)file.Length);
                    return values.Max();
                }
            }
        }
    }
}
