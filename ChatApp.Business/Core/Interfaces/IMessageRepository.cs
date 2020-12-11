using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Business.Repository;
using ChatApp.DataAccess.Models.Entities;

namespace ChatApp.Business.Core.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<List<Message>> GetAllMessageAsync();
    }
}
