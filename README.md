# MicroCouriers (On Going)

MicroCouriers is fictional courier service used to book and track orders. I am working on the sample app to demonstrate the  Architectural and design patterns. The purpose of this application is to create microservices and deploy the application on Kubernetes with complete build pipeline. This is on going project and i will keep on updating along the way. 

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

The solution consists of couple of microservices that work independently and autonomously.I have concentrated more on Back end not UI, so you may find some of the UI code is only for display purpose and has nothing to do with Backend , e.g. Price Estimation. 

Application is based on event-driven architecture so all individual apps are consuming and producing events. These events are key for this application. The application model is based on eventual consistency, so if any service(s) is not available the system will eventualy get consistent once down service is up and running. 

Since order is the main item of this whole system so BookingOrderID is the key that is used by all the microservices to track and update order. 

Service bus is used to deliver events and message and is backbone of the system. Azure function is subscribed to azure service bus and update the order history into cache. each event raised is appended to order history and that history is updated into the cahce. So active orders stay in the cache to avoid DB hit. 

###  Application Components
The application consists of 4 microservices mainly that are interacting using events.

#### Booking API
Booking API is used to book Order. An order consits of Origin , Destination and Item that has to be picked and delivered. The Price Estimation is random price calculator and is only front-end Function. As User books the order , and event is raised which is consumed by other services (described below) . Booking application is also subscribed to events e.g. Order staus change and Payment processed.

#### Payment API
Payment API is used to pay for Order. User is required to pay against the booking ID. Once user pay for Order and event is raised with payment reference that is consumed by Booking service.

#### Tracking API
Tracking API is the meat of this solution. It's using CQRS pattern . Every event raised in the system is consumed by tracking API. The events are stored in event store. Each event is appended to the existing Order ID trail. So we have track of all the events that are raised in the system. 

When user looks for order history we read the history from cache (if cache is available or event is available) else we read from DB and update the cache. 

#### Web App / API Gateway
Web App is working as gateway and every request to APIs is passed through this. Web app also contains Javscript and views to drive the UI , but Web app is driven by API responses. It also has resiliency logic embeded into it incase if any of the API is down , we don't send response to that API until it's back. 


#### Shipping Simulation
Shipping simulation is a desktop application that is used to simulate the order status change. As user pay for the order courier service is required to pick the order from origin and update the status until delivered. The shipping simulation app takes booking order ID and you can update the status of order e.g. Order Picked , which means courier service driver has picked the order and ready to progress futher until finally delivered at destination.

# How to Run Applcation ? 

### Prerequisites

#### Docker 
Docker installed and configured on local system. Make sure docker is set to Linux containers.


#### Azure Service Bus 

Application is using azure service bus and has topic/subscrptions assossiated with it. since we can't create local instance of service bus for development and testing and 
service bus has to be available on azure, so Basic SKU Azure Service bus (deployed and live on azure) is included for testing , however 
You need to replace with your own later as included service bus can be disabled. 


##### Creating your own service bus with topic/subscrption

You need to create following topic and subscrptions. Once done you need to replace the connection string into services appsetting.json

topic = microcouriers-topic
subscrption(s) = booking,tracking,readprojection   (remove default rules since applications will subscribe automatically)
 

#### Running Application
To run the application you need to run rebuildAllDockerImages.ps1 available in MicroCouriers/src/ . This script will build all the images. 
you can check all the images using "docker images" in command line. After this you need to run StartSolution.ps1 to fire up containers.
This will start the solution . in browser type http://localhost:5004 to launch the home page


#### Azure Function and Redis (optional)
Solution also contains azure function and redis cache. If Azure Function is deployed and redis cache is configured , you need to provide redis cache connection string in Azure Function and Tracking Service. Azure function is triggered by event bus and is listening to all the events that are raised. Using events azure function will create the tracking history which is active for 10 hours . In CQRS pattern the readmodel is created and updated into redis cache. 

If above setup is configured , the tracking history is maintained into cache to avoid DB hit e.g. if user search for booking order by ID , then we look into the cahce , if we have it in cache we return it to user. If we don't have history in the cache or history is already expired after 10 hours then we update the cache. 

If we don't have above setup , then every time we hit the tracking database to get the history which is default behaviour of this application. 



# Technical Stack 

#### IDE
Visual Studio 2017

#### Architecture
Clean Architecture , Event-Driven

#### Design Patterns
CQRS, DDD, Respotiory , Cache Aside 

#### Tech Stack / Libraries
.NET Core 2.2 , Azure Service Bus, Azure Redis Cache, Azure Function (serverless), Application Insights, SQL Server on Linux , Docker , Azure SDK , 
EntitiyFramework Core , Dapper , XUnit, Moq , Polly (resiliency), Shouldly (unit test), AutoFac , MediatR



*Since It's an on going project ,  I will keep on updating the description.
