using DovizAPI.Model;
using DovizAPI;
using AngleSharp.Html.Parser;
using System.Net;

namespace DovizAPI.Manager
{
    public class DovizComManager
    {
        public Currency GetCurrency(string code)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://kur.doviz.com/");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string html = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                var document = new HtmlParser().ParseDocument(html);
                var element = document.QuerySelectorAll("div").FirstOrDefault(c => c.InnerHtml == code.ToUpper());
                var mainTr = element?.ParentElement?.ParentElement?.ParentElement?.ParentElement;
                string buy = mainTr?.Children[1]?.InnerHtml;
                string sell = mainTr?.Children[2]?.InnerHtml;
                string max = mainTr?.Children[3]?.InnerHtml;
                string min = mainTr?.Children[4]?.InnerHtml;
                string rate = mainTr?.Children[5]?.InnerHtml.Trim().Substring(1).Replace("\n", "");
                var time = mainTr?.Children[6]?.InnerHtml?.Split(":");
                string title = element?.ParentElement?.Children[1]?.InnerHtml;

                return new Currency(code.ToUpper())
                {
                    Buy = buy.ToDecimal(),
                    Sell = sell.ToDecimal(),
                    Max = max.ToDecimal(),
                    Min = min.ToDecimal(),
                    Rate = rate.ToDecimal(),
                    Title = title,
                    Time = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), 0)
                };
            }
            catch
            {
                return null;
            }
        }

        public Stock GetStock(string code)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://borsa.doviz.com/hisseler");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string html = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                var document = new HtmlParser().ParseDocument(html);
                var element = document.QuerySelectorAll("div").FirstOrDefault(c => c.InnerHtml == code.ToUpper());
                var mainTr = element?.ParentElement?.ParentElement?.ParentElement?.ParentElement;
                string lValue = mainTr?.Children[1]?.InnerHtml;
                string maxValue = mainTr?.Children[2]?.InnerHtml;
                string minValue = mainTr?.Children[3]?.InnerHtml;
                string vValue = mainTr?.Children[4]?.InnerHtml.Trim().Substring(1);
                string rate = mainTr?.Children[5]?.InnerHtml.Trim().Substring(1).Replace("\n", "");
                var time = mainTr?.Children[6]?.InnerHtml?.Split(":");
                string title = element?.ParentElement?.Children[1]?.InnerHtml;

                return new Stock(code.ToUpper())
                {
                    Last = lValue.ToDecimal(),
                    Max = maxValue.ToDecimal(),
                    Min = minValue.ToDecimal(),
                    Rate = rate.ToDecimal(),
                    Volume = vValue.ToDecimal(),
                    Title = title,
                    Time = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), 0)
                };
            }
            catch
            {
                return null;
            }
        }

        public Stock GetFund(string code)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://borsa.doviz.com/borsa-yatirim-fonlari");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string html = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                var document = new HtmlParser().ParseDocument(html);
                var element = document.QuerySelectorAll("div").FirstOrDefault(c => c.InnerHtml == code.ToUpper());
                var mainTr = element?.ParentElement?.ParentElement?.ParentElement?.ParentElement;
                string lValue = mainTr?.Children[1]?.InnerHtml;
                string maxValue = mainTr?.Children[2]?.InnerHtml;
                string minValue = mainTr?.Children[3]?.InnerHtml;
                string vValue = mainTr?.Children[4]?.InnerHtml.Trim().Substring(1);
                string rate = mainTr?.Children[5]?.InnerHtml.Trim().Substring(1).Replace("\n", "");
                var time = mainTr?.Children[6]?.InnerHtml?.Split(":");
                string title = element?.ParentElement?.Children[1]?.InnerHtml;

                return new Stock(code.ToUpper())
                {
                    Last = lValue.ToDecimal(),
                    Max = maxValue.ToDecimal(),
                    Min = minValue.ToDecimal(),
                    Rate = rate.ToDecimal(),
                    Volume = vValue.ToDecimal(),
                    Title = title,
                    Time = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), 0)
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
