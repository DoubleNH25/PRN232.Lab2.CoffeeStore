# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire solution
COPY PRN232.Lab2.CoffeeStore/ ./PRN232.Lab2.CoffeeStore/

# Build the application
WORKDIR /src/PRN232.Lab2.CoffeeStore/PRN232.Lab2.CoffeeStore.API
RUN dotnet publish -c Release -o /app

# Use the ASP.NET Core runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Expose the ports
EXPOSE 80
EXPOSE 443

# Run the application
ENTRYPOINT ["dotnet", "PRN232.Lab2.CoffeeStore.API.dll", "--environment=Container"]