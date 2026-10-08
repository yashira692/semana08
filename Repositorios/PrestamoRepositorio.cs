using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class PrestamoRepositorio
{
    private readonly string _cn;

    public PrestamoRepositorio(IConfiguration config)
    {
        _cn = config.GetConnectionString("BibliotecaDb")
              ?? throw new InvalidOperationException("Falta la cadena de conexión BibliotecaDb");
    }

    public async Task<IEnumerable<PrestamoReporte>> ReporteAsync(DateTime desde, DateTime hasta)
    {
        using var db = new SqlConnection(_cn);
        return await db.QueryAsync<PrestamoReporte>("usp_Prestamos_ReportePorFechas",
            new { Desde = desde.Date, Hasta = hasta.Date }, commandType: CommandType.StoredProcedure);
    }
}