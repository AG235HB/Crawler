namespace Crawler
{
    public enum FtpCheckType
    {
        Connect,           //connect
        List,           //ftp ls
        DetailedList,   //ftp dir
        FileName,
        FileExtension
    }
}