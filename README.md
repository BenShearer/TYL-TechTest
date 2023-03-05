# TYL-TechTest Introduction

This is an API for a fictitional London Stock Exchange application. It allows traders to view the mean price of a stock by its ticker value, and also add details of trades that affect the average stock value.

You have been asked to perform two quick tasks in this project.

1. Add a timsestamp to the inserted stock values so the stock value can be tracked over time.
2. Add a mean return value for last five trades and last ten trades.

Tips:

All code changed should have corresponding tests.

Use as much existing code as possible!

## Techinical Overview

DotNet Core Version 6- LTS
EntityFramework Core Version 6.0.6

Uses EF InMemory Database

To run:
navigate to `./src/LSE.TradeHub/`

invoke commands
`dotnet restore`
`dotnet run --project LSE.TradeHub.API`

Open the following link:
https://localhost:7169/swagger

### Projects

- LSE.TradeHub.API - Primary API Application
- LSE.TradeHub.Core - Data models and DAL services

### Test Projects

- LSE.TradeHub.API.Tests - Controller and mapper unit tests
- LSE.TradeHub.Core.Tests - Data service integrations tests

## Test Tasks
