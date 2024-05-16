using DocumentManagementSystem.Data;
using DocumentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagementSystem.Repository
{
    public class DocumentRepository : IDocumentRepository
    {

        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Document.Include(x => x.DocumentContent).ToListAsync();
        }

        public async Task<Document?> GetByIdAsync(int DocumentID)
        {
            var document = await _context.Document
                .Include(x => x.DocumentContent)
               .FirstOrDefaultAsync(m => m.ID == DocumentID);
            return document;
        }
        public async Task InsertAsync(Document document)
        {
            await _context.Document.AddAsync(document);
        }
        public async Task UpdateAsync(Document document)
        {
            _context.Document.Update(document);
        }
        public async Task DeleteAsync(int documentId)
        {
            var document = await _context.Document.Include(x => x.DocumentContent).FirstOrDefaultAsync(m => m.ID == documentId);
            if (document != null)
            {
                _context.Document.Remove(document);
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}