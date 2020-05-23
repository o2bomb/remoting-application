using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SimpleDLL;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing .NET Server...");
            // The host service system
            ServiceHost host;
            // Represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcpBinding = new NetTcpBinding();

            host = new ServiceHost(typeof(DataServer));
            // Exposes the interface to the client
            // Any ip address (0.0.0.0) on port 8100 can access the service
            // The service is named DataService (can be named anything)
            host.AddServiceEndpoint(typeof(DataServerInterface), tcpBinding, "net.tcp://0.0.0.0:8100/DataService");
            host.Open();
            Console.WriteLine("Initialization complete. System is online.");
            Console.WriteLine("Press any key to close the server:");
            Console.ReadLine();
            host.Close();
        }
    }
}
