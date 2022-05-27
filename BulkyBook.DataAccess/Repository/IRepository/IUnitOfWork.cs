using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository ICategoryRepository { get; }
        public void Save();
        public ICoverRepository ICoverRepository { get; }
    }
}
