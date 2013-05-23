using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ReferEngine.Common.Data
{
    public class Queue
    {
        public string Name { get; set; }
        private NamespaceManager NamespaceManager { get; set; }
        private MessagingFactory MessagingFactory { get; set; }
        public Type SupportedType { get; private set; }
        private QueueClient Client { get; set; }
        private QueueClient DeadLetterClient { get; set; }
        private QueueDescription QueueDescription { get { return NamespaceManager.GetQueue(Client.Path); } }
        private const int MaximumNumberOfTries = 5;

        public Queue(NamespaceManager namespaceManager, MessagingFactory messagingFactory, string queueName, Type supportedType)
        {
            NamespaceManager = namespaceManager;
            MessagingFactory = messagingFactory;
            Name = queueName;
            SupportedType = supportedType;
            CreateIfNeeded();
            Client = MessagingFactory.CreateQueueClient(Name);
            DeadLetterClient = MessagingFactory.CreateQueueClient(Name + "/$DeadLetterQueue");
        }

        public void EnqueueBatch(IEnumerable<Object> objs)
        {
            var array = objs.ToArray();
            if (array.First().GetType() == SupportedType)
            {
                for (int i = 0; i < MaximumNumberOfTries; i++)
                {
                    try
                    {
                        var messages = array.Select(x => new BrokeredMessage(x)).ToArray();
                        Client.SendBatch(messages);
                        break;
                    }
                    catch (MessagingException e)
                    {
                        if (!e.IsTransient) throw;
                    }
                }
            }
        }

        public void Enqueue(Object obj)
        {
            if (obj.GetType() == SupportedType)
            {
                for (int i = 0; i < MaximumNumberOfTries; i++)
                {
                    try
                    {
                        var message = new BrokeredMessage(obj);
                        Client.Send(message);
                        break;
                    }
                    catch (MessagingException e)
                    {
                        if (!e.IsTransient) throw;
                    }
                }
            }
        }

        public BrokeredMessage Receive()
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(0);
            BrokeredMessage message = null;
            for (int i = 0; i < MaximumNumberOfTries; i++)
            {
                try
                {
                    message = Client.Receive(timeSpan) ?? DeadLetterClient.Receive(timeSpan);
                    break;
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient) throw;
                }
            }

            if (message != null)
            {
                message.ContentType = SupportedType.ToString();
            }
            return message;
        }

        public IEnumerable<BrokeredMessage> ReceiveBatch(int count)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(0);
            IEnumerable<BrokeredMessage> result = null;
            for (int i = 0; i < MaximumNumberOfTries; i++)
            {
                try
                {
                    result = Client.ReceiveBatch(count, timeSpan) ?? DeadLetterClient.ReceiveBatch(count, timeSpan);
                    break;
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient) throw;
                }
            }
            return result;
        }

        public long GetMessageCount()
        {
            return QueueDescription.MessageCount;
        }

        private void CreateIfNeeded()
        {
            if (NamespaceManager.QueueExists(Name))
            {
                return;
            }
            try
            {
                QueueDescription queueDescription = new QueueDescription(Name)
                    {
                        LockDuration = TimeSpan.FromMinutes(5),
                        MaxSizeInMegabytes = 1024*5
                    };
                NamespaceManager.CreateQueue(queueDescription);
            }
            catch (MessagingEntityAlreadyExistsException)
            {
                // already exists, good
            }
        }
    }
}
