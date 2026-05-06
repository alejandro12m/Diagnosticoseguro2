# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el proyecto y restaurar
COPY ["ConsultaExterna.csproj", "./"]
RUN dotnet restore "./ConsultaExterna.csproj"

# Copiar todo y publicar
COPY . .
RUN dotnet publish "ConsultaExterna.csproj" -c Release -o /app/publish

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Render usa el puerto 10000 por defecto
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ConsultaExterna.dll"]