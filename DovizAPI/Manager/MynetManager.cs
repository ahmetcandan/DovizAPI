using DovizAPI.Model;
using DovizAPI;
using AngleSharp.Html.Parser;
using System.Net;

namespace DovizAPI.Manager
{
    public class MynetManager
    {
        public Weather GetWeather(string city)
        {
            try
            {
                WebRequest request = WebRequest.Create($"https://www.mynet.com/hava-durumu/{city.ToLower()}-hava-durumu-bugun");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string html = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                var document = new HtmlParser().ParseDocument(html);
                var element = document.QuerySelectorAll("div").FirstOrDefault(c => c.ClassName == "detail-content-container card px-4 py-3 mb-4");
                string cityName = element?.Children[0]?.Children[0]?.TextContent;
                string date = element?.Children[0]?.Children[1]?.TextContent;
                string temp = element?.Children[1].Children[0].Children[1].Children[0].TextContent.Trim();
                string info1 = element?.Children[1].Children[0].Children[1].Children[1].TextContent;
                string info2 = element?.Children[1].Children[1].Children[1].TextContent;
                string info3 = element?.Children[1].Children[1].Children[2].TextContent;
                string info4 = element?.Children[1].Children[1].Children[3].TextContent;
                string info5 = element?.Children[1].Children[1].Children[4].TextContent;

                return new Weather
                {
                    City = cityName.Trim(),
                    DateTime = date.Trim(),
                    Temperature = temp.Trim(),
                    Info1 = info1.Trim(),
                    Info2 = info2.Trim(),  
                    Info3 = info3.Trim(),
                    Info4 = info4.Trim(),
                    Info5 = info5.Trim()
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
