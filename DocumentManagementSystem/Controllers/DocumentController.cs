using DocumentManagementSystem.Data;
using DocumentManagementSystem.Helpers;
using DocumentManagementSystem.Models;
using DocumentManagementSystem.Repository;
using DocumentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;

namespace DocumentManagementSystem.Controllers
{
    public class DocumentController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IDocumentRepository _documentRepository;
        public DocumentController(IDocumentRepository documentRepository, ApplicationDbContext context)
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
            return View(documentvmList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var document = await _documentRepository.GetByIdAsync(id.Value);
            if (document == null)
            {
                return NotFound();
            }
            var documentvm = new DocumentVM
            {
                ID = document.ID,
                Subject = document.Subject,
                DocumentTypeName = ((DocumentType)document.DocumentTypeID).ToString(),
                DocumentTypeID = (int)document.DocumentTypeID,
                DocumentDate = document.DocumentDate,
                filecontent = document.DocumentContent != null ? document.DocumentContent.filecontent : null,
                Remarks = document.Remarks,
                SerialNumber = document.SerialNumber,
                Year = document.Year,
                CreatedDate = document.CreatedDate.ToShortDateString(),
                UpdatedDate = document.UpdatedDate.HasValue ? document.UpdatedDate.Value.ToShortDateString() : "",
                UserName = document.UserName,
                FileName = document.DocumentContent != null ? document.DocumentContent.FileName : null
            };
            return View(documentvm);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Subject,DocumentTypeID,DocumentDate,InputFile,Remarks,SerialNumber,Year")] DocumentVM documentvm)
        {
            if (ModelState.IsValid)
            {
                byte[]? filebytesArray = null;
                if (documentvm.InputFile != null)
                {
                    filebytesArray = Helper.GetByteArrayFromImage(documentvm.InputFile);

                    var document = new Document()
                    {
                        Subject = documentvm.Subject,
                        DocumentTypeID = (DocumentType)documentvm.DocumentTypeID,
                        DocumentDate = documentvm.DocumentDate.Value,
                        DocumentContent = new DocumentContent() { filecontent = filebytesArray, UserName = User.Identity?.Name, CreatedDate = DateTime.Now, FileName = documentvm.InputFile.FileName, ContentType = documentvm.InputFile.ContentType },
                        Remarks = documentvm.Remarks,
                        SerialNumber = documentvm.SerialNumber,
                        Year = documentvm.Year,
                        UserName = User.Identity?.Name,
                        CreatedDate = DateTime.Now,
                    };
                    await _documentRepository.InsertAsync(document);
                    await _documentRepository.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(documentvm);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var document = await _documentRepository.GetByIdAsync(Convert.ToInt32(id));
            if (document == null)
            {
                return NotFound();
            }
            var documentvm = new DocumentVM
            {
                ID = document.ID,
                Subject = document.Subject,
                DocumentTypeName = ((DocumentType)document.DocumentTypeID).ToString(),
                DocumentTypeID = (int)document.DocumentTypeID,
                DocumentDate = document.DocumentDate,
                filecontent = document.DocumentContent != null ? document.DocumentContent.filecontent : null,
                Remarks = document.Remarks,
                SerialNumber = document.SerialNumber,
                Year = document.Year,
                FileName = document.DocumentContent != null ? document.DocumentContent.FileName : null
            };
            return View(documentvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Subject,DocumentTypeID,DocumentDate,InputFile,Remarks,SerialNumber,Year,IsFileUpload")] DocumentVM documentvm)
        {
            if (documentvm.IsFileUpload.HasValue && !documentvm.IsFileUpload.Value)
                ModelState.Remove("InputFile");
            if (ModelState.IsValid)
            {
                try
                {
                    var document = await _documentRepository.GetByIdAsync(Convert.ToInt32(id));
                    if (document == null)
                    {
                        return NotFound();
                    }

                    byte[]? filebytesArray = null;
                    if (documentvm.IsFileUpload.HasValue && documentvm.IsFileUpload.Value)
                    {
                        filebytesArray = Helper.GetByteArrayFromImage(documentvm.InputFile);
                    }
                    document.Subject = documentvm.Subject;
                    document.DocumentTypeID = (DocumentType)documentvm.DocumentTypeID;
                    document.DocumentDate = documentvm.DocumentDate.Value;
                    if (document.DocumentContent != null)
                    {
                        document.DocumentContent.filecontent = filebytesArray != null ? filebytesArray : document.DocumentContent.filecontent;
                        document.DocumentContent.UserName = User.Identity?.Name;
                        document.DocumentContent.UpdatedDate = DateTime.Now;
                        document.DocumentContent.FileName = filebytesArray != null ? documentvm.InputFile.FileName : document.DocumentContent.FileName;
                        document.DocumentContent.ContentType = filebytesArray != null ? documentvm.InputFile.ContentType : document.DocumentContent.ContentType;
                    }
                    else
                        document.DocumentContent = new DocumentContent() { filecontent = filebytesArray != null ? filebytesArray : document.DocumentContent.filecontent, UserName = User.Identity?.Name, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, FileName = filebytesArray != null ? documentvm.InputFile.FileName : document.DocumentContent.FileName, ContentType = filebytesArray != null ? documentvm.InputFile.ContentType : document.DocumentContent.ContentType };
                    document.Remarks = documentvm.Remarks;
                    document.SerialNumber = documentvm.SerialNumber;
                    document.Year = documentvm.Year;
                    document.UserName = User.Identity?.Name;
                    document.UpdatedDate = DateTime.Now;

                    await _documentRepository.UpdateAsync(document);
                    await _documentRepository.SaveAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    var doc = await _documentRepository.GetByIdAsync(documentvm.ID);
                    if (doc == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(documentvm);
        }
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document != null)
            {
                await _documentRepository.DeleteAsync(id);
                await _documentRepository.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var attachment = await _documentRepository.GetByIdAsync(id);
            return new FileContentResult(attachment.DocumentContent.filecontent, attachment.DocumentContent.ContentType)
            {
                FileDownloadName = $"{attachment.DocumentContent.FileName}"
            };
        }
    }
}