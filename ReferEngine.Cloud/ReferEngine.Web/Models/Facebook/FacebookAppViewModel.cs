using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Models.Facebook
{
    public class FacebookAppViewModel
    {
        public App App { get; private set; }
        public UserAgentProperties UserAgentProperties { get; private set; }

        public FacebookAppViewModel(App app, UserAgentProperties userAgentProperties)
        {
            App = app;
            UserAgentProperties = userAgentProperties;
        }
    }
}