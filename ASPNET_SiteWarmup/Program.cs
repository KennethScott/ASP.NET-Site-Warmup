using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;

namespace ASPNET_SiteWarmup
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                if (args.Length != 0 && !String.IsNullOrEmpty(args[0]) && args[0] != "-h" && args[0] != "/h"
                    && args[0] != "/help" && args[0] != "-help" && args[0] != "/?" && args[0] != "-?")
                {
                    xmlDoc.Load(args[0]);

                    foreach (XmlNode node in xmlDoc.SelectNodes("Sites/Site"))
                    {
                        XmlElement url = (XmlElement)node;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.SelectSingleNode("URL").InnerText);
                        var credentials = url.SelectSingleNode("Credentials");
                        if (credentials != null)
                        {
                            string domain = credentials.Attributes["Domain"].Value;
                            string user = credentials.Attributes["User"].Value;
                            string pass = credentials.Attributes["Pass"].Value;
                            if (String.IsNullOrEmpty(domain))
                                request.Credentials = new System.Net.NetworkCredential(user, pass);
                            else request.Credentials = new System.Net.NetworkCredential(user, pass, domain);
                        }
                        else // no credentials specified, just use defaults
                        {
                            request.Credentials = CredentialCache.DefaultCredentials;
                        }
                        // Chrome's User Agent
                        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.92 Safari/535.2";
                        WebResponse response = request.GetResponse();
                        response.Close();
                        Console.WriteLine(String.Format("{0} : Successful", url.SelectSingleNode("URL").InnerText));
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("  This program performs an HTTP GET to all sites listed in the xml file.");
                    Console.WriteLine();
                    Console.WriteLine("  Usage:");
                    Console.WriteLine(@"    ASPNET_SiteWarmup.exe <location>\<filename>");
                    Console.WriteLine(@"      Example:  ASPNET_SiteWarmup.exe c:\sites.xml");
                    Console.WriteLine(@"      Note:  If no credentials are necessary, simply remove the Credentials node");
                    Console.WriteLine();
                    Console.WriteLine("  XML File Layout:");
                    Console.WriteLine("  <?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    Console.WriteLine("    <Sites>");
                    Console.WriteLine("      <Site>");
                    Console.WriteLine("        <Comment>my comments about this site</Comment>");
                    Console.WriteLine("        <URL>http://server/site</URL>");
                    Console.WriteLine("        <Credentials Domain=\"\" User=\"\" Pass=\"\" />");
                    Console.WriteLine("      </Site>");
                    Console.WriteLine("      <Site>......</Site>");
                    Console.WriteLine("    </Sites>");
                    Console.WriteLine();
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error occurred.");
                Console.WriteLine("   Source : " + ex.Source);
                Console.WriteLine("   Message: " + ex.Message);
            }
        }
    }
}
