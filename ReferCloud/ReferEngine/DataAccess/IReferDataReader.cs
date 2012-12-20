using ReferLib;

namespace ReferEngineWeb.DataAccess
{
    public interface IReferDataReader
    {
        App GetApp(long id);
        App GetApp(string packageFamilyName);
        AppAuthorization GetAppAuthorization(string token, string userHostAddress);
        FacebookOperations GetFacebookOperations(string referEngineAuthToken);
    }
}
