using Microsoft.AspNetCore.Mvc;
using Turnos.Data;
using Turnos.Models;
using System;
using System.Collections.Generic;

namespace Turnos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly IProcedimientoAlmacenadoService _procedimientoAlmacenadoService;

        public TurnosController(IProcedimientoAlmacenadoService procedimientoAlmacenadoService)
        {
            _procedimientoAlmacenadoService = procedimientoAlmacenadoService;
        }

        [HttpPost("GenerarTurnos")]
        public IActionResult GenerarTurnos([FromBody] TurnosRequest request)
        {
            
            if (!ValidarFechas(request.FechaInicio, request.FechaFin))
            {
                return BadRequest("Fechas inválidas");
            }

            IEnumerable<TurnoGenerado> turnosGenerados = _procedimientoAlmacenadoService.GenerarTurnos(request.FechaInicio, request.FechaFin, request.IdServicio);

            return Ok(turnosGenerados);
        }

        // validación de fechas
        private bool ValidarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
           
            return fechaInicio < fechaFin && fechaInicio >= DateTime.Now.Date;
        }
    }
}
