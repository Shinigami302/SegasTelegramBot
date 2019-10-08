using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Net;
using System.Xml;
using System.IO;
using System.Globalization;

namespace SegasTelegramBotWebApplicationSP
{
    public class Forecast
    {
        // Enter your API key here.
        // Get an API key by making a free account at:
        //      http://home.openweathermap.org/users/sign_in
        private const string API_KEY = "de1039eacc077d91bf7922217e76886a";

        // Query URLs. Replace @LOC@ with the location.
        private const string CurrentUrl =
            "http://api.openweathermap.org/data/2.5/weather?" +
            "@QUERY@=@LOC@&mode=xml&units=metric&APPID=" + API_KEY;
        private const string ForecastUrl =
            "http://api.openweathermap.org/data/2.5/forecast?" +
            "@QUERY@=@LOC@&mode=xml&units=metric&APPID=" + API_KEY;

        // Query codes.
        private string[] QueryCodes = { "q", "zip", "id", };

        public Dictionary<string, string> GetLivsForecast()
        {
            string url = ForecastUrl.Replace("@LOC@", "702550");
            url = url.Replace("@QUERY@", QueryCodes[2]);
            using (WebClient client = new WebClient())
            {
                try
                {
                    return GetForecast(client.DownloadString(url));
                }
                catch
                {
                    return null;
                }
            }
        }

        private Dictionary<string, string> GetForecast(string xml)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            // Load the response into an XML document.
            XmlDocument xml_doc = new XmlDocument();
            String currentDay = String.Empty;
            xml_doc.LoadXml(xml);

            // Get the city, country, latitude, and longitude.
            XmlNode loc_node = xml_doc.SelectSingleNode("weatherdata/location");
            result.Add("Місто: ", loc_node.SelectSingleNode("name").InnerText);
            result.Add("Країна:", loc_node.SelectSingleNode("country").InnerText);
            XmlNode geo_node = loc_node.SelectSingleNode("location");

            char degrees = (char)176;

            foreach (XmlNode time_node in xml_doc.SelectNodes("//time"))
            {
                // Get the time in UTC.
                DateTime time =
                    DateTime.Parse(time_node.Attributes["from"].Value,
                        null, DateTimeStyles.AssumeUniversal);
                var culture = new CultureInfo("ua-UA");

                // Get the temperature.
                XmlNode temp_node = time_node.SelectSingleNode("temperature");
                string temp = temp_node.Attributes["value"].Value;
                if (!currentDay.Equals(time.DayOfWeek.ToString()))
                {
                    currentDay = time.DayOfWeek.ToString();
                    result.Add(culture.DateTimeFormat.GetDayName(time.DayOfWeek).ToString(), $" {temp.Substring(0, temp.IndexOf('.')) + degrees}");
                }
            }
            return result;
        }
    }
}
