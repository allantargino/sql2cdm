# sql2cdm

## Overview

This project allows you to quickly scaffold a [Common Data Model (CDM)](https://docs.microsoft.com/en-us/common-data-model/) folder using [SQL DDL](https://en.wikipedia.org/wiki/Data_definition_language) and
custom annotations in form of SQL comments:

```sql
CREATE TABLE Customer
(
	CUSTOMER_ID INT IDENTITY(1,1) PRIMARY KEY,
	CUSTOMER_NAME VARCHAR(50) NOT NULL /* {trait:means.fullname} */
);

CREATE TABLE CustomerAddresses
(
	CUSTOMER_ADDRESS_ID INT IDENTITY(1,1) PRIMARY KEY /* {trait:means.identity; trait:is.dataFormat.integer} */,
	CUSTOMER_ID INT NOT NULL,
	ADDRESS VARCHAR(100) NOT NULL /* {trait:means.address.main} */,

	FOREIGN KEY(CUSTOMER_ID) REFERENCES Customer(CUSTOMER_ID)
);

CREATE TABLE VipCustomer /* {extends:Customer} */(
	SPECIAL_CARD_NUMBER CHAR(10) NOT NULL
);
```

```shell
sql2cdm -i /sql/customer.sql -o ./cdm-generated
```

## Quick Start

### Requirements

- [.NET Core 5.0](https://dotnet.microsoft.com/download)

### Steps

1. Clone this repository on your machine.

2. Build and publish the project:

```shell
dotnet restore --interactive
dotnet publish --no-restore ./src/Sql2Cdm.CLI -o ./cli
```

> If you have problems with NuGet authentication, please check [this document](https://docs.microsoft.com/en-us/nuget/consume-packages/consuming-packages-authenticated-feeds).

3. Run the CLI using [`customer.sql`](./sql/customer.sql) sample:

```shell
dotnet ./cli/Sql2Cdm.CLI.dll -i ./sql/customer.sql -o ./cdm-generated
```

## Additional Documentation

- [Docker Quick Start](./docs/dev/docker-quick-start.md)
- [Command Diagrams](./docs/dev/command-diagrams.md)
- [SQLite Relational Model Reader](./docs/dev/relational-model-readers/sqlite.md)