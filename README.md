# MovieTime Backend

The backend for the MovieTime application. Built with **.NET** to provide REST APIs for movie data, user accounts, and personalized recommendations. This backend is used by the MovieTime frontend. See the movietime full app to find the project w/ the docker file so that you can run it locally yourself. (Or look at the AWS link... coming soon!)

Made with much love and passion by your cook, Jun Li <3

## Features
- REST API for user authentication, movie data, and recommendations
- Entity Framework Core for data access
- SQL database integration (configurable)
- Docker support for containerized deployment
- Separation of concerns with services, controllers, and models

## Project Structure
```bash
JasianE/
├── Controllers/        # API controllers (Movies, Account, Recommendations, etc.)
├── DTOs/               # Data transfer objects
├── Data/               # Database context and setup
├── Extensions/         # Extension methods and custom middleware
├── Helpers/            # Utility classes (pagination, filtering, etc.)
├── Interfaces/         # Service and repository interfaces
├── Mappers/            # Mapping logic between models and DTOs
├── Migrations/         # Entity Framework database migrations
├── Models/             # Data models/entities
├── Repositories/       # Database access layer
├── Seed/               # Database seeding scripts
└── appsettings.Development.json # I love appsetting.jsons (wish it was a package.json, mfw .NEt doesn't use package .json but a .dll file :( --> my npm package manager heart
```
## API Endpoints

### Account

| Method | Endpoint                 | Description                       | Authorization |
|--------|-------------------------|-----------------------------------|---------------|
| POST   | `/api/account/register`  | Register a new user               | No            |
| POST   | `/api/account/login`     | Log in a user and get a JWT token | No            |

---

### Movie

| Method | Endpoint                          | Description                                 | Authorization |
|--------|----------------------------------|---------------------------------------------|---------------|
| GET    | `/api/movie/all`                 | Get all movies, supports query parameters  | No            |
| POST   | `/api/movie/add?MovieTitle=`     | Add a movie to the database                 | Yes           |

---

### User

| Method | Endpoint                                 | Description                                                | Authorization |
|--------|-----------------------------------------|------------------------------------------------------------|---------------|
| GET    | `/api/users/all`                         | Get all users                                             | No            |
| GET    | `/api/users/username?userName=`         | Get a user by username                                     | No            |
| GET    | `/api/users/{id}`                        | Get a user by ID, including the list of their movies     | No            |

---

### UserMovie

| Method | Endpoint                                     | Description                                           | Authorization |
|--------|---------------------------------------------|-------------------------------------------------------|---------------|
| GET    | `/api/usermovie`                            | Get the currently logged-in user’s movies           | Yes           |
| POST   | `/api/usermovie`                            | Add a movie to the currently logged-in user         | Yes           |
| PUT    | `/api/usermovie/update/{MovieTitle}`       | Update the status of a user’s movie (e.g., watched)| Yes           |

