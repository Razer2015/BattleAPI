using Battlelog.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using Shared.Enums;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Battlelog
{
    public static class BattlelogClient
    {
        /// <summary>
        ///     Get post check sum used in some battlelog queries
        /// </summary>
        /// <returns></returns>
        public static string GetPostCheckSum()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://battlelog.battlefield.com/bf4/");
                request.CookieContainer = new CookieContainer();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var postChecksum = request.CookieContainer.GetCookies(response.ResponseUri)["beaker.session.id"].Value.Substring(0, 10);

                return postChecksum;
            }
            catch (Exception e)
            {
                //Handle exceptions here however you want
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Get the persona for the given soldierName
        /// </summary>
        /// <param name="soldierName"></param>
        /// <param name="postChecksum"></param>
        /// <returns></returns>
        public static Persona GetPersona(string soldierName, string postChecksum = null)
        {
            using var webClient = new GZipWebClient();

            var data = new NameValueCollection() {
                { "query", soldierName },
                { "post-check-sum", string.IsNullOrWhiteSpace(postChecksum) ? GetPostCheckSum() : postChecksum }
            };

            string searchResults = Encoding.UTF8.GetString(webClient.UploadValues($"https://battlelog.battlefield.com/bf4/search/query/", data));

            var res = JsonConvert.DeserializeObject<Response<Persona[]>>(searchResults);
            if (res.Type.Equals("success") && res.Message.Equals("RESULT") && res.Data.Length > 0)
            {
                var matches = res.Data
                    .Where(x => x.Namespace.Equals("cem_ea_id") && x.PersonaName.Equals(soldierName, StringComparison.OrdinalIgnoreCase))
                    .GroupBy(p => p.PersonaId).Select(grp => grp.FirstOrDefault());
                if (matches.Count() == 1) return matches.FirstOrDefault();

                if (matches.Count() > 1)
                {
                    throw new Exception($"An error occured while searching. Found multiple matches.");
                }
                else
                {
                    throw new Exception($"An error occured while searching. Found no matches.");
                }
            }

            return null;
        }

        /// <summary>
        ///     Get the server show data
        /// </summary>
        /// <param name="guid">Server guid</param>
        /// <param name="platform">Platform, usually pc</param>
        /// <returns></returns>
        public static dynamic GetServerShow(string guid, string platform = "pc")
        {
            try
            {
                using var webClient = new GZipWebClient();
                string result = webClient.DownloadString($"https://battlelog.battlefield.com/bf4/servers/show/{platform}/{guid}/SERVER/?json=1");

                JObject response = JObject.Parse(result);
                if (!response.TryGetValue("type", out var type))
                    throw new Exception("Request failed");

                if (!response.TryGetValue("message", out var message))
                    throw new Exception("message didn't exist");

                if (!message.ToObject<JObject>().TryGetValue("SERVER_INFO", out var serverInfo))
                    throw new Exception("SERVER_INFO didn't exist");

                return serverInfo.ToObject<dynamic>();
            }
            catch (Exception e)
            {
                //Handle exceptions here however you want
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Get the server snapshot
        /// </summary>
        /// <param name="guid">Server guid</param>
        /// <returns></returns>
        public static Snapshot GetServerSnapshot(string guid)
        {
            try
            {
                using var webClient = new GZipWebClient();
                string result = webClient.DownloadString($"https://keeper.battlelog.com/snapshot/{guid}");
                return JsonConvert.DeserializeObject<ServerInfo>(result)?
                    .Snapshot;
            }
            catch (Exception e)
            {
                //Handle exceptions here however you want
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Get users by personaNames
        /// </summary>
        /// <param name="personaNames">Array of persona names</param>
        /// <returns></returns>
        public static Response<Dictionary<ulong, DashUser>> GetUsersByPersonaNames(string[] personaNames, DashKind kind = DashKind.Light)
        {
            try
            {
                using var webClient = new GZipWebClient();
                var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);
                foreach (var personaName in personaNames)
                {
                    httpValueCollection.Add("personaNames", personaName);
                }
                httpValueCollection.Add("kind", kind.ToString().ToLower());
                var uriBuilder = new UriBuilder("https://battlelog.battlefield.com/bf4/battledash/getUsersByPersonaNames") {
                    Query = httpValueCollection.ToString()
                };
                string result = webClient.DownloadString(uriBuilder.ToString());
                return JsonConvert.DeserializeObject<Response<Dictionary<ulong, DashUser>>>(result);
            }
            catch (Exception e)
            {
                //Handle exceptions here however you want
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Get users by personaIds
        /// </summary>
        /// <param name="personaIds">Array of persona Ids</param>
        /// <returns></returns>
        public static Response<Dictionary<ulong, DashUser>> GetUsersByPersonaIds(string[] personaIds, DashKind kind = DashKind.Light)
        {
            try
            {
                using var webClient = new GZipWebClient();
                var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);
                foreach (var personaId in personaIds)
                {
                    httpValueCollection.Add("personaIds[]", personaId);
                }
                httpValueCollection.Add("kind", kind.ToString().ToLower());
                var uriBuilder = new UriBuilder("https://battlelog.battlefield.com/bf4/battledash/getUsersByPersonaIds") {
                    Query = httpValueCollection.ToString()
                };
                string result = webClient.DownloadString(uriBuilder.ToString());
                return JsonConvert.DeserializeObject<Response<Dictionary<ulong, DashUser>>>(result);
            }
            catch (Exception e)
            {
                //Handle exceptions here however you want
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Get users by userIds
        /// </summary>
        /// <param name="userIds">Array of user Ids</param>
        /// <returns></returns>
        public static Response<Dictionary<ulong, DashUser>> GetUsersByUserIds(string[] userIds, DashKind kind = DashKind.Light)
        {
            try
            {
                using var webClient = new GZipWebClient();
                var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);
                foreach (var userId in userIds)
                {
                    httpValueCollection.Add("userIds[]", userId);
                }
                httpValueCollection.Add("kind", kind.ToString().ToLower());
                var uriBuilder = new UriBuilder("https://battlelog.battlefield.com/bf4/battledash/getUsersById") {
                    Query = httpValueCollection.ToString()
                };
                string result = webClient.DownloadString(uriBuilder.ToString());
                return JsonConvert.DeserializeObject<Response<Dictionary<ulong, DashUser>>>(result);
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
