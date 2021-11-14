# VDsoft.CQRS

[![Nuget](https://img.shields.io/badge/nuget-VDsoft.CQRS-00325f)](https://www.nuget.org/packages/VDsoft.Cqrs/)
![Gated](https://github.com/VDSoft/CQRS/actions/workflows/gated.yml/badge.svg)

This library supports with the integration of the Command and Query Responsibility Segregation Pattern ([CQRS](https://martinfowler.com/bliki/CQRS.html)).

## Supported Frameworks

- .NET.Core 3.11
- Net 5
- Net 6 (or later)

## Installation

Install the package via [Nuget](https://www.nuget.org/packages/VDsoft.Cqrs/) or run
```bash
dotnet add package VDsoft.Cqrs
```

## Usage

The library provides you with interfaces to easy the implementation of the [CQRS](https://martinfowler.com/bliki/CQRS.html) pattern.

### New Command
To implement a new command inherit from `ICommand` and provide your custom `IArguments` for it.

#### Sample

``` csharp
using System.Threading.Tasks;
using VDsoft.Cqrs.Command

namespace SampleApplication.Command;

public class CustomArguments : IArguments
{
    ///<summary>
    /// Initializes a new instance of the CustomArguments class.
    ///</summary>
    public CustomArguments(string paramter)
    {
        Parameter = parameter;
    }

    ///<summary>
    /// Gets the Parameter.
    ///</summary>
    public string Parameter { get; }
}

public class CustomCommand : ICommand<CustomArguments>
{
    ///<summary>
    /// Initializes a new instance of the CustomCommand class.
    ///</summary>
    public CustomCommand()
    {
        // Can be used to inject any dependancies your command might need.
    }

    /// <summary>
    /// Executes the command asynchronously.
    /// </summary>
    /// <param name="arguments">The arguments needed for the command.</param>
    /// <returns>
    /// A <see cref="Task" /> for the asynchronous operation.
    /// </returns>
    public Task ExecuteAsync(CustomArguments arguments)
    {
        // implement your command here...
    }
}
```

### New Query
To add a new query to your application inherit from `IQueryHandler` and provide your arguments as an `IQuery`.

#### Sample

``` csharp
using System.Threading.Tasks;
using VDsoft.Cqrs.Query

namespace SampleApplication.Query;

public class CustomQuery : IQuery<string>
{
    ///<summary>
    /// Initializes a new instance of the CustomQuery class.
    ///</summary>
    public CustomQuery(string paramter)
    {
        Parameter = parameter;
    }

    ///<summary>
    /// Gets the Parameter.
    ///</summary>
    public string Parameter { get; }
}

public class CustomQueryHandler : IQueryHandler<CustomQuery, string>
{
    ///<summary>
    /// Initializes a new instance of the CustomQueryHandler class.
    ///</summary>
    public CustomQueryHandler()
    {
        // Can be used to inject any dependancies your command might need.
    }

    public Task<string> ExecuteAsync(CustomQuery query)
    {
        // implement your query here...
    }
}
```

## Dependency Injection support

All your Commands and Query can be integrated into any Dependency Injection Framework (DI Framework).
Please have a look at [VDsoft.Cqrs.DependencyInjectionExtension](https://www.nuget.org/packages/VDsoft.Cqrs.DependencyInjectionExtension/). There you can find for which DI Frameworks we currently provide extensions.