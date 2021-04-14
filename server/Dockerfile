FROM mcr.microsoft.com/dotnet/sdk:3.1

WORKDIR /src

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o out

WORKDIR /src/out

ENTRYPOINT ["dotnet", "EventMonitor.dll"]