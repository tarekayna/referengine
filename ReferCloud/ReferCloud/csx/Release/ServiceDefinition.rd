<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ReferCloud" generation="1" functional="0" release="0" Id="f730a940-682b-4e80-9b1a-282d55396e73" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ReferCloudGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ReferEngine:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ReferCloud/ReferCloudGroup/LB:ReferEngine:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ReferEngine:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReferCloud/ReferCloudGroup/MapReferEngine:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
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
      </channels>
      <maps>
        <map name="MapReferEngine:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
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
          <role name="ReferEngine" generation="1" functional="0" release="0" software="C:\Users\Tarek\Documents\GitHub\referengine\ReferCloud\ReferCloud\csx\Release\roles\ReferEngine" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ReferEngine&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ReferEngine&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
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
        <sCSPolicyFaultDomain name="ReferEngineFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ReferEngineInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="ec83d00a-4a60-49ba-b09f-231b593158b9" ref="Microsoft.RedDog.Contract\ServiceContract\ReferCloudContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="077551d0-bf0a-473a-9570-9796345b8a2e" ref="Microsoft.RedDog.Contract\Interface\ReferEngine:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ReferCloud/ReferCloudGroup/ReferEngine:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>