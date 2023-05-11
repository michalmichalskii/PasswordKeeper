using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PasswordKeeper.Services
{
    public class WebService
    {
        public bool CheckIsSiteAvailable(string readSite)
        {
            var ping = new Ping();
            PingReply result;
            try
            {
                result = ping.Send(readSite);
            }
            catch (Exception)
            {
                throw new Exception("This site actually doesn't exist");
            }

            return result.Status == IPStatus.Success;
        }
    }
}
