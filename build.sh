#!/bin/bash
echo "----------------------------------------------------------------------------------"
echo "-- Init build --"
echo "----------------------------------------------------------------------------------"

rm -rf ./out
VERSION="$(git rev-parse HEAD)"

printf "\n\n"
echo "-- dotnet publish --"
printf "\n\n"
dotnet publish src/WebApi/WebApi.csproj -c Release -o ./out

printf "\n\n"
echo "-- docker build --"
printf "\n\n"
docker build . -t ms-validaciones-datos-transferencia-bepsa:${VERSION} --build-arg artifact_path=out/
rm -rf ./out

printf "\n\n"
echo "-- helm build --"
printf "\n\n"
helm package deploy/k8s --app-version ${VERSION}

printf "\n\n"
echo "Para desplegar en kubernetes: helm upgrade -i RELEASE_NAME RELEASE_NAME-0.1.0.tgz"
printf "\n\n"
echo "----------------------------------------------------------------------------------"
echo "-- End Build --"
echo "----------------------------------------------------------------------------------"
