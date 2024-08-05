# Use the official .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY Sync.Common/Sync.Common.csproj Sync.Common/
COPY Sync.Dev/Sync.Dev.csproj Sync.Dev/
COPY Sync.DAL/Sync.DAL.csproj Sync.DAL/
COPY . . 

RUN dotnet restore Sync.Dev/Sync.Dev.csproj

ENTRYPOINT ["bash"]
