namespace SendArchive.Email
{
    /// <summary>
    /// Interface for description service email
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Method send message
        /// </summary>
        /// <param name="message">Messgae for send</param>
        /// <returns>Returns the result of sending</returns>
        System.Threading.Tasks.Task<Result> SendEmailAsync(Message message);
    }
}