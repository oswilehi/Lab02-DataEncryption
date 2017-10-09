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
        //Prime numbers
        private static int p;
        private static int q;
        //Multplication of p and q
        public int n { get; set; }  

        public int publicKey { get; set; }
        public int privateKey { get; set; } 

        /// <summary>
        /// Method to generate a prime number using a function
        /// </summary>
        private void GeneratePrimeNumber(int maxValueOfPlainText)
        {
            Random r = new Random();
            int[] array = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43 };
            var n = r.Next(0, 13);
            p = array[n];
            do
            {
                n = r.Next(0, 13);
                q = array[n];
            } while (q.CompareTo(p) == 0 || GenerateValueN() > 255 || GenerateValueN() < maxValueOfPlainText);

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
        /// This method multiply p and q
        /// </summary>
        /// <returns>p*q</returns>
        private int GenerateValueN()
        {
            return p * q;
        }
        /// <summary>
        /// This method find the value phi eulier of the numbers p and q
        /// Phi Eulier represents the number of numbers that are coprime with n
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
            while (ModBetweenTwoNumbers(e, n) == 0 || IsNotPrime(e));

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
        /// Method that validate if the parameter is a prime number.
        /// </summary>
        /// <param name="number">A number</param>
        /// <returns>True: no is a prime number, False: is a prime number</returns>
        private bool IsNotPrime(int number)
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
        /// Method to generate private and public key
        /// </summary>
        public void GenerateKeys(int maxValueOfText)
        {

            GeneratePrimeNumber(maxValueOfText);

            n = GenerateValueN();
            var z = PhiEulier();
            publicKey = CoprimeNumber(z);

            privateKey = GeneratePrivateKeyWithModInverse(publicKey,z);

        }
        /// <summary>
        /// This method create the private key
        /// </summary>
        /// <param name="z"></param>
        /// <param name="e"></param>
        /// <returns>private key</returns>
        private int GeneratePrivateKeyWithModInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = ModBetweenTwoNumbers(x, i); ;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
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
            for (int i = 0; i < exponent; i++)
                result = (result * number) % mod;
            return result;
        }
        /// <summary>
        /// Method that encrypts using RSA method
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns>The method returns an array of bytes</returns>
        public byte[] Encryption(byte[] plainText)
        {
            byte[] encryptedData = new byte[plainText.Length];

            for (int i = 0; i < plainText.Length; i++)
                encryptedData[i] = Convert.ToByte(Convert.ToInt64(Math.Pow(plainText[i], publicKey)) % n);
            return encryptedData;
        }
        /// <summary>
        /// Method that deencrypts using RSA method
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="mod"></param>
        /// <param name="key"></param>
        /// <returns>Returns an array of bytes</returns>
        public byte[] Deencryption(byte[] encryptedData, int mod, int key)
        {
            byte[] deencrypted = new byte[encryptedData.Length];
            for (int i = 0; i < deencrypted.Length; i++)
                deencrypted[i] = Convert.ToByte(Math.Pow(Convert.ToInt64(encryptedData[i]), key) % mod);
            return deencrypted;
        }
        /// <summary>
        /// Method that encrypts using RSA method.
        /// It writes the encrypted text into file.
        /// </summary>
        /// <param name="path"></param>
        public void EncryptionRSA(string path)
        {
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

                                var c = (char)ModularPow(bytes[i], publicKey, n);
                                writer.Write(c);

                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Method that deencrypts using RSA method.
        /// It writes the deencrypted text into a file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mod"></param>
        /// <param name="key"></param>
        public void DecryptionRSA(string path, int mod, int key)
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
                                writer.Write(c);

                            }
                        }
                    }

                }
            }
        }
    }
}
