# Three-tier .NET remoting application
A .NET remoting application that consists of a WCF .NET remoting interface (data tier), a RESTful web service interface (business tier) and a WPF client (presentation tier)

## Project structure
- **/Client** contains the WPF client
- **/WebService** contains the RESTful web API
- **/Server** contains the WCF .NET remoting interface
- **/SimpleDLL** is a DLL that provides classes that generates a mock database. It is used by Server
