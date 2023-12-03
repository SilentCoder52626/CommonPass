using ClosedXML.Excel;
using DomainModule.Dto.Pass;
using DomainModule.Entity.Pass;
using DomainModule.Enums;
using DomainModule.Exceptions;
using DomainModule.RepositoryInterface;
using DomainModule.RepositoryInterface.Pass;
using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NToastNotify;
using System.Data;
using WebApp.Extensions;
using WebApp.Helper;

namespace WebApp.Areas.Pass.Controllers
{
    [Area("Pass")]
    public class AccountsController : Controller
    {
        private readonly IAccountDetailsRepository _accountRepo;
        private readonly AppSettingsRepositoryInterface _appSettingRepo;
        private readonly IToastNotification _notify;

        public AccountsController(IAccountDetailsRepository accountRepo, IToastNotification notify, AppSettingsRepositoryInterface appSettingRepo)
        {
            _accountRepo = accountRepo;
            _notify = notify;
            _appSettingRepo = appSettingRepo;
        }

        [Authorize("Accounts-View")]
        public IActionResult Index()
        {
            var userId = GetCurrentUserExtension.GetCurrentUserId(this);
            var model = _accountRepo.GetAccountDetailsModel(userId);
            return View(model);
        }
        [Authorize("Accounts-AddOrUpdate")]
        public IActionResult AddorUpdate(int? id)
        {
            try
            {
                var dto = new AccountDetailsDto();
                if(id.GetValueOrDefault() > 0)
                {
                    var entity = _accountRepo.GetById(id.GetValueOrDefault()) ?? throw new CustomException("Account details not found.");
                    dto.Id = entity.Id;
                    dto.UserId = entity.UserId;
                    dto.UserName = entity.Name;
                    dto.Account = entity.Account;
                    dto.Pass = AccountDetails.DefaultPasswordString;
                }
                return PartialView("Partial/_AddOrUpdate", dto);
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize("Accounts-Export")]
        [HttpGet]
        public IActionResult ExportToExcel()
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                var accountExportDto = _accountRepo.GetAccountDetailsWithDecryptedData(userId);
                var ExcelPin = _appSettingRepo.GetByKey(AppSettingsEnum.ExportPin.ToString(), userId)?.Value;
                if (String.IsNullOrEmpty(ExcelPin))
                {
                    throw new CustomException("Please set Pin in Appsetting.");
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("AccountDetails");

                    // Set the height of the first row to 35 pixels for the company name
                    worksheet.Row(1).Height = 25;

                    // Merge cells for the company name
                    var companyCell = worksheet.Cell(1, 1);
                    companyCell.Style.Font.Bold = true;
                    companyCell.Style.Font.FontSize = 25;

                    companyCell.Value = accountExportDto.Company;
                    worksheet.Range(companyCell, worksheet.Cell(1, accountExportDto.DataTable.Columns.Count)).Merge();

                    // Set the styling for the merged company cell
                    companyCell.Style.Font.Bold = true;
                    companyCell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                    companyCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Set the styling for the customer details row
                    
                    for (int rowIndex = 2; rowIndex <= 3; rowIndex++)
                    {
                        var detailsRow = worksheet.Row(rowIndex);
                        detailsRow.Height = 20;
                        detailsRow.Style.Font.Bold = true;
                    }
                    // Apply background color only to the columns with data in row 2
                    for (int colIndex = 1; colIndex <= accountExportDto.DataTable.Columns.Count; colIndex++)
                    {
                        worksheet.Cell(2, colIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                        worksheet.Cell(3, colIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }


                    // Populate customer details in separate cells
                    worksheet.Cell(2, 1).Value = "Name";
                    worksheet.Cell(2, 2).Value = accountExportDto.Name;
                    worksheet.Cell(2, 3).Value = "Phone";
                    worksheet.Cell(2, 4).Value = accountExportDto.Phone;
                    worksheet.Cell(3, 1).Value = "Email";
                    worksheet.Cell(3, 2).Value = accountExportDto.Email;

                    // Add additional information
                    var infoTextCell = worksheet.Cell(5, 1);
                    infoTextCell.Value = accountExportDto.InfoText;
                    infoTextCell.Style.Font.Italic = true;
                    infoTextCell.Style.Alignment.WrapText = true;
                    infoTextCell.Style.Font.FontColor = XLColor.Red; // Set text color to red

                    // Merge cells for the info text
                    worksheet.Range(infoTextCell, worksheet.Cell(6, accountExportDto.DataTable.Columns.Count)).Merge();

                    // Set starting row for DataTable data
                    int dataStartRow = 7;


                    // Set DataTable styling
                    worksheet.Cell(dataStartRow, 1).InsertTable(accountExportDto.DataTable.AsEnumerable(), true);

                    // Set column width dynamically based on content
                    worksheet.Columns().AdjustToContents();

                    workbook.Protect(ExcelPin).DisallowElement(XLWorkbookProtectionElements.Everything);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Seek(0, SeekOrigin.Begin);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Account-Details-CommonPass.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                _notify.AddErrorToastMessage(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
