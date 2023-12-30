# Use the official Microsoft ASP.NET image as a base image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

# Set the working directory to /app
WORKDIR /app

# Copy the current directory contents to the container at /app
COPY . /app

# Install PowerShell
RUN Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force -Scope CurrentUser -AllowClobber
RUN Install-Module -Name PowerShellGet -Force -AllowClobber
RUN Install-Module -Name DockerMsftProvider -Force -AllowClobber
RUN Install-Package -Name docker -ProviderName DockerMsftProvider -Force -AllowClobber

# Other commands...

# Make port 80 available to the world outside this container
EXPOSE 80

# Define the entry point for the application
ENTRYPOINT ["powershell", "./YourStartupScript.ps1"]


# Ýmajýn temelini al
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

# Uygulamayý derleme aþamasý
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
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
