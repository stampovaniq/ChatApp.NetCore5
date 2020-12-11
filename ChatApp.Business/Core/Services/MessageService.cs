using ChatApp.DataAccess.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Business.Common.Enums;
using ChatApp.Business.Core.Interfaces;

namespace ChatApp.Business.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly  IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> AddMessageAsync(dynamic newMessage)
        {
            Message m = new Message()
            {
                CreatedBy = newMessage.CreatedBy, DateCreated = DateTime.Now, MessageBody = newMessage.MessageBody,
                StatusTypeId = (int) EnumMessageStatusType.Sent
            };
            return await _messageRepository.AddAsync(m);
        }

        public async Task<List<Message>> GetAllMessageAsync()
        {
            return await _messageRepository.GetAllMessageAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _messageRepository.GetAsync(id);
        }
    }
}
