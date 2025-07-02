using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.Interfaces.Messaging;
public interface IKafkaProducerService
{
    Task ProduceAsync<T>(string topic, T message);
}