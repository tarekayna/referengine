param($installPath, $toolsPath, $package, $project)

$scriptTag = "    <script type='text/javascript' src='/ReferEngine/ReferEngine.Initialize.js'></script>"

function getStartPagePath {
	foreach ($item in $project.ProjectItems) 
	{
	    if ($item.name -eq "package.appxmanifest") 
	    {
	        $manifestPath = $item.FileNames(0);
	        break;
	    }
	}

	[xml]$manifestContent = Get-Content -Path $manifestPath
	$startPathName = $manifestContent.Package.Applications.Application.StartPage

	foreach ($item in $project.ProjectItems) 
	{
	    if ($item.name -eq $startPathName) 
	    {
	        return $item.FileNames(0);
	    }
	}
}

$startPagePath = getStartPagePath

function removeScriptTagFromStartPage {
	$startPageContent = Get-Content -Path $startPagePath
	Set-Content $startPagePath $startPageContent[0]
	$i = 1;
	while ($i -lt $startPageContent.count)
	{
	    $line = $startPageContent[$i]
	    if ($line.indexOf($scriptTag) -eq -1)
	    {
	    	Add-Content $startPagePath $line
	    }
	    $i = $i + 1;
	}
}

removeScriptTagFromStartPage

