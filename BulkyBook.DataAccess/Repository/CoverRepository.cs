using BukbyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CoverRepository : Repository<Cover>, ICoverRepository
    {
        public readonly ApplicationDbContext _dbContext;
        public CoverRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Cover cover)
        {
            _dbContext.Update(cover);
        }
    }
}
