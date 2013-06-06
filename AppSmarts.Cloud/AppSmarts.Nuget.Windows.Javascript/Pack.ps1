del .\AppSmarts.Nuget.Windows.Javascript\AppSmarts.Javascript.*.nupkg
nuget pack .\AppSmarts.Nuget.Windows.Javascript\Package.nuspec -outputdirectory .\AppSmarts.Nuget.Windows.Javascript


#$out = nuget pack AppSmarts.Nuget.Windows.Javascript\Package.nuspec AppSmarts.Nuget.Windows.Javascript\.
#nuget push $out[1].substring($out[1].indexOf("C:\"), $out[1].indexOf(".nupkg") - $out[1].indexOf("C:\") + 6)
