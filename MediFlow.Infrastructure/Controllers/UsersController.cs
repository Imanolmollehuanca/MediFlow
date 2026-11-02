using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Users;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Infrastructure.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(
    CreateUserUseCase createUser,
    GetUserByIdUseCase getUserById,
    GetAllUsersUseCase getAllUsers,
    UpdateUserUseCase updateUser,
    DeleteUserUseCase deleteUser,
    AssignRoleToUserUseCase assignRole,
    RemoveRoleFromUserUseCase removeRole) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken ct) =>
        Ok(await getAllUsers.ExecuteAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetById(int id, CancellationToken ct) =>
        Ok(await getUserById.ExecuteAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request, CancellationToken ct)
    {
        var created = await createUser.ExecuteAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserResponse>> Update(int id, UpdateUserRequest request, CancellationToken ct) =>
        Ok(await updateUser.ExecuteAsync(id, request, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await deleteUser.ExecuteAsync(id, ct);
        return NoContent();
    }

    [HttpPost("{id:int}/roles/{roleId:int}")]
    public async Task<ActionResult<UserResponse>> AssignRole(int id, int roleId, CancellationToken ct) =>
        Ok(await assignRole.ExecuteAsync(id, roleId, ct));

    [HttpDelete("{id:int}/roles/{roleId:int}")]
    public async Task<IActionResult> RemoveRole(int id, int roleId, CancellationToken ct)
    {
        await removeRole.ExecuteAsync(id, roleId, ct);
        return NoContent();
    }
}