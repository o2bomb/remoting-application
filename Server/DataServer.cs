using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
using SimpleDLL;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : DataServerInterface
    {
        private DatabaseClass database;
        public DataServer()
        {
            database = DatabaseClass.getInstance();
        }

        public int GetNumEntries()
        {
            return database.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap profileImg)
        {
            // Log the operation
            Console.WriteLine("Accessing user at index: " + index);
            acctNo = database.GetAcctNoByIndex(index);
            pin = database.GetPINByIndex(index);
            bal = database.GetBalanceByIndex(index);
            fName = database.GetFirstNameByIndex(index);
            lName = database.GetLastNameByIndex(index);
            profileImg = database.GetProfileImgByIndex(index);
        }

        public void EditValuesForEntry(int index, uint acctNo, uint pin, int bal, string fName, string lName, Bitmap profileImg)
        {
            Console.WriteLine("Attempting to edit user at index: " + index);
            try
            {
                database.EditAcctNoByIndex(index, acctNo);
                database.EditPinByIndex(index, pin);
                database.EditBalanceByIndex(index, bal);
                database.EditFirstNameByIndex(index, fName);
                database.EditLastNameByIndex(index, lName);
                // Log the operation
                Console.WriteLine("Successfully edited user at index: " + index);
                Console.WriteLine(String.Format("First name: {0}", fName));
                Console.WriteLine(String.Format("Last name: {0}", lName));
                Console.WriteLine(String.Format("Account No: {0}", acctNo));
                Console.WriteLine(String.Format("PIN: {0}", pin));
                Console.WriteLine(String.Format("Balance: {0}", bal));
            }
            catch (ArgumentOutOfRangeException e)
            {
                // Log the error message
                Console.WriteLine("Failed to edit user: " + e.Message);
                // Pass it on to the web service to handle
                DatabaseFault f = new DatabaseFault
                {
                    Operation = "Edit",
                    ProblemType = e.Message
                };
                FaultReason reason = new FaultReason(e.Message);
                throw new FaultException<DatabaseFault>(f, reason);
            }
            catch (ArgumentNullException e)
            {
                // Log the error message
                Console.WriteLine("Failed to edit user: " + e.Message);
                // Pass it on to the web service to handle
                DatabaseFault f = new DatabaseFault
                {
                    Operation = "Edit",
                    ProblemType = e.Message
                };
                FaultReason reason = new FaultReason(e.Message);
                throw new FaultException<DatabaseFault>(f, reason);
            }
        }
    }
}
