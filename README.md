# RundownApplications

DB connection
```
Microsoft SQL Server (User = SA):
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Passw0rd123!' -p 1433:1433 --name RundownDB -d mcr.microsoft.com/mssql/server:2019-latest
```

Migrer db
```
dotnet ef migrations add [NAME]
dotnet ef database update
```

Hvis der kører en proces, som forhindre migrering
```
tasklist | findstr RundownDbService
taskkill /F /IM RundownDbService.exe
dotnet build
```

```
docker run -d --name zookeeper --env ALLOW_ANONYMOUS_LOGIN=yes bitnami/zookeeper:latest
docker run -d --name kafka --env KAFKA_CFG_ZOOKEEPER_CONNECT=zookeeper:2181 --env ALLOW_PLAINTEXT_LISTENER=yes --network="bridge" bitnami/kafka:latest
```
