using MediFlow.Application.Port.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IExcelReportService _excelReportService;

    public ReportsController(
        IExcelReportService excelReportService)
    {
        _excelReportService = excelReportService;
    }

    [HttpGet("patients/excel")]
    public async Task<IActionResult> ExportPatients()
    {
        var bytes = await _excelReportService
            .GeneratePatientsReportAsync();

        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"MediFlow_Pacientes_{DateTime.UtcNow:yyyyMMdd}.xlsx");
    }
}