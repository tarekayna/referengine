using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AppSmarts.Common.Tracing;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AppSmarts.Common.Data
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
                        SleepForOneSecond();
                    }
                    catch (TimeoutException e)
                    {
                        Tracer.Trace(TraceMessage.Warning(e.Message));
                        SleepForOneSecond();
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
                        SleepForOneSecond();
                    }
                    catch (TimeoutException e)
                    {
                        Tracer.Trace(TraceMessage.Warning(e.Message));
                        SleepForOneSecond();
                    }
                }
            }
        }

        public bool IsDisabled()
        {
            return QueueDescription.Status == EntityStatus.Disabled;
        }

        public void CompleteBatch(BrokeredMessage[] messages)
        {
            foreach (var brokeredMessage in messages)
            {
                for (int i = 0; i < MaximumNumberOfTries; i++)
                {
                    try
                    {
                        brokeredMessage.Complete();
                        break;
                    }
                    catch (MessagingException e)
                    {
                        if (!e.IsTransient) throw;
                        SleepForOneSecond();
                    }
                    catch (TimeoutException e)
                    {
                        Tracer.Trace(TraceMessage.Warning(e.Message));
                        SleepForOneSecond();
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
                    SleepForOneSecond();
                }
                catch (TimeoutException e)
                {
                    Tracer.Trace(TraceMessage.Warning(e.Message));
                    SleepForOneSecond();
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
            BrokeredMessage[] result = null;
            for (int i = 0; i < MaximumNumberOfTries; i++)
            {
                try
                {
                    result = Client.ReceiveBatch(count, timeSpan).ToArray();
                    if (!result.Any())
                    {
                        result = DeadLetterClient.ReceiveBatch(count, timeSpan).ToArray();
                    }
                    break;
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient) throw;
                    SleepForOneSecond();
                }
                catch (TimeoutException e)
                {
                    Tracer.Trace(TraceMessage.Warning(e.Message));
                    SleepForOneSecond();
                }
            }
            return result;
        }

        public long GetMessageCount()
        {
            return QueueDescription.MessageCount;
        }

        public bool HasMessages()
        {
            return GetMessageCount() > 0;
        }

        public bool IsEmpty()
        {
            return GetMessageCount() == 0;
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

        private void SleepForOneSecond()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
}
