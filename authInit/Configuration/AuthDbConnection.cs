namespace Configuration
{
    public class AuthDbConnection
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string ConnectionString
        {
            get { return $"Server={Server};port={Port};database={Database};user={User};password={Password}"; }
        }
    }
}
