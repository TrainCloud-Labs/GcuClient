# GcuClient

This repo shows how to create a `.NET8` client for GCU broker service `RSRD²`

## Set up user for API access in GCU portal
https://prod.gcubroker.org/login
## Prepare project as GCU Client
Install `dotnet-svcutil` 
```terminal
dotnet tool install --global dotnet-svcutil --version 2.2.0-preview1.23462.5
```

Add NuGet packages for WCF to your project.
```powershell
cd ./GcuClient
dotnet add package System.ServiceModel.Primitives
dotnet add package System.ServiceModel.Http
```

Generate client with `dotnet-svcutil` from WSDL. Authenticate with the previosly generated API user.
```powershell
mkdir Rsds
cd ./Rsds
dotnet-svcutil https://prod.gcubroker.org/rsds?wsdl
```
![dotnet-svcutil](/Docs/Assets/dotnet-svcutil.PNG)

## Use GCU Client in your project

```csharp
BasicHttpBinding httpBinding = new();
httpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
httpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

MessageHeader msgHeader = new()
{
    ...
};
    
rsdsRequest request = new(msgHeader, new string[] { "UIC", "...", ... });
 
EndpointAddress ea = new("https://prod.gcubroker.org/rsds"); // https://stage.gcubroker.org/rsds

RsdsEndpointClient client = new(httpBinding, ea);
client.ClientCredentials.UserName.UserName = userName;
client.ClientCredentials.UserName.Password = password;

rsdsResponse response = await client.rsdsAsync(request);
```
