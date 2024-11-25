# RundownApplications
Afgangsprojekt på Diplom i Softwareudvikling.  
## Sådan startes projektet/softwaren
Softwaren er afhængig af en række databaser, samt Kafka message broker.  

Der er lavet en Docker Compose til at starte disse afhængigheder, som ligger i roden af Solution. 
Brug Docker Desktop til at monitere og evt. genstarte containere der har brug for det.  

Bemærk at Kafka er afhængig af Zookeeper, som skal startes først. Lad Zookeeper køre i 15-20 sekunder før Kafka startes.  

Start alle containere, fra roden af Solution, med:
```
docker-compose up -d

```
Åben Solution i Visual Studio. Lav evt. en multible startup, med følgende projekter:  
- TemplateDbService
- RundownDbService
- ControlRoomDbService
- AggrregatorService
- ApiGateway
- LogStoreService
- RundownEditorCore

Eller start dem manuelt. RundownEditorCore skal helst startes til sidst.  
Herefter kan MediaRelationApp også startes, hvis den skal prøves af.  
Systemet er ikke afhængig af MediaRelationApp.

## Login
Der er oprettet to brugere i den medfølgende SQLite database:  
- Admin:  
  - Brugernavn: admin  
  - Password: 123456
	- Rolle: Administrator
- User:  
  - Brugernavn: user  
  - Password: 123456
	- Rolle: User  

Login som Administrator for at få alle rettigheder.

## Opret demo data i databaserne
Der er lavet en række seeders, som kan bruges til at oprette demo data i databaserne.  
I menubaren er der et link til "Reset Data" (kun synlig for Administrator).  
Klik på dette link og vælg Reset Databaser. Herefter vil der blive oprettet demo data i databaserne.


# Noter til mig selv

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
### Ved brug af MSSQL Server og EF Core  
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

Hvis der kører en proces, som forhindre migrering af DB
```
tasklist | findstr RundownDbService
taskkill /F /IM RundownDbService.exe
dotnet build
```
Tilføj Test projekt
```
dotnet new xunit -o [NAME].Tests
dotnet sln RundownApplications.sln add [NAME].Tests/[NAME].Tests.csproj
cd [NAME].Tests
dotnet add reference ../[NAME]/[NAME].csproj
```



