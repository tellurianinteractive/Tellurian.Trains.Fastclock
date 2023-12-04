namespace Fastclock.Contracts;

public record ClockUser
{ 
   
    public void Update(string? userName, string clientVersion)
    {
        UserName = userName;
        ClientVersion = clientVersion;
    }

    public string? IPAddress { get;  set; }
    public string? UserName { get;  set; } 
    public string? ClientVersion { get;  set; } 
   
}
