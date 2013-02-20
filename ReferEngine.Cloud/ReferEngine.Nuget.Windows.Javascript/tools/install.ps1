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

function addScriptTagToStartPage {
	$startPageContent = Get-Content -Path $startPagePath
	Set-Content $startPagePath $startPageContent[0]
	$i = 1;
	$openScriptFound = 0;
	$appendedLine = 0;
	while ($i -lt $startPageContent.count)
	{
	    $line = $startPageContent[$i]
	    Add-Content $startPagePath $line
	    
	    if ($appendedLine -eq 0)
	    {
	        if ($openScriptFound -eq 0)
	        {
	            if ($line.indexOf("base.js") -ne -1)
	            {
	                $openScriptFound = 1;
	            }
	        }
	        if ($openScriptFound -eq 1)
	        {
	            if ($line.indexOf("</script") -ne -1)
	            {
	                Add-Content $startPagePath $scriptTag
	                $appendedLine = 1
	            }
	        }
	    }
	    
	    $i = $i + 1;
	}
}

addScriptTagToStartPage
