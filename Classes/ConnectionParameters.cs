namespace Crawler
{
    public class ConnectionParameters
    {
        public string Host { get; set; }
        public int? Port { get; set; }
        public int? Timeout { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IgnoreSslErrors { get; set; }

        public ConnectionParameters() { }
        public ConnectionParameters(string host, int? port, int? timeout, string username, string password, bool ignoreSsleErrors)
        {
            Host = host;
            Port = port;
            Timeout = timeout;
            Username = username;
            Password = password;
            IgnoreSslErrors = ignoreSsleErrors;
        }
    }
}