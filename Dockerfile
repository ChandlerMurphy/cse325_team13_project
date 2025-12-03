# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the csproj and restore dependencies
COPY Sunflower/*.csproj ./Sunflower/
RUN dotnet restore ./Sunflower/Sunflower.csproj

# Copy the rest of the source code
COPY . .

# Publish the project to the /app/out folder
RUN dotnet publish ./Sunflower/Sunflower.csproj -c Release -o /app/out

# Use the official runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

# Expose port (Render will assign $PORT)
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

# Run the application
ENTRYPOINT ["dotnet", "Sunflower.dll"]
