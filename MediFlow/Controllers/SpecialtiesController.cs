
using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Specialties.Commands;
using MediFlow.Application.UseCases.Specialties.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly ISender _mediator;
    public SpecialtiesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var specialties = await _mediator.Send(
            new GetSpecialtiesQuery());
        return Ok(specialties);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var specialty = await _mediator.Send(new GetSpecialtyByIdQuery(id));
        if (specialty is null)
            return NotFound();
        return Ok(specialty);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSpecialtyDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("El nombre es obligatorio.");
        
        var command = new CreateSpecialtyCommand(dto.Name, dto.Description);
        var specialtyId = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id = specialtyId },
            new { SpecialtyId = specialtyId });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateSpecialtyDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("El nombre es obligatorio.");
        var command = new UpdateSpecialtyCommand(id, dto.Name, dto.Description);
        var updated = await _mediator.Send(command);
        
        if (!updated)
            return NotFound();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteSpecialtyCommand(id));
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
