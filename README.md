# TVShows API
A REST API that allows users to view American TV Show ratings from IMDB and Rotten Tomatoes. Also allows users to create their own reviews about other TV shows in the database.
## Endpoints:

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `api/TVShows` | Returns all TVShows in the database. |
| GET | `api/TVShows/{id}` | Returns TVShow of ShowId id. |
| GET | `api/TVShows/genre/{genre}` | Returns all TV Shows of a specific genre. |
| GET | `api/TVShows/top/{database}/{num}` | Returns top num of TV Shows ordered by ratings in the chosen database from highest to lowest |
| GET | `api/Users` | Returns all UserInfo in the database. |
| GET | `api/Users/{id}` | Returns Userinfo of UserId id. |
| GET | `api/UserReviews` | Returns all UserReviews in the database. |
| GET | `api/UserReviews/{type}/{id}` | Returns all UserReviews for ShowId or ReviewId or UserId) type=shows or users. |
| GET | `api/UserReviews/{id}` | Returns specific review of a show by a user. |
| PUT | `api/Users/{id}` | Update UserName and/or Password; not allowed to update UserId and NumOfUserRatings. |
| PUT | `api/UserReviews/{id}` | Update a UserReview except for ReviewId, ShowId, UserId. It updates the NumOfUserRatings from UserInfo and AVGUserRatings from TVShows. |
| POST | `api/Users` | Create a User with a UserId, unique Username, Password; not allowed to initialize NumOfUserRatings
| POST | `api/UserReviews` | Create a UserReview with ReviewId (optional), ShowId, UserId, UserRating, UserComment (optional). Also updates the table UserInfo the NumOfUserRatings and in TVShows the AVGUserRating. |
| DELETE | `api/TVShows/{id}` | Since users should not be able to delete TVShows, it will always return status code 405 method not allowed.
| DELETE | `api/Users/{id}` | Delete User with UserId id. Remove all of the user's posts and update the AVGUserRatings in table TVShows. |
| DELETE | `api/UserReviews/{id}` | Deletes a particular UserReview with ReviewId id. It updates the NumOfUserRatings from UserInfo and AVGUserRatings from TVShows.

## Samples and Explanations:

### GET: `api/TVShows`

SAMPLE RESPONSE

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved all TV Shows.",
    "result": [
        {
            "showId": 1,
            "showName": "The Flash",
            "showDesc": "After being struck by lightning, Barry Allen wakes up from his coma to discover he's been given the power of super speed, becoming the Flash, and fighting crime in Central City.",
            "genres": [
                {
                    "genreId": 1,
                    "genre": "Action"
                },
                {
                    "genreId": 11,
                    "genre": "Drama"
                },
                {
                    "genreId": 26,
                    "genre": "Superhero"
                }
            ],
            "numSeasons": 8,
            "numEpisodes": 171,
            "episodeLength": 43,
            "yearReleased": 2014,
            "ongoing": true,
            "rTrating": 0.89,
            "imdBrating": 7.6,
            "avgUserRating": 8.4
        },
        {
            "showId": 2,
            "showName": "Criminal Minds",
            "showDesc": "The cases of the F.B.I. Behavioral Analysis Unit (B.A.U.), an elite group of profilers who analyze the nation's most dangerous serial killers and individual heinous crimes in an effort to anticipate their next moves before they strike again.",
            "genres": [
                {
                    "genreId": 8,
                    "genre": "Crime Drama"
                },
                {
                    "genreId": 16,
                    "genre": "Mystery"
                },
                {
                    "genreId": 19,
                    "genre": "Police Procedural"
                },
                {
                    "genreId": 30,
                    "genre": "Thriller"
                }
            ],
            "numSeasons": 16,
            "numEpisodes": 326,
            "episodeLength": 42,
            "yearReleased": 2005,
            "ongoing": false,
            "rTrating": 0.85,
            "imdBrating": 8.1,
            "avgUserRating": 5
        },
        ...more shows,
        {
            "showId": 22,
            "showName": "Stranger Things",
            "showDesc": "When a young boy disappears, his mother, a police chief and his friends must confront terrifying supernatural forces in order to get him back.",
            "genres": [
                {
                    "genreId": 13,
                    "genre": "Horror"
                },
                {
                    "genreId": 16,
                    "genre": "Mystery"
                },
                {
                    "genreId": 27,
                    "genre": "Supernatural Fiction"
                },
                {
                    "genreId": 30,
                    "genre": "Thriller"
                }
            ],
            "numSeasons": 4,
            "numEpisodes": 34,
            "episodeLength": 51,
            "yearReleased": 2016,
            "ongoing": true,
            "rTrating": 0.92,
            "imdBrating": 8.7,
            "avgUserRating": 8
        }
    ]
}
```

**Explanation: Simply returns all TVShows. At the moment there are 22 shows in the database. To shorten the response in the README elements with ShowId 3-21 were replaced with '...more shows'.**
____________________________________________________________________________

#### GET: `api/TVShows/{id}`
GOOD SAMPLE RESPONSE `api/TVShows/5`

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved TV Show of id 5",
    "result": {
        "showId": 5,
        "showName": "The Walking Dead",
        "showDesc": "Sheriff Deputy Rick Grimes wakes up from a coma to learn the world is in ruins and must lead a group of survivors to stay alive.",
        "genres": [
            {
                "genreId": 13,
                "genre": "Horror"
            },
            {
                "genreId": 24,
                "genre": "Serial Drama"
            },
            {
                "genreId": 33,
                "genre": "Zombie Apocalypse"
            }
        ],
        "numSeasons": 11,
        "numEpisodes": 177,
        "episodeLength": 44,
        "yearReleased": 2010,
        "ongoing": false,
        "rTrating": 0.8,
        "imdBrating": 8.1,
        "avgUserRating": 8
    }
}
```

BAD SAMPLE RESPONSE `api/TVShows/42`

```
{
    "statusCode": 404,
    "statusDescription": "Could not find TV Show of id 42. The TV Show does not exist in the database.",
    "result": null
}
```

**Explanation: I only inserted 22 TVShows in here so anything above ShowId 22 is not a valid entry.**
____________________________________________________________________________

#### GET: api/TVShows/{database}/{order}/{num}

GOOD SAMPLE RESPONSE `api/TVShows/IMDB/top/4`
```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved the 4 top TV Shows from IMDB",
    "result": [
        {
            "showId": 21,
            "showName": "Game of Thrones",
            "showDesc": "Nine noble families fight for control over the lands of Westeros, while an ancient enemy returns after being dormant for millennia.",
            "genres": [
                {
                    "genreId": 1,
                    "genre": "Action"
                },
                {
                    "genreId": 3,
                    "genre": "Adventure"
                },
                {
                    "genreId": 12,
                    "genre": "Fantasy"
                },
                {
                    "genreId": 24,
                    "genre": "Serial Drama"
                },
                {
                    "genreId": 31,
                    "genre": "Tragedy"
                }
            ],
            "numSeasons": 8,
            "numEpisodes": 73,
            "episodeLength": 57,
            "yearReleased": 2011,
            "ongoing": false,
            "rTrating": 0.89,
            "imdBrating": 9.2,
            "avgUserRating": 8.8
        },
        {
            "showId": 13,
            "showName": "Rick and Morty",
            "showDesc": "An animated series that follows the exploits of a super scientist and his not-so-bright grandson.",
            "genres": [
                {
                    "genreId": 2,
                    "genre": "Adult Animation"
                },
                {
                    "genreId": 3,
                    "genre": "Adventure"
                },
                {
                    "genreId": 4,
                    "genre": "Animated Sitcom"
                },
                {
                    "genreId": 10,
                    "genre": "Dark Humor"
                },
                {
                    "genreId": 23,
                    "genre": "Science Fiction"
                }
            ],
            "numSeasons": 6,
            "numEpisodes": 58,
            "episodeLength": 23,
            "yearReleased": 2013,
            "ongoing": true,
            "rTrating": 0.93,
            "imdBrating": 9.1,
            "avgUserRating": 0
        },
        {
            "showId": 14,
            "showName": "Friends",
            "showDesc": "Follows the personal and professional lives of six twenty to thirty year-old friends living in the Manhattan borough of New York City.",
            "genres": [
                {
                    "genreId": 5,
                    "genre": "Comedy"
                },
                {
                    "genreId": 25,
                    "genre": "Sitcom"
                }
            ],
            "numSeasons": 10,
            "numEpisodes": 236,
            "episodeLength": 22,
            "yearReleased": 1994,
            "ongoing": false,
            "rTrating": 0.93,
            "imdBrating": 8.9,
            "avgUserRating": 0
        },
        {
            "showId": 22,
            "showName": "Stranger Things",
            "showDesc": "When a young boy disappears, his mother, a police chief and his friends must confront terrifying supernatural forces in order to get him back.",
            "genres": [
                {
                    "genreId": 13,
                    "genre": "Horror"
                },
                {
                    "genreId": 16,
                    "genre": "Mystery"
                },
                {
                    "genreId": 27,
                    "genre": "Supernatural Fiction"
                },
                {
                    "genreId": 30,
                    "genre": "Thriller"
                }
            ],
            "numSeasons": 4,
            "numEpisodes": 34,
            "episodeLength": 51,
            "yearReleased": 2016,
            "ongoing": true,
            "rTrating": 0.92,
            "imdBrating": 8.7,
            "avgUserRating": 8
        }
    ]
}
```

BAD SAMPLE RESPONSE `api/TVShows/top/FakeDB/3`

```
{
    "statusCode": 404,
    "statusDescription": "Could not find 3 top TV Shows from FakeDB.",
    "result": null
}
```

**Explanation: Since FakeDB isn't one of the 3 possible databases (IMDB, RT, DB), this doesn't work. When num is less than 1 or greater than 22, the user is given status code 400 for Bad Request since there are only 22 TV Shows.**

____________________________________________________________________________

#### GET: `api/TVShows/genre/{genre}`

GOOD SAMPLE RESPONSE `api/TVShows/genre/Superhero`

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved TVShows that are Superhero",
    "result": [
        {
            "showId": 1,
            "showName": "The Flash",
            "showDesc": "After being struck by lightning, Barry Allen wakes up from his coma to discover he's been given the power of super speed, becoming the Flash, and fighting crime in Central City.",
            "genre": "Superhero",
            "numSeasons": 8,
            "numEpisodes": 171,
            "episodeLength": 43,
            "yearReleased": 2014,
            "ongoing": true,
            "rTrating": 0.89,
            "imdBrating": 7.6,
            "avgUserRating": 0
        },
        {
            "showId": 6,
            "showName": "Arrow",
            "showDesc": "Spoiled billionaire playboy Oliver Queen is missing and presumed dead when his yacht is lost at sea. He returns five years later a changed man, determined to clean up the city as a hooded vigilante armed with a bow.",
            "genre": "Superhero",
            "numSeasons": 8,
            "numEpisodes": 170,
            "episodeLength": 42,
            "yearReleased": 2012,
            "ongoing": false,
            "rTrating": 0.86,
            "imdBrating": 7.5,
            "avgUserRating": 0
        },
        {
            "showId": 15,
            "showName": "Daredevil",
            "showDesc": "A blind lawyer by day, vigilante by night. Matt Murdock fights the crime of New York as Daredevil.",
            "genre": "Superhero",
            "numSeasons": 3,
            "numEpisodes": 39,
            "episodeLength": 54,
            "yearReleased": 2015,
            "ongoing": false,
            "rTrating": 0.92,
            "imdBrating": 8.6,
            "avgUserRating": 0
        }
    ]
}
```

BAD SAMPLE RESPONSE `api/TVShows/genre/TEST`

```
{
    "statusCode": 404,
    "statusDescription": "TVShows that are TEST could not be found.",
    "result": null
}
```
**Explanation: Since TEST isn't a genre, the bad sample response doesn't provide any TVShows of genre TEST.**

____________________________________________________________________________

#### GET: `api/Users`

SAMPLE RESPONSE

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved Users",
    "result": [
        {
            "userId": 1,
            "username": "Harry",
            "userPassword": "password",
            "numOfReviews": 0
        },
        {
            "userId": 2,
            "username": "Barry",
            "userPassword": "password",
            "numOfReviews": 0
        },
        {
            "userId": 3,
            "username": "Bob",
            "userPassword": "password",
            "numOfReviews": 0
        }
    ]
}
```
**Explanation: Simply returns all Users in the database and their information. Passwords are only shown for demo purposes. Realistically, passwords would not be shown.**

____________________________________________________________________________

#### GET: `api/Users/{id}`

GOOD SAMPLE RESPONSE `api/Users/1`

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved User of UserId 1",
    "result": {
        "userId": 1,
        "username": "Harry",
        "userPassword": "password",
        "numOfReviews": 0
    }
}
```

BAD SAMPLE RESPONSE `api/Users/11`

```
{
    "statusCode": 404,
    "statusDescription": "User of User Id 11 could not be found.",
    "result": null
}
```
**Explanation: A user of UserId 11 does not exist since there are currently 3 users in the database, hence an error occurred in the bad sample response.**

____________________________________________________________________________

#### GET: `api/UserReviews`

SAMPLE RESPONSE

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved User Reviews",
    "result": [
        {
            "reviewId": 8,
            "showId": 2,
            "userId": 3,
            "userRating": 10,
            "userComment": null
        },
        {
            "reviewId": 9,
            "showId": 6,
            "userId": 2,
            "userRating": 9,
            "userComment": "Loved this show!"
        },
        {
            "reviewId": 10,
            "showId": 2,
            "userId": 2,
            "userRating": 7,
            "userComment": null
        }
    ]
}
```
**Explanation: Simply returns all UserReviews.**

____________________________________________________________________________

#### GET: `api/UserReviews/{type}/{id}`

GOOD SAMPLE RESPONSE `api/UserReviews/shows/2`

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved UserReviews of ShowId 2",
    "result": [
        {
            "reviewId": 8,
            "showId": 2,
            "userId": 3,
            "userRating": 10,
            "userComment": null
        },
        {
            "reviewId": 10,
            "showId": 2,
            "userId": 2,
            "userRating": 7,
            "userComment": null
        }
    ]
}
```

BAD SAMPLE RESPONSE `api/UserReviews/shows/3`

```
{
    "statusCode": 404,
    "statusDescription": "UserReviews of ShowId 3 could not be found.",
    "result": null
}
```
**Explanation: The bad sample response demonstrates the case where there aren't any user reviews for the 3rd show. This case applies to other ShowId that are not at most 22.**

____________________________________________________________________________

#### GET: `api/UserReviews/{id}`

GOOD SAMPLE RESPONSE `api/UserReviews/8`

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved UserReview of ReviewId 8",
    "result": {
        "reviewId": 8,
        "showId": 2,
        "userId": 3,
        "userRating": 10,
        "userComment": null
    }
}
```

BAD SAMPLE RESPONSE `api/UserReviews/3`

```
{
    "statusCode": 404,
    "statusDescription": "UserReview of ReviewId 3 could not be found.",
    "result": null
}
```
**Explanation: Since there is no user review with ReviewId 3 in the bad sample response, the following response is provided.**

____________________________________________________________________________

#### PUT: `api/Users/{id}`

GOOD SAMPLE BODY `api/Users/1`

```
{
    "userid": 1,
    "username": "Larry",
    "userpassword": "password",
    "numOfReviews": 0
}
```

GOOD SAMPLE RESPONSE

*Nothing but Code 204 from Postman*

**Explanation: If a username is the same as an existing one, the request will still be valid, but username won't change. Name successfully changed from Harry to Larry.**

____________________________________________________________________________

#### PUT: `api/UserReviews/{id}`

GOOD SAMPLE BODY `api/UserReviews/8`

```
{
    "reviewId": 8,
    "showId": 2,
    "userId": 3,
    "userRating": 8,
    "userComment": "This is a comment"
}
```

GOOD SAMPLE RESPONSE

*Nothing but Code 204 from Postman*

BAD SAMPLE BODY `api/UserReviews/5`

```
{
    "reviewId": 8,
    "showId": 2,
    "userId": 3,
    "userRating": 8,
    "userComment": "This is a comment"
}
```

BAD SAMPLE RESPONSE

```
{
    "statusCode": 400,
    "statusDescription": "Invalid request. Check your request for typos.",
    "result": null
}
```

**Explanation: Since reviewId 5 existed, the request was invalid. However, if a UserReview of ReviewId5 existed, the response would match the good sample response, but the ReviewId would not be changed.**

____________________________________________________________________________

#### POST: `api/Users`
GOOD SAMPLE BODY

```
{
    "userid": 7,
    "username": "Matthew",
    "userpassword": "password",
    "numOfReviews": 10
}
```

GOOD SAMPLE RESPONSE

```
{
    "statusCode": 201,
    "statusDescription": "Successfully created User ",
    "result": {
        "userId": 7,
        "username": "Matthew",
        "userPassword": "password",
        "numOfReviews": 0
    }
}
```

**Explanation: This is good because user should never initialize numOfReviews. No matter what value it is, this will be set to 0 upon posting.**

BAD SAMPLE BODY

```
{
    "userid": 2,
    "username": "Harry",
    "userpassword": "password",
    "numOfReviews": 0
}
```

BAD SAMPLE RESPONSE

```
{
    "statusCode": 409,
    "statusDescription": "Error. The entry already exists",
    "result": null
}
```

**Explanation: This is bad because the name Harry is already being used. The same error occurs when repeating names and UserIds.**
____________________________________________________________________________


#### POST: `api/UserReviews`

GOOD SAMPLE BODY

```
{
    "reviewId": 11,
    "showId": 8,
    "userId": 3,
    "userRating": 7,
    "userComment": "Great show, but person A was a really bad actor."
}
```

GOOD SAMPLE RESPONSE

```
{
    "statusCode": 201,
    "statusDescription": "Successfully created User Review",
    "result": {
        "reviewId": 11,
        "showId": 8,
        "userId": 3,
        "userRating": 7,
        "userComment": "Great show, but person A was a really bad actor."
    }
}
```

BAD SAMPLE BODY

```
{
    "reviewId": 8,
    "showId": 2,
    "userId": 3,
    "userRating": 10
}
```

BAD SAMPLE RESPONSE

```
{
    "statusCode": 409,
    "statusDescription": "Error. The entry already exists",
    "result": null
}
```

**Explanation: Other Bad samples include using the same pair of (showId, userId) for a post or if a reviewId has already been used. ReviewId is optional so if
not provided the id will increment starting from the previously entered ReviewId (so if the last entered is 10, next is 11).**

____________________________________________________________________________

#### DELETE: `api/TVShows/{id}`

BAD SAMPLE RESPONSE ONLY `api/TVShows/2`

```
{
    "statusCode": 405,
    "statusDescription": "Method not allowed",
    "result": null
}
```

**Explanation: Method not implemented, but was autogenerated so I set it to not be allowed. Additionally, users should not be able to remove TVShows.**

____________________________________________________________________________

#### DELETE: `api/Users/{id}`

GOOD SAMPLE RESPONSE `api/Users/3`

*Nothing but Code 204 from Postman. Also removed all of the user's posts and updated the AVGUserRatings in table TVShows.*

BAD SAMPLE RESPONSE `api/Users/11`

```
{
    "statusCode": 404,
    "statusDescription": "User of User Id 11 could not be found.",
    "result": null
}
```
**Explanation: If the UserId provided exists, then the user and any of their data is deleted.**

____________________________________________________________________________

#### DELETE: `api/UserReviews/{id}`

GOOD SAMPLE RESPONSE `api/UserReviews/8`

*Nothing but Code 204 from Postman. Also other necessary values are removed/updated.*

BAD SAMPLE RESPONSE `api/UserReviews/5`

```
{
    "statusCode": 404,
    "statusDescription": "User Review of Review Id 5 could not be found.",
    "result": null
}
```
**Explanation: If the ReviewId provided exists, then the review is deleted.**
