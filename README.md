<!-- PROJECT LOGO -->
<br>  
<p align="center">
    <!-- <img src="https://intranet/SiteAssets/imagenes/logoContinental.jpg?csf=1&e=KS79bg" alt="Logo" width="80" height="80"> -->
    <img src="https://intranet/SiteAssets/imagenes/logo2Continental.png?csf=1&e=vzThCS" alt="Logo" width="80" height="80">

  <h3 align="center">README</h3>

  <p align="center">
    Bienvenido al Project Base
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Tabla de Contenido</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#uso">Uso</a></li>
    <li><a href="#documentation">Documentation</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

<br>

<!-- ABOUT THE PROJECT -->
## About The Project
----

Una breve descripción sobre el project, en qué consiste, el propósito y que soluciones da.

### Built With

Para el uso de este project se debera tener instalado previamente los siguientes recursos:
* [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)
* [Netcoreapp3.1](https://dotnet.microsoft.com/download/dotnet/3.1) (próximamente [Netcoreapp5](#))

<br>

<!-- GETTING STARTED -->
## Getting Started
----
Para empezar con este project, deberemos seguir los simples pasos de la instalación para tener el source localmente y posteriormente poder desplegarlo.



### Installation

1. Clonar el repositorio  

   ```sh
   git clone https://BancoContinental@dev.azure.com/BancoContinental/repo/_git/project
   ```
2. Agregar Origenes de Paquete al Visual Studio en caso que requiera el project  

   ```sh
   https://pkgs.dev.azure.com/BancoContinental/OpenBanking/_packaging/OpenBank/nuget/v3/index.json
   ```

<br>

<!-- USAGE EXAMPLES -->
## Uso
----
Para usar el siguiente project, puede acceder a su versión de DEMO-QA y realizar pruebas  

[Ir a Swagger](http://10.6.2.41:8088/swagger/index.html)

_Para más detalla, por favor véase en la [Documentation](#Documentation)_

<br>

<!-- DOCUMENTATION EXAMPLES -->
## Documentation
----
Json Request
```json
{
  "PrimerNombre": {"type" : "string", "required" : "true"},
  "Apellido": {"type" : "string", "required" : "true"},
  "Año": {"type" : "int", "required" : "false"}
}
```

Successful Type Response
| Status Code  | Type       | Description   |
| :---:        |    :----:  |     :---:     |
| 200          | OK         |     --        |
| 201          | Created    | Content       |
| 202          | Accepted   | --            |
| 204          | OK         | No Content    |

<br>

Json Error Response 400. Bad Request/Business Rule
```json
{
  "ErrorType": "error_validacion_cierre",
  "ErrorDescription": "El sistema se encuenta en proceso de cierre. Por favor intente más tarde"
}
```
Error Type Response
| Status Code  | Error Type                 | Error Description                                                         |
| :---:        |    :----:                  |          :---:                                                            |
| 400          | request_invalid_attribute  | El campo "PrimerNombre" no puede quedar vacío                             |
| 400          | error_validacion_cierre    | El sistema se encuenta en proceso de cierre. Por favor intente más tarde  |
| 401          | Unauthorized               | --                                                                        |
| 403          | Forbibben                  | --                                                                        |
| 500          | Internal Server Error      | --                                                                        |

<br>

<!-- CONTRIBUTING -->
## Contributing
----
Para poder contribuir deberás seguir los siguientes pasos. Cualquier contribución que puedas hacer será **gratamente apreciada y analizada para su posterior aplicación**.

1. Clonar el Project
2. Crear un nuevo Branch (Ej: `git checkout -b feature/NewGreatFeature`)
3. Commit tus cambios (Ej: `git commit -m 'Add some NewGreatFeature'`)
4. Push hacia el Branch creado (Ej: `git push origin feature/NewGreatFeature`)
5. Crear a Pull Request

<br>

<!-- LICENSE -->
## License
----
Propiedad intelectual del Banco Continentalª.

<br>

<!-- CONTACT -->
## Contact
----
Nombre del Desarrollador - email@ejemplo.com  

Project Leader - email2@ejemplo.com

Project Link: [https://BancoContinental@dev.azure.com/BancoContinental/Project/_git/Repo](https://BancoContinental@dev.azure.com/BancoContinental/Project/_git/Repo)



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->