# TYL-TechTest
Tech Test Response for Tyl

## Techinical Overview

DotNet Core Version 6- LTS
EntityFramework Core Version 6.0.6

Can use either InMemory Database, or LocalDb *(switch comments on lines 50 and 51 of Program.cs)*

To run:
navigate to `./src/LSE.TradeHub/`

invoke commands
 `dotnet restore`
 `dotnet run --project LSE.TradeHub.API`

Open the following link:
https://localhost:7169/swagger

### Projects

 - LSE.TradeHub.API - Primary API Application
 - LSE.TradeHub.Core - Data models and DAL services   
 - LSE.TradeHub.Utilities - Non essential utilities and Data Seeding

### Test Projects
- LSE.TradeHub.API.Tests - Controller and mapper unit tests
- LSE.TradeHub.Core.Tests - Data service integrations tests


## Architectural Overview
![TylTechTest drawio](https://user-images.githubusercontent.com/414287/176006459-c71d2c47-7a01-467e-a78d-a889d8be17c4.png)

Created as a simple 'n-tier' application wherein each tier is responsible for its own models and outputs. 

The API tier handles the Swagger/Swashbuckle UI and hosts the two controllers:

* TradeRecordController - Handles the POST of new trade records to be entered to the database.

* StockValuesController - Handles the requests of either individual stocks by symbol, or as a GetAll request

The Core DAL Layer hosts the DAL services, though only one service was required for this particular MVP as the mean stock value could be retrieved directly from the data source by grouping/filtering the TradeRecord data set and utilising LinqToSql's Average() method. 

The data layer itself has been left deliberately flat with a relation between the Stock table and the TradeRecord table.

The primary key of the Stock table is defined as the stock exchange Symbol, given that this is a unique identity and allows the TradeRecord queries to bypass the need to perform a lookup on the Stocks table to establish the relation when querying by Stock by symbol.

# Enhancements

As it stands, this wouldn't be very scalable at all. While the POST to create the new TradeRecords is very lightweight, it puts the onus of ensuring the data gets into the database on the request/response loop.

Likewise, the request for stock values goes directly to the database and as the volume of TradeRecords increases, the response time will increase in a linear fashion. This would make the DB a very tight bottleneck that would only get worse with application layer scaling.

Adding a caching layer to the system would solve this bottleneck issue. Indexed by the stock symbol, wherein once a TradeRecord is added, the record for that stock gets updated. This would also enable extra features to be added to the data output such as price trend, last five trades etc. without multiple and expensive calls to the database.

As it stands, the overall application is highly coupled between the application and database layers. While the tiered separation has its benefits, and traditionally, it could be made to stand up to high loads with highly available infrastructure, there are opportunities to use modern, serverless infrastructure to improve performance and scalability.

Depending on the tolerance of latency for the data to be updated. The insert of the TradeRecords could be done via a message queue. This would ensure that all data posted to the system got through without overloading the database server connections, given that queue capacity is naturally far greater than that of database connection capacity.

![TylTechTest-Enhancements drawio](https://user-images.githubusercontent.com/414287/176009609-244bcc43-8628-40f4-966d-cf6dbe8665f2.png)

This would have the issue of adding some latency to the price update, but it would guarantee the integrity of the data as the system would receive and 'store' the TradeRecord in a decoupled system.

To achieve this the implementation of the ITradeRecordService could be altered to push the new trade records to a queue, and then processed by either the application itself, or preferably, a serverless function (AWS Lambda or Azure Funtion). The cache could be updated via a message queue trigger, or by a SQL Trigger should SQL Server be the data store of choice, though a cloud native data store could provide better and faster triggering.

The cache would be handled perfectly by one of Redis, Dynamo or Elastic, sharded by the stock symbol. 
