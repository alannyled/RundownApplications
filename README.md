# RundownApplications

Microsoft SQL Server (User = SA):
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Passw0rd123!' -p 1433:1433 --name RundownDB -d mcr.microsoft.com/mssql/server:2019-latest
