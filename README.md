# FORBES Library Readme

## Introduction

This library contains pretty much all the methods I've written that I find useful and handy for reusing. This file covers general information on what is in the library. See CHANGELOG.md for changes.

## Documentation Notes

All publicly accessible methods and objects are documented with XML documentation. This allows documentation to be viewed in the Visual Studio Object Browser and to work with IntelliSense. https://docs.microsoft.com/en-us/dotnet/csharp/codedoc

Private methods and objects are documented with traditional double-slash comments.

## Namespaces

### KEY_LOGGER_NAMESPACE

This namespace and associated classes tracks all the user keyboard input and can simulate user keyboard input. This namespace depends on the library written by fabriciorissetto located at https://github.com/fabriciorissetto/KeystrokeAPI.

### LOGGER_NAMESPACE

This namespace and associated classes is a event logger that stores events, calling methods, messages and other details about each event. The LOGGER class is heavily used in the other namespaces of this library.

### MYSQL_COMS_NAMESPACE

This namespace and associated classes does all the legwork for interacting with a MySQL Database.

### TABLE_PROCESSOR_NAMESPACE

This namespace and associated classes can process a CSV file and do a bunch of very useful transformations on the data by feeding the method a list of commands. See documentation in the TABLE_PROCESSOR namespace directory for more details of all the commands. The documentation is actually from a much older version of the program but I believe its still 100% good.

### TRIANGULATOR_NAMESPACE

This namespace and associated classes can determine where a point is in space given three datums and the distance to those datums. This is useful for surveying. I wrote it so that I can plot out my yard with a tape measure and three stakes in the ground.

### SERIAL_COMS_NAMESPACE

This namespace and associated classes handles serial connections.

### ASCII_RENDER_NAMESPACE

This namespace allows you to render gifs to the console.