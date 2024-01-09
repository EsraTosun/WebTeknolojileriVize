# Use the official Microsoft ASP.NET image as a base image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

# Install PowerShell
RUN Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force -Scope CurrentUser -AllowClobber
RUN Install-Module -Name PowerShellGet -Force -AllowClobber
RUN Install-Module -Name DockerMsftProvider -Force -AllowClobber
RUN Install-Package -Name docker -ProviderName DockerMsftProvider -Force -AllowClobber

# Set the working directory to /app
WORKDIR /app

# Copy the current directory contents to the container at /app
COPY . .

# Ýmajýn temelini al
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver AS build-env

# Uygulamayý derleme aþamasý
FROM base AS build
WORKDIR /src
COPY ["SaglikWebUygulamasi1/SaglikWebUygulamasi1.csproj", "SaglikWebUygulamasi1/"]
RUN dotnet restore "SaglikWebUygulamasi1/SaglikWebUygulamasi1.csproj"
COPY . .
WORKDIR "/src/SaglikWebUygulamasi1"
RUN dotnet build "SaglikWebUygulamasi1.csproj" -c Release -o /app/build

# Uygulamayý yayýnlama aþamasý
FROM build AS publish
RUN dotnet publish "SaglikWebUygulamasi1.csproj" -c Release -o /app/publish

# Uygulamayý çalýþtýrmak için kullanýlan adým
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaglikWebUygulamasi1.dll"]
