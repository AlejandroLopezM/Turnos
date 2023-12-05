using Xunit;
using Turnos.Controllers;
using Turnos.Models;
using Turnos.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace TurnosTest
{
    public class SimulatedProcedimientoAlmacenadoService : IProcedimientoAlmacenadoService
    {
        public IEnumerable<TurnoGenerado> GenerarTurnos(DateTime fechaInicio, DateTime fechaFin, int idServicio)
        {
            return new List<TurnoGenerado>(); // Devolver datos simulados
        }
    }

}
