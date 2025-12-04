# Letras Vivas

## Descripción

"Letras Vivas" es una aplicación educativa desarrollada en Unity diseñada para enseñar a leer y escribir a través de una serie de juegos y ejercicios interactivos.

## Características

El proyecto incluye varias actividades de aprendizaje:

-   **Módulos de aprendizaje interactivos**: Lecciones estructuradas para un aprendizaje progresivo.
-   **Ejercicios de Sonido**: Actividades para asociar letras con sus sonidos.
-   **Sopa de Letras**: Juego clásico para el reconocimiento de palabras.
-   **Lienzo de Escritura**: Un espacio para practicar la escritura a mano alzada.

## Detalles Técnicos

-   **Motor**: Unity 2021.3.8f1 (o superior)
-   **Scripts Principales**:
    -   `Principal.cs`: Actúa como el controlador principal de la aplicación, gestionando la lógica de los módulos de aprendizaje.
    -   `UIManager.cs`: Gestiona la navegación y la presentación de las diferentes vistas (pantallas) de la aplicación.

## ¿Cómo Empezar?

1.  Clona o descarga este repositorio.
2.  Abre el proyecto con Unity Hub, asegurándote de usar una versión compatible de Unity.
3.  La escena principal del proyecto es `Assets/Scenes/LetrasVivas.unity`. Ábrela desde el editor de Unity.
4.  Presiona "Play" para iniciar la aplicación en el editor.

## Estructura del Proyecto

```
/Assets
|-- /Prefabs           # Componentes reutilizables como los juegos y letras.
|-- /resources         # Archivos multimedia (audio, imágenes, videos).
|-- /Scenes            # Escenas de la aplicación (niveles, menús).
|-- /Script            # Todos los scripts C# del proyecto.
|   |-- /Controlador   # Lógica de negocio y control de la aplicación.
|   |-- /ManagerView   # Scripts para la gestión de la interfaz de usuario.
```
