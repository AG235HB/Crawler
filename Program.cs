using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;
using Crawler.Logging;
using Crawler.Testers;

namespace Crawler
{
    class Program
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            Log.Write(LogLevel.Debug, null, new Dictionary<string, object>(), "Application started...");

            if (args.Length > 0) foreach (var path in args)
                {
                    try
                    {
                        var data = ReadConfig(path);
                        List<Tester> testers = new List<Tester>();
                        foreach (var item in data)
                        {
                            foreach (var ds in item.ReglamentDatasources)
                            {
                                switch (ds.DataSourceType)
                                {
                                    case Protocol.FTP:
                                        foreach (var chk in ds.Checks)
                                        {
                                            if (chk.Active)
                                            {
                                                if (chk.CheckType.Equals(CheckType.Ping))
                                                {
                                                    CreatePingTester(testers, item, ds, chk);
                                                }
                                                else
                                                {
                                                    testers.Add(new FtpTester(
                                                        ds.ConnectionParameters.Host,
                                                        ds.ConnectionParameters.Port == null ? 21 : (int)ds.ConnectionParameters.Port,
                                                        5000,
                                                        ds.ConnectionParameters.Username,
                                                        ds.ConnectionParameters.Password,
                                                        chk.CheckType,
                                                        chk.AdditionalParameters,
                                                        CreateLoggerAttributes(item, ds, chk)));
                                                }
                                            }
                                        }
                                        break;
                                    case Protocol.SFTP:
                                        foreach (var chk in ds.Checks)
                                        {
                                            if (chk.Active)
                                            {
                                                if (chk.CheckType.Equals(CheckType.Ping))
                                                {
                                                    CreatePingTester(testers, item, ds, chk);
                                                }
                                                else
                                                {
                                                    testers.Add(new SftpTester(
                                                        ds.ConnectionParameters.Host,
                                                        ds.ConnectionParameters.Port == null ? 22 : (int)ds.ConnectionParameters.Port,
                                                        5000,
                                                        ds.ConnectionParameters.Username,
                                                        ds.ConnectionParameters.Password,
                                                        chk.CheckType,
                                                        chk.AdditionalParameters,
                                                        CreateLoggerAttributes(item, ds, chk)));
                                                }
                                            }
                                        }
                                        break;
                                    case Protocol.FTPS:
                                        foreach (var chk in ds.Checks)
                                        {
                                            if (chk.Active)
                                            {
                                                if (chk.CheckType.Equals(CheckType.Ping))
                                                {
                                                    CreatePingTester(testers, item, ds, chk);
                                                }
                                                else
                                                {
                                                    testers.Add(new FtpsTester(
                                                        ds.ConnectionParameters.Host,
                                                        ds.ConnectionParameters.Port == null ? 21 : (int)ds.ConnectionParameters.Port,
                                                        5000,
                                                        ds.ConnectionParameters.Username,
                                                        ds.ConnectionParameters.Password,
                                                        chk.CheckType,
                                                        chk.AdditionalParameters,
                                                        CreateLoggerAttributes(item, ds, chk),
                                                        ds.ConnectionParameters.IgnoreSslErrors));
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        foreach (var tester in testers)
                        {
                            tester.Run();
                        }
                    }
                    catch (FileNotFoundException fEx)
                    {
                        Log.Write(1, fEx, new Dictionary<string, object>(), fEx.Message);
                        Environment.Exit((int)ExitCode.FileNotFound);
                    }
                    catch (IOException ioEx)
                    {
                        Log.Write(1, ioEx, new Dictionary<string, object>(), ioEx.Message);
                        Environment.Exit((int)ExitCode.OpenFailure);
                    }
                    catch (UnauthorizedAccessException uaEx)
                    {
                        Log.Write(1, uaEx, new Dictionary<string, object>(), uaEx.Message);
                        Environment.Exit((int)ExitCode.UnauthorizedAccess);
                    }
                    catch (JsonException jEx)
                    {
                        Log.Write(1, jEx, new Dictionary<string, object>(), jEx.Message);
                        Environment.Exit((int)ExitCode.JsonParseFailure);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(1, ex, new Dictionary<string, object>(), ex.Message);
                    }
                }
            else
            {
                Log.Write(LogLevel.Error, null, new Dictionary<string, object>(), "No path specified!");
                Environment.Exit((int)ExitCode.NoPathSpecified);
            }
            Log.Write(LogLevel.Debug, null, new Dictionary<string, object>(), "Moor did his job...");
            Environment.Exit((int)ExitCode.Success);
        }

        private static Dictionary<string, object> CreateLoggerAttributes(Reglament item, DataSource dataSource, Check check) => new Dictionary<string, object>() {
            {"ReglamentName",item.ReglamentName},
            {"ReglamentCode",item.ReglamentCode},
            {"DataSourceName",dataSource.DataSourceName},
            {"DataSourceType",dataSource.DataSourceType.ToString()},
            {"DataSourceCheckType",check.CheckType.ToString()},
            {"Username",dataSource.ConnectionParameters.Username}
        };

        private static void CreatePingTester(List<Tester> testers, Reglament item, DataSource dataSource, Check check) => testers.Add(new PingTester(
                dataSource.ConnectionParameters.Host,
                dataSource.ConnectionParameters.Timeout == null ? 5000 : (int)dataSource.ConnectionParameters.Timeout,
                CreateLoggerAttributes(item, dataSource, check)));

        private static Reglament[] ReadConfig(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open))
            {
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                var str = System.Text.Encoding.UTF8.GetString(bytes).TrimEnd('\0');
                var result = JsonConvert.DeserializeObject<Reglament[]>(str);
                return result;
            }
        }
    }
}
