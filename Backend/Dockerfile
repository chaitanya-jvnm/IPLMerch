# build the .NET backend
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy backend source
COPY Backend/ ./Backend
WORKDIR /src/Backend

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5191
EXPOSE 5191

# run the published dll (uses wildcard to avoid hardcoding assembly name)
ENTRYPOINT ["sh", "-c", "dotnet /app/publish/*.dll"]