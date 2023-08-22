-- Database Operations
DROP DATABASE IF EXISTS Zoo;
CREATE DATABASE Zoo;
USE Zoo;

-- Table Creation

CREATE TABLE account
(
    id         INT PRIMARY KEY AUTO_INCREMENT,
    accountbalance DOUBLE NOT NULL
);

CREATE TABLE checkout
(
    id         INT PRIMARY KEY AUTO_INCREMENT,
    accountbalance DOUBLE NOT NULL,
    account_id   INT,
    FOREIGN KEY (account_id) REFERENCES account (id)
);

CREATE TABLE shop
(
    id       INT PRIMARY KEY AUTO_INCREMENT,
    area   INT NOT NULL,
    account_id INT,
    FOREIGN KEY (account_id) REFERENCES account (id)
);
drop table if exists product;
CREATE TABLE product
(
    id        INT PRIMARY KEY AUTO_INCREMENT,
    price     DOUBLE       NOT NULL,
    name      VARCHAR(255) NOT NULL,
    time_point TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    stock  INT          NOT NULL,
    account_id  INT,
    FOREIGN KEY (account_id) REFERENCES checkout (id),
    shop_id   INT,
    FOREIGN KEY (shop_id) REFERENCES shop (id)
);

CREATE TABLE employee
(
    id                INT PRIMARY KEY AUTO_INCREMENT,
    position          VARCHAR(255) NOT NULL,
    employee_age INT          NOT NULL,
    name              VARCHAR(255) NOT NULL,
    shop_id           INT,
    FOREIGN KEY (shop_id) REFERENCES shop (id),
    account_id          INT,
    FOREIGN KEY (account_id) REFERENCES checkout (id)
);

CREATE TABLE enclosure
(
    id             INT PRIMARY KEY AUTO_INCREMENT,
    volume         INT          NOT NULL,
    location       VARCHAR(255) NOT NULL,
    enclosure_type     VARCHAR(255) NOT NULL,
    employee_id INT,
    FOREIGN KEY (employee_id) REFERENCES employee (id)
);

drop table if exists animals;
CREATE TABLE animals
(
    id        INT PRIMARY KEY AUTO_INCREMENT,
    species   VARCHAR(255) NOT NULL,
    food   VARCHAR(255) NOT NULL,
    enclosure_id INT,
    FOREIGN KEY (enclosure_id) REFERENCES enclosure (id)
);

-- Ticket Table
drop table if exists tickets;
CREATE TABLE tickets
(
    id         INT PRIMARY KEY AUTO_INCREMENT,
    type       VARCHAR(255) NOT NULL,
    price      DOUBLE    NOT NULL,
    dateOfSale TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP # format: YYYY-MM-DD HH:MM:SS
);


ALTER TABLE tickets MODIFY type ENUM('Children', 'Adult', 'Senior');
UPDATE Zoo.tickets
SET type = 'Children'
WHERE price = 10;
UPDATE Zoo.tickets
SET type = 'Adult'
WHERE price = 20;
UPDATE Zoo.tickets
SET type = 'Senior'
WHERE price = 15;

ALTER TABLE Zoo.tickets
    ADD COLUMN dateOfSale TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP;



CREATE INDEX i_dateOfSale ON Zoo.tickets (dateOfSale);

-- User Operations
DROP USER IF EXISTS 'zoo_user'@'Zoo';
CREATE USER 'zoo_user'@'Zoo' IDENTIFIED BY 'password';
GRANT INSERT, SELECT ON Zoo.* TO 'zoo_user'@'Zoo';
FLUSH PRIVILEGES;

-- Data Insertions
-- Add initial values for account, checkout, shop
INSERT INTO account(accountbalance)
VALUES (10000);
INSERT INTO checkout(accountbalance, account_id)
VALUES (500, 1),
       (550, 1);
INSERT INTO shop(area, account_id)
VALUES (100, 1),
       (200, 1);

-- Add products
INSERT INTO product(price, name, stock, account_id, shop_id)
VALUES (5.50, 'Popcorn', 10, 1, 1),
       (4.50, 'Coke', 50, 2, 2),
       (10.50, 'Toy', 15, 1, 1);
      

-- Add employee
INSERT INTO employee(position, employee_age, name, shop_id, account_id)
VALUES ('AnimalKeeper', 40, 'Jana', NULL, NULL),
       ('AnimalKeeper', 22, 'Alla', NULL, NULL),
       ('Cashier', 27, 'Anya', NULL, 1),
       ('Shop Manager', 42, 'Igor', 1, NULL),
       ('Cashier', 33, 'Alex', NULL, 2);

-- Add enclosure for animals
INSERT INTO enclosure(volume, location, enclosure_type, employee_id)
VALUES (500, 'First', 'Predators', 1),
       (700, 'Second', 'Plant eaters', 2),
       (300, 'Third', 'Amphibian', 2),
       (250, 'Fourth', 'Birds', 1),
       (100, 'Fifth', 'Insects', 1);

-- Add animals
INSERT INTO animals(species, food, enclosure_id)
VALUES ('Tiger', 'Meat', 1),
       ('Camel', 'Grass', 2),
       ('Frog', 'Fly', 3),
       ('Flamingo', 'Fish', 4),
       ('Butterfly', 'Flowers', 5);

-- Add Tickets
INSERT INTO tickets(price, type) VALUES (10, 'Children');
INSERT INTO tickets(price, type) VALUES (20, 'Adult');
INSERT INTO tickets(price, type) VALUES (15, 'Senior');
INSERT INTO Zoo.tickets (type, price, dateOfSale)VALUES (@type, @price, @dateOfSale);
SELECT CONCAT(price, ' €') AS formatted_price FROM Zoo.tickets;