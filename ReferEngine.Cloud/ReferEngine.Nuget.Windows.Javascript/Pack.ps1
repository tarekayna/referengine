del .\ReferEngine.Nuget.Windows.Javascript\ReferEngine.Windows.Javascript.*.nupkg
nuget pack .\ReferEngine.Nuget.Windows.Javascript\Package.nuspec -outputdirectory .\ReferEngine.Nuget.Windows.Javascript


#$out = nuget pack ReferEngine.Nuget.Windows.Javascript\Package.nuspec ReferEngine.Nuget.Windows.Javascript\.
#nuget push $out[1].substring($out[1].indexOf("C:\"), $out[1].indexOf(".nupkg") - $out[1].indexOf("C:\") + 6)
