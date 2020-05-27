# Three-tier .NET remoting application
A .NET remoting application that consists of a WCF .NET remoting interface (data tier), a RESTful web service interface (business tier) and a WPF client (presentation tier)

## Features
- Search for user via last name
- Search for user via index in database
- Edit existing user in database
- File-based logging for business tier
- Console-based logging for data tier
- Complete exception handling between the presentation, business, and data tier

## Project structure
- **/Client** contains the WPF client
- **/WebService** contains the RESTful web API. Also contains a generated log.txt, where logs of operations are stored
- **/Server** contains the WCF .NET remoting interface
- **/SimpleDLL** is a DLL that provides classes that generates a mock database. It is used by Server
- **/APIClasses** contains class definitions that are used by the client and web API
