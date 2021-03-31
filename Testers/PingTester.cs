using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Collections.Generic;

namespace Crawler.Testers
{
    public class PingTester : Tester
    {
        int Timeout;
        public PingTester(string host, int timeout, Dictionary<string, object> attributes)
        {
            Host = host;
            Timeout = timeout;
            base.SetLogAttributes(attributes);
        }

        public override void Run() => IcmpCheck();

        private void IcmpCheck()
        {
            try
            {
                base.LogDebug("Attempting ICMP check for remote host: " + Host);
                Ping ping = new Ping();
                PingReply pReply = ping.Send(Host, Timeout, Encoding.ASCII.GetBytes("................................"), new PingOptions(50, true));
                if (pReply.Status.Equals(IPStatus.Success))
                    base.LogInfo(
                    new Dictionary<string, object>(){
                        {"DataSourceCheckResult","PASSED"},
                        {"DataSourceCheckResultMessage","Reply from: " + pReply.Address.ToString() + "(" + Host + ") Time: " + pReply.RoundtripTime.ToString() + " Status: " + pReply.Status.ToString()}
                    },
                    "Reply from: " + pReply.Address.ToString() + "(" + Host + ") Time: " + pReply.RoundtripTime.ToString() + " Status: " + pReply.Status.ToString());
                else
                    base.LogError(
                        new Dictionary<string, object>(){
                        {"DataSourceCheckResult","FAILED"},
                        {"DataSourceCheckResultMessage","Reply from: " + pReply.Address.ToString() + " Time: " + pReply.RoundtripTime.ToString() + " Status: " + pReply.Status.ToString()}
                        },
                        "Reply from: " + pReply.Address.ToString() + " Time: " + pReply.RoundtripTime.ToString() + " Status: " + pReply.Status.ToString());
            }
            catch (Exception ex)
            {
                base.LogException(ex,
                    new Dictionary<string, object>(){
                        {"DataSourceCheckResult","FAILED"},
                        {"DataSourceCheckResultMessage",ex.Message}},
                    ex.Message);
            }

            // base.LogDebug("USELESS LOG");
        }
    }
}