# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file and restore dependencies (via NuGet)
COPY Nexora.sln ./
COPY Nexora/Nexora.csproj ./Nexora/
COPY Application/Application.csproj ./Application/
COPY Common/Common.csproj ./Common/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/

# Restore the dependencies
RUN dotnet restore Nexora.sln

# Copy the entire solution
COPY . ./

# Build the application
RUN dotnet publish Nexora/Nexora.csproj -c Release -o /app/publish --no-restore

# Use the official .NET runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Set the environment variables (Optional)
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port your app will run on
EXPOSE 80

# Copy the published application from the build image
COPY --from=build /app/publish ./

# Set the entrypoint to run your application
ENTRYPOINT ["dotnet", "Nexora.dll"]
