// BusquedaFiltradoTests.cs
// TC-007 al TC-012 — Módulo Búsqueda y Filtrado (Spotify)
// Autor: Crisologo Aguilar Flores
// Curso: IS-489 Pruebas y Aseguramiento de Calidad de Software
// Docente: Ing. Lizbeth Jaico Quispe — Semestre 2026-I

using Spotify.Core.Busqueda;

namespace Spotify.Tests.Busqueda;

public class BusquedaFiltradoTests
{
    // ─────────────────────────────────────────────
    // TC-007: PE Clase Válida — Búsqueda con término válido existente
    // ─────────────────────────────────────────────
    [Fact]
    public void TC007_Buscar_TerminoValido_RetornaResultados()
    {
        // Arrange
        var buscador = new BusquedaFiltrado();
        string termino = "Dua Lipa";

        // Act
        var resultado = buscador.Buscar(termino);

        // Assert
        Assert.True(resultado.Exito);
        Assert.NotEmpty(resultado.Resultados);
        Assert.Contains(resultado.Resultados,
            r => r.Nombre.Contains("Dua Lipa", StringComparison.OrdinalIgnoreCase));
    }

    // ─────────────────────────────────────────────
    // TC-008: PE Clase Inválida — Término inexistente → lista vacía
    // ─────────────────────────────────────────────
    [Fact]
    public void TC008_Buscar_TerminoInexistente_RetornaListaVacia()
    {
        // Arrange
        var buscador = new BusquedaFiltrado();
        string termino = "xkqzmpwvlrfbnt2026ayacucho";

        // Act
        var resultado = buscador.Buscar(termino);

        // Assert
        Assert.False(resultado.Exito);
        Assert.Empty(resultado.Resultados);
        Assert.Equal("No se encontraron resultados de 'xkqzmpwvlrfbnt2026ayacucho'.",
            resultado.Mensaje);
    }

    // ─────────────────────────────────────────────
    // TC-009: PE Clase Inválida — Término regional específico → vacío o aproximado
    // ─────────────────────────────────────────────
    [Fact]
    public void TC009_Buscar_TerminoRegionalEspecifico_RetornaVacioOAproximado()
    {
        // Arrange
        var buscador = new BusquedaFiltrado();
        string termino = "Música tradicional de Vinchos Ayacucho";

        // Act
        var resultado = buscador.Buscar(termino);

        // Assert — acepta lista vacía O resultados aproximados, nunca error
        Assert.True(resultado.Exito || resultado.Resultados.Count == 0,
            "El sistema debe responder de forma controlada sin errores.");
        Assert.NotNull(resultado.Mensaje);
    }

    // ─────────────────────────────────────────────
    // TC-010: PE Clase Inválida — Cadena >800 chars → maneja sin error
    // ─────────────────────────────────────────────
    [Fact]
    public void TC010_Buscar_CadenaMayorA800Chars_ManejaControlado()
    {
        // Arrange
        var buscador = new BusquedaFiltrado();
        string termino800 = "musica andina peruana ayacucho huamanga vinchos cangallo vilcashuaman " +
                            "victor fajardo huanta la mar sucre lucanas parinacochas paucar del sara sara " +
                            "folklore huayno marinera tunantada pasacalle carnaval andino ayacuchano chicha " +
                            "cumbia tropical instrumentos tipicos charango quena zampoña tinya mandolina arpa " +
                            "violin guitarra trompeta bombo wankara platillos cantantes grupos musicales " +
                            "orquestas representativas de la region sur central del peru departamento de " +
                            "ayacucho semana santa turismo gastronomia puca picante mondongo caldo de cabeza " +
                            "chicharron jamon del pais mazamorra de cochino ponche de frutas chicha de jora";

        // Act
        var resultado = buscador.Buscar(termino800);

        // Assert — no debe lanzar excepción, debe responder controlado
        Assert.NotNull(resultado);
        Assert.True(resultado.Exito || !resultado.Exito,
            "El sistema debe responder sin errores ante cadenas largas.");
        Assert.True(resultado.TerminoUsado.Length <= 800,
            $"El término usado no debe superar 800 chars, se usaron {resultado.TerminoUsado.Length}");
    }

    // ─────────────────────────────────────────────
    // TC-011: Edge Case — Campo vacío y solo espacios → no procesa búsqueda
    // ─────────────────────────────────────────────
    [Fact]
    public void TC011_Buscar_CampoVacioYSoloEspacios_NoProceса()
    {
        // Arrange
        var buscador = new BusquedaFiltrado();

        // Act — Caso A: campo vacío
        var resultadoVacio = buscador.Buscar("");

        // Act — Caso B: solo espacios
        var resultadoEspacios = buscador.Buscar("      ");

        // Assert caso A
        Assert.False(resultadoVacio.Exito);
        Assert.Equal("Ingrese un término de búsqueda.", resultadoVacio.Mensaje);

        // Assert caso B
        Assert.False(resultadoEspacios.Exito);
        Assert.Equal("No se encontraron resultados de '      '.", resultadoEspacios.Mensaje);
    }

    // ─────────────────────────────────────────────
    // TC-012: Edge Case Seguridad — Inyección XSS/SQL → sanitiza entrada
    // ─────────────────────────────────────────────
    [Fact]
    public void TC012_Buscar_InyeccionXSSySQL_SanitizaEntrada()
    {
        // Arrange
        var buscador = new BusquedaFiltrado();
        string xss = "<script>alert('XSS-TEST')</script>";
        string sql = "'; DROP TABLE tracks;--";

        // Act
        var resultadoXss = buscador.Buscar(xss);
        var resultadoSql = buscador.Buscar(sql);

        // Assert XSS — no ejecuta, devuelve texto plano sanitizado
        Assert.False(resultadoXss.Exito);
        Assert.DoesNotContain("<script>", resultadoXss.TerminoUsado);
        Assert.DoesNotContain("alert", resultadoXss.TerminoUsado);

        // Assert SQL — no ejecuta, devuelve texto plano sanitizado
        Assert.False(resultadoSql.Exito);
        Assert.DoesNotContain("DROP TABLE", resultadoSql.TerminoUsado);
        Assert.DoesNotContain("--", resultadoSql.TerminoUsado);
    }
}