@ECHO OFF

REM Path to the folder with .NET reference assemblies:
SET DOTNET="C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1"

REM Full path to the Visual Studio Build Tools compiler:
SET MSBUILD="C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"

REM Full path to the NuGet utility:
SET NUGET="C:\Utils\NuGet\NuGet.exe"

REM Directory where the solution file is:
SET SOLUTION_DIR="C:\Builds\WebAppsAutomated\Source\WebApps\AuthService"

REM Full path to the solution file:
SET SOLUTION="%SOLUTION_DIR%\AuthService.sln"

REM ******* Executing the commands here ***********

REM Installing all the NuGet packages required by the project:
%NUGET% install "%SOLUTION_DIR%\packages.config" -OutputDirectory "%SOLUTION_DIR%\packages"

REM Building the solution:
%MSBUILD% %SOLUTION% /p:Configuration=Release /t:AuthService /p:FrameworkPathOverride=%DOTNET%

REM ******* Copying deployment files ***********

XCOPY /Q %SOLUTION_DIR%\AuthService.xml build\
XCOPY /Q %SOLUTION_DIR%\Web.config build\
XCOPY /Q %SOLUTION_DIR%\Global.asax build\
XCOPY /Q %SOLUTION_DIR%\bin\*.dll build\bin\
XCOPY /Q %SOLUTION_DIR%\bin\*.xml build\bin\
