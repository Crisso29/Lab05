//! GestionPlaylistTests.cs
//! TC-001 al TC-006 — Módulo Gestión de Playlist (Spotify)

using Spotify.Core.Playlist;

namespace Spotify.Tests.Playlist;

public class GestionPlaylistTests
{
    // ─────────────────────────────────────────────
    // TC-001: PE Clase Válida — Creación exitosa
    // ─────────────────────────────────────────────
    [Fact]
    public void TC001_CrearPlaylist_NombreYDescripcionValidos_RetornaExito()
    {
        // Arrange
        var gestor = new GestionPlaylist();
        string nombre = "Para Llullu Sara";
        string descripcion = "Huaynos y música andina tradicional para los momentos en el campo. " +
                            "Dedicado a la comunidad de Vinchos, Ayacucho.";
        // Act
        var resultado = gestor.CrearPlaylist(nombre, descripcion);

        // Assert
        Assert.True(resultado.Exito);
        Assert.Equal("Para Llullu Sara", resultado.NombreAsignado);
        Assert.Contains("Vinchos", resultado.DescripcionAsignada);
        Assert.Equal("Playlist creada exitosamente.", resultado.Mensaje);
    }

    // ─────────────────────────────────────────────
    // TC-002: PE Clase Válida — Edición exitosa
    // ─────────────────────────────────────────────
    [Fact]
    public void TC002_EditarPlaylist_NombreYDescripcionValidos_RetornaExito()
    {
        // Arrange
        var gestor = new GestionPlaylist();
        gestor.CrearPlaylist("Para Llullu Sara", "Descripción inicial");
        string nuevoNombre = "Mejores Huaynos Ayacuchanos 2026";
        string nuevaDescripcion = "Selección especial de huaynos y música andina del departamento " +
                                "de Ayacucho. Incluye artistas de Vinchos, Huamanga y Cangallo.";
        // Act
        var resultado = gestor.EditarPlaylist("Para Llullu Sara", nuevoNombre, nuevaDescripcion);

        // Assert
        Assert.True(resultado.Exito);
        Assert.Equal("Mejores Huaynos Ayacuchanos 2026", resultado.NombreAsignado);
        Assert.Contains("Ayacucho", resultado.DescripcionAsignada);
        Assert.Equal("Playlist editada exitosamente.", resultado.Mensaje);
    }

    // ─────────────────────────────────────────────
    // TC-003: PE Clase Inválida — Descripción 301 chars → trunca a 300
    // ─────────────────────────────────────────────
    [Fact]
    public void TC003_CrearPlaylist_DescripcionDe301Chars_TruncaA300()
    {
        // Arrange
        var gestor = new GestionPlaylist();
        string nombre = "Playlist Ayacucho";
        // Dato exacto del caso de prueba: 301 chars
        string descripcion301 = "Seleccion especial de huaynos, musica andina y folklore tradicional " +
                                "del departamento de Ayacucho, Peru. Esta playlist incluye artistas y " +
                                "agrupaciones representativas de las provincias de Huamanga, Cangallo, " +
                                "Vilcashuaman, Vinchos y Victor Fajardo. Para escuchar personalmente " +
                                "y en familia. comparte XD";

        // Act
        var resultado = gestor.CrearPlaylist(nombre, descripcion301);

        // Assert
        Assert.True(resultado.Exito);
        Assert.True(resultado.DescripcionAsignada.Length <= 300,
            $"Se esperaba máximo 300 chars, se obtuvo {resultado.DescripcionAsignada.Length}");
        Assert.Equal(300, resultado.DescripcionAsignada.Length);
    }

    // ─────────────────────────────────────────────
    // TC-004: AVL N — Nombre exacto de 100 chars → acepta
    // ─────────────────────────────────────────────
    [Fact]
    public void TC004_CrearPlaylist_NombreExacto100Chars_RetornaExito()
    {
        // Arrange
        var gestor = new GestionPlaylist();
        // Dato exacto del caso de prueba: 100 chars
        string nombre100 = "Musica Andina Peruana Tradicional de la Region de Ayacucho Para Escuchar y Disfrutar!!!!!!!!!!!!!!!!";

        // Act
        var resultado = gestor.CrearPlaylist(nombre100, "Descripción válida");

        // Assert
        Assert.True(resultado.Exito);
        Assert.Equal(100, resultado.NombreAsignado.Length);
        Assert.Equal("Playlist creada exitosamente.", resultado.Mensaje);
    }

    // ─────────────────────────────────────────────
    // TC-005: AVL N+1 — Nombre de 101 chars → trunca a 100
    // ─────────────────────────────────────────────
    [Fact]
    public void TC005_CrearPlaylist_NombreDe101Chars_TruncaA100()
    {
        // Arrange
        var gestor = new GestionPlaylist();
        // Dato exacto del caso de prueba: 101 chars
        string nombre101 = "Musica Andina Peruana Tradicional de la Region de Ayacucho Para Escuchar y Disfrutar ABCDEFGHAIJKLMN";

        // Act
        var resultado = gestor.CrearPlaylist(nombre101, "Descripción válida");

        // Assert
        Assert.True(resultado.Exito);
        Assert.Equal(100, resultado.NombreAsignado.Length);
        Assert.True(resultado.NombreAsignado.Length <= 100,
            $"Se esperaba máximo 100 chars, se obtuvo {resultado.NombreAsignado.Length}");
    }

    // ─────────────────────────────────────────────
    // TC-006: Edge Case — Nombre vacío → error obligatorio
    // ─────────────────────────────────────────────
    [Fact]
    public void TC006_CrearPlaylist_NombreVacio_RetornaMensajeObligatorio()
    {
        // Arrange
        var gestor = new GestionPlaylist();

        // Act — caso A: campo vacío
        var resultadoVacio = gestor.CrearPlaylist("", "Descripción válida");

        // Act — caso B: solo espacios
        var resultadoEspacios = gestor.CrearPlaylist("   ", "Descripción válida");

        // Assert caso A
        Assert.False(resultadoVacio.Exito);
        Assert.Equal("El nombre de la lista de reproducción es obligatorio.",
            resultadoVacio.Mensaje);

        // Assert caso B
        Assert.False(resultadoEspacios.Exito);
        Assert.Equal("El nombre de la lista de reproducción es obligatorio.",
            resultadoEspacios.Mensaje);
    }
}