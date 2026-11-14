using ClosedXML.Excel;
using MediFlow.Application.Port.Services;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Services.Implementation;

public class ExcelReportService : IExcelReportService
{
    private readonly MediFlowContext _context;

    public ExcelReportService(MediFlowContext context)
    {
        _context = context;
    }

    public async Task<byte[]> GeneratePatientsReportAsync()
    {
        var patients = await _context.Patients
            .AsNoTracking()
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Pacientes");

        sheet.Cell(1, 1).Value = "ID";
        sheet.Cell(1, 2).Value = "Paciente";

        var row = 2;

        foreach (var patient in patients)
        {
            sheet.Cell(row, 1).Value = patient.PatientId;
            sheet.Cell(row, 2).Value = patient.ToString();
            row++;
        }

        sheet.Range(1, 1, 1, 2).Style.Font.Bold = true;
        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}