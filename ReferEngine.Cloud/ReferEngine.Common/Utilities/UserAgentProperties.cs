namespace ReferEngine.Common.Utilities
{
    public class UserAgentProperties
    {
        public bool IsWindows8 { get; private set; }
        public bool SupportsProtocolLaunching { get; private set; }

        public UserAgentProperties(string userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                IsWindows8 = userAgent.Contains("Windows NT 6.2");
                SupportsProtocolLaunching = !userAgent.Contains("Chrome") && !userAgent.Contains("Opera");
            }
            {
                IsWindows8 = false;
                SupportsProtocolLaunching = false;
            }
        }
    }
}
