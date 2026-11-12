using MediatR;
using Microsoft.AspNetCore.Mvc;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.MedicalRecords.Commands;
using MediFlow.Application.UseCases.MedicalRecords.Queries;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await mediator.Send(new GetMedicalRecordsQuery());
        return Ok(records);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await mediator.Send(new GetMedicalRecordByIdQuery(id));
        if (record is null)
            return NotFound();
        return Ok(record);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicalRecordDto dto)
    {
        try
        {
            var id = await mediator.Send(new CreateMedicalRecordCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateMedicalRecordDto dto)
    {
        var updated = await mediator.Send(new UpdateMedicalRecordCommand(id, dto));
        if (!updated)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await mediator.Send(new DeleteMedicalRecordCommand(id));
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}