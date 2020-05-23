using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ServiceModel;

namespace SimpleDLL
{
    public class DatabaseClass
    {
        private static DatabaseClass instance = null;
        private List<DataStruct> dataStructs;

        public static DatabaseClass getInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseClass(10000);
            }
            return instance;
        }

        private DatabaseClass(int count)
        {
            dataStructs = new List<DataStruct>();
            for (int i = 0; i < count; i++)
            {
                DatabaseGenerator generator = new DatabaseGenerator(i);
                DataStruct tempData = new DataStruct();
                generator.GetNextAccount(out tempData.pin, out tempData.acctNo, out tempData.firstName, out tempData.lastName, out tempData.balance, out tempData.profileImg);
                dataStructs.Add(tempData);
            }
        }

        public uint GetAcctNoByIndex(int index)
        {
            try
            {
                return dataStructs.ElementAt(index).acctNo;
            }
            catch
            {
                return 0;
            }
            
        }
    
        public uint GetPINByIndex(int index)
        {
            try
            {
                return dataStructs.ElementAt(index).pin;
            }
            catch
            {
                return 0;
            }
        }

        public string GetFirstNameByIndex(int index)
        {
            try
            {
                return dataStructs.ElementAt(index).firstName;
            }
            catch
            {
                return "NOT_FOUND";
            }
        }

        public string GetLastNameByIndex(int index)
        {
            try
            {
                return dataStructs.ElementAt(index).lastName;
            }
            catch
            {
                return "NOT_FOUND";
            }
        }

        public int GetBalanceByIndex(int index)
        {
            try
            {
                return dataStructs.ElementAt(index).balance;
            }
            catch
            {
                return -1;
            }
        }

        public Bitmap GetProfileImgByIndex(int index)
        {
            try
            {
                return dataStructs.ElementAt(index).profileImg;
            }
            catch
            {
                return null;
            }
        }

        public int GetNumRecords()
        {
            return dataStructs.Count();
        }

        public void EditAcctNoByIndex(int index, uint newAcctNo)
        {
            try
            {
                dataStructs.ElementAt(index).acctNo = newAcctNo;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the element cannot be found
                throw new ArgumentOutOfRangeException("User cannot be found at specified index");
            }
        }

        public void EditPinByIndex(int index, uint newPin)
        {
            try
            {
                dataStructs.ElementAt(index).pin = newPin;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the element cannot be found
                throw new ArgumentOutOfRangeException("User cannot be found at specified index");
            }
        }

        public void EditFirstNameByIndex(int index, string newFName)
        {
            if(newFName.Length == 0 || newFName == null)
            {
                // If the new first name is empty
                throw new ArgumentNullException("User cannot have an null/empty first name");
            }

            try
            {
                dataStructs.ElementAt(index).firstName = newFName;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the element cannot be found
                throw new ArgumentOutOfRangeException("User cannot be found at specified index");
            }
        }

        public void EditLastNameByIndex(int index, string newLName)
        {
            if (newLName.Length == 0 || newLName == null)
            {
                // If the new first name is empty
                throw new ArgumentNullException("User cannot have an null/empty first name");
            }

            try
            {
                dataStructs.ElementAt(index).lastName = newLName;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the element cannot be found
                throw new ArgumentOutOfRangeException("User cannot be found at specified index");
            }
        }

        public void EditBalanceByIndex(int index, int newBalance) 
        {
            try
            {
                dataStructs.ElementAt(index).balance = newBalance;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the element cannot be found
                throw new ArgumentOutOfRangeException("User cannot be found at specified index");
            }
        }
    }
}
