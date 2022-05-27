using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            ICategoryRepository = new CategoryRepository(_dbContext);
            ICoverRepository = new CoverRepository(_dbContext);
        }
        public ICategoryRepository ICategoryRepository { get; private set; }

        public ICoverRepository ICoverRepository { get; private set; }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
