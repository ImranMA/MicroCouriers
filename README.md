# MicroCouriers (On Going)

MicroCouriers is courier service application used to book and track orders. I am working on this application to demonstrate the modern   Architectural and design patterns. This is on going project and i will keep on updating along the way. 

* DDD
* CQRS/Event Sourcing
* Event-Driven Microservices
* Clean Architecture
* Dependency Injection
* Kubernetes
* Docker Containers


# Solution Architecture
![Microcouriers Solution Diagram](https://github.com/ImranMA/MicroCouriers/blob/master/Solution-Architecture.JPG)


# Application Introduction

MicroCouriers is courier service to book , pay and track orders online. User book items that are supposed to be picked by courier service and delivered at destination. The order will have state that will define its current status. e.g. Bookged, Paymenet process, order picked, order in transit and order delivered. User can check the order status in near realtime to know how far order has progressed.

The solution consists of couple of microservices that work independently and autonomously.I have concentrated more on Back-end but not UI, so you may find some of the UI code is only for display purpose and has nothing to do with Backend , e.g. Price Estimation on booking screen.

Application is based on event-driven architecture so all individual apps are consuming and producing events. These events are key for this application. The application model is based on eventual consistency, so if any service(s) is not available the system will eventualy get consistent once down service is up and running. 

Since Order is the main item of this whole system so BookingOrderID is the key that is used by all the microservices to track and update order. 

Service bus is used to deliver events and message and is backbone of the system. Azure function is subscribed to azure service bus and update the order history into cache. each event raised is appended to order history and that history is updated into the cahce. So active orders stay in the cache to avoid DB hit. 

#  Application Components
The application consists of 4 microservices mainly that are interacting using events. Since application is using DDD each microservice is build inside bounded context. 

#### Booking API
Booking API is used to book Order. An order consits of Origin , Destination and Item that has to be picked and delivered. The Price Estimation is random price calculator and is only front-end Function. As User books the order , and event is raised which is consumed by other services (described below) . Booking application is also subscribed to events e.g. Order staus change and Payment processed.

#### Payment API
Payment API is used to pay for Order. User is required to pay against the booking ID. Once user pay for Order and event is raised with payment reference that is consumed by Booking service.

#### Tracking API
Tracking API is the meat of this solution. It's using CQRS pattern . Every event raised in the system is consumed by tracking API. The events are stored in event store. Each event is appended to the existing Order ID trail. So we have track of all the events that are raised in the system. 

When user looks for order history we read the history from cache (if cache is available or event is available) else we read from DB and update the cache. 

#### Web App / API Gateway
Web App is working as gateway and every request to APIs is passed through this. Web app also contains Javscript and views to drive the UI , but Web app is driven by API responses. It also has resiliency logic embeded into it incase if any of the API is down , we don't send request to that API until it's back. 


#### Shipping Simulation
Shipping simulation is a desktop application that is used to simulate the order status change. As user pay for the order courier service is required to pick the order from origin and update the status until delivered. The shipping simulation app takes booking order ID and you can update the status of order e.g. Order Picked , which means courier service driver has picked the order and ready to progress futher until finally delivered at destination.


#  Application Architecture and Design Patterns
Top Application architecture is following event-driven microservices. Each individual microservices is implementing clean architecture and focus on domain. Application is using DDD so you may find DDD elements like bounded context , aggregate roots etc. 


# How to Run Applcation ? 

### Prerequisites

#### Docker 
Docker installed and configured on local system. Make sure docker is set to Linux containers.

#### Running Application
To run the application you need to run rebuildAllDockerImages.ps1 available in MicroCouriers/src/ . This script will build all the images. 
you can check all the images using "docker images" in command line. After this you need to run StartSolution.ps1 to fire up containers.
This will start the solution . in browser type http://localhost:5004 to launch the home page

#### Azure Service Bus 

Application is using azure service bus and has topic/subscrptions assossiated with it. since we can't create local instance of service bus for development and testing and 
service bus has to be available on azure, so Basic SKU Azure Service bus (deployed and live on azure) is included for testing , however 
You need to replace with your own later as included service bus can be disabled. 

##### Creating your own service bus with topic/subscrption

You need to create following topic and subscrptions. Once done you need to replace the connection string into services appsetting.json

topic = microcouriers-topic

subscrption(s) = booking,tracking,readprojection   (remove default rules since applications will subscribe automatically)
 
#### Application Insights
To view performance metrics or issues, you need to replace application insights ID in appsettings for each individual service. 


#### Azure Function and Redis (optional)
Solution also contains azure function and redis cache. If Azure Function is deployed and redis cache is configured , you need to provide redis cache connection string in Azure Function and Tracking Service. Azure function is triggered by event bus and is listening to all the events that are raised. Using events azure function will create the tracking history which is active for 10 hours . In CQRS pattern the readmodel is created and updated into redis cache. 

If above setup is configured , the tracking history is maintained into cache to avoid DB hit e.g. if user search for booking order by ID , then we look into the cahce , if we have it in cache we return it to user. If we don't have history in the cache or history is already expired after 10 hours then we update the cache. 

If we don't have above setup , then every time we hit the tracking database to get the history which is default behaviour of this application. 

#### SQL Login
Application is using SQL Linux container for development purpose. You can login to SQL management studio using following credentials

servername : localhost,1433

Login : sa

password : 99888ukGh43hnDw89Hol8LN21112

# Technical Stack 

#### IDE
Visual Studio 2017

#### Architecture
Clean Architecture , Event-Driven

#### Design Patterns
CQRS, Event Sourcing , Materialized view, DDD, Respotiory , Cache Aside 

#### Tech Stack / Libraries
.NET Core 2.2 , Azure Service Bus, Azure Redis Cache, Azure Function (serverless), Application Insights, SQL Server on Linux , Docker , Azure SDK , 
EntitiyFramework Core , Dapper , XUnit, Moq , Polly (resiliency), Shouldly (unit test), AutoFac , MediatR

# Unit and Load Tests
Each service includes unit and functional tests. Please not tests does not have complete code coverage.  

JMeter load test files are included in the project. Simply import the load test xml files in JMeter and run the load test. Please not this is not comprehensive load test.


# Kubernetes

The solution is tested on Azure Kubernetes. You need to create AKS cluster and http-routing turned on. Deployment steps and yaml files are provided . In deployment yaml replace host (e1519b70bda84a609fd5.australiaeast.aksapp.io ) with your own DNS value. You also need to configure SQL databases hosted and live.  

# Road Map

This is ongoing project and i expect lots of updates and features in the future. Some of the features that i am interested to include are

- SPA Replace web front end with SPA and host it independeny using blob storage. 
- Replace event-sourcing database with NoSQL DB for high performance
- Integrate Bot Service for intellegent order booking.
- Mobile application for tracking and booking. 
- Real time order status updates using SignalR.
- Complete CI/CD pipeline

# Things to be ignored
The projects will have IntegrationEventService (BookingIntegrationEventService)derived from IIntegrationEventLogService . This service is not being used at this point and can be ignored. The purpose of this service is to implement "outbox pattern" which means save events in DB before publishing them . If service bus is broken or down  , the events can be saved locally and then published . This way events are not lost.

# Disclaimer
The project is only for demo purpose and is not production-ready as it may have bugs or missing features. 

# Contribution
I am happy to take anyone on board as we progress. If you find any bug simply create issue . If you have any trouble/questions i can be contacted on https://imranarshad.com/contact






*Since It's an on going project ,  I will keep on updating the description.
