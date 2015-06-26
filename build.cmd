@ECHO OFF
cd %~dp0

SETLOCAL
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
.nuget\NuGet.exe restore british-proverbs.sln

:octopustools
IF EXIST packages\OctopusTools goto run
.nuget\nuget.exe install OctopusTools -ExcludeVersion -o packages -Version 2.6.3.59 -nocache

:run
IF "%1" == "" (

  @powershell -NoProfile -ExecutionPolicy unrestricted -File "scripts\build.ps1" "-buildParams /p:Configuration=Release"
  
) else (

  @powershell -NoProfile -ExecutionPolicy unrestricted -File "scripts\build.ps1" -buildParams "/p:Configuration=%1%"
)

:packapp
@powershell -NoProfile -ExecutionPolicy unrestricted -File "scripts\pack-app.ps1"