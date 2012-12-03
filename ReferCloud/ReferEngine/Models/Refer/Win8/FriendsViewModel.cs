using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReferLib;

namespace ReferEngine.Models.Refer.Win8
{
    public class FriendsViewModel
    {
        public Person Me { get; private set; }
        public dynamic Friends { get; private set; }
        public App App { get; private set; }
        public string UserAccessToken { get; set; }

        public FriendsViewModel(dynamic me, dynamic friends, App app, string userAccessToken)
        {
            Me = me;
            Friends = friends;
            App = app;
            UserAccessToken = userAccessToken;
        }
    }
}