#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["DockerComposeLocalStack/DockerComposeLocalStack.csproj", "DockerComposeLocalStack/"]
RUN dotnet restore "DockerComposeLocalStack/DockerComposeLocalStack.csproj"
COPY . .
WORKDIR "/src/DockerComposeLocalStack"
RUN dotnet build "DockerComposeLocalStack.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerComposeLocalStack.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerComposeLocalStack.dll"]