using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Lab02_DataEncription
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo f;

            string filePath= "", entry= "", type= "";
            int key = 0, mod = 0;
            bool exit = true;
            while (exit)
            {
                RSA rsa = new RSA();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Example: -d -f\"c:/filePath.txt\" -k 123,921 \n -c -f\"c:/filePath.txt\" \n[X] Close the program");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("c:/encryption/:" );
                
                Console.SetCursorPosition(15, Console.CursorTop - 1);
                entry = Console.ReadLine();

                if (!Validation(entry, ref filePath, ref type,ref mod, ref key))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("¡Error! Asegúrese de haber escrito correctamente los comandos del programa. Verifique que exista el archivo.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    //Here goes RSA
                    // exit = true;
                    switch (type.ToLower())
                    {
                        case "-c":
                            Console.WriteLine("Write your user to generate key:");
                            Console.SetCursorPosition(32, Console.CursorTop - 1);
                            var user = Console.ReadLine();
                            rsa.GenerateKeys();
                            Console.WriteLine("Your private key is:" + rsa.n.ToString() + ","+ rsa.privateKey.ToString());
                            rsa.EncryptionRSA(filePath);
                            messageC();
                            
                            break;
                       case "-d":
                            rsa.DecryptionRSA(filePath,mod,key);
                            messageD();
                            break;
                        case "X":
                            exit = false;
                            break;
                        default:
                            Console.WriteLine("To close the program write X");
                            Console.Clear();
                            break;
                    }
                }

                
            }
                     
        }
        //Validation
        static bool Validation(string entry, ref string filePath, ref string type,ref int mod ,ref int key)
        {
            string[] current = entry.Split(' ');
            if (current[0].ToLower() == "x")
            {
                type = "X";
                return true;
            }
            else
            {
                if ( current.Length == 2)
                {
                    if (!(current[0] == "-c" ))
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
        //operation message 
        static void messageC()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The document was successfully encrypted.\n\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void messageD()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Document is successfully decrypted.\n\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
