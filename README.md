# 🎵 LAB 05 — Pruebas Unitarias Automatizadas con xUnit.NET

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![xUnit](https://img.shields.io/badge/xUnit.net-2.9-blue?style=for-the-badge)
![TDD](https://img.shields.io/badge/Metodología-TDD-red?style=for-the-badge)
![Tests](https://img.shields.io/badge/Tests-12%20Passed-brightgreen?style=for-the-badge)
![Status](https://img.shields.io/badge/Estado-Completado-success?style=for-the-badge)

---

## 📋 Información General

| Campo | Detalle |
|---|---|
| **Asignatura** | IS-489 Pruebas y Aseguramiento de Calidad de Software |
| **Docente** | Ing. Lizbeth Jaico Quispe |
| **Semestre** | 2026-I — Presencial |
| **Laboratorio** | Laboratorio 05 |
| **Tecnología** | C# + .NET 10 + xUnit.net |
| **Sistema bajo prueba** | Spotify Web — open.spotify.com |
| **Autor** | Crisologo Aguilar Flores |
| **Universidad** | Universidad Nacional de San Cristóbal de Huamanga (UNSCH) |
| **Escuela** | Ingeniería de Sistemas |

---

## 🎯 Objetivo

Implementar **pruebas unitarias automatizadas** aplicando la metodología **TDD (Test Driven Development)** con el ciclo **Red → Green → Refactor**, sobre dos módulos del sistema **Spotify Web**, tomando como base los 12 casos de prueba manuales desarrollados en el Laboratorio 03 (técnicas PE, AVL y Edge Case).

Al término de este laboratorio se es capaz de:
- Comprender qué es una prueba unitaria y automatizar el trabajo del QA.
- Ejecutar suites de pruebas con xUnit.net e interpretar el reporte de resultados.
- Aplicar el patrón **AAA (Arrange → Act → Assert)** en cada test.
- Aplicar el ciclo **TDD (Red → Green → Refactor)** sobre Historias de Usuario reales.
- Versionar la suite de pruebas en Git y ejecutarla con un solo comando.

---

## 🏗️ Estructura del Proyecto

```
Lab05/
├── SpotifyTDD.sln                              ← Solución que une ambos proyectos
├── .gitignore                                  ← Excluye bin/, obj/, TestResults/
├── README.md                                   ← Este archivo
│
├── src/
│   └── Spotify.Core/                           ← Proyecto: código de producción
│       ├── Spotify.Core.csproj
│       ├── Playlist/
│       │   └── GestionPlaylist.cs              ← Lógica de gestión de playlists
│       └── Busqueda/
│           └── BusquedaFiltrado.cs             ← Lógica de búsqueda y filtrado
│
└── tests/
    └── Spotify.Tests/                          ← Proyecto: pruebas unitarias
        ├── Spotify.Tests.csproj
        ├── Playlist/
        │   └── GestionPlaylistTests.cs         ← 6 tests (TC-001 al TC-006)
        └── Busqueda/
            └── BusquedaFiltradoTests.cs        ← 6 tests (TC-007 al TC-012)
```

> **¿Por qué 2 proyectos separados?**
> Siguiendo las buenas prácticas de QA, el código de producción (`src/`) y las pruebas (`tests/`) viven en proyectos distintos. Los tests referencian el src gracias a la directiva `ProjectReference` en el `.csproj`, pero nunca se mezclan.

---

## 🧪 Casos de Prueba Implementados

### 📁 Módulo 1: Gestión de Playlist (TC-001 al TC-006)

> Valida las reglas de negocio de Spotify para crear y editar playlists:
> nombre máximo 100 caracteres, descripción máximo 300 caracteres, nombre no puede estar vacío.

| ID | Descripción | Técnica | Resultado Esperado | Estado |
|---|---|---|---|---|
| TC-001 | Crear playlist con nombre `"Para Llullu Sara"` y descripción válida | PE Clase Válida | `Exito = true`, nombre asignado correctamente | ✅ PASS |
| TC-002 | Editar nombre a `"Mejores Huaynos Ayacuchanos 2026"` y descripción nueva | PE Clase Válida | `Exito = true`, nombre y descripción actualizados | ✅ PASS |
| TC-003 | Descripción de 301 caracteres → debe truncarse a 300 | PE Clase Inválida | `DescripcionAsignada.Length == 300` | ✅ PASS |
| TC-004 | Nombre de exactamente 100 caracteres → debe aceptarse (AVL N) | AVL — N | `Exito = true`, `NombreAsignado.Length == 100` | ✅ PASS |
| TC-005 | Nombre de 101 caracteres → debe truncarse a 100 (AVL N+1) | AVL — N+1 | `NombreAsignado.Length == 100` | ✅ PASS |
| TC-006 | Nombre vacío `""` y solo espacios `"   "` → error obligatorio | Edge Case | `Exito = false`, mensaje de error exacto | ✅ PASS |

### 📁 Módulo 2: Búsqueda y Filtrado (TC-007 al TC-012)

> Valida el motor de búsqueda de Spotify: resultados válidos, manejo de términos inexistentes,
> cadenas largas, campo vacío, y sanitización de inyecciones XSS/SQL.

| ID | Descripción | Técnica | Resultado Esperado | Estado |
|---|---|---|---|---|
| TC-007 | Buscar `"Dua Lipa"` (artista existente en catálogo) | PE Clase Válida | `Exito = true`, resultados no vacíos, contiene "Dua Lipa" | ✅ PASS |
| TC-008 | Buscar `"xkqzmpwvlrfbnt2026ayacucho"` (término sin sentido) | PE Clase Inválida | `Exito = false`, lista vacía, mensaje "No se encontraron resultados..." | ✅ PASS |
| TC-009 | Buscar `"Música tradicional de Vinchos Ayacucho"` (término regional) | PE Clase Inválida | Respuesta controlada: vacío o aproximado, sin errores | ✅ PASS |
| TC-010 | Cadena de más de 800 caracteres → debe manejarse sin error | PE Clase Inválida | `TerminoUsado.Length <= 800`, sin excepciones | ✅ PASS |
| TC-011 | Campo vacío `""` → mensaje "Ingrese un término" / Espacios `"   "` → sin resultados | Edge Case | Respuesta controlada en ambos casos, `Exito = false` | ✅ PASS |
| TC-012 | Inyección `<script>alert('XSS')</script>` y `'; DROP TABLE tracks;--` | Edge Case Seguridad | Entrada sanitizada, sin `<script>` ni `DROP TABLE` en `TerminoUsado` | ✅ PASS |

---

## 🔄 Ciclo TDD Aplicado

El desarrollo siguió estrictamente las 3 fases del ciclo TDD:

```
╔══════════════════════════════════════════════════════════════════╗
║  🔴 FASE RED — Escribir los tests ANTES que el código           ║
║                                                                  ║
║  1. Se escribieron los tests en GestionPlaylistTests.cs          ║
║     y BusquedaFiltradoTests.cs con los datos reales de          ║
║     los casos de prueba del Lab 03.                             ║
║                                                                  ║
║  2. Se ejecutó: dotnet build                                     ║
║     Resultado: ERROR (esperado) — la clase no existe aún        ║
║     "The type or namespace 'GestionPlaylist' could not          ║
║      be found"                                                   ║
╚══════════════════════════════════════════════════════════════════╝
                            ↓
╔══════════════════════════════════════════════════════════════════╗
║  🟢 FASE GREEN — Implementar el código mínimo para pasar        ║
║                                                                  ║
║  3. Se implementó GestionPlaylist.cs y BusquedaFiltrado.cs      ║
║     con la lógica mínima necesaria:                             ║
║     - Validación de campos vacíos                               ║
║     - Truncado de nombre (100 chars) y descripción (300 chars)  ║
║     - Motor de búsqueda con catálogo simulado                   ║
║     - Sanitización de entradas peligrosas (XSS/SQL)            ║
║                                                                  ║
║  4. Se ejecutó: dotnet test                                      ║
║     Resultado: Passed: 12, Failed: 0 ✅                         ║
╚══════════════════════════════════════════════════════════════════╝
                            ↓
╔══════════════════════════════════════════════════════════════════╗
║  🔵 FASE REFACTOR — Mejorar sin romper los tests                ║
║                                                                  ║
║  5. Se enriquecieron los tests con datos exactos de los         ║
║     casos de prueba: nombres reales, textos en quechua,         ║
║     términos de Vinchos/Ayacucho, cadenas de inyección.         ║
║                                                                  ║
║  6. Se ejecutó: dotnet test --logger "console;verbosity=         ║
║     detailed"                                                    ║
║     Resultado: Passed: 12, Failed: 0 ✅ (sigue en verde)        ║
╚══════════════════════════════════════════════════════════════════╝
```

---

## 📐 Estructura de un Test en xUnit.net

Cada test sigue el patrón **AAA (Arrange → Act → Assert)**:

```csharp
// El atributo [Fact] marca el método como un test ejecutable por xUnit
[Fact]
public void TC001_CrearPlaylist_NombreYDescripcionValidos_RetornaExito()
{
    // ── ARRANGE: preparar los datos y objetos necesarios ──────────────
    var gestor = new GestionPlaylist();
    string nombre = "Para Llullu Sara";
    string descripcion = "Huaynos y música andina tradicional para los momentos en el campo. " +
                         "Dedicado a la comunidad de Vinchos, Ayacucho.";

    // ── ACT: ejecutar la función que se está probando ─────────────────
    var resultado = gestor.CrearPlaylist(nombre, descripcion);

    // ── ASSERT: verificar que el resultado es el esperado ─────────────
    Assert.True(resultado.Exito);
    Assert.Equal("Para Llullu Sara", resultado.NombreAsignado);
    Assert.Contains("Vinchos", resultado.DescripcionAsignada);
    Assert.Equal("Playlist creada exitosamente.", resultado.Mensaje);
}
```

### Convención de nombres de métodos de test

```
TC001  _  CrearPlaylist  _  NombreYDescripcionValidos  _  RetornaExito
  │              │                     │                        │
  ID del       Qué función        Bajo qué condición       Qué se espera
  caso         se prueba          se prueba                 obtener
```

Esta convención permite leer el reporte de xUnit y entender exactamente qué falló sin abrir el código.

---

## ✅ Matchers (Asserts) utilizados en xUnit.net

| Assert xUnit | ¿Qué verifica? | Equivalente en Jest |
|---|---|---|
| `Assert.True(x)` | que `x` sea `true` | `expect(x).toBe(true)` |
| `Assert.False(x)` | que `x` sea `false` | `expect(x).toBe(false)` |
| `Assert.Equal(esperado, actual)` | igualdad exacta | `expect(actual).toBe(esperado)` |
| `Assert.Contains(texto, cadena)` | que la cadena contenga el texto | `expect(cadena).toContain(texto)` |
| `Assert.DoesNotContain(texto, cadena)` | que la cadena NO contenga el texto | `expect(cadena).not.toContain(texto)` |
| `Assert.Empty(lista)` | que la lista esté vacía | `expect(lista).toHaveLength(0)` |
| `Assert.NotEmpty(lista)` | que la lista tenga elementos | `expect(lista).not.toHaveLength(0)` |
| `Assert.NotNull(x)` | que `x` no sea `null` | `expect(x).not.toBeNull()` |
| `Assert.True(x, "mensaje")` | con mensaje personalizado al fallar | `expect(x).toBe(true)` |

---

## 📊 Comparativa: Testing Manual vs Automatizado

| Aspecto | Testing Manual (Lab 03) | Testing Automatizado con xUnit (Lab 05) |
|---|---|---|
| ¿Quién ejecuta? | El tester, manualmente | El computador, automáticamente |
| ¿Cuánto tarda? | Minutos por caso | Milisegundos |
| ¿Se repite? | Solo cuando el tester lo decide | En cada `git commit`, automáticamente |
| ¿Qué detecta? | Comportamiento visual e inesperado | Regresiones: bugs introducidos al cambiar código |
| Evidencia | Capturas de pantalla | Reporte de xUnit con `Passed/Failed` y duración |
| Reproducibilidad | Depende del tester | Siempre igual, en cualquier máquina |

---

## 🚀 Cómo ejecutar el proyecto

### Requisitos previos
- .NET SDK 10.0 o superior → [descargar](https://dotnet.microsoft.com/download)
- Visual Studio Code o Visual Studio 2022
- Git

### Pasos

```bash
# 1. Clonar el repositorio
git clone https://github.com/DevCrisso/Lab05.git
cd Lab05

# 2. Compilar toda la solución
dotnet build

# 3. Ejecutar todos los tests (resumen)
dotnet test

# 4. Ejecutar con resultado detallado por test (recomendado)
dotnet test --logger "console;verbosity=detailed"

# 5. Ejecutar solo el módulo de Playlist
dotnet test --filter "FullyQualifiedName~Playlist"

# 6. Ejecutar solo el módulo de Búsqueda
dotnet test --filter "FullyQualifiedName~Busqueda"
```

### Resultado esperado

```
Spotify.Tests net10.0 → tests\Spotify.Tests\bin\Debug\net10.0\Spotify.Tests.dll

  ✓ TC001_CrearPlaylist_NombreYDescripcionValidos_RetornaExito
  ✓ TC002_EditarPlaylist_NombreYDescripcionValidos_RetornaExito
  ✓ TC003_CrearPlaylist_DescripcionDe301Chars_TruncaA300
  ✓ TC004_CrearPlaylist_NombreExacto100Chars_RetornaExito
  ✓ TC005_CrearPlaylist_NombreDe101Chars_TruncaA100
  ✓ TC006_CrearPlaylist_NombreVacio_RetornaMensajeObligatorio
  ✓ TC007_Buscar_TerminoValido_RetornaResultados
  ✓ TC008_Buscar_TerminoInexistente_RetornaListaVacia
  ✓ TC009_Buscar_TerminoRegionalEspecifico_RetornaVacioOAproximado
  ✓ TC010_Buscar_CadenaMayorA800Chars_ManejaControlado
  ✓ TC011_Buscar_CampoVacioYSoloEspacios_NoProcesa
  ✓ TC012_Buscar_InyeccionXSSySQL_SanitizaEntrada

Resumen: total: 12; con errores: 0; correcto: 12; omitido: 0
```

---

## 🛠️ Tecnologías utilizadas

| Herramienta | Versión | Rol en el proyecto |
|---|---|---|
| C# | 13.0 | Lenguaje de programación |
| .NET | 10.0 | Framework de ejecución |
| xUnit.net | 2.9 | Framework de pruebas unitarias |
| Visual Studio Code | Latest | Editor de código |
| Visual Studio 2022 | Latest | IDE alternativo |
| Git & GitHub | Latest | Control de versiones |

---

## 📁 Descripción de archivos principales

### `src/Spotify.Core/Playlist/GestionPlaylist.cs`
Contiene la clase `GestionPlaylist` con los métodos:
- `CrearPlaylist(nombre, descripcion)` → valida campos, trunca si excede límites, retorna `ResultadoPlaylist`
- `EditarPlaylist(nombreActual, nuevoNombre, nuevaDescripcion)` → edita y valida metadatos

### `src/Spotify.Core/Busqueda/BusquedaFiltrado.cs`
Contiene la clase `BusquedaFiltrado` con:
- `Buscar(termino)` → valida entrada, sanitiza XSS/SQL, trunca si >800 chars, busca en catálogo simulado
- `SanitizarEntrada(entrada)` → elimina patrones peligrosos de la cadena de búsqueda

### `tests/Spotify.Tests/Playlist/GestionPlaylistTests.cs`
Suite de 6 tests (TC-001 al TC-006) con datos reales de los casos de prueba manuales del Lab 03.

### `tests/Spotify.Tests/Busqueda/BusquedaFiltradoTests.cs`
Suite de 6 tests (TC-007 al TC-012) incluyendo pruebas de seguridad (XSS, SQL Injection).

---

## 👨‍💻 Autor

**Crisologo Aguilar Flores**
Estudiante de Ingeniería de Sistemas — 6 mo Semestre
Universidad Nacional de San Cristóbal de Huamanga (UNSCH)
Ayacucho, Perú 🇵🇪
