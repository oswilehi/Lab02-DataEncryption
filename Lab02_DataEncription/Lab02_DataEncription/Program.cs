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
            string filePath = "", entry = "", type = "";
            int key = 0, mod = 0;
            bool keepRunning = true;
            
            while (keepRunning)
            {
                RSA rsa = new RSA();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Encrypt:\n-c -f\"C:\\filePath.txt\"\nDecrypt:\n-d -f\"C:\\filePath.cif\" -k 143,200\"\nClose:\nx");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Write the path here: ");

                Console.SetCursorPosition(21, Console.CursorTop - 1);
                entry = Console.ReadLine();

                if (!Utilities.Validation(entry, ref filePath, ref type, ref mod, ref key))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("¡Error! Asegúrese de haber escrito correctamente los comandos del programa. Verifique que exista el archivo.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    switch (type.ToLower())
                    {
                        case "-c":
                            rsa.GenerateKeys(Utilities.MaxValueOfText(filePath));
                            Console.WriteLine("Your private key is:" + rsa.n.ToString() + "," + rsa.privateKey.ToString());
                            rsa.EncryptionRSA(filePath);
                            Utilities.MessageC();
                            break;
                        case "-d":
                            rsa.DecryptionRSA(filePath, mod, key);
                            Utilities.MessageD();
                            break;
                        case "x":
                            keepRunning = false;
                            break;
                        default:
                            Console.WriteLine("To close the program write X");
                            Console.Clear();
                            break;
                    }
                }
            }
            Environment.Exit(0);
        }
    }
}
