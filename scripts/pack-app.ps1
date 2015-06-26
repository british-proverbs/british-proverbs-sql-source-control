$currentPath = Split-Path -parent $MyInvocation.MyCommand.Definition
$artifactsPath = join-path $currentPath "..\artifacts"
$artifactsPackagesPath = join-path $currentPath "packages" 
$appPath = join-path $artifactsPath "_PublishedWebsites\BritishProverbs.Web\**"
$octoPath = join-path $currentPath "..\packages\OctopusTools"
$octoExePath = join-path $octoPath "octo.exe"
$buildNumber = $env:BUILD_NUMBER
if($buildNumber -eq $null)
{
    $buildNumber = "0.9.9.9999"
}
    
& "$octoExePath" pack --include="$appPath" --outFolder="$artifactsPackagesPath" --id="british-proverbs-app" --version="$buildNumber"