namespace ReferEngine.Common.Utilities
{
    public class UserAgentProperties
    {
        public bool IsWindows8 { get; private set; }

        public UserAgentProperties(string userAgent)
        {
            IsWindows8 = !string.IsNullOrEmpty(userAgent) && userAgent.Contains("Windows NT 6.2");
        }
    }
}
