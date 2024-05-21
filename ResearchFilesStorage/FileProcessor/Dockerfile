FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Docker
WORKDIR /App

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["./FileProcessor/FileProcessor.csproj", "FileProcessor/"]
RUN dotnet restore "./FileProcessor/FileProcessor.csproj"
COPY . .
WORKDIR "/src/FileProcessor"
RUN dotnet build "./FileProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./FileProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileProcessor.dll"]