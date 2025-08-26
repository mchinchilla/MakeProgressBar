# MakeProgressBar

Aplicación de consola (.NET) para generar:
1. Una barra de progreso en ASCII directamente en la terminal.
2. Una imagen PNG con un indicador circular + barra horizontal y datos de avance hacia una fecha objetivo.

---
## 📌 ¿Qué problema resuelve?
Cuando sigues una cuenta regresiva (por ejemplo, fin de año, una meta personal o una fecha límite), esta herramienta te permite visualizar el avance de forma clara tanto en texto (rápido para compartir en chats) como en una imagen estilizada (para redes sociales, presentaciones o status reports).

---
## ✨ Características
- Cálculo automático del porcentaje de progreso entre el 1 de enero y una fecha objetivo configurable.
- Barra de progreso ASCII (ancho fijo de 50 caracteres).
- Generación de imagen PNG (800x450) con:
  - Indicador circular de progreso.
  - Barra horizontal complementaria.
  - Texto con días restantes y fecha objetivo en formato amigable.
- Fuente personalizada (Roboto) incluida en el repo.
- Colores personalizables (paleta azul clara estilo material design).
- Nombre de archivo de salida con estampado de fecha: `progress_YYYYMMDD.png`.

---
## 🧪 Ejemplo de salida en consola
```
--- Progress Towards Target Date ---
      Target Date: November 30, 2025
        Days Left: 97
Progress Complete: 73.4 %

#SEVAN en 97 días:
[███████████████████████████████---------] 73.4 %
✅ Image successfully generated at: C:/ruta/completa/progress_20250825.png
```
(Los números variarán según el día de ejecución.)

---
## 🖼 Ejemplo de imagen generada
El archivo PNG se guarda en el directorio de ejecución (por defecto `bin/Debug/net9.0/`). Puedes abrirlo o adjuntarlo donde lo necesites.

Nombre típico: `progress_20250825.png`.

---
## ⚙️ Configuración rápida
Requisitos:
- .NET 9 (o versión compatible indicada en el `.csproj`).
- Sistema operativo: probado en Windows (compatible también con otros SO para ImageSharp).

Clona el repositorio y compila:
```
dotnet build
```
Ejecuta:
```
dotnet run --project MakeProgressBar
```

---
## 🛠 Personalización
Edita en `Program.cs` las siguientes secciones:
- Fecha objetivo:
  ```csharp
  var targetDate = new DateTime(2025, 11, 30);
  // var targetDate = new DateTime(2026, 1, 1);
  ```
- Texto mostrado (frases como `#SEVAN`, `ZDM en ...`, etc.).
- Colores (variables `backgroundColor`, `primaryColor`, etc.).
- Dimensiones (`width`, `height`, radios, espesores).

Fuente:
- El archivo `Roboto-Regular.ttf` está incluido. Asegúrate de que su propiedad sea "Copy to Output Directory" (Copiar siempre / si es más reciente).

---
## 📁 Estructura mínima del proyecto
```
MakeProgressBar/
 ├─ Program.cs
 ├─ MakeProgressBar.csproj
 ├─ Roboto-Regular.ttf
 └─ bin/ ... (salidas compiladas)
```

---
## 📦 Dependencias principales
- SixLabors.ImageSharp
- SixLabors.ImageSharp.Drawing
- SixLabors.Fonts

Estas permiten dibujar formas, texto y exportar imágenes sin depender de System.Drawing en Windows.

---
## 🚀 Ideas de mejora futura
- Pasar la fecha objetivo como argumento CLI (`dotnet run -- 2025-11-30`).
- Soporte para rangos personalizados (fecha inicio distinta a 1 de enero).
- Salida JSON / CSV para pipelines.
- Generación de GIF con progresión.
- Tema claro / oscuro con parámetro.

---
## 🧯 Resolución de problemas
| Problema | Causa probable | Solución |
|----------|----------------|----------|
| "The target date has already passed" | Ejecutas después de la fecha objetivo | Cambia `targetDate` a una fecha futura |
| Error al cargar la fuente | Falta `Roboto-Regular.ttf` en output | Verifica propiedad "Copy to Output Directory" |
| Imagen vacía o sin texto | Fuente no válida o no encontrada | Usa otra fuente TTF local y actualiza el nombre |
| Porcentaje extraño | Fecha del sistema incorrecta | Ajusta la fecha del sistema / revisa zona horaria |

---
## 📄 Licencia
Este proyecto se distribuye bajo la licencia MIT (ver `LICENSE.txt`).

---
## 🤝 Contribuciones
Se aceptan mejoras pequeñas vía PR (documentación, parámetros CLI, temas). Abre un issue para discutir cambios mayores.

---
## ✅ Resumen
Ejecuta, observa el progreso, comparte el ASCII o la imagen. Modifica fechas y colores para adaptarlo a tu caso.

¡Listo para usar!
