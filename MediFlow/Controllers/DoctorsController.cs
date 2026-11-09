using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Doctors.Commands;
using MediFlow.Application.UseCases.Doctors.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ISender _mediator;

    public DoctorsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _mediator.Send(new GetDoctorsQuery());
        return Ok(doctors);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _mediator.Send(new GetDoctorByIdQuery(id));

        if (doctor is null)
            return NotFound();

        return Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
    {
        try
        {
            var doctorId = await _mediator.Send(new CreateDoctorCommand(dto));

            return CreatedAtAction(
                nameof(GetById),
                new { id = doctorId },
                new { DoctorId = doctorId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto)
    {
        try
        {
            var updated = await _mediator.Send(new UpdateDoctorCommand(id, dto));

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
        var deleted = await _mediator.Send(new DeleteDoctorCommand(id));

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
