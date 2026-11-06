using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Patients.Commands;
using MediFlow.Application.UseCases.Patients.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ISender _mediator;

    public PatientsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _mediator.Send(new GetPatientsQuery());
        return Ok(patients);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _mediator.Send(
            new GetPatientByIdQuery(id));

        if (patient is null)
            return NotFound();

        return Ok(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePatientDto dto)
    {
        var patientId = await _mediator.Send(
            new CreatePatientCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = patientId },
            new { PatientId = patientId });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePatientDto dto)
    {
        var updated = await _mediator.Send(
            new UpdatePatientCommand(id, dto));

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(
            new DeletePatientCommand(id));

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}