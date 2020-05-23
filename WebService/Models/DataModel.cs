using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using SimpleDLL;
using APIClasses;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.IO;

namespace WebService.Models
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class DataModel
    {
        private DataServerInterface foob;
        private static uint logNumber = 0; // keeps track of how many log-able tasks have been performed

        public DataModel()
        {
            string URL = "net.tcp://localhost:8100/DataService";
            NetTcpBinding tcp = new NetTcpBinding();
            ChannelFactory<SimpleDLL.DataServerInterface> foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            string fileName = "log.txt";
            // Store log.txt in the project root directory
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            
            string textToAppend = String.Format("[{0}] {1}", logNumber, logString) + Environment.NewLine;
            if (!File.Exists(filePath))
            {
                // If the file does not exist yet, create it and write to it
                File.WriteAllText(filePath, textToAppend);
            }
            else
            {
                // Else, Append to the existing log.txt file
                File.AppendAllText(filePath, textToAppend);
            }
        }

        public int GetNumEntries()
        {
            logNumber++;
            Log("OPERATION: Get number of entries in database");
            return foob.GetNumEntries();
        }

        /**
         * Retrieves the user's details based on the specified index in the database
         */
        public DataIntermed GetValuesFromIndex(int index)
        {
            logNumber++;
            Log(String.Format("OPERATION: Get a user from specified index: {0}", index));
            DataIntermed result = new DataIntermed();
            result.index = index;
            foob.GetValuesForEntry(index, out result.acct, out result.pin, out result.bal, out result.fName, out result.lName, out result.profileImg);

            return result;
        }

        /**
         * Retrieves the user's details based on the specified last name. Is case sensitive
         */
        public DataIntermed GetValuesFromLName(string query)
        {
            logNumber++;
            Log(String.Format("OPERATION: Get a user from specified last name: {0}", query));
            DataIntermed result = new DataIntermed();
            // Iterate through each of the entries in the database, until a matching user is encountered. Return that user's details
            for (int i = 0; i < foob.GetNumEntries(); i++)
            {
                foob.GetValuesForEntry(i, out result.acct, out result.pin, out result.bal, out result.fName, out result.lName, out result.profileImg);
                if (result.lName.Equals(query))
                {
                    result.index = i;
                    return result;
                }
            }

            // If no matching user is encountered, then just return error values
            result.index = -1;
            result.acct = 0;
            result.pin = 0;
            result.bal = -1;
            result.fName = "NOT_FOUND";
            result.lName = "NOT_FOUND";
            result.profileImg = null;

            return result;
        }

        /**
         * Edits the user's details based on the specified index and given details to edit
         */
        public void EditValuesFromIndex(int index, uint acctNo, uint pin, int bal, string fName, string lName, Bitmap profileImg)
        {
            logNumber++;
            Log(String.Format("OPERATION: Edit a user at specified index {0} with the following values: ", index));
            Log(String.Format("First name: {0}", fName));
            Log(String.Format("Last name: {0}", lName));
            Log(String.Format("Account No: {0}", acctNo));
            Log(String.Format("PIN: {0}", pin));
            Log(String.Format("Balance: {0}", bal));
            try
            {
                foob.EditValuesForEntry(index, acctNo, pin, bal, fName, lName, profileImg);
            }
            catch (FaultException<DatabaseFault> e)
            {
                // When an error occurs, log it as well
                string errorMessage = String.Format("Failed to perform {0}, {1}", e.Detail.Operation, e.Detail.ProblemType);
                Log(String.Format("ERROR: {0}", errorMessage));
                // Throw an exception to the controllers
                throw new ArgumentException(errorMessage);
            }
        }
    }
}