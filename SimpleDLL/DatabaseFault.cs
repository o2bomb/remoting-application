using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDLL
{
    /**
         * This is a custom definition for a fault
         * BASED ON:
         *  - https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/fault-contract
         */
    [DataContract]
    public class DatabaseFault
    {
        private String operation;
        private String problemType;

        [DataMember]
        public string Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        [DataMember]
        public string ProblemType
        {
            get { return problemType; }
            set { problemType = value; }
        }
    }
}
