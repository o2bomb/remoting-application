using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
using SimpleDLL;

namespace BusinessServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServer : BusinessServerInterface
    {
        private DataServerInterface foob;

        public BusinessServer ()
        {
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataService";
            ChannelFactory<SimpleDLL.DataServerInterface> foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        public int GetNumEntries()
        {
            return foob.GetNumEntries();
        }

        public DataStruct GetValuesFromIndex(int index)
        {
            DataStruct result = new DataStruct();
            foob.GetValuesForEntry(index, out result.acctNo, out result.pin, out result.balance, out result.firstName, out result.lastName, out result.profileImg);

            return result;
        }

        public DataStruct GetValuesFromLName(string query)
        {
            DataStruct result = new DataStruct();
            for (int i = 0; i < GetNumEntries(); i++)
            {
                foob.GetValuesForEntry(i, out result.acctNo, out result.pin, out result.balance, out result.firstName, out result.lastName, out result.profileImg);
                if (result.lastName.Equals(query))
                {
                    return result;
                }
            }

            result.acctNo = 0;
            result.pin = 0;
            result.balance = -1;
            result.firstName = "NOT_FOUND";
            result.lastName = "NOT_FOUND";
            result.profileImg = null;
            return result;
        }
    }
}
