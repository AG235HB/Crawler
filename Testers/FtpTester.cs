using System;
using BytesRoad.Net.Ftp;
using System.Collections.Generic;

namespace Crawler.Testers
{
    public class FtpTester : Tester
    {
        int Port;
        int Timeout;
        string Username;
        string Password;
        CheckType CheckType;
        string Parameters;

        public FtpTester(string host, int port, int timeout, string username, string password, CheckType type, string parameters, Dictionary<string, object> attributes)
        {
            Host = host;
            Port = port;
            Timeout = timeout;
            Username = username;
            Password = password;
            CheckType = type;
            Parameters = parameters;

            base.SetLogAttributes(attributes);
        }

        public override void Run() => BytesRoadFtpCheck();

        private void BytesRoadFtpCheck()
        {
            base.LogDebug("Attempting FTP check for remote host: " + Host);
            try
            {
                FtpClient client = new FtpClient();
                client.PassiveMode = true;
                client.Connect(Timeout, Host, Port);
                client.Login(Timeout, Username, Password);

                string result = "";
                FtpItem[] ftpResponse;
                switch (CheckType)
                {
                    case CheckType.Connect:
                        //FTP !pwd
                        result += "Connection successfull...";
                        break;
                    case CheckType.List:
                        //FTP ls
                        ftpResponse = client.GetDirectoryList(Timeout);
                        if (ftpResponse.Length > 0) foreach (var item in ftpResponse) result += item.ItemType.ToString() + ": " + item.Name + "\n";
                        else result += "Storage is empty...";
                        break;
                    case CheckType.DetailedList:
                        //FTP dir
                        ftpResponse = client.GetDirectoryList(Timeout);
                        if (ftpResponse.Length > 0) foreach (var item in ftpResponse) result += item.RawString + "\n";
                        else result += "Storage is empty...";
                        break;
                    case CheckType.FileName:
                        break;
                    case CheckType.FileExtension:
                        break;
                    default:
                        base.LogError("Неизвестный тип проверки");
                        break;
                }

                client.Disconnect(Timeout);

                base.LogInfo(new Dictionary<string, object>(){
                    {"DataSourceCheckResult","PASSED"},
                    {"DataSourceCheckResultMessage",result}
                }, "Checking \"" + CheckType.ToString() + "\" for remote host " + Host + " successfull.");
            }
            catch (Exception ex)
            {
                base.LogException(ex, new Dictionary<string, object>(){
                    {"DataSourceCheckResult","FAILED"},
                    {"DataSourceCheckResultMessage","FtpTester exception: "+ex.Message}
                }, ex.Message);
            }
        }
    }
}