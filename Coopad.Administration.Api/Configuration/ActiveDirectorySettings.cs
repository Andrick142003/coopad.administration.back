namespace Coopad.Administration.Api.Configuration
{
    public class ActiveDirectorySettings
    {
        public string Domain { get; set; } = null!;

        public string Server { get; set; } = null!;

        public int Port { get; set; }

        public bool UseSsl { get; set; }
    }
}
