FROM mcr.microsoft.com/dotnet/sdk:3.1

WORKDIR /src

COPY . .

RUN dotnet restore EventMonitor

RUN dotnet publish EventMonitor -c Release -o out

WORKDIR /src/out

ENTRYPOINT ["dotnet", "EventMonitor.dll"]