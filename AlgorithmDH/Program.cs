using System;

namespace AlgorithmDH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var alice = new Person(5);
            var bob = new Person(7);

            var publicKeyAlice = alice.GetPublicKey();
            var publicKeyBob = bob.GetPublicKey();

            bob.Send(publicKeyAlice);
            alice.Send(publicKeyBob);

            var generalPublicKey1 = bob.GetGeneralPublicKey();
            var generalPublicKey2 = alice.GetGeneralPublicKey();

            if(generalPublicKey1 == generalPublicKey2)
                Console.WriteLine($"General public key has been generated: {generalPublicKey1}.");
            else
                Console.WriteLine("General public key generation is failed!");
        }
    }

    public class Person
    {
        private int _number;
        private readonly Secrets _secrets;
        private long _generalPublicKey;

        public Person(int input)
        {
            this._number = input;
            this._secrets = new Secrets();
        }

        public long GetPublicKey()
        {
            return _secrets.GeneratePublicKey(this._number);
        }

        public void Send(long publicKeyOther)
        {
            _generalPublicKey = _secrets.GetGeneralKey(_number, publicKeyOther);
        }

        public long GetGeneralPublicKey()
        {
            return this._generalPublicKey;
        }
    }

    public class Secrets
    {
        private const int publicG = 3; // G & P
        private const int publicP = 5;

        public long GeneratePublicKey(long numberToEncrypt)
        {
            var powered = (long)Math.Pow(publicG, numberToEncrypt);
            return powered % publicP;
        }

        public long GetGeneralKey(int number, long publicKeyOther)
        {
            var modifiedModKey = (long)Math.Pow(publicKeyOther, number);
            return modifiedModKey % publicP;
        }
    }
}
