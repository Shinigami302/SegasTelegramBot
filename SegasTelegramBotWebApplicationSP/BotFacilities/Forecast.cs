using System;
using System.Collections.Generic;

using System.Net;
using System.Xml;
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

        private string nightTemp = String.Empty;

        public Dictionary<string, string> GetLvivsForecast()
        {
            string url = ForecastUrl.Replace("@LOC@", "702550");
            url = url.Replace("@QUERY@", QueryCodes[2]);
            using (WebClient client = new WebClient())
            {
                try
                {
                    return GetForecast(client.DownloadString(url));
                }
                catch (Exception ex)
                {
                    throw new Exception("Exeption in Forecast part: " + ex.Message);
                }
            }
        }

        private Dictionary<string, string> GetForecast(string xml)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                // Load the response into an XML document.
                XmlDocument xml_doc = new XmlDocument();
                String currentDay = String.Empty;
                xml_doc.LoadXml(xml);

                // Get the city, country, latitude, and longitude.
                XmlNode loc_node = xml_doc.SelectSingleNode("weatherdata/location");
                result.Add("City: ", loc_node.SelectSingleNode("name").InnerText);
                //result.Add("Країна:", loc_node.SelectSingleNode("country").InnerText);
                //XmlNode geo_node = loc_node.SelectSingleNode("location");

                char degrees = (char)176;

                //result.Add("TIME SPAN  ", new TimeSpan(03, 00, 00).ToString().ToString());
                //int i = 0;

                foreach (XmlNode time_node in xml_doc.SelectNodes("//time"))
                {
                    // Get the time in UTC.
                    DateTime time =
                        DateTime.Parse(time_node.Attributes["from"].Value,
                            null, DateTimeStyles.AssumeUniversal);
                    //var culture = new CultureInfo("uk-UA");
                    //var culture = new CultureInfo("en-EN");
                    // Get the temperature.
                    XmlNode temp_node = time_node.SelectSingleNode("temperature");
                    string temp = temp_node.Attributes["value"].Value;
                    XmlNode clouds_node = time_node.SelectSingleNode("clouds");
                    string cloudsVal = clouds_node.Attributes["value"].Value;
                    //double tempNum = Math.Round(double.Parse(temp.Replace('.', ',')));
                    double tempNum = Math.Round(double.Parse(temp));
                    //result.Add(" " + i++, time.TimeOfDay.ToString());
                    //result.Add(i.ToString(), (time.TimeOfDay == new TimeSpan(03, 00, 00)).ToString());


                    //in USA format night time comes after DAY TIME. Why so? Who fucking knows...
                    if (time.TimeOfDay == new TimeSpan(02, 00, 00))
                    {
                        nightTemp = tempNum.ToString();
                    }

                    if (!currentDay.Equals(time.DayOfWeek.ToString()) && time.TimeOfDay == new TimeSpan(14, 00, 00))
                    {
                        currentDay = time.DayOfWeek.ToString();
                        if (!string.Empty.Equals(nightTemp))
                        {
                            result.Add(time.DayOfWeek.ToString(),
                                $":  {nightTemp + degrees} / {tempNum.ToString() + degrees}  {cloudsVal}");
                        }
                        else
                        {
                            result.Add(time.DayOfWeek.ToString(), $":  {tempNum.ToString() + degrees}  {cloudsVal}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exeption in Forecast part: " + ex.Message);
            }
            return result;
        }
    }
}
