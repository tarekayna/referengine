<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ReferCloud" generation="1" functional="0" release="0" Id="1f62c0a2-a4a5-40ee-8b6e-497155aae0db" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ReferCloudGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ReferEngine.Web:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ReferCloud/ReferCloudGroup/LB:ReferEngine.Web:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="ReferEngine.Web:HttpsIn" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/ReferCloud/ReferCloudGroup/LB:ReferEngine.Web:HttpsIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|ReferEngine.Web:ReferEngineTestCertificate" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapCertificate|ReferEngine.Web:ReferEngineTestCertificate" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web.CacheInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web.CacheInstances" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Web:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Web:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.WebInstances" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Workers.DataWriter:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Workers.DataWriter:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Workers.DataWriter:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Workers.DataWriter:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Workers.DataWriter:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Workers.DataWriter:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Workers.DataWriterInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Workers.DataWriterInstances" />
          </maps>
        </aCS>
        <aCS name="ReferEngine.Workers.PingerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine.Workers.PingerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ReferEngine.Web:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ReferEngine.Web:HttpsIn">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web/HttpsIn" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|ReferEngine.Web:ReferEngineTestCertificate" kind="Identity">
          <certificate>
            <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web/ReferEngineTestCertificate" />
          </certificate>
        </map>
        <map name="MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" />
          </setting>
        </map>
        <map name="MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" />
          </setting>
        </map>
        <map name="MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" />
          </setting>
        </map>
        <map name="MapReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.Cache/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngine.Web.CacheInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.CacheInstances" />
          </setting>
        </map>
        <map name="MapReferEngine.Web:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web/Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </setting>
        </map>
        <map name="MapReferEngine.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngine.Web:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngine.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.WebInstances" />
          </setting>
        </map>
        <map name="MapReferEngine.Workers.DataWriter:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.DataWriter/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngine.Workers.DataWriter:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.DataWriter/Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </setting>
        </map>
        <map name="MapReferEngine.Workers.DataWriter:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.DataWriter/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngine.Workers.DataWriterInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.DataWriterInstances" />
          </setting>
        </map>
        <map name="MapReferEngine.Workers.PingerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.PingerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ReferEngine.Web" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferEngine.Cloud\ReferEngine.Cloud\csx\Release\roles\ReferEngine.Web" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="3584" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="HttpsIn" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web/ReferEngineTestCertificate" />
                </certificate>
              </inPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferEngine.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferEngine.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine.Web.Cache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine.Workers.DataWriter&quot; /&gt;&lt;r name=&quot;ReferEngine.Workers.Pinger&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0ReferEngineTestCertificate" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web/ReferEngineTestCertificate" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="ReferEngineTestCertificate" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.WebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.WebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.WebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="ReferEngine.Web.Cache" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferEngine.Cloud\ReferEngine.Cloud\csx\Release\roles\ReferEngine.Web.Cache" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp" />
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferEngine.Web.Cache&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferEngine.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine.Web.Cache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine.Workers.DataWriter&quot; /&gt;&lt;r name=&quot;ReferEngine.Workers.Pinger&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[20000,20000,20000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.CacheInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.CacheUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web.CacheFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="ReferEngine.Workers.DataWriter" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferEngine.Cloud\ReferEngine.Cloud\csx\Release\roles\ReferEngine.Workers.DataWriter" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferEngine.Workers.DataWriter&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferEngine.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine.Web.Cache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine.Workers.DataWriter&quot; /&gt;&lt;r name=&quot;ReferEngine.Workers.Pinger&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.DataWriterInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.DataWriterUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.DataWriterFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="ReferEngine.Workers.Pinger" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferEngine.Cloud\ReferEngine.Cloud\csx\Release\roles\ReferEngine.Workers.Pinger" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="768" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
                </outToChannel>
              </outPort>
              <outPort name="ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferEngine.Web.Cache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferEngine.Workers.Pinger&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferEngine.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine.Web.Cache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine.Workers.DataWriter&quot; /&gt;&lt;r name=&quot;ReferEngine.Workers.Pinger&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.PingerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.PingerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Workers.PingerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ReferEngine.WebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="ReferEngine.Web.CacheUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="ReferEngine.Workers.DataWriterUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="ReferEngine.Workers.PingerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ReferEngine.WebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="ReferEngine.Web.CacheFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="ReferEngine.Workers.DataWriterFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="ReferEngine.Workers.PingerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ReferEngine.Web.CacheInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ReferEngine.WebInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ReferEngine.Workers.DataWriterInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ReferEngine.Workers.PingerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="5e22a0d3-db0a-48cc-b228-c31ef6018772" ref="Microsoft.RedDog.Contract\ServiceContract\ReferCloudContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="c49697e8-025d-4b1a-ab5d-308b9a48a468" ref="Microsoft.RedDog.Contract\Interface\ReferEngine.Web:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="bcb6c10c-a06c-4ff6-aa1e-8462f2b7a176" ref="Microsoft.RedDog.Contract\Interface\ReferEngine.Web:HttpsIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine.Web:HttpsIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>