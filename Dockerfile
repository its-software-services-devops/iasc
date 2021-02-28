FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY iasc-app/* ./iasc-app/
COPY iasc.sln .

RUN ls -lrt
RUN dotnet restore iasc-app/iasc-app.csproj
RUN dotnet publish iasc-app/iasc-app.csproj -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app .
RUN ls -lrt

ENTRYPOINT ["dotnet", "iasc-app.dll"]
