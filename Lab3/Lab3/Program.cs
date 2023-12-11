using System;
using System.IO;

namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "";
            int hashLen;
            Console.Write("Вкажіть початкову строку: ");
            message = Console.ReadLine();
            Console.Write("Вкажіть довжину хеша(2, 4 або 8): ");
            hashLen = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nЙде обробка початкової строки");
            Console.WriteLine("Початковий хеш строки: {0}", GetHash(message, hashLen));

            Console.WriteLine("\nЙде спроба знайти колізії:");
            int col = SearchCollisions(message, hashLen);
            if (col > 0)
                Console.WriteLine("Було знайдено колізій: {0}", col);
            else
                Console.WriteLine("Колізії не знайдені");

            message += "dd";
            Console.WriteLine("\nЙде обробка зміненої строки: ");
            Console.WriteLine("Нова строка: \"{0}\"", message);
            Console.WriteLine("Хеш нової строки: {0}", GetHash(message, hashLen));

            Console.Write("\nЙде робота з зображеннями: ");
            string image = @"image.png";
            Console.WriteLine("Хеш зображення: {0}", GetHash(GetBytes(image), hashLen));

            Console.Write("\nЙде робота з файлом csproj: ");
            string file = @"Lab3.csproj";
            Console.WriteLine("Хеш csproj: {0}", GetHash(GetBytes(file), hashLen));

            Console.Write("\nЙде робота з текстовим файлом: ");
            string docx = @"Lab3.docx";
            Console.WriteLine("Хеш docx: {0}", GetHash(GetBytes(docx), hashLen));
        }

        private static string GetHash(string message, int value)
        {
            string messageToByte = "";
            for (int i = 0; i < message.Length; i++)
            {
                messageToByte += "0" + Convert.ToString(Convert.ToInt64(message[i]), 2);
            }

            int size = messageToByte.Length / 8;
            byte[] blockArray = new byte[size];
            for (int i = 0; i < size; ++i)
            {
                blockArray[i] = Convert.ToByte(messageToByte.Substring(8 * i, 8), 2);
            }

            byte resultInByte = 0;
            foreach (byte b in blockArray) resultInByte ^= b;
            int result = resultInByte >> ((value == 8) ? 0 : value);

            return Convert.ToString(result, 2);
        }

        private static string GetBytes(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }

        private static string GetRandomString(int len, int iter)
        {
            Random rnd = new Random(iter);
            byte[] rndBytes = new byte[len];
            rnd.NextBytes(rndBytes);
            return System.Text.Encoding.ASCII.GetString(rndBytes);
        }

        private static int SearchCollisions(string message, int value)
        {
            int totalIterations = 100000;
            int collisionsCount = 0;
            while (totalIterations >= 0)
            {
                string randomString = GetRandomString(message.Length, totalIterations);
                string randomStringHash = GetHash(randomString, value);
                if (randomStringHash == GetHash(message, value))
                {
                    Console.WriteLine($"Була знайдена колізія в повідомленні: {randomString} \n Її хеш: {randomStringHash}");
                    collisionsCount++;
                }
                totalIterations--;
            }
            return collisionsCount;
        }
    }
}
