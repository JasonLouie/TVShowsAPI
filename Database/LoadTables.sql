USE TVShowsDatabase;

# Load data into respective table. Make sure to load data in this order.
# NOTE: If you want an empty database except for TVShows and related genre data, comment out the Users and UserReviews data load.
# Also, when loading in data, replace {directory} with the path to each respective file.

LOAD DATA LOCAL INFILE '{directory}/Genres.csv' INTO TABLE Genres FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"' LINES TERMINATED BY '\r\n' ignore 1 lines;

LOAD DATA LOCAL INFILE '{directory}/TVShowsWithReviews.csv' INTO TABLE TVShows FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"' LINES TERMINATED BY '\r\n' ignore 1 lines;
# LOAD DATA LOCAL INFILE '{directory}/TVShows.csv' INTO TABLE TVShows FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"' LINES TERMINATED BY '\r\n' ignore 1 lines;

LOAD DATA LOCAL INFILE '{directory}/ShowGenres.csv' INTO TABLE ShowGenres FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"' LINES TERMINATED BY '\r\n' ignore 1 lines;

LOAD DATA LOCAL INFILE '{directory}/Users.csv' INTO TABLE Users FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"' LINES TERMINATED BY '\r\n' ignore 1 lines;

LOAD DATA LOCAL INFILE '{directory}/UserReviews.csv' 
INTO TABLE UserReviews FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"' LINES TERMINATED BY '\r\n' ignore 1 lines;
