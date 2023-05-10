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
        public int CheckIsSiteAvailable(string readSite)
        {
            var ping = new System.Net.NetworkInformation.Ping();
            PingReply result;
            try
            {
                result = ping.Send(readSite);
            }
            catch (Exception)
            {
                throw new Exception("This site actually doesn't exist");
            }

            //I am not sure is if statement necessary
            if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                return -1;

            return 0;
        }
    }
}
