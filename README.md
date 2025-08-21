# RestaurantMS 🍽️

A simple Restaurant Management System built with **ASP.NET Core MVC** and **Entity Framework Core**.  
The project is designed to demonstrate the basic workflow of managing a restaurant's products, categories, ingredients, and orders.  

---

## 📌 Project Overview
This system allows restaurant staff to:
- Manage Products and Categories.  
- Track Ingredients used in each product.  
- Create and manage Orders.  
- Relate Orders with specific Products.  

The goal is to build a **simple, clean system** that represents a real-life restaurant scenario.

---

## 🗄️ Database Design

The database follows a relational model to cover all entities of the restaurant:

- **Category ↔ Product**: One-to-Many (each product belongs to one category).  
- **Product ↔ Ingredient**: Many-to-Many (through `ProductIngredient`).  
- **Order ↔ Product**: Many-to-Many (through `OrderItem`).  
- **User ↔ Order**: One-to-Many (each user can place multiple orders).  


ER Diagram:
![ER Diagram](assets/ER%20Diagram.png)

---

## Technologies Used
- C# / ASP.NET Core MVC  
- Entity Framework Core  
- SQL Server  
- Bootstrap, HTML, CSS  

---

## Project Structure
> *This section will be updated once the project structure is finalized.*

