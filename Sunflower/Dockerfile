# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as separate layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Use the official runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose port (Render will assign $PORT)
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

# Run the application
ENTRYPOINT ["dotnet", "Sunflower.dll"]
