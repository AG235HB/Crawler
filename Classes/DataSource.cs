namespace Crawler
{
    public class DataSource
    {
        public string DataSourceName { get; set; }
        public Protocol DataSourceType { get; set; }
        public ConnectionParameters ConnectionParameters { get; set; }
        public Check[] Checks { get; set; }

        public DataSource() { }
        public DataSource(string name, Protocol type, ConnectionParameters connection, Check[] checks)
        {
            DataSourceName = name;
            DataSourceType = type;
            ConnectionParameters = connection;
            Checks = checks;
        }
    }
}