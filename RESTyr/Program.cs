using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace RESTyr
{
    class Program
    {
        static void Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                string uri = "http://www.yr.no/sted/Danmark/Sjælland/Roskilde/varsel.xml";
                string xmlString = GetProductAsync(client, uri).Result;
                Console.WriteLine(xmlString);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                XmlNodeList namesXmlList = xmlDocument.GetElementsByTagName("time");
                List<Weather> weather = new List<Weather>();
                for (int i = 0; i < namesXmlList.Count; i++)
                {
                    XmlNode namesNode = namesXmlList[i];
                    string from = namesNode.Attributes["from"].Value;
                    string to = namesNode.Attributes["to"].Value;

                    XmlNode temperatureNode = namesNode.SelectSingleNode("temperature");
                    string temperatureString = temperatureNode.Attributes["value"].Value;
                    int temperature = int.Parse(temperatureString);

                    XmlNode pressureNode = namesNode.SelectSingleNode("pressure");
                    string pressureSTring = pressureNode.Attributes["value"].Value;
                    double pressure = double.Parse(pressureSTring);

                    XmlNode precipitationNode = namesNode.SelectSingleNode("precipitation");
                    string precipitationString = precipitationNode.Attributes["value"].Value;
                    double precipitation = double.Parse(precipitationString);
                    //string countryName = namesXmlList[i].InnerText;
         
                    Weather w = new Weather { From = from, To = to, Temperature = temperature, Pressure = pressure, Precipitation = precipitation };
                    weather.Add(w);
                }
                Console.WriteLine(string.Join("\n", weather));
            }
        }

        static async Task<string> GetProductAsync(HttpClient client, string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }

    class Weather
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Temperature { get; set; }
        public double Precipitation { get; set; }
        public double Pressure { get; set; }

        public override string ToString()
        {
            return From + " " + To + " " + Temperature + " " + Pressure + " " + Precipitation;
        }
    }
}
