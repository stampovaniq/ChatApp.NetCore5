using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatApp.DataAccess.Models.Entities;

namespace ChatApp.Business.Core.Services
{
    public interface IMessageService
    {
        Task<List<Message>> GetAllMessageAsync();
        Task<Message> GetMessageByIdAsync(int id);
        Task<Message> AddMessageAsync(dynamic newMessage);
    }
}
