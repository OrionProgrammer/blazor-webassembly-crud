using EventSystem.Model;

namespace EventSystem.Client.Store.Event
{
    public record EventActions();

    // Load Actions
    public record LoadEventsAction();

    public record LoadEventsSuccessAction(IEnumerable<EventModel> Events);

    public record LoadEventsFailedAction(string ErrorMessage);

    // Select Action
    public record SelectEventAction(int EventId);

    // Add Actions
    public record AddEventAction(EventModel Event, string JwtToken);

    public record AddEventSuccessAction(EventModel Event);
    
    public record AddEventFailedAction(string ErrorMessage);

    // Update Actions
    public record UpdateEventAction(EventModel Event, string JwtToken);

    public record UpdateEventSuccessAction(EventModel Event);

    public record UpdateEventFailedAction(string ErrorMessage);

    // Delete Actions
    public record DeleteEventAction(long EventId, string JwtToken);
    
    public record DeleteEventSuccessAction(long EventId);

    public record DeleteEventFailedAction(string ErrorMessage);
}
