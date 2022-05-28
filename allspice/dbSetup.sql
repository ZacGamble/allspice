CREATE TABLE IF NOT EXISTS accounts(
    id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    name varchar(255) COMMENT 'User Name',
    email varchar(255) COMMENT 'User Email',
    picture varchar(255) COMMENT 'User Picture'
) default charset utf8 COMMENT '';

CREATE TABLE IF NOT EXISTS recipes(
    id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY,
    picture TEXT NOT NULL,
    title TEXT NOT NULL,
    subtitle TEXT NOT NULL,
    category TEXT NOT NULL,
    creatorId VARCHAR(255) NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) default charset utf8;

CREATE TABLE IF NOT EXISTS ingredients(
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name TEXT COMMENT 'Ingredient name',
    quantity TEXT COMMENT 'Amount',
    recipeId INT COMMENT 'primary key',
    creatorId VARCHAR(255) NOT NULL,
    FOREIGN KEY (recipeId) REFERENCES recipes(id) ON DELETE CASCADE,
    FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) DEFAULT charset utf8;

CREATE TABLE IF NOT EXISTS steps(
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    position INT COMMENT 'Chronological order',
    body TEXT COMMENT 'Instructions',
    recipeId INT COMMENT 'primary key',
    creatorId VARCHAR(255) NOT NULL,
    FOREIGN KEY (recipeId) REFERENCES recipes(id) ON DELETE CASCADE,
    FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) DEFAULT charset utf8;

CREATE TABLE IF NOT EXISTS favorites(
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    recipeId INT NOT NULL COMMENT 'primary key',
    accountId VARCHAR(255) NOT NULL,
    isFavorite TINYINT NOT NULL DEFAULT 0,
    FOREIGN KEY (recipeId) REFERENCES recipes(id) ON DELETE CASCADE,
    FOREIGN KEY (accountId) REFERENCES accounts(id) ON DELETE CASCADE
) default charset utf8;