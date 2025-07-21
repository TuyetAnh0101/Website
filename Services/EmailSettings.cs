namespace SportsStore.Services
{
public class EmailSettings
{
    public string SmtpServer { get; set; }   // non-nullable
    public string SenderEmail { get; set; }  // non-nullable
    public string Password { get; set; }     // non-nullable
    public int Port { get; set; }
}

}
