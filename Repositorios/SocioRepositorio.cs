using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio
{
    private readonly string _cn;

    public SocioRepositorio(IConfiguration config)
    {
        _cn = config.GetConnectionString("BibliotecaDb")
              ?? throw new InvalidOperationException("Falta la cadena de conexión BibliotecaDb");
    }

    public async Task<IEnumerable<Socio>> ListarAsync()
    {
        using var db = new SqlConnection(_cn);
        return await db.QueryAsync<Socio>("usp_Socios_ListarActivos",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ExisteDniAsync(string dni)
    {
        using var db = new SqlConnection(_cn);
        var n = await db.ExecuteScalarAsync<int>("usp_Socios_ExisteDni",
            new { Dni = dni }, commandType: CommandType.StoredProcedure);
        return n > 0;
    }

    public async Task InsertarAsync(Socio s)
    {
        using var db = new SqlConnection(_cn);
        await db.ExecuteAsync("usp_Socios_Insertar",
            new { s.Nombre, s.Dni }, commandType: CommandType.StoredProcedure);
    }
}