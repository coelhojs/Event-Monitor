FROM mcr.microsoft.com/dotnet/sdk:3.1

WORKDIR /src

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o out

WORKDIR /app

COPY /src/out .

ENTRYPOINT ["dotnet", "Event-Simulator.dll"]