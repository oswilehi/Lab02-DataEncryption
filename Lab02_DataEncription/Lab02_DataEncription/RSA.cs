using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;


namespace Lab02_DataEncription
{
    class RSA
    {
        /*ALgoritmo:
        1. Generar las llaves privadas y públicas son dos números primos
        2.  Cifrar el mensaje P^e = E ( mod n ) P es el mensaje en texto plano,n y e son la clave pública,E es el mensaje cifrado*/


        private int p;
        
        private int q;
        public int n { get; set; }  //llave pública y privada
        /// <summary>
        /// Public key
        /// </summary>

        public int publicKey { get; set; }// Lave pública

        /// <summary>
        /// Private key
        /// </summary>
        public int privateKey { get; set; } //llave privada

        public RSA()
        {

        }
        
        /// <summary>
        /// Method to generate a prime number using a function
        /// </summary>
        private void GeneratePrimeNumber()
        {
            Random r = new Random();

            var n = r.Next(0, 14); //de cero a 14 para que la función PolinomialToGeneratePrimeNumber devuelva un valor entre 0 y 251

        

            p = PolinomialToGeneratePrimeNumber(n);
            do
            {
                n = r.Next(0, 14);
                q = PolinomialToGeneratePrimeNumber(n);
            } while (q.CompareTo(p) == 0);

        }
        /// <summary>
        /// Function polinomial to generate a prime number. The min value is 41 and the max value is 1601
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int PolinomialToGeneratePrimeNumber(int n)
        {
            //Polinomio de Euler que generan un número primo. El valor de n va entre cero y treinta nueve.
            return (int)Math.Pow(n, 2) + n + 41;
        }
        /// <summary>
        /// This method multiply both p and q
        /// </summary>
        /// <returns>p*q</returns>
        private int GenerateValueN()
        {
            return p * q;

        }


        /// <summary>
        /// This method find the value phi eulier of the numbers p and q
        /// </summary>
        /// <returns>Phi eulier value</returns>
        private int PhiEulier()
        {
            return (p - 1) * (q - 1);
        }
        /// <summary>
        /// This method find a number called e that is co-prime number with another number called n
        /// </summary>
        /// <param name="n"></param>
        /// <returns>co-prime number of the parameter n</returns>
        private int CoprimeNumber(int n)
        {
            int e = 1;
            do
                e++;
            while (ModBetweenTwoNumbers(e, n) == 0 || NoIsPrime(e));

            return e;
        }
        /// <summary>
        /// Method to apply the operation mod between two numbers
        /// </summary>
        /// <param name="e">number 1</param>
        /// <param name="n">number 2</param>
        /// <returns>mod between e and n</returns>
        private int ModBetweenTwoNumbers(int e, int n)
        {
            return n % e;
        }
        /// <summary>
        /// Method to validate if the parameter number is a prime number.
        /// </summary>
        /// <param name="number">A number</param>
        /// <returns>True: no is a prime number, False: is a prime number</returns>
        private bool NoIsPrime(int number)
        {
            int count = 0;
            for (int i = 1; i < number; i++)
            {
                if (number % i == 0)
                {
                    count++;
                }
            }
            return count > 2 ? true : false;
        }
        /// <summary>
        /// This method create the private key
        /// </summary>
        /// <param name="z"></param>
        /// <param name="e"></param>
        /// <returns>private key</returns>
        private int GeneratePrivateKey(int z, int e)
        {
            var aux = z;
           
            do
            {
                aux++;
            } while (aux % z != 1);
            aux = aux / e;
            
            return aux;
        }
        /// <summary>
        /// Method to generate private and public key
        /// </summary>
        public void GenerateKeys()
        {
            GeneratePrimeNumber();
            n = GenerateValueN();
            var  z = PhiEulier();
            publicKey = CoprimeNumber(z);
            privateKey = GeneratePrivateKeyWithModInverse(publicKey,z);
           
              
        }
        
        private int GeneratePrivateKeyWithModInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }


        public byte[] Encryption(byte[] plainText)
        {

            // 2.Cifrar el mensaje P ^ e = E(mod n) P es el mensaje en texto plano,n y e son la clave pública,E es el mensaje cifrado
            //y = x^e mod n  y = E(x) be the encryption function where x is an integer and y is            the encrypted form of x
            byte[] encryptedData = new byte[plainText.Length];

            for (int i = 0; i < plainText.Length; i++)
                encryptedData[i] = Convert.ToByte((plainText[i] ^ publicKey) % n);

            return encryptedData;

        }
        //Método de prueba con RSA
        public void EncryptionRSA(string path)
        {
            Console.WriteLine(p.ToString() +" q:" +  q.ToString());

            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    var bytes = reader.ReadBytes((int)file.Length);
                    
                    string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Path.GetFileNameWithoutExtension(path));
                    using (var outputFile = new FileStream(outputPath + ".cif", FileMode.Append))
                    {
                        using (var writer = new BinaryWriter(outputFile, Encoding.ASCII))
                        {
                            for (int i = 0; i < bytes.Length; i++)
                            {
                                var c = (char)((bytes[i] ^ publicKey) % n);
                                writer.Write(c);
                                //writer.Write(Convert.ToByte((bytes[i] ^ publicKey) % n));
                            }
                               // writer.Write(Convert.ToByte((bytes[i] ^ publicKey) % n));
                            
                        }
                    }

                }
            }
        }
        //Método de prueba con RSA
        public void DecryptionRSA(string path, int mod,int key)
        {
            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    var bytes = reader.ReadBytes((int)file.Length);

                    string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Path.GetFileNameWithoutExtension(path));
                    using (var outputFile = new FileStream(outputPath + "(1).txt", FileMode.Append))
                    {
                        using (var writer = new BinaryWriter(outputFile, Encoding.ASCII))
                        {
                            for (int i = 0; i < bytes.Length; i++)
                            {
                                var c = (char)ModularPow(bytes[i], key, mod);
                              //  var v = Convert.ToByte(c);
                                writer.Write(c);
                            }
                               // writer.Write(Convert.ToByte(Math.Pow(Convert.ToInt32(bytes[i]), key) % n));
                               

                        }
                    }

                }
            }
        }
        /// <summary>
        /// Method to calculate modular exponentiation when the base and the exponent are too big
        /// </summary>
        /// <param name="number">Base</param>
        /// <param name="exponent">exponente</param>
        /// <param name="mod">modulo</param>
        /// <returns>modular exponentiation</returns>
        private int ModularPow(int number, int exponent, int mod)
        {
            var result = 1;
      //      var ejemplo = Convert.ToByte(exponent);
            for (int i = 1; i < exponent; i++)
                result = (result * Convert.ToInt32(number)) % mod;

            return result;
        }
        public byte[] Deencryption(byte[] encryptedData, int key)
        {          
            byte[] deencrypted = new byte[encryptedData.Length];

            for (int i = 0; i < deencrypted.Length; i++)
                deencrypted[i] = Convert.ToByte(Math.Pow(Convert.ToInt32(encryptedData[i]), key) % n);

            return deencrypted;
        }      
    }
}
