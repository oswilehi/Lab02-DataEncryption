using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace Lab02_DataEncription
{
    class RSA
    {
        /*ALgoritmo:
        1. Generar las llaves privadas y públicas son dos números primos
        2.  Cifrar el mensaje P^e = E ( mod n ) P es el mensaje en texto plano,n y e son la clave pública,E es el mensaje cifrado*/

        //Numero primo 'p'
        private int p;
        //Numero primo 'q'
        private int q;

        private int n;  //llave pública y privada
        /// <summary>
        /// Public key
        /// </summary>
        public int publicKey { get; set; }//llave pública
        /// <summary>
        /// Private key
        /// </summary>
        public int privateKey { get; set; } //llave privada
        private string filePath;

        public RSA()
        {

        }
        private void GeneratePrimeNumber()
        {
            Random r = new Random();
            var n = r.Next(0, 59);

            p = PolinomialToGeneratePrimeNumber(n);
            do
            {
                n = r.Next(0, 59);
                q = PolinomialToGeneratePrimeNumber(n);
            } while (q.CompareTo(p) == 0);

        }
        private int PolinomialToGeneratePrimeNumber(int n)
        {
            //Polinomio de J. Brox que generan un número primo. El valor de n va entre cero y cincuenta nueve.
            return 6 * (n ^ 2) - 342 * n + 4903;
        }
        private int GenerateValueN()
        {
            return p * q;
        }
        private int PhiEulier()
        {
            return (p - 1) * (q - 1);
        }
        private int CoprimeNumber(int n)
        {
            int e = 1;
            do
                e++;
            while ((e.CompareTo(n) != -1) || ModBetweenTwoNumbers(e, n) != 0);

            return e;
        }
        private int ModBetweenTwoNumbers(int e, int n)
        {
            return n % e;
        }

        private int GenerateValueJ(int n, int j)
        {
            j = n;
            do
            {
                j++;
            } while (j % n == 1);
            return j;
        }
        /// <summary>
        /// Method to generate private and public key
        /// </summary>
        public void GenerateKeys()
        {
            GeneratePrimeNumber();
            n = GenerateValueN();
            var z = PhiEulier();
            publicKey = CoprimeNumber(n);
            privateKey = GenerateValueJ(n, privateKey);

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


        public byte[] Deencryption(byte[] encryptedData, int key)
        {          
            byte[] deencrypted = new byte[encryptedData.Length];

            for (int i = 0; i < deencrypted.Length; i++)
                deencrypted[i] = Convert.ToByte(Math.Pow(Convert.ToInt32(encryptedData[i]), key) % n);

            return deencrypted;
        }

    }
}
