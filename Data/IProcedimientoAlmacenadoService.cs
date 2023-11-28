using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Turnos.Models;

namespace Turnos.Data
{
    public interface IProcedimientoAlmacenadoService
    {
        IEnumerable<TurnoGenerado> GenerarTurnos(DateTime fechaInicio, DateTime fechaFin, int idServicio);
    }

    public class ProcedimientoAlmacenadoService : IProcedimientoAlmacenadoService
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public ProcedimientoAlmacenadoService(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<TurnoGenerado> GenerarTurnos(DateTime fechaInicio, DateTime fechaFin, int idServicio)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("GenerarTurnos", (SqlConnection)connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);
                    command.Parameters.AddWithValue("@IdServicio", idServicio);

                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var turnosGenerados = new List<TurnoGenerado>();

                            while (reader.Read())
                            {
                                var turno = new TurnoGenerado
                                {
                                    IdTurno = reader["IdTurno"] != DBNull.Value ? (int)reader["IdTurno"] : 0,
                                    IdServicio = reader["IdServicio"] != DBNull.Value ? (int)reader["IdServicio"] : 0,
                                    FechaTurno = reader["FechaTurno"] != DBNull.Value ? (DateTime)reader["FechaTurno"] : DateTime.MinValue,
                                    HoraInicio = reader["HoraInicio"] != DBNull.Value ? (TimeSpan)reader["HoraInicio"] : TimeSpan.MinValue,
                                    HoraFin = reader["HoraFin"] != DBNull.Value ? (TimeSpan)reader["HoraFin"] : TimeSpan.MinValue,
                                    Estado = reader["Estado"] != DBNull.Value ? (string)reader["Estado"] : null
                                };

                                turnosGenerados.Add(turno);
                            }

                            return turnosGenerados;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al ejecutar el procedimiento almacenado: {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
