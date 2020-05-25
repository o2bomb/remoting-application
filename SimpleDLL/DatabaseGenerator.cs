using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomNameGeneratorLibrary;
using System.Drawing;
using System.IO;
using System.Net;

namespace SimpleDLL
{
    internal class DatabaseGenerator
    {
        private PersonNameGenerator personGenerator;
        private Random numberGenerator;
        public DatabaseGenerator(int seed)
        {
            personGenerator = new PersonNameGenerator(new Random(seed));
            numberGenerator = new Random(seed);
        }

        private string GetFirstname()
        {
            string firstName = personGenerator.GenerateRandomFirstName();
            return firstName;
        }

        private string GetLastname()
        {
            string lastName = personGenerator.GenerateRandomLastName();
            return lastName;
        }

        private uint GetPIN()
        {
            uint pin = (uint)numberGenerator.Next(9999);
            return pin;
        }

        private uint GetAcctNo()
        {
            uint acctNo = (uint)numberGenerator.Next(9999);
            return acctNo;
        }

        private int GetBalance()
        {
            int balance = numberGenerator.Next(999999);
            return balance;
        }

        private byte[] GetProfileImg()
        {
            // Instantiate each profile pic as null. The user can insert their own later
            return null;
        }

        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance, out byte[] profileImg)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstName = GetFirstname();
            lastName = GetLastname();
            balance = GetBalance();
            profileImg = GetProfileImg();
        }
    }
}
