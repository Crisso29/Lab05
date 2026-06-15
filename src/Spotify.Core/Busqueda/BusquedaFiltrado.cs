// BusquedaFiltrado.cs
// Módulo: Búsqueda y Filtrado — Spotify
// Autor: Crisologo Aguilar Flores
// Curso: IS-489 Pruebas y Aseguramiento de Calidad de Software
// Docente: Ing. Lizbeth Jaico Quispe — Semestre 2026-I
// Fase GREEN: código mínimo para pasar los 6 tests

namespace Spotify.Core.Busqueda;

// Objeto resultado de búsqueda
public class ResultadoBusqueda
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = "";
    public string TerminoUsado { get; set; } = "";
    public List<ItemBusqueda> Resultados { get; set; } = new();
}

// Representa un item encontrado
public class ItemBusqueda
{
    public string Nombre { get; set; } = "";
    public string Tipo { get; set; } = ""; // artista, cancion, album
}

public class BusquedaFiltrado
{
    // Catálogo simulado — representa el backend de Spotify
    private readonly List<ItemBusqueda> _catalogo = new()
    {
        new ItemBusqueda { Nombre = "Dua Lipa", Tipo = "artista" },
        new ItemBusqueda { Nombre = "Levitating", Tipo = "cancion" },
        new ItemBusqueda { Nombre = "Future Nostalgia", Tipo = "album" },
        new ItemBusqueda { Nombre = "Bad Guy", Tipo = "cancion" },
        new ItemBusqueda { Nombre = "Billie Eilish", Tipo = "artista" },
        new ItemBusqueda { Nombre = "Música Andina Ayacucho", Tipo = "cancion" },
        new ItemBusqueda { Nombre = "Huayno Ayacuchano", Tipo = "cancion" },
    };

    // Caracteres peligrosos para sanitización
    private readonly string[] _patronesPeligrosos =
    {
        "<script>", "</script>", "alert(", "DROP TABLE", "SELECT *",
        "INSERT INTO", "DELETE FROM", "--", "/*", "*/"
    };

    public ResultadoBusqueda Buscar(string termino)
    {
        // TC-011 Caso A: campo completamente vacío
        if (termino == "")
            return new ResultadoBusqueda
            {
                Exito = false,
                Mensaje = "Ingrese un término de búsqueda.",
                TerminoUsado = termino
            };
        
        if (termino.Trim() == "")
            return new ResultadoBusqueda
            {
            Exito = false,
            Mensaje = $"No se encontraron resultados de '{termino}'.",
            TerminoUsado = termino
            };

        // TC-012: sanitizar entrada peligrosa
        string terminoSanitizado = SanitizarEntrada(termino);

        // TC-010: truncar si supera 800 chars
        if (terminoSanitizado.Length > 800)
            terminoSanitizado = terminoSanitizado[..800];

        // TC-011 Caso B: solo espacios → busca pero no encuentra nada
        // TC-008: término inexistente → lista vacía
        var resultados = _catalogo
            .Where(item => item.Nombre.Contains(
                terminoSanitizado.Trim(),
                StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (resultados.Count == 0)
            return new ResultadoBusqueda
            {
                Exito = false,
                Mensaje = $"No se encontraron resultados de '{termino}'.",
                TerminoUsado = terminoSanitizado,
                Resultados = new List<ItemBusqueda>()
            };

        // TC-007, TC-009: resultados encontrados
        return new ResultadoBusqueda
        {
            Exito = true,
            Mensaje = $"Se encontraron {resultados.Count} resultado(s).",
            TerminoUsado = terminoSanitizado,
            Resultados = resultados
        };
    }

    // TC-012: elimina patrones peligrosos XSS y SQL
    private string SanitizarEntrada(string entrada)
    {
        foreach (var patron in _patronesPeligrosos)
            entrada = entrada.Replace(patron, "", StringComparison.OrdinalIgnoreCase);

        return entrada;
    }
}