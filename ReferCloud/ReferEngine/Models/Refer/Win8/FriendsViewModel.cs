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
        public string Message { get; set; }

        public FriendsViewModel(string msg)
        {
            Message = msg;
            Me = null;
            Friends = null;
            App = null;
        }

        public FriendsViewModel(dynamic me, dynamic friends, App app)
        {
            Me = me;
            Friends = friends;
            App = app;
            Message = string.Empty;
        }
    }
}