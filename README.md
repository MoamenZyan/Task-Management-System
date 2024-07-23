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
![Task_Management_System_Architecture](https://github.com/user-attachments/assets/f7fbef10-126b-42fd-abf0-ff233c99615a)


## System Design & Design Pattern
### I used several design patterns in TMS like the following
- Strategy design pattern => To make application easy to scale and maintain
- Factory design pattern => To centralize object creation and reduce dependencies
- Singleton design pattern => To insure global point of access of some objects
  
### I used CLEAN architecture & cache-aside system design
- I insured to that business logic is independent of external frameworks and made logical structure
- I used cache-aside design between backend services and database/cache to make it easier to be up to date with the cached data

### Notifications
![projectAddedEmail](https://github.com/user-attachments/assets/4f14c14b-acde-4f90-839f-63dae9aab752)
![removedProjectEmail](https://github.com/user-attachments/assets/dabed467-2c77-46af-b6a5-dacd79f79307)
![Screenshot 2024-07-22 195340](https://github.com/user-attachments/assets/a4e2604e-9f87-4c13-97eb-94cb8e5a70a6)
![UnassignedFromTaskEmail](https://github.com/user-attachments/assets/1ede6f67-f87b-41cf-8ba7-ecb619acaf2d)


## Summary
I thoroughly enjoyed working on this project, especially as it gave me the opportunity to implement design patterns and apply CLEAN architecture for the first time.
