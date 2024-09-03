-- Kreiranje tablice Categories (Kategorije)
CREATE TABLE Categories (
    category_id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT
);

-- Kreiranje tablice Products (Proizvodi)
CREATE TABLE Products (
    product_id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    category_id UUID,
    image_url VARCHAR(255),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);

-- Kreiranje tablice Users (Korisnici)
CREATE TABLE Users (
    user_id UUID PRIMARY KEY,
    username VARCHAR(255) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    created_at DATETIME
);

-- Kreiranje tablice Orders (Narudžbe)
CREATE TABLE Orders (
    order_id UUID PRIMARY KEY,
    user_id UUID,
    order_date DATETIME,
    status VARCHAR(50) NOT NULL,
    total_amount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- Kreiranje tablice Order_Items (Stavke narudžbe)
CREATE TABLE Order_Items (
    order_item_id UUID PRIMARY KEY,
    order_id UUID,
    product_id UUID,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);
