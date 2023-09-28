#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Document.API/Document.API.csproj", "Document.API/"]
COPY ["Document.Core/Document.Core.csproj", "Document.Core/"]
COPY ["Document.Infrastructure/Document.Infrastructure.csproj", "Document.Infrastructure/"]
COPY ["SharedLib.Infrastructure/SharedLib.Infrastructure.csproj", "SharedLib.Infrastructure/"]
COPY ["Metadata.Core/Metadata.Core.csproj", "Metadata.Core/"]
COPY ["SharedLib.Core/SharedLib.Core.csproj", "SharedLib.Core/"]
COPY ["SharedLib.API/SharedLib.API.csproj", "SharedLib.API/"]
RUN dotnet restore "Document.API/Document.API.csproj"
COPY . .
WORKDIR "/src/Document.API"
RUN dotnet build "Document.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Document.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Document.API.dll"]