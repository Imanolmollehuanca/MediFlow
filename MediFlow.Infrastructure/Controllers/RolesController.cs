using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Roles;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Infrastructure.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController(
    CreateRoleUseCase createRole,
    GetRoleByIdUseCase getRoleById,
    GetAllRolesUseCase getAllRoles,
    UpdateRoleUseCase updateRole,
    DeleteRoleUseCase deleteRole) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoleResponse>>> GetAll(CancellationToken ct) =>
        Ok(await getAllRoles.ExecuteAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoleResponse>> GetById(int id, CancellationToken ct) =>
        Ok(await getRoleById.ExecuteAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<RoleResponse>> Create(CreateRoleRequest request, CancellationToken ct)
    {
        var created = await createRole.ExecuteAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.RoleId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoleResponse>> Update(int id, UpdateRoleRequest request, CancellationToken ct) =>
        Ok(await updateRole.ExecuteAsync(id, request, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await deleteRole.ExecuteAsync(id, ct);
        return NoContent();
    }
}