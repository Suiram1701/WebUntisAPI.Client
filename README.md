# WebUntisAPI.Client

This .NET library allows you to connect you with a WebUntis account and load all of the data you need from there.

## Download sources:
- [![GitHub](https://img.shields.io/badge/GitHub-Releases-black)](https://github.com/Suiram1701/WebUntisAPI.Client/releases)
- [![NuGet](https://img.shields.io/badge/NuGet-Package-blue)](https://www.nuget.org/packages/Suiram1.WebUntisAPI.Client)

## Documentation:
The documentation is integrated in the package so that all classes and methods explain what they do. Here're the basics to use the library:

## Licence:
When your project has .NET 6 or greater as target you must be ensure that your project also agrees the [Six Labors Split License](https://www.nuget.org/packages/SixLabors.ImageSharp/3.0.1/License) as [Transitive Package Dependency](https://en.wikipedia.org/wiki/Transitive_dependency).
This is in cause of that this package use in .NET 6 or greater the packages [SixLabors.ImageSharp](https://www.nuget.org/packages/SixLabors.ImageSharp) and [SixLabors.ImageSharp.Drawing](https://www.nuget.org/packages/SixLabors.ImageSharp.Drawing) for dynamic image loading and rendering.

### 1. Add references to this library
The simplest way is to add the [NuGet](https://www.nuget.org/packages/Suiram1.WebUntisAPI.Client) package to your project,
but when you don't want to use NuGet you can also download the [binaries](https://github.com/Suiram1701/WebUntisAPI.Client/releases) of the package and add the reference the contained .dll

### 2. Creating a client and login:

```C#
using (WebUntisClient client = new WebUntisClient("App name"))
{
    await client.LoginAsync("example.webuntis.com", "exampleSchool", "username", "password")
    // Here can you send your requests
}
```
Overloads:
- The `LoginAsync()` method has an overload where you can use instead of the `serverName` and the `loginName` an instance of `School` that returned from the school search.

Remarks:
- When you use the client in a using statement you would automatically logged out when it disposed
- Under no circumstances should 10 req. per sec., more than 1800req. per hr (but in no case more than 3600 req. per hr). If the specifications are exceeded, access to WebUntis could permanently blocked by the WebUntis API.

### 3. Send requests
After your login you can send requests to get information about your timetable and all about.
The methods an what they do should be self-explained.

## Issues
When you had an error that you don't understand or you don't understand how you can use the library you can create an issue so that I can help you by your problem.
[![GitHub](https://img.shields.io/badge/GitHub-Issues-red)](https://github.com/Suiram1701/WebUntisAPI.Client/issues)

---
This is an unofficial library that I created from the WebUntis API [documentation](https://untis-sr.ch/wp-content/uploads/2019/11/2018-09-20-WebUntis_JSON_RPC_API.pdf) by my-self.
I stand in no association with the Units GmbH
