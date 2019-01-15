using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService
    {
        private string _mailTo = "docaotri211997@gmail.com";
        private string _mailFrom = "dctri211997@gmail.com";

        public void Send(string subject, string message)
        {
            //send mail - output to debug window
            Debug.WriteLine($"mail from {_mailFrom} to {_mailTo}, with local service");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
