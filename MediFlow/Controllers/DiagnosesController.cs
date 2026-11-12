using MediatR;
using Microsoft.AspNetCore.Mvc;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Diagnoses.Commands;
using MediFlow.Application.UseCases.Diagnoses.Queries;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosesController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var diagnoses = await mediator.Send(new GetDiagnosesQuery());

        return Ok(diagnoses);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var diagnosis = await mediator.Send(new GetDiagnosisByIdQuery(id));

        if (diagnosis is null)
            return NotFound();

        return Ok(diagnosis);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDiagnosisDto dto)
    {
        try
        {
            var id = await mediator.Send(new CreateDiagnosisCommand(dto));

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDiagnosisDto dto)
    {
        try
        {
            var updated = await mediator.Send(new UpdateDiagnosisCommand(id, dto));

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
        var deleted = await mediator.Send(new DeleteDiagnosisCommand(id));

        if (!deleted)
            return NotFound();
        return NoContent();
    }
}