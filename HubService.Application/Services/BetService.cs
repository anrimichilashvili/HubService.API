using HubService.Application.Dtos;
using HubService.Application.Interfaces.Repositories;
using HubService.Application.Interfaces.Services;
using HubService.Application.Messaging;
using HubService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Application.Services
{
    public class BetService : IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly IMessageProducer _messageProducer;
        public BetService(IBetRepository betRepository, IMessageProducer messageProducer)
        {
            _betRepository = betRepository;
            _messageProducer = messageProducer;
        }

        public async Task<ResultDTO> CreateBetAsync(decimal amount)
        {
            try
            {
                var bet = new BetEntity
                {
                    Amount = amount,
                    PlacedAt = DateTime.UtcNow
                };

                var result = await _betRepository.CreateAsync(bet);
                _messageProducer.SendMessage(result);
                return new ResultDTO { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ResultDTO { Success = false, Data = ex.Message };
            }
        }
    }
}
