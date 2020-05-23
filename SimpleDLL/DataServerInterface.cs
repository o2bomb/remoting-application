using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;

namespace SimpleDLL
{
    [ServiceContract]
    public interface DataServerInterface
    {
        [OperationContract]
        int GetNumEntries();
        [OperationContract]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap profileImg);
        [OperationContract]
        [FaultContract(typeof(DatabaseFault))]
        void EditValuesForEntry(int index, uint acctNo, uint pin, int bal, string fName, string lName, Bitmap profileImg);
    }
}
