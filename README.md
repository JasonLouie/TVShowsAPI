# TVShows API
A REST API that allows users to view American TV Show ratings from IMDB and Rotten Tomatoes. Also allows users to create their own reviews about other TV shows in the database.
## Endpoints:

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `api/TVShows` | Returns all TVShows in the database. |
| GET | `api/TVShows/{id}` | Returns TVShow of ShowId id. |
| GET | `api/TVShows/top/{database}/{num}` | Returns top num of TV Shows ordered by ratings in the chosen database from highest to lowest |
| GET | `api/TVShows/genre/{genre}` | Returns all TV Shows of a specific genre. |
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
    "statusDescription": "Successfully retrieved TV Show of ShowId 5",
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
    "statusDescription": "Could not find TV Show of ShowId 42. The TV Show does not exist in the database.",
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

**Explanation: Since FakeDB isn't one of the 3 possible databases (IMDB, RT, DB), this doesn't work. When num is less than 1 or greater than 22, the user is given status code 400 for Bad Request since there are only 22 TV Shows.
If the order of ranking is not 'top', 'best', or 'worst' a 'Not Found' status code of 404 is returned because the order of ranking does not exist in the database.**

____________________________________________________________________________

#### GET: `api/TVShows/genre/{genre}`

GOOD SAMPLE RESPONSE `api/TVShows/genre/Superhero`

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved 3 TV Shows of genre Superhero.",
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
            "showId": 6,
            "showName": "Arrow",
            "showDesc": "Spoiled billionaire playboy Oliver Queen is missing and presumed dead when his yacht is lost at sea. He returns five years later a changed man, determined to clean up the city as a hooded vigilante armed with a bow.",
            "genres": [
                {
                    "genreId": 1,
                    "genre": "Action"
                },
                {
                    "genreId": 7,
                    "genre": "Crime"
                },
                {
                    "genreId": 11,
                    "genre": "Drama"
                },
                {
                    "genreId": 16,
                    "genre": "Mystery"
                },
                {
                    "genreId": 26,
                    "genre": "Superhero"
                }
            ],
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
            "genres": [
                {
                    "genreId": 1,
                    "genre": "Action"
                },
                {
                    "genreId": 8,
                    "genre": "Crime Drama"
                },
                {
                    "genreId": 14,
                    "genre": "Legal Drama"
                },
                {
                    "genreId": 26,
                    "genre": "Superhero"
                }
            ],
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
    "statusDescription": "Successfully retrieved all users.",
    "result": [
        {
            "userId": 1,
            "username": "Matt",
            "userPassword": "bananana234",
            "numOfReviews": 2
        },
        {
            "userId": 2,
            "username": "George",
            "userPassword": "setasde3!",
            "numOfReviews": 1
        },
        {
            "userId": 3,
            "username": "Johnny",
            "userPassword": "password",
            "numOfReviews": 1
        },
        {
            "userId": 4,
            "username": "Mimi",
            "userPassword": "pwa234",
            "numOfReviews": 3
        },
        {
            "userId": 5,
            "username": "Monica",
            "userPassword": "password",
            "numOfReviews": 2
        },
        {
            "userId": 6,
            "username": "Jonathan",
            "userPassword": "password",
            "numOfReviews": 1
        },
        {
            "userId": 7,
            "username": "Sara",
            "userPassword": "p@$$w0rd",
            "numOfReviews": 0
        },
        {
            "userId": 8,
            "username": "Thomas",
            "userPassword": "p@$$w0rd",
            "numOfReviews": 2
        },
        {
            "userId": 9,
            "username": "Jacob",
            "userPassword": "329asd62#ds5",
            "numOfReviews": 1
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
    "statusDescription": "Successfully retrieved user of UserId 1.",
    "result": {
        "userId": 1,
        "username": "Matt",
        "userPassword": "bananana234",
        "numOfReviews": 2
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
**Explanation: A user of UserId 11 does not exist, hence the status code 404 response in the bad sample response.**

____________________________________________________________________________

#### GET: `api/UserReviews`

SAMPLE RESPONSE

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved all user reviews.",
    "result": [
        {
            "reviewId": 1,
            "showId": 5,
            "userId": 1,
            "userRating": 8,
            "userComment": "Great show, but did not like it when Rick wasn't around."
        },
        {
            "reviewId": 2,
            "showId": 8,
            "userId": 1,
            "userRating": 10,
            "userComment": "Characters are hilarious!"
        },
        {
            "reviewId": 3,
            "showId": 21,
            "userId": 4,
            "userRating": 8.8,
            "userComment": "Overall, I loved the show. The only part I didn't like was the ending."
        },
        {
            "reviewId": 4,
            "showId": 7,
            "userId": 2,
            "userRating": 8,
            "userComment": "A classic! MUST WATCH!"
        },
        {
            "reviewId": 5,
            "showId": 2,
            "userId": 4,
            "userRating": 5,
            "userComment": "Not my cup of tea."
        },
        {
            "reviewId": 6,
            "showId": 3,
            "userId": 3,
            "userRating": 9,
            "userComment": ""
        },
        {
            "reviewId": 7,
            "showId": 3,
            "userId": 5,
            "userRating": 7,
            "userComment": ""
        },
        {
            "reviewId": 8,
            "showId": 10,
            "userId": 4,
            "userRating": 9.3,
            "userComment": ""
        },
        {
            "reviewId": 9,
            "showId": 4,
            "userId": 5,
            "userRating": 5,
            "userComment": ""
        },
        {
            "reviewId": 10,
            "showId": 1,
            "userId": 3,
            "userRating": 6.6,
            "userComment": "Decent show. The plot of the more recent seasons is lacking."
        },
        {
            "reviewId": 11,
            "showId": 1,
            "userId": 6,
            "userRating": 8.5,
            "userComment": "Great show, but person A was a really bad actor."
        },
        {
            "reviewId": 12,
            "showId": 1,
            "userId": 9,
            "userRating": 10,
            "userComment": "Fantastic show! Grant Gustin is a really talented actor."
        },
        {
            "reviewId": 13,
            "showId": 20,
            "userId": 8,
            "userRating": 9,
            "userComment": "Enjoyed the show!"
        },
        {
            "reviewId": 14,
            "showId": 22,
            "userId": 8,
            "userRating": 8,
            "userComment": "Enjoyed the show! Would definitely watch again."
        }
    ]
}
```
**Explanation: Simply returns all data for the table UserReviews.**

____________________________________________________________________________

#### GET: `api/UserReviews/{type}/{id}`

GOOD SAMPLE RESPONSE `api/UserReviews/shows/2`

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved all user reviews of ShowId 2.",
    "result": [
        {
            "reviewId": 5,
            "showId": 2,
            "userId": 4,
            "userRating": 5,
            "userComment": "Not my cup of tea."
        }
    ]
}
```

BAD SAMPLE RESPONSE `api/UserReviews/shows/11`

```
{
    "statusCode": 404,
    "statusDescription": "Could not find any user reviews for TV Show of ShowId 11.",
    "result": null
}
```
**Explanation: The bad sample response demonstrates the case where there aren't any user reviews for the 11th show. This case applies to other shows that haven't been reviewed yet.**

____________________________________________________________________________

#### GET: `api/UserReviews/{id}`

GOOD SAMPLE RESPONSE `api/UserReviews/4`

```
{
    "statusCode": 200,
    "statusDescription": "Successfully retrieved user review with ReviewId of 4.",
    "result": {
        "reviewId": 4,
        "showId": 7,
        "userId": 2,
        "userRating": 8,
        "userComment": "A classic! MUST WATCH!"
    }
}
```

BAD SAMPLE RESPONSE `api/UserReviews/30`

```
{
    "statusCode": 404,
    "statusDescription": "Could not find user review with ReviewId of 30. The review does not exist in the database.",
    "result": null
}
```
**Explanation: The following response is provided whenever the ReviewId provided does not exist in the database.**

____________________________________________________________________________

#### PUT: `api/Users/{id}`

GOOD SAMPLE BODY `api/Users/1`

```
{
    "userid": 1,
    "username": "Matthew",
    "userpassword": "bananana234",
    "numOfReviews": 100
}
```

GOOD SAMPLE RESPONSE

*Nothing but Code 204 from Postman*

BAD SAMPLE BODY `api/Users/4`

```
{
    "userid": 4,
    "username": "Matthew",
    "userpassword": "pwa234",
    "numOfReviews": 23
}
```

BAD SAMPLE RESPONSE

```
{
    "statusCode": 409,
    "statusDescription": "Error. The entry already exists.",
    "result": null
}
```

**Explanation: The username Matthew has not been used yet so the change was a success in the good sample response. However, a different user
                of user id 4 tries to use this name afterwards and since it is used, the appropriate response for status code 409 is returned. Notice how an attempt
                has been made to change the number of user reviews in both examples. The values remained the same since users cannot have direct control of how
                many review they have. The same would not happen for userid since providing a different userid returns the status code of 400 prompting the user
                to check their input values for typos.**

____________________________________________________________________________

#### PUT: `api/UserReviews/{id}`

GOOD SAMPLE BODY `api/UserReviews/4`

```
{
    "reviewId": 4,
    "showId": 7,
    "userId": 2,
    "userRating": 8.8,
    "userComment": "A classic! MUST WATCH!"
}
```

GOOD SAMPLE RESPONSE

*Nothing but Code 204 from Postman*

BAD SAMPLE BODY `api/UserReviews/5`

```
{
    "showId": 7,
    "userId": 2,
    "userRating": 9.8,
    "userComment": "A classic! MUST WATCH!"
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

**Explanation: Each body needs to include the ReviewId of the user review, but ShowId and UserId can be left out. Even
                if "reviewId": 4 was included, the request would still be bad because the ReviewId in the request and body must be the same.
                Leaving out UserRating and UserComment will set the UserRating back to 0 and erase the current comment.**

____________________________________________________________________________

#### POST: `api/Users`
GOOD SAMPLE BODY

```
{
    "userId": 23,
    "username": "Wayne",
    "userPassword": "e34se#q2g4@",
    "numOfReviews": 20
}
```

GOOD SAMPLE RESPONSE

```
{
    "statusCode": 201,
    "statusDescription": "Successfully created user.",
    "result": {
        "userId": 10,
        "username": "Wayne",
        "userPassword": "e34se#q2g4@",
        "numOfReviews": 0
    }
}
```

**Explanation: Notice how the user provided userId and numOfReviews, but the following variables in the response are different.
                This correctly demonstrates that the user has no control over the number of reviews or their own user id.**

BAD SAMPLE BODY

```
{
    "userid": 2,
    "username": "Matthew",
    "userpassword": "matt123",
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

**Explanation: he following body did not create a user because the name Matthew is already used. UserId is not a conflict because the user id provided is always ignored.**
____________________________________________________________________________


#### POST: `api/UserReviews`

GOOD SAMPLE BODY

```
{
    "reviewId": 4,
    "showId": 20,
    "userId": 4,
    "userRating": 8.8
}
```

GOOD SAMPLE RESPONSE

```
{
    "statusCode": 201,
    "statusDescription": "Successfully created user review for user of UserId 4 for ShowId 20.",
    "result": {
        "reviewId": 15,
        "showId": 20,
        "userId": 4,
        "userRating": 8.8,
        "userComment": null
    }
}
```

BAD SAMPLE BODY

```
{
    "reviewId": 8,
    "showId": 3,
    "userId": 5,
    "userRating": 10,
    "userComment": "Great show!"
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

**Explanation: In the good sample response notice how the userComment varaible is missing and the ReviewId is different. This is
                fine because comments by the user are optional and review id provided by the user is always replaced by one from the database.
                All bad samples include reusing the same pair of (showId, userId) for a post or if a reviewId has already been used.
                ReviewId provided by the user is ignored and will be provided by the database. It starts from the previously entered ReviewId
                (so if the last entered is 10, next is 11). If the last record added was ReviewId = 12 and that review was deleted, the next ReviewId
                will still be 13.**

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

**Explanation: Users should not be able to remove TVShows, so it will always return this message.**

____________________________________________________________________________

#### DELETE: `api/Users/{id}`

GOOD SAMPLE RESPONSE `api/Users/3`

*Nothing but Code 204 from Postman. Also removed all of the user's posts and updated the AVGUserRatings in table TVShows.*

BAD SAMPLE RESPONSE `api/Users/13`

```
{
    "statusCode": 404,
    "statusDescription": "Could not find user of UserId 13. User does not exist in the database.",
    "result": null
}
```
**Explanation: If the UserId provided exists, then the user and any of their data is deleted. Any of the user's reviews are also deleted and each TV show's AVGUserRating is updated.**

____________________________________________________________________________

#### DELETE: `api/UserReviews/{id}`

GOOD SAMPLE RESPONSE `api/UserReviews/8`

*Nothing but Code 204 from Postman. Also other necessary values are removed/updated.*

BAD SAMPLE RESPONSE `api/UserReviews/35`

```
{
    "statusCode": 404,
    "statusDescription": "User Review of Review Id 35 could not be found.",
    "result": null
}
```
**Explanation: If the ReviewId provided exists, then the review is deleted. The respective TV show's AVGUserRating is updated while deleting the review.**
