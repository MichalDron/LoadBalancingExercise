# LoadBalancer Exercise

## Description
Simplistic implementation of Load Balancer. It is exercise to write load balancer in steps with few simple functionalities like:
* LoadBalancer that distributes incoming request to list of registered providers
* Every Provider has unique identifier, which is returned by get() method
* LoadBalancer can have max 10 Provider registered

## Code
Solution is devided into .NET Core projects:
* `LoadBalancing`
* `LoadBalancing.Providers.Abstractions`
* `LoadBalancing.Algorithms.Abstractions`
* `LoadBalancing.Algorithms.RandomInvocation`
* `LoadBalancing.Tests`
* `LoadBalancing.App`

#### LoadBalancing
Core project with implementation of load balancing functionality. 

#### LoadBalancing.Providers.Abstractions
Package with abstractions needed to implement compatible `IProvider`, which can be used then by `LoadBalancer`

#### LoadBalancing.Algorithms.Abstractions
Package with abstractions needed to implement compatible `IInvocationAlgorithm`, which can be used then by `LoadBalancer`

#### LoadBalancing.Algorithms.RandomInvocation
Package with implemented `RandomInvocationAlgorithm`

#### LoadBalancing.Tests
Test project for load balancing functionality.

#### LoadBalancing.App
Sample console app. To run it check [Run section](###Run)

### Requirements
.NET Core version [2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2) installed [link](https://docs.microsoft.com/en-us/dotnet/core/install/sdk?pivots=os-windows)

### Build
```console
dotnet build
```

### Run
```console
dotnet run --project LoadBalancing.App
```

### Tests
```console
dotnet test
```