# .Net minimal api for an address lookup

This is a simple unambitious minimal api for pointer addresses for Northern Ireland which act as an address lookup. 
This is a perfect example of a micro service that can be easily fired up.

Tech used:

- .Net Core 7
- Entity framework
- Swagger
- Jwt Authentication

To run:

Dotnet build - to build

Then to create the database (you need sql server installed) with 1 seeded entry:
update-database in the Nuget package manager tools in Visual studio.

Jwt authentication

You need to add a key, Issuer and Audience in appsettings but I recommned always keeping these in your secrets file.
Use Jwt.io to create a token or your own method and add to the swagger auth box.

I have commented out RequireAuthorization in the  GetPointerAddressesEndpoint endpoints so you can test it without authentication if required.

e.g.
