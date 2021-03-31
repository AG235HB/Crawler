using System;
using System.Collections.Generic;
using Renci.SshNet;

namespace Crawler.Testers
{
    public class SftpTester : Tester
    {
        int Port;
        int Timeout;
        string Username;
        string Password;
        CheckType CheckType;
        string Parameters;

        public SftpTester(string host, int port, int timeout, string username, string password, CheckType type, string parameters, Dictionary<string, object> attributes)
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

        public override void Run() => SftpCheck();

        private void SftpCheck()
        {
            base.LogDebug("Attempting SFTP check for remote host: " + Host);
            try
            {
                var result = "";
                using (SftpClient sftp = new SftpClient(Host, Port, Username, Password))
                {
                    switch (CheckType)
                    {
                        case CheckType.Connect:
                            sftp.Connect();
                            result += "Connection successfull...";
                            break;
                        case CheckType.List:
                            sftp.Connect();
                            var list = sftp.ListDirectory(sftp.WorkingDirectory);
                            foreach (var item in list) result += item.Name + "\n";
                            break;
                        case CheckType.DetailedList:
                            sftp.Connect();
                            var detailedList = sftp.ListDirectory(sftp.WorkingDirectory);
                            foreach (var item in detailedList) result += item.FullName + "\n";
                            break;
                        case CheckType.FileName:
                            break;
                        case CheckType.FileExtension:
                            break;
                        default:
                            base.LogError("Неизвестный тип проверки");
                            break;
                    }
                    sftp.Disconnect();
                    base.LogInfo(new Dictionary<string, object>(){
                            {"DataSourceCheckResult","PASSED"},
                            {"DataSourceCheckResultMessage",result}
                        }, "Checking \"" + CheckType.ToString() + "\" for remote host " + Host + " successfull.");
                }
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