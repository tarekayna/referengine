using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReferEngine.Common.Models
{
    public enum AppRewardPlanType
    {
        None = 0,
        Cash,
        Custom
    }

    [DataContract]
    public class AppRewardPlan
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public AppRewardPlanType Type { get; set; }

        [DataMember]
        public int NumberOfRecommendations { get; set; }

        [DataMember]
        public double CashAmount { get; set; }
    }
}
