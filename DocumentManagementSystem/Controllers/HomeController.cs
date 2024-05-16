using DocumentManagementSystem.Data;
using DocumentManagementSystem.Models;
using DocumentManagementSystem.Repository;
using DocumentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DocumentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDocumentRepository _documentRepository;
        public HomeController(IDocumentRepository documentRepository, ApplicationDbContext context)
        {
            _documentRepository = documentRepository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var documents = await _documentRepository.GetAllAsync();
            var documentvmList = documents.Select(x => new DocumentVM
            {
                ID = x.ID,
                Subject = x.Subject,
                DocumentTypeName = ((DocumentType)x.DocumentTypeID).ToString(),
                DocumentTypeID = (int)x.DocumentTypeID,
                DocumentDate = x.DocumentDate,
                filecontent = x.DocumentContent != null ? x.DocumentContent.filecontent : null,
                Remarks = x.Remarks,
                SerialNumber = x.SerialNumber,
                Year = x.Year,
                CreatedDate = x.CreatedDate.ToShortDateString(),
                UpdatedDate = x.UpdatedDate.HasValue ? x.UpdatedDate.Value.ToShortDateString() : "",
                UserName = x.UserName,
                FileName = x.DocumentContent != null ? x.DocumentContent.FileName : null
            });
            ViewBag.ImageCount = documentvmList.Count(x => x.fileExtension != null && (x.fileExtension == FileExtension.png || x.fileExtension == FileExtension.jpg || x.fileExtension == FileExtension.jpeg));
            ViewBag.PDFCount = documentvmList.Count(x => x.fileExtension != null && x.fileExtension == FileExtension.pdf);
            ViewBag.ExcelCount = documentvmList.Count(x => x.fileExtension != null && (x.fileExtension == FileExtension.xlsx || x.fileExtension == FileExtension.xls || x.fileExtension == FileExtension.csv));
            ViewBag.WordCount = documentvmList.Count(x => x.fileExtension != null && (x.fileExtension == FileExtension.docx || x.fileExtension == FileExtension.doc));

            return View(documentvmList);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
