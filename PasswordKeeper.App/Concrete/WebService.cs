﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PasswordKeeper.App.Concrete
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
                return false;
            }

            return result.Status == IPStatus.Success;
        }
    }
}
