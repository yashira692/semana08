using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class LibroRepositorio
{
    private readonly string _cn;

    public LibroRepositorio(IConfiguration config)
    {
        _cn = config.GetConnectionString("BibliotecaDb")
              ?? throw new InvalidOperationException("Falta la cadena de conexión BibliotecaDb");
    }

    public async Task<IEnumerable<Libro>> ListarAsync()
    {
        using var db = new SqlConnection(_cn);
        return await db.QueryAsync<Libro>("usp_Libros_ListarActivos",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Libro>> BuscarAsync(string titulo)
    {
        using var db = new SqlConnection(_cn);
        return await db.QueryAsync<Libro>("usp_Libros_BuscarPorTitulo",
            new { Titulo = titulo }, commandType: CommandType.StoredProcedure);
    }

    public async Task<Libro?> ObtenerAsync(int id)
    {
        using var db = new SqlConnection(_cn);
        return await db.QueryFirstOrDefaultAsync<Libro>("usp_Libros_ObtenerPorId",
            new { LibroId = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task InsertarAsync(Libro l)
    {
        using var db = new SqlConnection(_cn);
        await db.ExecuteAsync("usp_Libros_Insertar",
            new { l.Titulo, l.AutorId, l.Ejemplares }, commandType: CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(Libro l)
    {
        using var db = new SqlConnection(_cn);
        await db.ExecuteAsync("usp_Libros_Actualizar",
            new { l.LibroId, l.Titulo, l.AutorId, l.Ejemplares }, commandType: CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int id)
    {
        using var db = new SqlConnection(_cn);
        await db.ExecuteAsync("usp_Libros_Eliminar",
            new { LibroId = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Autor>> ListarAutoresAsync()
    {
        using var db = new SqlConnection(_cn);
        return await db.QueryAsync<Autor>("usp_Autores_ListarActivos",
            commandType: CommandType.StoredProcedure);
    }
}