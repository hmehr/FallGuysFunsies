#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Configure the build process
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app
COPY ./src ./src

# Trigger the restore/build of the project
RUN dotnet publish "./src/MediatonicFunsies.API/" -c Release -o "./src/MediatonicFunsies.API/out"

# Now execute the worker using dotnet runtimes
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
COPY --from=build /app/src/MediatonicFunsies.API/out/ .

# Configure Kestral Envrionment
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

ENTRYPOINT ["sh", "-c", "dotnet MediatonicFunsies.API.dll"]