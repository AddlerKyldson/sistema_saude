# Usando a imagem base do .NET 8.0 para a execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5279

# Usando a imagem SDK do .NET 8.0 para construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["sistema_saude.csproj", "./"]
RUN dotnet restore "sistema_saude.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "sistema_saude.csproj" -c Release -o /app/build

# Publicando o aplicativo
FROM build AS publish
RUN dotnet publish "sistema_saude.csproj" -c Release -o /app/publish

# Configurando o estágio final para a execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Definindo a URL onde o aplicativo vai escutar
ENV ASPNETCORE_URLS=http://+:5279
ENTRYPOINT ["dotnet", "sistema_saude.dll"]
