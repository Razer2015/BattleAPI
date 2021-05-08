using CompanionAPI.Battlelog.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace CompanionAPI.Battlelog
{
    public static class BattlelogClient
    {
        public static string FetchWebPage(ref string html_data, string url, bool ajax = false)
        {
            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                if (ajax)
                {
                    request.Headers.Add("X-AjaxNavigation", "1");
                }

                using (var response = request.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    html_data = sr.ReadToEnd();
                }

                return html_data;

            }
            catch (WebException e)
            {
                if (e.Status.Equals(WebExceptionStatus.Timeout))
                    throw new Exception("HTTP request timed-out");
                else
                    throw;
            }
        }

        /// <summary>
        ///     Get the server snapshot
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static ServerInfo GetServerSnapshot(string guid)
        {
            try
            {
                string result = "";
                FetchWebPage(ref result, $"https://keeper.battlelog.com/snapshot/{guid}");
                return JsonConvert.DeserializeObject<ServerInfo>(result);
            }
            catch (Exception e)
            {
                //Handle exceptions here however you want
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
