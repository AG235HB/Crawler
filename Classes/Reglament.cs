namespace Crawler
{
    public class Reglament
    {
        public string ReglamentName { get; set; }
        public string ReglamentCode { get; set; }
        public DataSource[] ReglamentDatasources { get; set; }

        public Reglament() { }
        public Reglament(string name, string code, DataSource[] datasources)
        {
            ReglamentName = name;
            ReglamentCode = code;
            ReglamentDatasources = datasources;
        }
    }
}