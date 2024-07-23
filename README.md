# Task-Management-System
## TMS is a optimized system to manage projects and tasks like NOTION & JIRA

## Technologies & Dev Tools Used
- .NET Core v8 => API Service
- Redis v2.8.0 => Cache DB
- SQL Server v8.0.7 => Main DB
- Bcrypt v1.6.0 => Hashing Service
- HtmlSanitizer v8.0.8 => XSS Sanitizer
- RestSharp v111.4.0 => SMS Service
- SendGrid v9.29.3 => Email Service


## Task-Management-System Architecture


## System Design & Design Pattern
### I used several design patterns in TMS like the following
- Strategy design pattern => To make application easy to scale and maintain
- Factory design pattern => To centralize object creation and reduce dependencies
- Singleton design pattern => To insure global point of access of some objects
### I used CLEAN architecture & cache-aside system design
- I insured to that business logic is independent of external frameworks and made logical structure
- I used cache-aside design between backend services and database/cache to make it easier to be up to date with the cached data

### Notifications



## Summary
I thoroughly enjoyed working on this project, especially as it gave me the opportunity to implement design patterns and apply CLEAN architecture for the first time.
