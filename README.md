Quantity Measurement App
Overview

The Quantity Measurement App is a progressive, incremental project designed to compare, convert, and perform arithmetic on different quantities such as length, weight, and other measurable units.

The application is built step by step using clearly defined use cases. Each use case adds a small, focused piece of functionality. The goal is to keep the codebase maintainable, testable, and extensible.

The project starts with simple comparisons and gradually evolves to support:

Unit equality checks

Unit conversions

Arithmetic operations on quantities

Multiple measurement categories (e.g., length, weight, etc.)

Project Goals

Build a clean and extensible Quantity Measurement API

Follow incremental development using use cases

Apply OOP principles and good design practices

Support unit testing for all core behaviors

Keep the implementation focused on requirements

Features (Planned & Implemented)

✅ Compare two quantities of the same type (e.g., feet vs feet, inches vs inches)

✅ Compare quantities of different units within the same category (e.g., feet vs inches)

✅ Convert between supported units

✅ Add two quantities of the same category

⏳ Extend to more quantity types (e.g., weight, volume, etc.)

⏳ Support more arithmetic operations

Supported Concepts

Measurement categories (e.g., Length, later Weight, etc.)

Units within a category (e.g., Feet, Inches, Yards, Centimeters)

Conversion using a common base unit

Equality checks with proper value normalization

Arithmetic operations using consistent units

Development Approach

The app is developed use case by use case

Each use case:

Introduces a small, well-defined feature

Adds or updates tests

Extends the existing design without breaking previous behavior

This keeps the project:

Easy to understand

Easy to test

Easy to extend

Tech Stack

Language: C#

Framework: .NET

Testing: NUnit

IDE: VS Code

Project Structure 

QuantityMeasurementApp/

Core domain classes (Quantity, Units, Converters, etc.)

QuantityMeasurementAppTests/

Unit tests for all use cases

Solution file (.sln / .slnx)

How to Run

Clone the repository

Open the solution in VS Code

Restore dependencies

Build the solution

Run the application or execute the test suite

Using CLI:

dotnet restore
dotnet build
dotnet test
dotnet run
