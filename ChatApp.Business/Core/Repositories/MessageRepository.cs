using ChatApp.Business.Core.Interfaces;
using ChatApp.Business.Repository;
using ChatApp.DataAccess.DBContext;
using ChatApp.DataAccess.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Business.Core.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Message>> GetAllMessageAsync()
        {
            return await GetAll().ToListAsync();
        }
    }
}
