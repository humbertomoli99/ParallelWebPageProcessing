# ParallelWebPageProcessing

Este es un programa de ejemplo que utiliza la biblioteca Playwright y Serilog para procesar varias páginas web en paralelo y registrar información sobre cada página visitada.

## Requisitos

- Visual Studio (o cualquier otro entorno de desarrollo de C#)
- Paquete NuGet: Microsoft.Playwright
- Paquete NuGet: Serilog
- Paquete NuGet: Serilog.Sinks.File

Antes de ejecutar el programa, asegúrate de haber descargado y configurado los navegadores necesarios para Playwright. Puedes hacerlo ejecutando el siguiente comando en la consola de PowerShell:

```
pwsh bin/Debug/netX/playwright.ps1 install
```

Esto descargará los navegadores necesarios, como Chromium, para que Playwright pueda funcionar correctamente.

## Uso

1. Abre el proyecto en Visual Studio.
2. Asegúrate de tener los paquetes NuGet necesarios instalados.
3. Ejecuta el programa.

## Descripción

El programa realiza lo siguiente:

1. Obtiene el número de hilos actualmente en ejecución en el proceso.
2. Define una lista de URLs a procesar. Puedes agregar más URLs según sea necesario.
3. Configura Serilog para registrar la información en un archivo llamado "archivo.log".
4. Invoca el método `GetPageTitlesParallel` para procesar las páginas web en paralelo.
5. Itera sobre los resultados y registra el título de la página y el contenido del elemento `h1` utilizando Serilog.
6. Cierra y guarda los registros de Serilog.

El método `GetPageTitlesParallel` realiza lo siguiente:

1. Crea una instancia de Playwright y lanza el navegador Chromium.
2. Crea un contexto del navegador y recorre la lista de URLs en paralelo utilizando la clase `Parallel`.
3. Para cada URL, crea una nueva página, navega a la URL, obtiene el título de la página y el contenido del elemento `h1`.
4. Registra la información en la consola y en los registros de Serilog.
5. Cierra la página.
6. Captura cualquier excepción y registra un mensaje de error utilizando Serilog.
7. Cierra el navegador.

El método `GetAndPrintElementContent` obtiene y registra el contenido de un elemento en una página web.

Recuerda instalar los paquetes NuGet necesarios antes de ejecutar el programa y asegurarte de haber descargado los navegadores necesarios para Playwright.

¡Disfruta explorando y procesando páginas web en paralelo con Playwright y Serilog!
