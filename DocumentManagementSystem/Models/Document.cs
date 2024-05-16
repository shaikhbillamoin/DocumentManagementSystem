using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DocumentManagementSystem.Models
{
    public class Document : BaseEntity
    {        
        public required string Subject { get; set; }
        public DocumentType DocumentTypeID { get; set; }
        public DateTime DocumentDate { get; set; }
        public required string SerialNumber { get; set; }
        public string Year { get; set; }
        public string? Remarks { get; set; }
        public DocumentContent DocumentContent { get; set; }
    }
    public class DocumentContent : BaseEntity
    {
        public int DocumentID { get; set; }
        public byte[]? filecontent { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
    public class BaseEntity
    {
        public int ID { get; set; }
        public required string UserName { get; set; }
        public required DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public enum DocumentType
    {
        Incoming = 1,
        Outgoing = 2,
    }
}