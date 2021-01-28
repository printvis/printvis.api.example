using System;
using System.IO;
using System.Xml;

namespace printvis.api.example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            WriteXMLToRest();
        }

        static void WriteXMLToRest()
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(GetXMLFromFile());
            string XMLAsBase64 = Convert.ToBase64String(plainTextBytes);

            var tenandId = "your tenant ID here";
            var companyId = "your company ID here";
            var apiPublisher = "PrintVis";
            var apiGroup = "JDF";
            var apiVersion = "v0.9";
            var apiEndpoint = "JMFs";
            var sandBoxName = "your sandboxname here";

            var client = new RestSharp.RestClient(String.Format("https://api.businesscentral.dynamics.com/v2.0/{0}/{6}/api/{1}/{2}/{3}/companies({4})/{5}",
                tenandId, apiPublisher, apiGroup, apiVersion, companyId, apiEndpoint, sandBoxName));
            client.Timeout = -1;

            var request = new RestSharp.RestRequest(RestSharp.Method.POST);

            var userName = "your username here";
            var webServiceAccessKey = "your webservice access key here";

            byte[] UserInfo = System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", userName, webServiceAccessKey));

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(UserInfo));
            request.AddHeader("Content-Type", "application/json");

            var json = new Newtonsoft.Json.Linq.JObject();
            json.Add("comment", "Sending file from example code... " + DateTime.Now.ToString());
            json.Add("JMFAsBase64", XMLAsBase64);

            request.AddParameter("application/json", json.ToString(), RestSharp.ParameterType.RequestBody);

            RestSharp.IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content.ToString());

        }

        private static string GetXMLFromFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("your XML file here");

            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

            doc.WriteTo(xmlTextWriter);

            return (stringWriter.ToString());
        }
    }

}

