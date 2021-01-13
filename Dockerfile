FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ENV HTTP_PROXY http://10.1.1.12:8080

WORKDIR /src
COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]
COPY ["src/Core/Core.csproj", "src/Core/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/WebApi/WebApi.csproj"

COPY . .

WORKDIR "/src/src/WebApi"
RUN dotnet build "WebApi.csproj" --no-restore -c Release -o /app/publish

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV HTTP_PROXY ""

ENTRYPOINT ["dotnet", "WebApi.dll"]