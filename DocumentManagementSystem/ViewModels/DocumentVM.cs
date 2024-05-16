using DocumentManagementSystem.CustomAttribute;
using DocumentManagementSystem.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DocumentManagementSystem.ViewModels
{
    public class DocumentVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter a Subject."), DisplayName("Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please select a Document Type."), DisplayName("Document Type")]
        public int? DocumentTypeID { get; set; }
        [DisplayName("Document Type Name")]
        public string? DocumentTypeName { get; set; }
        [Required(ErrorMessage = "Please select a Document date."), DisplayName("Document Date")]
        [DataType(DataType.DateTime)]
        [DocumentDate(ErrorMessage = "Document date must be less than & equal to Current date.")]
        public DateTime? DocumentDate { get; set; }
        [Required(ErrorMessage = "Please enter Serial Number."), DisplayName("Serial Number")]
        [Length(maximumLength: 2, minimumLength: 2, ErrorMessage = "Serial Number must be with exact length of 2.")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "Please enter a valid Year."), DisplayName("Year")]
        [Range(1950, 2100, ErrorMessage = "Year must be between 1950 and 2100.")]
        [Length(maximumLength: 4, minimumLength: 4, ErrorMessage = "Year must be with exact length of 4.")]
        public string Year { get; set; }
        public string? Remarks { get; set; }
        [Required(ErrorMessage = "Please select a file to upload."), DisplayName("File")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg", ".pdf", ".csv", ".xls", ".xlsx", ".doc", ".docx" })]
        public IFormFile? InputFile { get; set; }
        public byte[]? filecontent { get; set; }
        public string? FileName { get; set; }
        public string? CreatedDate { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UserName { get; set; }
        public bool? IsFileUpload { get; set; }
        public FileExtension? fileExtension
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    var extension = FileName.Split('.')[1];
                    return (FileExtension)Enum.Parse(typeof(FileExtension), extension);
                }
                return null;
            }
        }
    }
    public enum FileExtension
    { png, jpg, jpeg, pdf, csv, xls, xlsx, doc, docx }
}