namespace Coopad.Administration.Api.Configuration;

public class AseConnectionSettings
{
    public string Server { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Database { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    public int ConnectionTimeout { get; set; }
    public string Charset { get; set; } = string.Empty;
}