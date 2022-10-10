FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

RUN apk update && apk add tzdata
ENV TZ=America/Asuncion

ARG artifact_path
WORKDIR /app
COPY ${artifact_path} .
EXPOSE 80

ENTRYPOINT ["dotnet", "WebApi.dll"]