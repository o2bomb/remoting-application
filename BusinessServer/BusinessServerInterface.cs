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
    [ServiceContract]
    public interface BusinessServerInterface
    {
        [OperationContract]
        int GetNumEntries();
        [OperationContract]
        DataStruct GetValuesFromIndex(int index);
        [OperationContract]
        DataStruct GetValuesFromLName(string query);
    }
}
