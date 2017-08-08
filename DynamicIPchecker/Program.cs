using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DynamicIPchecker
{
    class Program
    {
        private static string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ip.txt");

        static string userName = ConfigurationManager.AppSettings["UserName"].ToString();
        static string password = ConfigurationManager.AppSettings["Password"].ToString();
        static string SMTP_Host = ConfigurationManager.AppSettings["SMTP_Host"].ToString();
        static int SMTP_Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_Port"]);
        static string fromAddress = ConfigurationManager.AppSettings["From_Address"].ToString();
        static string toAddress = ConfigurationManager.AppSettings["To_Address"].ToString();

        static void Main(string[] args)
        {
            Console.WriteLine("Looking up Public IP");
            string currentIP = getCurrentIP();

            string savedIP;
            Console.WriteLine("Looking up Saved Public IP");
            if (File.Exists(_path))
            {
                savedIP = File.ReadAllText(_path, Encoding.UTF8);
            }
            else
            {
                using (StreamWriter sw = File.CreateText(_path)) { };
                savedIP = null;
            }

            if (currentIP != savedIP)
            {
                Console.WriteLine("Public IP has changed");
                Console.WriteLine("Saving New Public IP");
                File.WriteAllText(_path, currentIP);

                Console.WriteLine("Emailing New Public IP");
                emailNewIP(currentIP);
            }
            else
            {
                Console.WriteLine("Public IP Address has not changed");
            }
            Console.WriteLine("Closing...");

        }

        private static string getCurrentIP()
        {
            WebRequest request = WebRequest.Create("http://ipv4.icanhazip.com/");
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd().Replace("\n", "");
        }



        private static void emailNewIP(string ip)
        {
            SmtpClient client = new SmtpClient(SMTP_Host, SMTP_Port)
            {
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = true
            };
            client.Send(fromAddress, toAddress, "Dynamic IP Change", ip);
        }
    }
}
