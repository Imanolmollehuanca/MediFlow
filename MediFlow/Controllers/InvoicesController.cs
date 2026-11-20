using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Invoices.Commands;
using MediFlow.Application.UseCases.Invoices.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/invoices")]
[Authorize(Roles = "Admin")]
public class InvoicesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await mediator.Send(new GetAllInvoicesQuery()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var invoice = await mediator.Send(
            new GetInvoiceByIdQuery(id));

        return invoice is null
            ? NotFound()
            : Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceDto dto)
    {
        var invoice = await mediator.Send(
            new CreateInvoiceCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = invoice.InvoiceId },
            invoice);
    }
}