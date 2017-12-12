using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace RESTyr
{
    class Program
    {
        // http://fil.nrk.no/yr/viktigestader/verda.txt
        private const string Uri = "http://www.yr.no/sted/Danmark/Sjælland/Roskilde/varsel.xml";

        static void Main()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(Uri).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    string xmlString = CallRestServiceAsync(client, Uri).Result;
                    Console.WriteLine(xmlString);
                    IEnumerable<Weather> weather = GetWeatherFromXml(xmlString);
                    Console.WriteLine(string.Join("\n", weather));
                }
                else
                {
                    Console.WriteLine("HttpResponse status code:" + (int)response.StatusCode + " " + response.ReasonPhrase);
                }
            }
        }

        private static async Task<string> CallRestServiceAsync(HttpClient client, string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            throw new IOException("HttpResponse " + response.StatusCode + " " + response.ReasonPhrase);
        }

        private static IEnumerable<Weather> GetWeatherFromXml(string xmlString)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);

            XmlNodeList namesXmlList = xmlDocument.GetElementsByTagName("time");
            IList<Weather> weather = new List<Weather>();
            for (int i = 0; i < namesXmlList.Count; i++)
            {
                XmlNode namesNode = namesXmlList[i];
                string fromStr = namesNode.Attributes["from"].Value;
                DateTime from = DateTime.Parse(fromStr);
                string toStr = namesNode.Attributes["to"].Value;
                DateTime to = DateTime.Parse(toStr);

                XmlNode temperatureNode = namesNode.SelectSingleNode("temperature");
                string temperatureString = temperatureNode.Attributes["value"].Value;
                int temperature = int.Parse(temperatureString);

                XmlNode pressureNode = namesNode.SelectSingleNode("pressure");
                string pressureSTring = pressureNode.Attributes["value"].Value;
                double pressure = double.Parse(pressureSTring);

                XmlNode precipitationNode = namesNode.SelectSingleNode("precipitation");
                string precipitationString = precipitationNode.Attributes["value"].Value;
                double precipitation = double.Parse(precipitationString);

                Weather w = new Weather
                {
                    From = from,
                    To = to,
                    Temperature = temperature,
                    Pressure = pressure,
                    Precipitation = precipitation
                };
                weather.Add(w);
            }
            return weather;
        }
    }
}