# .Net api for an address lookup using GDS standards

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=MichaelStevenson2207_nidirect-pointer-poc-api-demo&metric=alert_status&token=9688a5b5428078033e4fcdd24ab616faa6312dbd)](https://sonarcloud.io/summary/new_code?id=MichaelStevenson2207_nidirect-pointer-poc-api-demo)

This is an api for addresses for Northern Ireland which act as an address lookup which utilises a Redis cache to cache requests. 
This was designed with the api principles created by <a href="https://www.gov.uk/guidance/gds-api-technical-and-data-standards">GDS in Gov.uk</a> in mind.

## What does this solve?

This helps to cache common queries into a Redis cache to lift the burden off the database. There are alot of other features as mentioned below.

## Tech used

- .Net Core 7
- Entity framework
- Redis
- Swagger
- Jwt Authentication
- Api key based authentication
- Fluent Assertions
- MediatR CQRS pattern
- Azure key vault
- Automapper
- AspNetRateLimiting
- Watchdog monitoring
- Health checks

## Features

- Jwt Authentication
- Api key based authentication
- Swagger xml comments
- Versioning
- Ratelimiting
- Fluent validation
- Paging
- Health checks
- Health check UI
- Monitoring
- Redis caching
- Result / maybe monad

To run:

Dotnet build - to build

Then to create the database (you need sql server installed) with 1 seeded entry:
update-database in the Nuget package manager tools in Visual studio.

Redis

You will have to create a Redis service in Azure to utilise the caching.

Jwt authentication

You need to add a key, Issuer and Audience in appsettings but I recommned always keeping these in your secrets file.
Use Jwt.io to create a token or your own method and add to the swagger auth box.

I have commented out the authorization attribute in the controller so uncomment this to use it.

Also, to use api key based authentication uncomment the below in the pointer controller

[ServiceFilter(typeof(ApiKeyAuthFilter))] // uncomment to use api key authentication

## Watchdog

If deploying to the cloud remember to add a username and password for watchdog in your settings.

## Secrets

Keep it secret, keep it safe.  I always make sure locally I use app secrets instead of appsettings and to use Azure Key vault in the cloud.


![gandalf](https://github.com/MichaelStevenson2207/nidirect-pointer-poc-api-demo/assets/66303816/db8468bc-04ea-48ef-afa7-a896a92baa0e)

Please make sure not to merge in your secrets but I do have Git guardian running on the repo.

![frodo](https://github.com/MichaelStevenson2207/nidirect-pointer-poc-api-demo/assets/66303816/0bf31ff8-67d8-4c33-8d56-7f88173d1a40)





