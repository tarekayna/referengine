using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.Data
{
    public static class IpCheckOperations
    {
        public static IpAddressLocation CheckIpAddress(string ipAddress)
        {
            var uri = new Uri("https://api.datamarket.azure.com/Data.ashx/MelissaData/IPCheck/v1/");
            var client = new IpCheckContainer(uri)
                             {
                                 Credentials = new NetworkCredential("accountKey", "m7k+1WAGXgcgy4GLpZJpdcUnudxc7DUKqV/xvc3pRrk=")
                             };

            var marketData = client.SuggestIpAddresses(ipAddress, 1, 0.7).Execute();

            return marketData != null ? marketData.ToList().First() : null;
        }
    }

    internal class IpCheckContainer : DataServiceContext
    {
        public IpCheckContainer(Uri serviceRoot) : base(serviceRoot) { }

        public DataServiceQuery<IpAddressLocation> SuggestIpAddresses(String ipAddress, Int32 maximumSuggestions, Double minimumConfidence)
        {
            if (ipAddress == null) throw new ArgumentNullException("ipAddress");

            DataServiceQuery<IpAddressLocation> query = CreateQuery<IpAddressLocation>("SuggestIPAddresses")
                                                        .AddQueryOption("MaximumSuggestions", maximumSuggestions)
                                                        .AddQueryOption("MinimumConfidence", minimumConfidence)
                                                        .AddQueryOption("IPAddress", string.Concat("\'", Uri.EscapeDataString(ipAddress), "\'"));
            return query;
        }
    }
}
