<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ReferCloud" generation="1" functional="0" release="0" Id="41f665a0-1842-4f85-80cf-48d7ec43c7a4" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ReferCloudGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ReferEngineWeb:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ReferCloud/ReferCloudGroup/LB:ReferEngineWeb:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="ReferEngineWeb:HttpsIn" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/ReferCloud/ReferCloudGroup/LB:ReferEngineWeb:HttpsIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|ReferEngineWeb:ReferEngineTestCertificate" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapCertificate|ReferEngineWeb:ReferEngineTestCertificate" />
          </maps>
        </aCS>
        <aCS name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferCache:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" />
          </maps>
        </aCS>
        <aCS name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferCache:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferCache:Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" />
          </maps>
        </aCS>
        <aCS name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferCache:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" />
          </maps>
        </aCS>
        <aCS name="ReferCache:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferCache:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferCacheInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferCacheInstances" />
          </maps>
        </aCS>
        <aCS name="ReferDataWorker:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferDataWorker:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferDataWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferDataWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferDataWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferDataWorkerInstances" />
          </maps>
        </aCS>
        <aCS name="ReferEngineWeb:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngineWeb:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </maps>
        </aCS>
        <aCS name="ReferEngineWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngineWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngineWeb:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngineWeb:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngineWebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngineWebInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ReferEngineWeb:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ReferEngineWeb:HttpsIn">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb/HttpsIn" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|ReferEngineWeb:ReferEngineTestCertificate" kind="Identity">
          <certificate>
            <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb/ReferEngineTestCertificate" />
          </certificate>
        </map>
        <map name="MapReferCache:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" />
          </setting>
        </map>
        <map name="MapReferCache:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" />
          </setting>
        </map>
        <map name="MapReferCache:Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" />
          </setting>
        </map>
        <map name="MapReferCache:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" />
          </setting>
        </map>
        <map name="MapReferCache:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferCache/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferCacheInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferCacheInstances" />
          </setting>
        </map>
        <map name="MapReferDataWorker:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferDataWorker/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferDataWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferDataWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferDataWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferDataWorkerInstances" />
          </setting>
        </map>
        <map name="MapReferEngineWeb:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb/Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </setting>
        </map>
        <map name="MapReferEngineWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngineWeb:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngineWebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWebInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ReferCache" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferCloud\ReferCloud\csx\Debug\roles\ReferCache" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp" />
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.DiagnosticLevel" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferCache&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferCache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferDataWorker&quot; /&gt;&lt;r name=&quot;ReferEngineWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[20000,20000,20000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferCacheInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferCacheUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferCacheFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="ReferDataWorker" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferCloud\ReferCloud\csx\Debug\roles\ReferDataWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferDataWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferCache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferDataWorker&quot; /&gt;&lt;r name=&quot;ReferEngineWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferDataWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferDataWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferDataWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="ReferEngineWeb" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferCloud\ReferCloud\csx\Debug\roles\ReferEngineWeb" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="HttpsIn" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb/ReferEngineTestCertificate" />
                </certificate>
              </inPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
                </outToChannel>
              </outPort>
              <outPort name="ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ReferCloud/ReferCloudGroup/SW:ReferCache:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferEngineWeb&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferCache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferDataWorker&quot; /&gt;&lt;r name=&quot;ReferEngineWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0ReferEngineTestCertificate" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb/ReferEngineTestCertificate" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="ReferEngineTestCertificate" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ReferEngineWebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="ReferCacheUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="ReferDataWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ReferCacheFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="ReferDataWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="ReferEngineWebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ReferCacheInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ReferDataWorkerInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ReferEngineWebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="c72f4a54-6150-427f-9b8e-ade1ade7848f" ref="Microsoft.RedDog.Contract\ServiceContract\ReferCloudContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="353eeaf8-500f-4739-b4a0-d2e31d89e037" ref="Microsoft.RedDog.Contract\Interface\ReferEngineWeb:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="dbfc62d2-6416-421e-94f9-47288f5a3d40" ref="Microsoft.RedDog.Contract\Interface\ReferEngineWeb:HttpsIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineWeb:HttpsIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>