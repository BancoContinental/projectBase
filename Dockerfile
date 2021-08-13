FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
ARG artifact_path
WORKDIR /app
COPY ${artifact_path} .
EXPOSE 80
ENTRYPOINT ["dotnet", "WebApi.dll"]