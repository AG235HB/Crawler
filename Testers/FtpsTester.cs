using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace Crawler.Testers
{
    public class FtpsTester : Tester
    {
        int Port;
        int Timeout;
        string Username;
        string Password;
        CheckType CheckType;
        string Parameters;
        static bool IgnoreSslErrors;

        public FtpsTester(string host, int port, int timeout, string username, string password, CheckType type, string parameters, Dictionary<string, object> attributes, bool ignoreSslErrors)
        {
            Host = host;
            Port = port;
            Timeout = timeout;
            Username = username;
            Password = password;
            CheckType = type;
            Parameters = parameters;

            base.SetLogAttributes(attributes);
            IgnoreSslErrors = ignoreSslErrors;
        }

        public override void Run() => FtpsCheck();

        private void FtpsCheck()
        {
            base.LogDebug("Attempting FTPS check for remote host: " + Host);
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + Host + "/");
                switch (CheckType)
                {
                    case CheckType.Connect:
                        request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;//с pi приходит "" вместо "/home/pi"
                        break;
                    case CheckType.List:
                        request.Method = WebRequestMethods.Ftp.ListDirectory;
                        break;
                    case CheckType.DetailedList:
                        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                        break;
                    case CheckType.FileName:
                        break;
                    case CheckType.FileExtension:
                        break;
                    default:
                        base.LogError("Неизвестный тип проверки");
                        break;
                }

                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

                request.Credentials = new NetworkCredential(Username, Password);
                request.EnableSsl = true;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                var Result = "";
                Result += reader.ReadToEnd();
                base.LogInfo(new Dictionary<string, object>(){
                    {"DataSourceCheckResult","PASSED"},
                    {"DataSourceCheckResultMessage",Result}
                }, "Checking \"" + CheckType.ToString() + "\" for remote host " + Host + " successfull.");

                reader.Close();
                responseStream.Close();

                response.Close();
            }
            catch (Exception ex)
            {
                base.LogException(ex, new Dictionary<string, object>(){
                    {"DataSourceCheckResult","FAILED"},
                    {"DataSourceCheckResultMessage","FtpsTester exception: "+ex.Message}
                }, ex.Message);
            }
        }

        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;
             if (IgnoreSslErrors) return true;
             return false;
        }
    }
}