# Hotel Reservations Manager
Web application to serve as a management system for a hotel, handling reservations and employees.

## Description
Made as a course project for IT Career National Programme Module 13 "Software Engineering" by Tsvetilin Tsvetilov, Stoyan Zlatev and Pavlin Marinov. Fully functional ASP.NET 5 MVC application using various technologies.

### Details
The application is capable of adding rooms with specified cpacity, type and image, and managing them. Every registered user is able to make a reservation for a room and this proccess is accompanied by dynamic price calculation and room availabilty check for desired period. Every non-past reservation can be canceled or modified. The systems allows adding employee accounts. They are capable of seeing all the reservations made by clients and modify them when needed. There is one master admin account that hires the employees and handles their resignations. They are capable of managing the hotel price policy as well.

## Note before download and usage
- The project relies on external service providers. Thus accessing them via their API requires authentication. An API Key need to be supplied in order to achive the full functional capacity of the app. Please fill in the **Sendgrid** and **Cloudinary** sections in the **appsettings.json** with your personal keys.
- The application will still work if not supplied with valid api keys but with a limited functionality. Email sending will be disabled and adding rooms will be impossible without image uploaded to cloudinary.
- An admin account is seeded with login credentials **Admin** for username and **AdminPass** for password

## Stucture of the project
Three-tier architecture following the MVC pattern

### Data Layer
- Code First database approach
- MSSQL Server Database
- Entity Framework Core
- Migraions
- Automated data seeding on database creation

### Service Layer
- AutoMapper
- Data services handling database access and the logic of the application
- External services
    - SendGrid -> Email sending
    - Cloudinary -> Uploading and storing application images in the cloud
- Common logic extension methods separated
  ```
  public static IEnumerable<T> GetPageItems<T>(this IEnumerable<T> items, int page, int elementsOnPage)
  {
       return items.Skip(elementsOnPage * (page - 1)).Take(elementsOnPage).AsQueryable().ToList();
  }
  ```
  
### Presentation Layer
- ASP.NET 5
- InMemory Cache
- GDPR compatible
- Bundling and minification, client-side libraries restoration
- Responsive design

### Tests
- 70% code coverage
- Technologies
  - NUnit
  - xUnit
  - Moq
  - InMemory database
- Unit tests of the service layer use **InMemory database** and cover all logic operation of the data manipulation
- Integration tests use **SQL Server databse** initialized for the test and deleted after its completion

## Summary
The project has a lot of improvement opportunities that include but are not limited to:
- UI / UX improvement
- Multiple images per room
- Forced reservation cancelation request user confirmation

## Team
- Tsvetilin Tsvetilov - [GitHub](https://github.com/Tsvetilin "Tsvetilin's GitHub profile")
- Stoyan Zlatev - [GitHub](https://github.com/Tony5768 "Stoyan's GitHub profile")
- Pavlin Marinov - [GitHub](https://github.com/pavlin1004 "Pavlin's GitHub profile")

## License
HotelReservationManager is distributed under the GNU General Public License GPLv3 or higher, see the file LICENSE for details.
