﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Application.RabbitMq.Interfaces
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
