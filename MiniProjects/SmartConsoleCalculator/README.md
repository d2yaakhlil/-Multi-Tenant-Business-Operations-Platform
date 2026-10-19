# Smart Console Calculator

A feature-rich console calculator built with C# and .NET 10. It supports basic arithmetic, scientific operations, expression evaluation, calculation history, memory functions, statistics, and customizable settings.

## Features

- **Basic Calculations:** Perform addition, subtraction, multiplication, and division.
- **Scientific Operations:** Perform additional mathematical operations.
- **Expression Evaluator:** Evaluate mathematical expressions while respecting operator precedence.
- **Calculation History:** View previously performed calculations.
- **Memory Functions:** Store, recall, update, and clear values in calculator memory.
- **Answer Recall:** Reuse the result of a previous calculation.
- **Statistics:** Perform supported statistical calculations.
- **Settings:** Customize available calculator preferences.
- **Persistent Data:** Retain supported history and settings between application sessions.

## Technologies

- C#
- .NET 10
- .NET Console Application

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

Verify your installation by running:

```powershell
dotnet --version
```

## Getting Started

### 1. Open the Solution

Open `SmartConsoleCalculator.sln` using Visual Studio, or open the project folder in Visual Studio Code.

### 2. Build the Application

Open a terminal in the directory containing `SmartConsoleCalculator.sln` and run:

```powershell
dotnet build src/SmartConsoleCalculator/SmartConsoleCalculator.csproj
```

### 3. Run the Application

From the same directory, run:

```powershell
dotnet run --project src/SmartConsoleCalculator/SmartConsoleCalculator.csproj
```

The calculator menu will appear in the console. Follow the on-screen instructions to select an operation.

## Project Structure

```text
SmartConsoleCalculator/
├── .gitignore
├── README.md
├── SmartConsoleCalculator.sln
└── src/
    └── SmartConsoleCalculator/
        ├── SmartConsoleCalculator.csproj
        ├── Program.cs
        ├── Application/
        ├── Models/
        │   └── Enums/
        ├── Services/
        ├── Storage/
        ├── UI/
        └── Utilities/
```

The directories under `src/SmartConsoleCalculator/` contain the application's entry point, models, services, storage components, utilities, and user interface code.
