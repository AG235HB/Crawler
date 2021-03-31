namespace Crawler
{
    public class Check
    {
        public bool Active { get; set; }
        public CheckType CheckType { get; set; }
        public string AdditionalParameters { get; set; }

        public Check() { }
        public Check(bool active, CheckType type, string parameters)
        {
            Active = active;
            CheckType = type;
            AdditionalParameters = parameters;
        }
    }
}