using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;

namespace SegasTelegramBotWebApplicationSP
{
    class Exchange
    {
        private const string PRIVAR_URL = "https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5";

        public List<string> GetCourse()
        {
            Dictionary<string, string> course = new Dictionary<string, string>();
            List<string> result = new List<string>();
            using (WebClient client = new WebClient())
            {
                try
                {
                    List<Cash> cashes = ParseResult(client.DownloadString(PRIVAR_URL));
                    foreach (Cash item in cashes)
                    {
                        result.Add($"Curency:  {item.ccy}  to:  {item.base_ccy}  Buy:  " +
                            $"{item.buy.Substring(0, item.buy.IndexOf('.') + 3)}  Sale:  {item.sale.Substring(0, item.sale.IndexOf('.') + 3)}");
                    }
                }
                catch
                {

                }
            }
            return result;
        }

        private List<Cash> ParseResult(string xml)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            // Load the response into an XML document.
            XmlDocument xml_doc = new XmlDocument();
            List<Cash> cashes = new List<Cash>();

            xml_doc.LoadXml(xml);

            foreach (XmlNode item in xml_doc.SelectNodes("exchangerates")[0].SelectNodes("row"))
            {
                cashes.Add(new Cash(item.SelectSingleNode("exchangerate").Attributes["ccy"].Value,
                    item.SelectSingleNode("exchangerate").Attributes["base_ccy"].Value,
                    item.SelectSingleNode("exchangerate").Attributes["buy"].Value,
                    item.SelectSingleNode("exchangerate").Attributes["sale"].Value));
            }


            return cashes;
        }

        class Cash
        {
            public Cash() { }

            public Cash(string ccy, string base_ccy, string buy, string sale)
            {
                this.ccy = ccy;
                this.base_ccy = base_ccy;
                this.buy = buy;
                this.sale = sale;
            }
            public string ccy { get; set; }
            public string base_ccy { get; set; }
            public string buy { get; set; }
            public string sale { get; set; }
        }
    }
}
