using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Data.MixPanel
{
    public static class MixPanelOperations
    {
        public static string GetData(MixPanelRequestInfo requestInfo)
        {            
            var queryParameters = new List<string>
                                      {
                                          string.Format("event={0}", requestInfo.Event.GetStringValue()),
                                          string.Format("name={0}", requestInfo.PropertyName),
                                          string.Format("values={0}", StringifyStringList(requestInfo.PropertyValues)),
                                          string.Format("type={0}", requestInfo.AnalysisType.GetStringValue()),
                                          string.Format("unit={0}", requestInfo.EventUnit.GetStringValue()),
                                          string.Format("interval={0}", requestInfo.Interval),
                                          string.Format("format={0}", requestInfo.ReturnFormat.GetStringValue()),
                                          string.Format("api_key={0}", MixPanelProjectAccessInfo.ApiKey),
                                          string.Format("expire={0}", DateTime.UtcNow.AddMinutes(1).Ticks),
                                          string.Format("limit={0}", 255),
                                      };
            queryParameters.Sort(string.Compare);

            StringBuilder hashSourceBuilder = new StringBuilder();
            foreach (string parameter in queryParameters)
            {
                hashSourceBuilder.Append(parameter);
            }
            hashSourceBuilder.Append(MixPanelProjectAccessInfo.ApiSecret);

            string hash = Util.GetMd5Hash(hashSourceBuilder.ToString());
            queryParameters.Add(string.Format("sig={0}", hash));

            StringBuilder queryBuilder = new StringBuilder();
            foreach (string query in queryParameters)
            {
                queryBuilder.Append(query);
                queryBuilder.Append("&");
            }

            UriBuilder uriBuilder = new UriBuilder(MixPanelBaseUri.Events.GetStringValue()) { Query = queryBuilder.ToString() };

            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(uriBuilder.Uri);
            HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
            if (response == null) throw new InvalidDataException();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new InvalidDataException();

            StreamReader streamReader = new StreamReader(responseStream);
            string result = streamReader.ReadToEnd();

            return result;
        }

        private static string StringifyStringList(IEnumerable<string> list)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (string value in list)
            {
                builder.Append("\"");
                builder.Append(value);
                builder.Append("\"");
                builder.Append(",");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }
    }
}
