FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 8080

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore 

# copy everything else and build 
COPY . ./
COPY ./Data/UserSeedData.json ./Data/UserSeedData.json
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/Data/UserSeedData.json ./Data/UserSeedData.json
ENTRYPOINT ["dotnet", "API.dll"]
