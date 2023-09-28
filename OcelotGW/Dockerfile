#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OcelotGW/OcelotGW.csproj", "OcelotGW/"]
COPY ["SharedLib.API/SharedLib.API.csproj", "SharedLib.API/"]
COPY ["SharedLib.Infrastructure/SharedLib.Infrastructure.csproj", "SharedLib.Infrastructure/"]
COPY ["Metadata.Core/Metadata.Core.csproj", "Metadata.Core/"]
COPY ["SharedLib.Core/SharedLib.Core.csproj", "SharedLib.Core/"]
RUN dotnet restore "OcelotGW/OcelotGW.csproj"
COPY . .
WORKDIR "/src/OcelotGW"
RUN dotnet build "OcelotGW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OcelotGW.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotGW.dll"]