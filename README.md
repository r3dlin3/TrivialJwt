# TrivialJWT

TrivialJWT is a set of libraries to ease:

- The creation of JWT tokens
- The validation of JWT tokens

`TrivialJWT` exposes an end point to generate JWT token. 
It relies on Microsoft's libraries for JWT generation.

`TrivialJWT.Bearer` helps configure the `Microsoft.AspNetCore.Authentication.JwtBearer` library based on `TrivialJWT` configuration.

`TrivialJWT.AspNetIdentity` implements the required interfaces to bridge `TrivialJWT` with `Microsoft.AspNetCore.Identity`.

2 samples are provided:

- [With AspNetIdentity](samples/SimpleAppAspNetIdentity)
- [Without AspNetIdentity](samples/SimpleApp)

## How to use `TrivialJWT` with AspNetIdentity

### Install dependencies

With .NET CLI

```bash
dotnet add package TrivialJwt.Bearer
dotnet add package TrivialJwt.AspNetIdentity
```

or with Package Manager:

```bash
Install-Package TrivialJwt.Bearer
Install-Package TrivialJwt.AspNetIdentity
```

### Update `Startup.cs`

In the example below, a HMAC-SHA265 signature

```csharp
(...)
using TrivialJwt;
using TrivialJwt.AspNetIdentity;
using TrivialJwt.Bearer;
(...)

public void ConfigureServices(IServiceCollection services)
{
    (...)

    services.AddTrivialJwtAspNetIdentity<AppUser>(options =>
            {
                options.Secret = "<Base64Secret>"
            });

    services.AddTrivialJwtAuthentication();
    
    (...)
}

public void Configure(IApplicationBuilder app, 
                IWebHostEnvironment env)
{
    (...)

    app.UseAuthentication();
    app.UseAuthorization();

    (...)
}
```

## How to use `TrivialJWT` without AspNetIdentity

### Install dependencies

With .NET CLI

```bash
dotnet add package TrivialJwt.Bearer
```

or with Package Manager:

```bash
Install-Package TrivialJwt.Bearer
```

### Update `Startup.cs`

In the example below, a HMAC-SHA265 signature

```csharp
(...)
using TrivialJwt;
using TrivialJwt.Bearer;
(...)

public void ConfigureServices(IServiceCollection services)
{
    (...)

    services.AddTrivialJwt(options =>
            {
                options.Secret = "<Base64Secret>"
            });

    services.AddTrivialJwtAuthentication();

    services.AddScoped<IPasswordValidator, PasswordValidator>();
    services.AddScoped<IClaimsIdentityProvider, ClaimsIdentityProvider>();

    (...)
}

public void Configure(IApplicationBuilder app, 
                IWebHostEnvironment env)
{
    (...)

    app.UseAuthentication();
    app.UseAuthorization();

    (...)
}
```

An implementation for [`IPasswordValidator`](src/TrivialJwt/Services/IPasswordValidator.cs) and [`IClaimsIdentityProvider`](src/TrivialJwt/Services/IClaimsIdentityProvider.cs) must be provided.

## Configuration

Configuration can be done by using options as shown above or by binding:

```csharp
services.AddTrivialJwtAspNetIdentity<IdentityUser>(
    Configuration.GetSection(TrivialJwtOptions.Section));
```

For instance, the `appsettings.json` can contain the configuration:

```json
{
    "TrivialJwt": {
        "Secret": "U3VwZXJfU2VjcmV0X1Bhc3N3b3JkIQ=="
    }
}
```

## Endpoints

### Token generation endpoint

The endpoint is `/auth/login`.

The payload is a JSON with `username` and `password`.

Example:

```json
{
    "username": "bob",
    "password": "bob"
}
```

The response would be:

```json
{
    "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdW...k_Riw4RSK7g",
    "refresh_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdW...k_Riw4RSK7g",
    "expires_in": 3600,
    "token_type": "bearer"
}
```

### Refresh token endpoint

The endpoint is `/auth/refresh_token`.

The payload is a JSON file with `refresh_token`.

Example:

```json
{
    "refresh_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdW...k_Riw4RSK7g"
}
```

The response would be:

```json
{
    "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdW...k_Riw4RSK7g",
    "refresh_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdW...k_Riw4RSK7g",
    "expires_in": 3600,
    "token_type": "bearer"
}
```

### Refresh Token endpoint

The endpoint is `/auth/refreshtoken`.

The payload is a JSON file with `username` and `password`.

Example:

```json
{
    "username": "bob",
    "password": "bob"
}
```

## TODO

- [ ] support .Net 5.0
- [ ] Implement elliptic curves
- [ ] Enhance asymmetric key management
