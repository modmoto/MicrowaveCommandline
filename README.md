# GenericWebServiceBuilder
A code generator that generates a domain driven webservice with only a schema file to describe Models and generating functions to implement domain logic. The Goal is to only have to implement the relevant Domain logic in your Domain Classes and not care aboute Data Storage or WebApi Implementation. To make this possible the Framework generates code depending on the given Schema.wsb file that defines Domain, WebApi and Database on its own. All that should be left to do is implement the Domainlogic on the generated functions.

[![Build status](https://ci.appveyor.com/api/projects/status/n3n2qey19pm4ako4?svg=true)](https://ci.appveyor.com/project/Lauchi/genericwebservicebuilder)
[![Coverage](https://codecov.io/gh/Lauchi/GenericWebServiceBuilder/branch/master/graph/badge.svg)](https://codecov.io/gh/Lauchi/GenericWebServiceBuilder)

## Setup
Use the [BootStrapProject](https://github.com/Lauchi/GeneratedWebServiceBootstrap) to get the correct setup for the framework. So far there is no option to use it with another project structure, as the folders have to be exactly named like in the BootStrapProject.

## Architecture of the Generated Service
The generated service is implemented in three Layers: Domain, Application and Ports and Adapters. There are two adapters, HttpAdapter for the Web Api (controllers and such) and the SQLAdapter that realizes the data storage to an SQL Database. Commands are defined in the domain layer and are used to make mutations on the domain objects. When a object gets mutated or created the function returns a result object that contains the data that was changed, wrapped in a domain event. If an error occured the result can also transport error messages to the client. If the mutation was successfull, the domain events are then stored in the database and the mutated object gets persisted, too.

## The Syntax to generate a Service
The Domain is defined in the Schema.wsb file that is usually located in the Host Project. The syntax is as following.

### DomainClass
To define a Class that will be persisted in the Database and gets an own endpoint, use this syntax. You can put anything as a type, there will be no error, if the type does not exist. But due to a limitation in the CodeDom Library, you can not use primitive types like `string` or `int`. Also note the [] Braces that define a List of a given type.
```javascript
DomainClass User {
  Name: String
  Age: Int32
  Posts: [Post]
}
```

### Create Methods
There is currently only one Create Method supported and you define it like below. Create is a keyword, so be sure to write it correctly. The Parameters define what the command will look like but not what is mandatory to pass. That should be handled by the Domain. The result will be a wrapper with the created entity or the occured errors.
```javascript
DomainClass User {
  Name: String
  Create(Name: String)
}
```

### Update Methods
All other methods on a domain class are treated like normal mutation methods and can have any wording that you want. The parameters are again used to define the command that gets passed into the method. The {} Braces after the mutations are used to define the domain event, that gets created. They usually are very similar to the parameters, but depending on the usecase the data that you change can have more or less information. Always keep in mind that you have to be able to recreate the domain object with those events. In a Update Method there is also a `@Load` Keyword available, that will automatically load the Entity, so you can use it in the method for validation. This should be used, if you want to add Entities to a list and if you do not want to send the whole entity. The api for the loaded entity only need the guid, the framework loads the rest.
```javascript
DomainClass User {
  Name: String
  UpdateName(Name: String): {
    Name: String
  }
  
  AddPost(NewPost: @Load Post) {
    NewPostId: Guid
  }
}
```

### Synchronous Domain Hook
A `SynchronouseDomainHook` is used to listen to events that occur and starts his execute method before the events or the changes are persisted. That gives you the opportunity to other things in the domain that are crucial but can not be done in the update method, as you might want to check for another domain rule. For example this could be a mail that has to be sent when a user creates an account. If your requirement is, that the account must not be created if the mail could not be sent (for example when the mail is not registered with the provider), then you should use a `SynchronouseDomainHook`. You define it like this outside the domain class braces:

```javascript
SynchronousDomainHook SendRegisterMail on User.Create
```

The first word defines that this is a `SynchronouseDomainHook`. The second is the name of the hook, the on is syntactic Sugar and the last word describes on wich event the Hook gets triggered. In this case, the hook gets triggered, if the Method `Crete` from `User` gets called.

### Asynchronouse Domain Hook (not implemented yet)
An `AsyncDomainHook` also listens to domain events but does this after a given set of time. This mechanism is used to implement eventual consistency in the service. Sending Birthday mails could be one of the applications. The Hook has also a retry counter that can be used to escalate things, for example write an error log when a mail was not able to be sent five times in a row. If the event is handles sucessfully, the hook marks the event as done and goes on with the next one. As this might create a deadlock, the `AsyncDomainHook` iterates over all events, starting from the oldest that is not done and ignoring the ones that are allready done. This mechanism ensures, that all events are being handled, even if there are some errors in between.

## Roadmap
Here are some ideas, that i would like to implement, not necessarly in that particular order
- [X] @Load Syntax to load other domain classes when they are used in a method. 
- [ ] Add entity and aggregate separation, to be more domain driven. Entities should only contain IDs, Aggregates are the current DomainClass
- [ ] Add Grapqhl endpoint for being able to do useful filtering besides id
- [ ] Pub/Sub System with Signal R that gets setup within the schema.wsb file between two services effortless
- [ ] Authentication or at leas a place where one could implement it
- [ ] Use domain events in generated hooks to update the entitys
- [ ] implement replay function with domain events
