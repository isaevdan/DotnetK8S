# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY DotnetK8S.Worker/DotnetK8S.Worker.csproj ./DotnetK8S.Worker/
COPY DotnetK8S.DAL/DotnetK8S.DAL.csproj ./DotnetK8S.DAL/
COPY DotnetK8S.Server/DotnetK8S.Server.csproj ./DotnetK8S.Server/
COPY *.sln .
RUN dotnet restore DotnetK8S.sln

# copy and publish app and libraries
COPY . .
RUN dotnet publish ./DotnetK8S.Worker/DotnetK8S.Worker.csproj -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "DotnetK8S.Worker.dll"]