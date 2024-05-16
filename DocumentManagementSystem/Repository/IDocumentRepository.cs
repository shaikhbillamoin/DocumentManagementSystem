using DocumentManagementSystem.Models;

namespace DocumentManagementSystem.Repository
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAllAsync();
        Task<Document?> GetByIdAsync(int documentID);
        Task InsertAsync(Document document);
        Task UpdateAsync(Document document);
        Task DeleteAsync(int documentId);
        Task SaveAsync();
    }
}