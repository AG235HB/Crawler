namespace Crawler
{
    public enum ExitCode
    {
        Success,
        UnauthorizedAccess,
        NoPathSpecified,
        FileNotFound,
        OpenFailure,
        JsonParseFailure
    }
}