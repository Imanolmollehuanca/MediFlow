using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Payments.Commands;
using MediFlow.Application.UseCases.Payments.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize(Roles = "Admin")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await mediator.Send(new GetAllPaymentsQuery()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await mediator.Send(
            new GetPaymentByIdQuery(id));

        return payment is null
            ? NotFound()
            : Ok(payment);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePaymentDto dto)
    {
        var payment = await mediator.Send(
            new CreatePaymentCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = payment.PaymentId },
            payment);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdatePaymentStatusDto dto)
    {
        var payment = await mediator.Send(
            new UpdatePaymentStatusCommand(id, dto));

        return payment is null
            ? NotFound()
            : Ok(payment);
    }
}