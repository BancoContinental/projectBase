# Project Base para APIs

Para poder instalar este template abrir una terminal y clonar este proyecto a la ubicacion deseada.

Nos ubicamos en la carpeta del proyecto y ejecutamos el siguiente comando

```
dotnet new -i .
```

Si el comando se ejecuta correctamente veremos la lista de templates de dotnet core, con la presencia de WebApi Banco Continental

Podemos utilizar el template desde el cli escribiendo en la terminal el comando:
```
dotnet new apibanking -n ApiBanking.MiProyecto
```

O desde Visual Studio, utilizando el template WebApi Banco Continental.


## Autorizacion (Token)
Para utilizar el Keycloak, se debe contar con el Realm y el Client previamente creados en el servidor de desarrollo y modificar los valores de la seccion Keycloak:Jwt acorde al servidor. Los valores se utilizan en la aplicacion a traves de variables de entorno. En entorno local se puede utilizar el archivo src/WebApi/properties/launchSettings.json para configurar las variables.

## Version Previa
La version previa del template se encuentra en la rama template-2020
