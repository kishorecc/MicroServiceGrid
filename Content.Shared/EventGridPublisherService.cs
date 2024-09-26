using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace ContentReactor.Shared
{
    public interface IEventGridPublisherService
    {
        Task PostEventGridEventAsync<T>(string type, string subject, T payload);
    }

    public class EventGridPublisherService : IEventGridPublisherService
    {
        public Task PostEventGridEventAsync<T>(string type, string subject, T payload)
        {
            // get the connection details for the Event Grid topic
            var topicEndpointUri = new Uri("https://contenttopic.southindia-1.eventgrid.azure.net/api/events");
            var topicEndpointHostname = topicEndpointUri.Host;
            var topicKey ="1CnQFVxUVTUUEuUu80gQ0jBWh2Cs2aTCXWyCzvUSK7zyD8DjpXtEJQQJ99AIAC77bzfXJ3w3AAABAZEG7usV";
            var topicCredentials = new TopicCredentials(topicKey);

            // prepare the events for submission to Event Grid
            var events = new List<Microsoft.Azure.EventGrid.Models.EventGridEvent>
            {
                new Microsoft.Azure.EventGrid.Models.EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = type,
                    Subject = subject,
                    EventTime = DateTime.UtcNow,
                    Data = payload,
                    DataVersion = "1"
                }
            };

            // publish the events
            var client = new EventGridClient(topicCredentials);
            return client.PublishEventsWithHttpMessagesAsync(topicEndpointHostname, events);
        }
    }
}
