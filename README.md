# MicroCouriers (On Going)

MicroCouriers is fictional courier service used to book and track orders. I am working on the sample app to demonstrate the  Architectural and design patterns. The purpose of this application is to create microservices and deploy the application on Kubernetes with complete build pipeline. This is on going project and i will keep on updating along the way. 

* DDD
* CQRS/Event Sourcing
* Event-Driven Microservices
* Clean Architecture
* Kubernetes
* Containers
* Dependency Injection

# Solution Architecture
![Microcouriers Solution Diagram](https://github.com/ImranMA/MicroCouriers/blob/master/Solution-Architecture.JPG)


# Application Introduction

MicroCouriers is courier service to book , pay and track orders online. User book items that are supposed to be picked by courier service and delivered at destination. The order will have state that will define its current status. e.g. Bookged, Paymenet process, order picked, order in transit and order delivered. User can check the order status in near realtime to know how far order has progressed.

The solution consists of couple of microservices that work independently and autonomously.I have concentrated more on Back end not UI, so you may find some of the UI code is only for display purpose and has nothing to do with Backend , e.g. Price Estimation. 

Application is based on event-driven architecture so all individual apps are consuming and producing events. These events are key for this application. The application model is based on eventual consistency, so if any service(s) is not available the system will eventualy get consistent once down service is up and running. 

Since order is the main item of this whole system so BookingOrderID is the key that is used by all the microservices to track and update order. 

Service bus is used to deliver events and message and is backbone of the system. Azure function is subscribed to azure service bus and update the order history into cache. each event raised is appended to order history and that history is updated into the cahce. So active orders stay in the cache to avoid DB hit. 

###  Application Components
The application consists of 3 microservices mainly that are interacting using events.

#### Booking API
Booking API is used to book Order. An order consits of Origin , Destination and Item that has to be picked and delivered. The Price Estimation is random price calculator and is only front-end Function. As User books the order , and event is raised which is consumed by other services (described below) . Booking application is also subscribed to events e.g. Order staus change and Payment processed.

#### Payment API
Payment API is used to pay for Order. User is required to pay against the booking ID. Once user pay for Order and event is raised with payment reference that is consumed by Booking service.

#### Shipping Simulation
Shipping simulation is a desktop application that is used to simulate the order status change. As user pay for the order courier service is required to pick the order from origin and update the status until delivered. The shipping simulation app takes booking order ID and you can update the status of order e.g. Order Picked , which means courier service driver has picked the order and ready to progress futher until finally delivered at destination.

#### Tracking API
Tracking API is the meat of this solution. It's using CQRS pattern . Every event raised in the system is consumed by tracking API. The events are stored in event store. Each event is appended to the existing Order ID trail. So we have track of all the events that are raised in the system. 

When user looks for order history we read the history from cache (if cache is available or event is available) else we read from DB and update the cache. 

*Since It's an on going project ,  I will keep on updating the description.
