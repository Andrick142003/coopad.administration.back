namespace Coopad.Administration.Api.Configuration
{
    public class ActiveDirectorySettings
    {
        public string Server { get; set; } = null!;
        public int Port { get; set; }
        public string Domain { get; set; } = null!;
        public bool UseSsl { get; set; }
    }
}
