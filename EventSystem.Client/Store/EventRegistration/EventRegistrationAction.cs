using EventSystem.Model;

namespace EventSystem.Client.Store.EventRegistration
{
    // Get Actions
    public record GetUserRegisteredAction(long EventId, string UserId);
    public record GetUserRegisteredSuccessAction(long EventId, string UserId, bool IsRegistered);
    public record GetUserRegisteredFailureAction(string ErrorMessage);

    // Add Actions
    public record CreateEventRegistrationAction(EventRegistrationModel EventRegistrationModel, string JwtToken);
    public record CreateEventRegistrationSuccessAction(EventRegistrationModel EventRegistrationModel);
    public record CreateEventRegistrationFailureAction(string ErrorMessage);

}
