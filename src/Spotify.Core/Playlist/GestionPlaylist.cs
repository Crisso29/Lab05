//*GestionPlaylist.cs
//*Módulo: Gestión de Playlist — Spotify
//* Fase GREEN: código mínimo para pasar los 6 tests

namespace Spotify.Core.Playlist;

// Objeto que devuelven las operaciones
public class ResultadoPlaylist
{
    public bool Exito { get; set; }
    public string NombreAsignado { get; set; } = "";
    public string DescripcionAsignada { get; set; } = "";
    public string Mensaje { get; set; } = "";
}

public class GestionPlaylist
{
    private readonly List<(string Nombre, string Descripcion)> _playlists = new();

    // TC-001, TC-002, TC-003, TC-004, TC-005, TC-006
    public ResultadoPlaylist CrearPlaylist(string nombre, string descripcion)
    {
        // TC-006: nombre vacío o solo espacios
        if (string.IsNullOrWhiteSpace(nombre))
            return new ResultadoPlaylist
            {
                Exito = false,
                Mensaje = "El nombre de la lista de reproducción es obligatorio."
            };

        // TC-005: truncar nombre si supera 100 chars
        if (nombre.Length > 100)
            nombre = nombre[..100];

        // TC-003: truncar descripción si supera 300 chars
        if (descripcion.Length > 300)
            descripcion = descripcion[..300];

        _playlists.Add((nombre, descripcion));

        return new ResultadoPlaylist
        {
            Exito = true,
            NombreAsignado = nombre,
            DescripcionAsignada = descripcion,
            Mensaje = "Playlist creada exitosamente."
        };
    }

    // TC-002: editar playlist existente
    public ResultadoPlaylist EditarPlaylist(string nombreActual, string nuevoNombre, string nuevaDescripcion)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            return new ResultadoPlaylist
            {
                Exito = false,
                Mensaje = "El nombre de la lista de reproducción es obligatorio."
            };

        if (nuevoNombre.Length > 100)
            nuevoNombre = nuevoNombre[..100];

        if (nuevaDescripcion.Length > 300)
            nuevaDescripcion = nuevaDescripcion[..300];

        return new ResultadoPlaylist
        {
            Exito = true,
            NombreAsignado = nuevoNombre,
            DescripcionAsignada = nuevaDescripcion,
            Mensaje = "Playlist editada exitosamente."
        };
    }
}