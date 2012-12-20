using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReferLib;

namespace ReferEngineWeb.DataAccess
{
    public interface IReferDataWriter
    {
        void AddFacebookOperations(string referEngineAuthToken, FacebookOperations facebookOperations);
        void AddPersonAndFriends(CurrentUser currentUser);
        void AddAppRecommendation(AppRecommendation appRecommendation);
        void AddAppAuthorization(AppAuthorization appAuthorization, TimeSpan expiresIn);
    }
}
