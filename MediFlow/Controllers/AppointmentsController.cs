using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Appointments.Commands;
using MediFlow.Application.UseCases.Appointments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
    {
        var appointments = await sender.Send(
            new GetAllAppointmentsQuery());

        return Ok(appointments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id)
    {
        var appointment = await sender.Send(
            new GetAppointmentByIdQuery(id));

        if (appointment is null)
            return NotFound(new { message = "Cita no encontrada." });

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Create(
        [FromBody] CreateAppointmentDto dto)
    {
        var appointment = await sender.Send(
            new CreateAppointmentCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = appointment.AppointmentId },
            appointment);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateAppointmentDto dto)
    {
        var updated = await sender.Send(
            new UpdateAppointmentCommand(id, dto));

        if (!updated)
            return NotFound(new { message = "Cita no encontrada." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cancelled = await sender.Send(
            new DeleteAppointmentCommand(id));

        if (!cancelled)
            return NotFound(new { message = "Cita no encontrada." });

        return NoContent();
    }
}