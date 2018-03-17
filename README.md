# GenericWebServiceBuilder
A framework that generates a domain driven webservice with only a schema file to describe Models and generating functions to implement domain logic. The Goal is to only have to implement the relevant Domain logic in your Domain Classes and not care aboute Data Storage or WebApi Implementation. To make this possible the Framework generates code depending on the given Schema.wsb file that defines Domain, WebApi and Database on its own. All that should be left to do is implement the Domainlogic on the generated functions.

## Setup
Use the [BootStrapProject](https://github.com/Lauchi/GeneratedWebServiceBootstrap) to get the correct Setup for the Framework. So far there is no option to use it with another structure, as the folders have to be exactly named like in the BootStrapProject.

## Architecture of the Generated Service
The generated service is implemented in three Layers: Domain, Application and Ports and Adapters. There are two Adapters, HttpAdapter for the Web Api (Controllers and such) and the SQLAdapter that realizes the Data Storage to an SQL Database. Commands are defined in the domain layer and are used to make mutations on the domain objects. When a object gets mutated or created the function returns a result object that contains the data that was changed, wrapped in a domain event, and a status indicator, if the mutation was sucessfull. If an error occured the result can also transport error messages to the client. If the mutation was successfull, the domain events are then stored in the database and the mutated object gets persisted, too.

## The Syntax to generate a Service
The Domain is defined in the Schema.wsb file that is usually located in the Host Project. The syntax is as following.

### DomainClass

