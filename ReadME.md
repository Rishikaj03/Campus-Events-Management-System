# **Campus Events Management System**



#### Project Overview



The Campus Events Management System is a web-based application built using ASP.NET Core MVC that allows students to explore, register, and manage campus events, while administrators control and monitor the entire system.

This system includes role-based access (Admin \& Student), real-time event capacity handling, and secure authentication using ASP.NET Identity.



1. ##### Core Features



* Authentication System
* User Registration \& Login (ASP.NET Identity)
* Role-based access: Admin and Student
* Auto role assignment for new users



##### 2\. Features



* Dashboard with: Total available events \& Registered events count
* Browse upcoming events (only future \& open events)
* View detailed event information
* Register for events
* Prevents duplicate registrations
* Automatic capacity tracking
* Cancel event registration
* View "My Events"



##### 3\. Admin Features



* Admin Dashboard: Total events count \& Total registrations count
* Add new events
* Edit event details
* Delete events
* Open/Close event registration manually
* View participants of each event
* View all registered users



##### 4\. Tech Stack



* Frontend: HTML, CSS, Bootstrap
* Backend: ASP.NET Core MVC (C#)
* Database: SQL Server
* ORM: Entity Framework Core
* Authentication: ASP.NET Core Identity
* IDE: Visual Studio



##### 5\. Dependencies



Make sure the following are installed:



* .NET SDK (6.0 or later)
* SQL Server / SQL Server Express
* Visual Studio 2022+
* Entity Framework Core Tools





**#Install EF CLI (if not installed):**



*dotnet tool install --global dotnet-ef*





##### 6\. Installation \& Setup



**Step 1:** **Open Project**

Open CampusEventsApp.sln in Visual Studio



**Step 2: Configure Database**

Go to appsettings.json:



*"ConnectionStrings": {*

&#x20; *"DefaultConnection": "Server=YOUR\_SERVER;Database=CampusEventsDB;Trusted\_Connection=True;"*

*}*



**Step 3: Apply Migrations**

*dotnet ef database update*



**Step 4: Run the Application**

Press F5 in Visual Studio

OR

*dotnet run*



##### 7\. Application Flow (Actual Working Logic)



**1. User Authentication**

* User registers/login
* System automatically assigns Student role (if not admin)



**2. Role Redirection**

* Admin → redirected to Admin Dashboard
* Student → redirected to Student Dashboard



**3. Event Handling**



**Students:**

* Can view only:

&#x20;    - Future events

&#x20;    - Open events



* Cannot register:

&#x20;    - If already registered

&#x20;    - If event is full

&#x20;    - If event is closed



**4. Registration Logic**



* When user registers:

&#x20;     - Entry added to EventRegistrations

&#x20;     - Capacity decreases dynamically

* If capacity reaches 0:

&#x20;     - Event automatically closes



**5. Cancellation Logic**



* User cancels registration:

&#x20;     - Registration removed

&#x20;     - Event reopens if seats become available



**6. Admin Controls**



* Full CRUD operations on events
* Toggle event status (Open/Close)
* View participants (with email \& registration date)
* Monitor system usage



##### 8\. Database Structure



**Event Table:**

* Id
* Title
* Category
* Description
* EventDate
* EventTime
* Venue
* TotalCapacity
* IsOpen



**EventRegistration Table:**

* Id
* EventId (FK)
* StudentId (FK)
* Status
* RegistrationDate



**Identity Tables**



* Users
* Roles (Admin, Student)
* UserRoles



##### **📁 Project Structure**

CampusEventsApp/

│

├── Areas/Identity/        # Authentication UI (Login/Register)

├── Controllers/

│   ├── AdminController

│   ├── StudentController

│   ├── EventsController

│   ├── HomeController

│

├── Models/

│   ├── Event

│   ├── EventRegistration

│

├── Data/

│   ├── ApplicationDbContext

│   ├── Migrations

│

├── Views/

│   ├── Admin/

│   ├── Student/

│   ├── Events/

│   ├── Home/

│

├── wwwroot/               # CSS, JS, Bootstrap

├── appsettings.json

├── Program.cs



###### **Default Admin Credentials**



*Email: admin@campus.com*

*Password: Admin@123*



(Auto-created when the application runs)

##### 

##### **Important Notes**



* Users cannot register twice for same event
* Events automatically close when full
* Only Admin can manage events
* Students cannot access admin routes
* Role assignment may require re-login

##### 

##### **Common Errors \& Fixes**

###### 

1. ###### Database Not Connecting

&#x20;    - Check connection string



###### 2\. Migration Issues



*dotnet ef migrations add InitialCreate*

*dotnet ef database update*



###### 3\. Roles Not Working

&#x20;    - Logout and login again (cookie refresh issue)



##### **Future Enhancements**



* Email notifications for registration
* Event reminders
* Advanced filtering/search
* UI improvements
* Mobile responsiveness
* Payment integration (optional)



###### **Author**

Nishant Velhankar

Rishika Jaiswal

