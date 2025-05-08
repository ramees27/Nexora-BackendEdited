# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file and all project files
COPY Nexora.sln ./
COPY Nexora/Nexora.csproj ./Nexora/
COPY Application/Application.csproj ./Application/
COPY Common/Common.csproj ./Common/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/

# Restore the dependencies
RUN dotnet restore Nexora.sln

# Copy the rest of the application files
COPY . ./

# Build the application
RUN dotnet publish Nexora/Nexora.csproj -c Release -o /app/publish

# Use the .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Nexora.dll"]
