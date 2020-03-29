using authInit.Attributes;

namespace Configuration
{
    public class AuthDbConnectionSettings
    {
        [LoggableSettings]
        public string Server { get; set; }
        [LoggableSettings]
        public string Port { get; set; }
        [LoggableSettings]
        public string Database { get; set; }
        [LoggableSettings]
        public string User { get; set; }
        [LoggableSettings(LoggableSettingOutput.Secured)]
        public string Password { get; set; }

        public string ConnectionString
        {
            get { return $"Server={Server};port={Port};database={Database};user={User};password={Password}"; }
        }
    }
}
