#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FileDetailAPI.csproj", "FileDetailAPI/"]
RUN dotnet restore "FileDetailAPI/FileDetailAPI.csproj"

WORKDIR "/src/FileDetailAPI"
COPY . .

RUN dotnet build "FileDetailAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileDetailAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir -p /app/Photos
COPY --from=publish /app/publish .
COPY ["Photos", "/app/Photos"]
ENTRYPOINT ["dotnet", "FileDetailAPI.dll"]

ENV FileDetailAppCon="Data Source=Localhost;database=FileVersionManagement;uid=sa;password=P@ssw0rd;"