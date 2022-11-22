FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

RUN apk add --no-cache icu-libs
RUN apk update && apk add tzdata

ENV TZ=America/Asuncion
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ARG artifact_path
WORKDIR /app
COPY ${artifact_path} .
EXPOSE 80

ENTRYPOINT ["dotnet", "WebApi.dll"]