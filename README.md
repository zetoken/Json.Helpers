Json Helpers by ZTn
===================

[![Build status](https://ci.appveyor.com/api/projects/status/my14x2mrd23h2ium/branch/master?svg=true)](https://ci.appveyor.com/project/zetoken/json-helpers/branch/master)

Some extension methods to help playing with JSON data.

2 nuget packages are now available using [Newtonsoft.Json](https://www.newtonsoft.com/json) API for one and [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/api/system.text.json) API for the other.

# ZTn.Json.Helpers

[![NuGet Badge](https://buildstats.info/nuget/ZTn.Json.Helpers)](https://www.nuget.org/packages/ZTn.Json.Helpers/)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fzetoken%2FJson.Helpers.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fzetoken%2FJson.Helpers?ref=badge_shield)

A very simple library providing some extension methods around NewtonSoft.Json library.

I've been using most of these extensions in several projects since 2012 and just decided it's time to make it public (some parts were already available [on SO](https://stackoverflow.com/a/17925665/1774251)).

## Json serialization

### To a `string`
```cs
var json = obj.WriteToJsonString();
```
### To a file given its path
```cs
obj.WriteToJsonFile("data.json");
```

## Json deserialization

### From a `Stream`
The stream can automatically be closed when done
```cs
var obj = aStream.CreateFromJsonStream<MyType>();
```
or kept open for further use
```cs
var obj = aStream.CreateFromJsonPersistentStream<MyType>();
```
### From a `string`
```cs
var obj = "{\"key\":\"value\"}".CreateFromJsonString<MyType>();
```
### From content of a file given its path
```cs
var obj = "data.json".CreateFromJsonFile<MyType>();
```

# ZTn.System.Text.Json.Helpers

[![NuGet Badge](https://buildstats.info/nuget/ZTn.System.Text.Json.Helpers)](https://www.nuget.org/packages/ZTn.System.Text.Json.Helpers/)

A library similar to ZTn.Json.Helpers but based on System.Text.Json API.

Note that the method names and signatures are different from the other package.

## Json serialization

### To a `string`
```cs
var json = obj.ToJson();
```
or get an indented string:
```cs
var json = obj.ToPrettyJson();
```
### To a file given its path
```cs
obj.WriteAsJson("data.json");
```
or indent the file using:
```cs
obj.WriteAsPrettyJson("data.json");
```

## Json deserialization

### From a `Stream`
```cs
var obj = aStream.ReadAsJson<MyType>();
```
### From a `string`
```cs
var obj = "{\"key\":\"value\"}".FromJson<MyType>();
```
### From content of a file given its path
```cs
var obj = "data.json".ReadAsJson<MyType>();
```

## Remark
* These methods support an optional parameter of type [JsonSerializerOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions) for configuration.
* The sub-namespace "Migration" allows to use the same API as Ztn.Helpers.Json but based on System.Text.Json, allowing (I hope) an easy transition.

## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fzetoken%2FJson.Helpers.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fzetoken%2FJson.Helpers?ref=badge_large)