using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Appointments.Commands;
using MediFlow.Application.UseCases.Appointments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly ISender _mediator;

    public AppointmentsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _mediator.Send(
            new GetAppointmentsQuery());
        return Ok(appointments);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await _mediator.Send(new GetAppointmentByIdQuery(id));

        if (appointment is null)
            return NotFound();

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            var id = await _mediator.Send(
                new CreateAppointmentCommand(dto.PatientId, dto.DoctorId, dto.StartDateTime, dto.EndDateTime, dto.Reason));

            return CreatedAtAction(nameof(GetById),
                new { id },
                new { AppointmentId = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
    {
        try
        {
            var updated = await _mediator.Send(
                new UpdateAppointmentCommand(id, dto.PatientId, dto.DoctorId, dto.StartDateTime, dto.EndDateTime, dto.Status, dto.Reason));

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(
            new DeleteAppointmentCommand(id));

        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
