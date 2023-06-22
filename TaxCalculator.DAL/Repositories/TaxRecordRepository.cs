using Microsoft.EntityFrameworkCore;
using TaxCalculator.DAL.Context;
using TaxCalculator.DAL.Entities;
using TaxCalculator.DAL.Interfaces;
using TaxCalculator.DAL.Models;

namespace TaxCalculator.DAL.Repositories
{
    public class TaxRecordRepository : ITaxRecordRepository
    {
        private readonly TaxCalculatorDbContext _context;

        public TaxRecordRepository(TaxCalculatorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaxRecord>> GetAllTaxRecordsAsync()
        {
            return await _context.TaxRecords.ToListAsync();
        }

        public async Task<TaxRecord> GetTaxRecordByIdAsync(int id)
        {
            return await _context.TaxRecords.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaxRecord> AddTaxCalculationAsync(TaxRecord taxRecord)
        {
            taxRecord.CreatedAt= DateTime.Now;

            _context.TaxRecords.Add(taxRecord);
            await _context.SaveChangesAsync();

            return taxRecord;
        }

        public async Task UpdateTaxRecordAsync(TaxRecord taxRecord)
        {
            _context.Entry(taxRecord).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaxRecordAsync(TaxRecord taxRecord)
        {
            _context.TaxRecords.Remove(taxRecord);
            await _context.SaveChangesAsync();
        }
    }
}