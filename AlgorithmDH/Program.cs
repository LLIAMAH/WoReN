using System;

namespace AlgorithmDH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var alice = new Person(15);
            var bob = new Person(8);

            var publicKeyAlice = alice.GetPublicKey();
            var publicKeyBob = bob.GetPublicKey();

            bob.Send(publicKeyAlice);
            alice.Send(publicKeyBob);

            var generalPublicKey1 = bob.GetGeneralPublicKey();
            var generalPublicKey2 = alice.GetGeneralPublicKey();

            Console.WriteLine(generalPublicKey1 == generalPublicKey2
                ? $"General public key has been generated: {generalPublicKey1}."
                : "General public key generation is failed!");
        }
    }

    public class Person
    {
        private readonly int _number;
        private long _generalPublicKey;

        public Person(int input)
        {
            this._number = input;
        }

        public long GetPublicKey()
        {
            return Secrets.GeneratePublicKey(this._number);
        }

        public void Send(long publicKeyOther)
        {
            _generalPublicKey = Secrets.GetGeneralKey(_number, publicKeyOther);
        }

        public long GetGeneralPublicKey()
        {
            return this._generalPublicKey;
        }
    }

    public class Secrets
    {
        private const int PublicG = 3; // G & P
        private const int PublicP = 5;

        public static long GeneratePublicKey(long numberToEncrypt)
        {
            var powered = (long)Math.Pow(PublicG, numberToEncrypt);
            return powered % PublicP;
        }

        public static long GetGeneralKey(int number, long publicKeyOther)
        {
            var modifiedModKey = (long)Math.Pow(publicKeyOther, number);
            return modifiedModKey % PublicP;
        }
    }
}
