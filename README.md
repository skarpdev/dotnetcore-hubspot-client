[![Build status](https://ci.appveyor.com/api/projects/status/kwl0jx7cfmeel1jh?svg=true)](https://ci.appveyor.com/project/nover/dotnetcore-hubspot-client-qxwcp)
[![nuget version][nuget-image]][nuget-url]

# dotnet core HubSpot client

This repository contains a dotnet / dotnet core compatible HubSpot client with support for custom property mapping from your DTOs to HubSpot fields.

Currently working is almost complete support for `Contact`, `Company` and `Deal` in the HubSpot api.

If you find something missing or broken, please [report an issue][github-issue] or even better fork the repo and submit a PR!

## Dotnet targets

Packages for the following dotnet versions are available:

- dotnet 4.6
- netstandard 1.6

Which means that you can use the library with `> fat-framework 4.6` or `netcoreapp 1.0 / 1.1 / 2.0`

Verified working on Linux, macOS and Windows.

## Versioning

We use [SemVer 2.0](http://semver.org/), which means you can depend on HubSpotClient version `<major>.*`.

This aslo means that while we are in the `0.*` version range, **breaking** API changes can be made between minor versions - we strive to keep these at a minimum and will be explicitly stated in the release notes.

## Using the library

The library has been split into separate clients for each "HubSpot API feature", which means that `Contacts`, `Companies` and `Deals` have separate clients for you to depend on.

### Contact

To interact with HubSpot contacts you must use the `HubSpotContactClient` - it has two constructors, one for quickly getting started (you just provide the `api-key`) and one with all dependencies as arguments (eager constructor).

It is generally recommended that you use the "eager" constructor as this allows replacing the HTTP client and other dependencies when testing.

However, getting started is as simple as:

```csharp
using Skarp.HubSpotClient.Contact;
using Xunit;
using Xunit.Abstractions;

public class ContactTest
{
  [Fact]
  public async Task Getting_contacts_work()
  {
    var client = new HubSpotContactClient("my-awesome-api-key");
    var contact = await client.GetByEmailAsync<ContactHubSpotEntity>("adrian@hubspot.com");
    Assert.NotNull(contact); // victory!
  }
}
```

All client operations takes in a generic type argument `T` - this is in order to support (de)serialization. The provided `ContactHubSpotEntity` provides the basic properties one could want on a contact person.

If you require the default props and some additional custom props, simply create your own class instance and inherit from either `IHubSpotEntity` or the `ContactHubSpotEntity`:

```csharp
[DataContract]
public class MyContactEntity : ContactHubSpotEntity
{
  [DataMember(Name="nick-name")] // required so we can serialize to the hubspot property name defined in your account!
  public string NickName {get; set;}
}
```

All operations (`get`, `create`, `update`) should now include this custom property.
If you don't include the `DataContract` and `DataMember` attributes your new props will not be serialized and sent to HubSpot!

### Company

To consume the HubSpot company api you should use the `HubSpotCompanyClient` - as with the contacts there is a simple constructor taking in just the `apiKey` and an eager constructor taking in all dependencies.

Getting started looks something like...

```csharp
using Skarp.HubSpotClient.Contact;
using Xunit;
using Xunit.Abstractions;

public class CompanyTest
{
  [Fact]
  public async Task Getting_company_works()
  {
    var client = new HubSpotCompanyClient("my-awesome-api-key");
    var company = await client.GetByIdAsync<CompanyHubSpotEntity>(42L);
    Assert.NotNull(company); // victory!
  }
}
```

To create custom DTOs follow the guidelines given for Contacts above.

### Deal

**TODO**


[nuget-image]: https://img.shields.io/nuget/v/HubSpotClient.svg
[nuget-url]: https://www.nuget.org/packages/HubSpotClient
[github-issue]: https://github.com/skarpdev/dotnetcore-hubspot-client/issues/new