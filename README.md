# Coolector.Services.SignalR

####**Keep your commune clean in just a few clicks.**

**What is Coolector?**
----------------

Have you ever felt unhappy or even angry about the litter left on the streets or in the woods? Or the damaged things that should've been fixed a long time ago, yet the city council might not even be aware of them?

**Coolector** is an open source & cross-platform solution that provides applications and a services made for all of the inhabitants to make them even more aware about keeping the community clean. 
Within a few clicks you can greatly improve the overall tidiness of the place where you live in. 

**Coolector** may help you not only to quickly submit a new remark about the pollution or broken stuff, but also to browse the already sent remarks and help to clean them up if you feel up to the task of keeping your neighborhood a clean place.

**Coolector.Services.SignalR**
----------------

The **Coolector.Services.SignalR** is a service responsible for pushing the real-time notifications e.g. about new or resolved remarks, to the end-users which makes the applications dynamically updatable.  

**Quick start**
----------------

## Docker way

Coolector is built as a set of microservices, therefore the easiest way is to run the whole system using the *docker-compose*.

Clone the [Coolector.Docker](https://github.com/noordwind/Coolector.Docker) repository and run the *start.sh* script:

```
git clone https://github.com/noordwind/Coolector.Docker
./start.sh
```

Once executed, you shall be able to access the following services:

|Name               |URL                                                  |Repository 
|-------------------|-----------------------------------------------------|-----------------------------------------------------------------------------------------------
|API                |[http://localhost:5000](http://localhost:5000)       |[Coolector.Api](https://github.com/noordwind/Coolector.Api) 
|Mailing            |[http://localhost:10005](http://localhost:10005)     |[Coolector.Services.Mailing](https://github.com/noordwind/Coolector.Services.Mailing) 
|Operations         |[http://localhost:10000](http://localhost:10000)     |[Coolector.Services.Operations](https://github.com/noordwind/Coolector.Services.Operations)
|Remark             |[http://localhost:10002](http://localhost:10002)     |[Coolector.Services.Remarks](https://github.com/noordwind/Coolector.Services.Remarks)
|**SignalR**        |**[http://localhost:15000](http://localhost:15000)** |**[Coolector.Services.SignalR](https://github.com/noordwind/Coolector.Services.SignalR)** 
|Storage            |[http://localhost:10000](http://localhost:10000)     |[Coolector.Services.Storage](https://github.com/noordwind/Coolector.Services.Storage) 
|Users              |[http://localhost:10001](http://localhost:10001)     |[Coolector.Services.Users](https://github.com/noordwind/Coolector.Services.Users) 
|Web                |[http://localhost:9000](http://localhost:9000)       |[Coolector.Web](https://github.com/noordwind/Coolector.Web) 

## Classic way

In order to run the **Coolector.Services.SignalR** you need to have installed:
- [.NET Core](https://dotnet.github.io)
- [RabbitMQ](https://www.rabbitmq.com)

Clone the repository and start the application via *dotnet run* command:

```
git clone https://github.com/noordwind/Coolector.Services.SignalR
cd Coolector.Services.SignalR/Coolector.Services.SignalR
dotnet restore --source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/coolector/api/v3/index.json --no-cache
dotnet run
```

Now you should be able to access the service under the [http://localhost:15000](http://localhost:15000). 

Please note that the following solution will only run the SignalR Service which is merely one of the many parts required to run properly the whole Coolector system.

**Configuration**
----------------

Please edit the *appsettings.json* file in order to use the custom application settings. To configure the docker environment update the *dockerfile* - if you would like to change the exposed port, you need to also update it's value that can be found within *Program.cs*.
For the local testing purposes the *.local* or *.docker* configuration files are being used (for both *appsettings* and *dockerfile*), so feel free to create or edit them.

**Tech stack**
----------------
- **[.NET Core](https://dotnet.github.io)** - an open source & cross-platform framework for building applications using C# language.
- **[SignalR](https://github.com/SignalR/SignalR)** - an open source library for adding a real-time web funcionality via WebSockets.
- **[RawRabbit](https://github.com/pardahlman/RawRabbit)** - an open source library for integration with [RabbitMQ](https://www.rabbitmq.com) service bus.

**Solution structure**
----------------
- **Coolector.Services.SignalR** - core and executable project via *dotnet run* command.
- **Coolector.Services.SignalR.Tests** - unit & integration tests executable via *dotnet test* command.
- **Coolector.Services.SignalR.Tests.EndToEnd** - End-to-End tests executable via *dotnet test* command.