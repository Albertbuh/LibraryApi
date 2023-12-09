# Reference the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory inside the container
WORKDIR /app

# Copy the project file(s) and restore dependencies
COPY Library.API.csproj .
RUN dotnet restore

# Copy the remaining source code and build the project
COPY . .
RUN dotnet publish -c Release -o out

# Reference the official ASP.NET Core runtime image as the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build environment
COPY --from=build-env /app/out .

# Expose port 80 for HTTP traffic
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "Library.API.dll"]
