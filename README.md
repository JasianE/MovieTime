# MovieTime Backend

The backend for the MovieTime application. Built with **.NET** to provide REST APIs for movie data, user accounts, friend requests, and recommendations. This backend is used by the MovieTime frontend.

## Features
- REST API for user authentication, movie data, and recommendations
- Entity Framework Core for data access
- SQL database integration (configurable)
- Docker support for containerized deployment
- Friend requests and friend-only recommendations
- Recommendation metadata with rating/notes/watched updates
- Separation of concerns with services, controllers, and models

## Docker Compose

From the repo root:

```bash
docker compose up --build
```

Compose reads environment defaults from the repo root .env file.

- Backend: http://localhost:5000
- Frontend: http://localhost:3000

## Project Structure
```bash
api/
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
└── appsettings.Development.json # Local configuration
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
| GET    | `/api/users/all`                         | Get all users                                             | Yes           |
| GET    | `/api/users/username?userName=`         | Get a user by username                                     | Yes           |
| GET    | `/api/users/{id}`                        | Get a user by ID, including the list of their movies     | Yes           |

---

### UserMovie

| Method | Endpoint                                     | Description                                           | Authorization |
|--------|---------------------------------------------|-------------------------------------------------------|---------------|
| GET    | `/api/usermovie`                            | Get the currently logged-in user’s movies           | Yes           |
| POST   | `/api/usermovie`                            | Add a movie recommendation to a friend              | Yes           |
| PUT    | `/api/usermovie/update/{MovieTitle}`       | Update the status of a user’s movie (e.g., watched)| Yes           |
| GET    | `/api/usermovie/recommendations/{id}`      | Get recommendation detail                           | Yes           |
| PUT    | `/api/usermovie/recommendations/{id}`      | Update rating/notes/watched for recommendation      | Yes           |

---

### Friends

| Method | Endpoint                                  | Description                                   | Authorization |
|--------|-------------------------------------------|-----------------------------------------------|---------------|
| GET    | `/api/friends/list`                        | Get the current user's friends               | Yes           |
| GET    | `/api/friends/requests/incoming`           | Incoming friend requests                      | Yes           |
| GET    | `/api/friends/requests/outgoing`           | Outgoing friend requests                      | Yes           |
| POST   | `/api/friends/requests`                    | Send a friend request                         | Yes           |
| PUT    | `/api/friends/requests/{id}/accept`        | Accept a friend request                       | Yes           |
| PUT    | `/api/friends/requests/{id}/decline`       | Decline a friend request                      | Yes           |


