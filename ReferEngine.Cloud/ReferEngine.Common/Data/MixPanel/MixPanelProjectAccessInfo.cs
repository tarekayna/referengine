using System;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Data.MixPanel
{
    public static class MixPanelProjectAccessInfo
    {
        public static string ApiKey
        {
            get
            {
                switch (Util.CurrentServiceConfiguration)
                {
                    case Util.ReferEngineServiceConfiguration.ProductionCloud:
                        return "947b3ac6fe666f3b619ec16d14d2519f";
                    case Util.ReferEngineServiceConfiguration.TestCloud:
                        return "18408f2e6580685f17f44df585f87e16";
                    case Util.ReferEngineServiceConfiguration.Local:
                        return "7dc11fac8b86e5ec4a01c122084aadf6";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static string ApiSecret
        {
            get
            {
                switch (Util.CurrentServiceConfiguration)
                {
                    case Util.ReferEngineServiceConfiguration.ProductionCloud:
                        return "21c80f6cb5ef61129dfb03161b5c7391";
                    case Util.ReferEngineServiceConfiguration.TestCloud:
                        return "67ce626c984928b745ff65649fd5b456";
                    case Util.ReferEngineServiceConfiguration.Local:
                        return "b87f6cf9d8a9af2ba80a5ab1c829da9e";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static string Token
        {
            get
            {
                switch (Util.CurrentServiceConfiguration)
                {
                    case Util.ReferEngineServiceConfiguration.ProductionCloud:
                        return "d76136086d701abbecf55a6de775127c";
                    case Util.ReferEngineServiceConfiguration.TestCloud:
                        return "8b975bba223e30932a1ef8cd028f1c1c";
                    case Util.ReferEngineServiceConfiguration.Local:
                        return "6a8d387692d3e39d9b7851edbfcc9b8d";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}