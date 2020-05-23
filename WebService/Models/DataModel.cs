using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using SimpleDLL;
using APIClasses;
using System.Drawing;

namespace WebService.Models
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class DataModel
    {
        private DataServerInterface foob;

        public DataModel()
        {
            string URL = "net.tcp://localhost:8100/DataService";
            NetTcpBinding tcp = new NetTcpBinding();
            ChannelFactory<SimpleDLL.DataServerInterface> foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        public int GetNumEntries()
        {
            return foob.GetNumEntries();
        }

        public DataIntermed GetValuesFromIndex(int index)
        {
            DataIntermed result = new DataIntermed();
            result.index = index;
            foob.GetValuesForEntry(index, out result.acct, out result.pin, out result.bal, out result.fName, out result.lName, out result.profileImg);

            return result;
        }

        public DataIntermed GetValuesFromLName(string query)
        {
            DataIntermed result = new DataIntermed();
            for (int i = 0; i < GetNumEntries(); i++)
            {
                foob.GetValuesForEntry(i, out result.acct, out result.pin, out result.bal, out result.fName, out result.lName, out result.profileImg);
                if (result.lName.Equals(query))
                {
                    result.index = i;
                    return result;
                }
            }

            result.index = -1;
            result.acct = 0;
            result.pin = 0;
            result.bal = -1;
            result.fName = "NOT_FOUND";
            result.lName = "NOT_FOUND";
            result.profileImg = null;
            return result;
        }

        public void EditValuesFromIndex(int index, uint acctNo, uint pin, int bal, string fName, string lName, Bitmap profileImg)
        {
            foob.EditValuesForEntry(index, acctNo, pin, bal, fName, lName, profileImg);
        }
    }
}