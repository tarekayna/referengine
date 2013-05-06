using System.Diagnostics;
using Facebook;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ReferEngine.Common.Data
{
    [DataContract]
    public class FacebookAccessToken
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public DateTime ExpiresAt { get; set; }

        public FacebookAccessToken(string token, DateTime expiresAt)
        {
            Token = token;
            ExpiresAt = expiresAt;
        }

        public bool IsExpired()
        {
            return DateTime.Now.CompareTo(ExpiresAt) > 0;
        }
    }

    [DataContract]
    public class FacebookOperations
    {
        private const string ReferEngineAppId = "368842109866922";
        private const string ReferEngineAppSecret = "b673f45aa978225ae8c9e4817a726be7";
        private const string RequestQuery = "fields=id,name,devices,first_name,last_name,email," +
                                            "gender,address,birthday,picture,relationship_status," + 
                                            "timezone,verified,work";

        private FacebookClient _facebookClient;
        private FacebookClient FacebookClient
        {
            get { return _facebookClient ?? (_facebookClient = new FacebookClient(AccessToken.Token)); }
        }

        [DataMember]
        public string ReferEngineAuthToken { get; set; }

        [DataMember]
        private FacebookAccessToken AccessToken { get; set; }

        [DataMember]
        private DateTime AccessTokenExpiresAt { get; set; }

        public FacebookOperations(FacebookAccessToken accessToken)
        {
            AccessToken = accessToken;
        }

        public FacebookOperations(FacebookAccessToken accessToken, string referEngineAuthToken)
        {
            AccessToken = accessToken;
            ReferEngineAuthToken = referEngineAuthToken;
        }

        [DataMember]
        private Person _currentUser;
        public async Task<Person> GetCurrentUserAsync()
        {
            if (_currentUser == null)
            {
                string path = string.Format("me?{0}", RequestQuery);
                dynamic me = await FacebookClient.GetTaskAsync(path);
                _currentUser = new Person(me);
            }
            return _currentUser;
        }

        public Person GetCurrentUser()
        {
            if (_currentUser == null)
            {
                string path = string.Format("me?{0}", RequestQuery);
                dynamic me = FacebookClient.Get(path);
                _currentUser = new Person(me);
            }
            return _currentUser;
        }
        
        [DataMember]
        private IList<Person> _friends;
        public async Task<IList<Person>> GetFriendsAsync()
        {
            if (_friends == null)
            {
                _friends = new List<Person>();
                string path = string.Format("me/friends?{0}", RequestQuery);
                dynamic friends = await FacebookClient.GetTaskAsync(path);
                for (int i = 0; i < friends.data.Count; i++)
                {
                    _friends.Add(new Person(friends.data[i]));
                }
            }
            return _friends;
        }

        public IList<Person> GetFriends()
        {
            if (_friends == null)
            {
                _friends = new List<Person>();
                string path = string.Format("me/friends?{0}", RequestQuery);
                dynamic friends = FacebookClient.Get(path);
                for (int i = 0; i < friends.data.Count; i++)
                {
                    _friends.Add(new Person(friends.data[i]));
                }
            }
            return _friends;
        }

        // BUG: Do I need to change to require app token?

        public async Task<AppRecommendation> PostAppRecommendationAsync(App app, AppRecommendation appRecommendation)
        {
            string appParameter = string.Format("{0}/app-store/windows/{1}", Util.BaseUrl, app.LinkPart);
            var parameters = new Dictionary<string, object>();
            parameters["app"] = appParameter;
            parameters["access_token"] = AccessToken.Token;
            parameters["fb:explicitly_shared"] = "true";
            parameters["message"] = appRecommendation.UserMessage;
            dynamic postResult = await FacebookClient.PostTaskAsync("me/referengine:recommend", parameters);
            Int64 postId;
            if (Util.TryConvertToInt64(postResult.id, out postId))
            {
                appRecommendation.FacebookPostId = postId;
                return appRecommendation;
            }

            return null;
        }

        #region Static Methods
        public static async Task<FacebookOperations> CreateAsync(string accessCode, string referEngineAuthToken)
        {
            FacebookAccessToken accessToken = await ExchangeCodeForTokenAsync(accessCode);
            return new FacebookOperations(accessToken, referEngineAuthToken);
        }

        public static async Task<FacebookAccessToken> ExchangeCodeForTokenAsync(string accessCode)
        {
            string query = String.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                                         ReferEngineAppId,
                                         "https://www.referengine.com/recommend/win8/success",
                                         ReferEngineAppSecret,
                                         accessCode);
            UriBuilder uriBuilder = new UriBuilder("https", "graph.facebook.com")
            {
                Path = "oauth/access_token",
                Query = query
            };

            HttpClient httpClient = new HttpClient();

            string response;
            for (int i = 0; true; i++)
            {
                if (i == 2)
                {
                    HttpResponseMessage responseMessage = await httpClient.GetAsync(uriBuilder.Uri);
                    responseMessage.EnsureSuccessStatusCode();
                    response = await responseMessage.Content.ReadAsStringAsync();
                    break;
                }

                try
                {
                    HttpResponseMessage responseMessage = await httpClient.GetAsync(uriBuilder.Uri);
                    responseMessage.EnsureSuccessStatusCode();
                    response = await responseMessage.Content.ReadAsStringAsync();
                    break;
                }
                catch (HttpRequestException e)
                {
                    Trace.TraceError(e.Message);
                }
            }

            const string str = "access_token=";
            const string expiresStr = "&expires=";
            int start = response.IndexOf(str, StringComparison.Ordinal) + str.Length;
            int end = response.IndexOf(expiresStr, StringComparison.Ordinal);
            string token = response.Substring(start, end - start);
            int expiresIn = Convert.ToInt32(response.Substring(end + expiresStr.Length));
            DateTime expiresAt = DateTime.Now.AddSeconds(expiresIn);
            return new FacebookAccessToken(token, expiresAt);
        }

        public static async Task<FacebookAccessToken> ExchangeToken(string accessToken)
        {
            string query = String.Format("client_id={0}&client_secret={1}&grant_type=fb_exchange_token&fb_exchange_token={2}",
                                         ReferEngineAppId,
                                         ReferEngineAppSecret,
                                         accessToken);
            UriBuilder uriBuilder = new UriBuilder("https", "graph.facebook.com")
            {
                Path = "oauth/access_token",
                Query = query
            };

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(uriBuilder.Uri);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            const string str = "access_token=";
            const string expiresStr = "&expires=";
            int start = response.IndexOf(str, StringComparison.Ordinal) + str.Length;
            int end = response.IndexOf(expiresStr, StringComparison.Ordinal);
            string token = response.Substring(start, end - start);
            int expiresIn = Convert.ToInt32(response.Substring(end + expiresStr.Length));
            DateTime expiresAt = DateTime.Now.AddSeconds(expiresIn);
            return new FacebookAccessToken(token, expiresAt);
        }
        #endregion Static Methods
    }
}