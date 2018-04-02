//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqlAdapter
{
    using System;
    using Application;
    using System.Threading.Tasks;
    using Domain;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    
    
    public class HangfireQueue : IHangfireQueue
    {
        
        public EventJobRegistration RegisteredJobs { get; private set; }
        
        public IQueueRepository QueueRepository { get; private set; }
        
        public HangfireQueue(EventJobRegistration RegisteredJobs, IQueueRepository QueueRepository)
        {
            this.RegisteredJobs = RegisteredJobs;
            this.QueueRepository = QueueRepository;
        }
        
        public async Task AddEvents(List<DomainEventBase> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var jobsTriggereByEvent = RegisteredJobs.EventJobs.Where(tuple => domainEvent.GetType().ToString() == tuple.DomainType);
                foreach (var job in jobsTriggereByEvent)
                {
                    var combination = new EventAndJob(domainEvent, job.JobName);
                    await QueueRepository.AddEventForJob(combination);
                }
            };
        }
        
        public async Task<List<EventAndJob>> GetEvents( string jobName)
        {
            var eventList = await QueueRepository.GetEvents(jobName);
            return eventList;
        }
        
        public async Task RemoveEventsFromQueue(List<EventAndJob> handledEvents)
        {
            await QueueRepository.RemoveEventsFromQueue(handledEvents);
        }
    }
}
