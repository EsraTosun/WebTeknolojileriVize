# Ýmajýn temelini al
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Uygulamanýn derlenmiþ kodunu al
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SaglikWebUygulamasi/SaglikWebUygulamasi.csproj", "SaglikWebUygulamasi/"]
RUN dotnet restore "SaglikWebUygulamasi/SaglikWebUygulamasi.csproj"
COPY . .
WORKDIR "/src/SaglikWebUygulamasi"
RUN dotnet build "SaglikWebUygulamasi.csproj" -c Release -o /app/build

# Uygulamayý yayýnlamak için kullanýlan adýmlar
FROM build AS publish
RUN dotnet publish "SaglikWebUygulamasi.csproj" -c Release -o /app/publish

# Uygulamayý çalýþtýrmak için kullanýlan adýmlar
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaglikWebUygulamasi.dll"]
