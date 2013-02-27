using Glimpse.Core.Extensibility;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web
{
    public class GlimpseSecurityPolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            if (Util.CurrentServiceConfiguration == Util.ReferEngineServiceConfiguration.Local ||
                Util.CurrentServiceConfiguration == Util.ReferEngineServiceConfiguration.TestCloud)
            {
                return RuntimePolicy.On;
            }

            return RuntimePolicy.Off;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }
    }
}