<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ReferCloud" generation="1" functional="0" release="0" Id="7859062f-6fec-4a4e-8fa4-b4cb8ad2d1d4" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ReferCloudGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ReferEngine:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ReferCloud/ReferCloudGroup/LB:ReferEngine:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="ReferEngine:HttpsIn" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/ReferCloud/ReferCloudGroup/LB:ReferEngine:HttpsIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|ReferEngine:ReferEngineTestCertificate" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapCertificate|ReferEngine:ReferEngineTestCertificate" />
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
        <aCS name="ReferEngine:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </maps>
        </aCS>
        <aCS name="ReferEngine:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngine:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="ReferEngineInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngineInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ReferEngine:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ReferEngine:HttpsIn">
          <toPorts>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/HttpsIn" />
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
        <map name="MapCertificate|ReferEngine:ReferEngineTestCertificate" kind="Identity">
          <certificate>
            <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/ReferEngineTestCertificate" />
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
        <map name="MapReferEngine:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </setting>
        </map>
        <map name="MapReferEngine:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngine:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapReferEngineInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ReferCache" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferCloud\ReferCloud\csx\Release\roles\ReferCache" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
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
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferCache&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferCache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
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
          <role name="ReferEngine" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferCloud\ReferCloud\csx\Release\roles\ReferEngine" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="HttpsIn" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/ReferEngineTestCertificate" />
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
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferEngine&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferCache&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ReferEngine&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0ReferEngineTestCertificate" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/ReferEngineTestCertificate" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="ReferEngineTestCertificate" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ReferCloud/ReferCloudGroup/ReferEngineFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ReferEngineUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="ReferCacheUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ReferCacheFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="ReferEngineFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ReferCacheInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ReferEngineInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a13d2272-c25e-4d55-8c91-8692d829cffb" ref="Microsoft.RedDog.Contract\ServiceContract\ReferCloudContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="540c1a45-d267-4abc-8348-b6ce3b7ef7d9" ref="Microsoft.RedDog.Contract\Interface\ReferEngine:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="cbcc0058-d561-43e0-8c37-1241d902473e" ref="Microsoft.RedDog.Contract\Interface\ReferEngine:HttpsIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine:HttpsIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>