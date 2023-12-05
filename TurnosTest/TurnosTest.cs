using Xunit;
using Turnos.Controllers;
using Turnos.Models;
using Turnos.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace TurnosTest
{
    public class TurnosTest
    {
        private readonly TurnosController _controller;
        private readonly IProcedimientoAlmacenadoService _procedimientoAlmacenadoService;

        public TurnosTest()
        {
            _procedimientoAlmacenadoService = new SimulatedProcedimientoAlmacenadoService();
            _controller = new TurnosController(_procedimientoAlmacenadoService);
        }

        [Fact]
        public void TestGenerarTurnos_ValidRequest_ReturnsOk()
        {
            // Arrange
            var validRequest = new TurnosRequest
            {
                FechaInicio = DateTime.Now.Date,
                FechaFin = DateTime.Now.Date.AddDays(1),
                IdServicio = 1 
            };

            // Act
            var result = _controller.GenerarTurnos(validRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);

            var body = result.Value as IEnumerable<TurnoGenerado>;
          
            Assert.Empty(body);
            
           
            if (body != null && body.Any())
            {
                foreach (var turno in body)
                {
                    Assert.InRange(turno.IdTurno, 1, int.MaxValue); // Verificar si el IdTurno es válido (mayor que cero)
                    Assert.NotEqual(DateTime.MinValue, turno.FechaTurno); // Verificar si la fecha es distinta de DateTime.MinValue
                    Assert.NotEqual(TimeSpan.MinValue, turno.HoraInicio); // Verificar si la hora de inicio es distinta de TimeSpan.MinValue
                    Assert.NotEqual(TimeSpan.MinValue, turno.HoraFin); // Verificar si la hora de fin es distinta de TimeSpan.MinValue
                    Assert.NotNull(turno.Estado); // Verificar si el estado no es nulo
                    Assert.NotEmpty(turno.Estado); // Verificar si el estado no está vacío
                    Assert.True(turno.Estado.Length <= 50); // Verificar la longitud máxima del estado, por ejemplo

                   
                }
            }
        }
    }
}
