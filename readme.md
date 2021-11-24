# Hot Drink Generator
A simple console application that creates a hot drink of your choice.  
Currently supported drinks are Cappuccino, Coffee and Latte.

## Usage
### Requirements
- [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)
- [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

## Contributing
### New Drink Types
To add a new drink types, it will need to be added to the *HotDrinkType* enum and to the *HotDrinkRecipes* dictionary (along with its recipe) in *HotDrinkService.cs*.

An *appsettings.json* file controls configuration presets for the generator.
