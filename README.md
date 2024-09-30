# RundownApplications

Der er lavet en Docker Compose, som ligger i roden af Solution. Kør med:
```
docker-compose up -d

```

Manuel start af Kafka, uden Docker-Compose (Vigtigt at Zookeeper startes først)
```
docker network create kafka-net

docker run -d --name zookeeper --network kafka-net `
  -e ALLOW_ANONYMOUS_LOGIN=yes `
  bitnami/zookeeper:latest

docker run -d --name kafka --network kafka-net `
  -e KAFKA_CFG_ZOOKEEPER_CONNECT=zookeeper:2181 `
  -e ALLOW_PLAINTEXT_LISTENER=yes `
  -e KAFKA_CFG_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 `
  -e KAFKA_CFG_LISTENER_SECURITY_PROTOCOL_MAP=PLAINTEXT:PLAINTEXT `
  -e KAFKA_CFG_LISTENERS=PLAINTEXT://0.0.0.0:9092 `
  -p 9092:9092 `
  bitnami/kafka:latest

```
### Ved brug af MSSQL Server og EF Core  DB connection
```
Microsoft SQL Server (User = SA):
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Passw0rd123!' -p 1433:1433 --name RundownDB -d mcr.microsoft.com/mssql/server:2019-latest
```

Migrer db
```
dotnet ef migrations add [NAME]
dotnet ef database update
```

Hvis der kører en proces, som forhindre migrering af DB
```
tasklist | findstr RundownDbService
taskkill /F /IM RundownDbService.exe
dotnet build
```


