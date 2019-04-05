@ECHO OFF

REM #########################################################################
REM File:		build.cmd (for recls.NET)
REM
REM Created:	26th September 2017
REM Updated:	5th April 2019
REM
REM Copyright (c) 2009-2019, Matthew Wilson and Synesis Software. All rights
REM reserved.
REM
REM #########################################################################


REM ##############################################
REM compatibility

SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

IF ERRORLEVEL 1 (

	ECHO This script requires extensions and delayed expansion

	EXIT /B 1
)

REM ##############################################
REM constants

SET dir_script=%~dp0
SET dir_project_root=%dir_script%
SET dir_build=%dir_project_root%\build\packages

SET build_msbuild=MSBUILD.exe
SET build_nologo=-nologo

SET build_solution=recls.NET.sln

SET build_target_ALL=Build
SET build_target_DEFAULT=Build
SET build_configuration_ALL=Debug;Release;
SET build_platform_ALL="Any CPU"
SET build_platform_DEFAULT="Any CPU"


REM ##############################################
REM command-line parsing

IF /I {%~1} == {--help} GOTO show_usage

IF /I {%~1} == {package} GOTO build_package

IF {%~1} == {} GOTO build_all

IF NOT {%~2} == {} GOTO build_specific

GOTO show_usage

GOTO :EOF

REM ##############################################
REM Targets


REM ################
REM show_usage()

:show_usage

ECHO USAGE: %~n0^(%~x0^) [ { --help ^| package ^<version^> ^<key-path^> ^| ^<target^> ^<configuration^> [ ^<platform^> ] ^| } ]
ECHO.
ECHO   where ^<target^> is one of { Build ^| Clean ^| Rebuild }
ECHO.

GOTO :EOF




REM ################
REM build_package()

:build_package

IF {%~3} == {} GOTO show_usage

ECHO.
ECHO Building ...
CALL %~dpnx0 Build Release
IF ERRORLEVEL 1 (

	REM
) ELSE (

	ECHO.
	ECHO Signing ...
	%dir_project_root%\scripts\sign_assemblies %3

	ECHO Packaging ...

	IF NOT EXIST %dir_build% mkdir %dir_build%

	IF {%NUGET%} == {} (

		ECHO %%NUGET%% is not defined
		EXIT /B 1
	) ELSE (

		"%NUGET:"=%" pack %dir_project_root%\buildsrc\nuget\recls.NET.nuspec -Version %2 -OutputDirectory %dir_build%
	)
)

GOTO :EOF




REM ################
REM build_all()

:build_all


ECHO building with following:
ECHO ^ ^ target(s):
for %%t in (!build_target_ALL!) do @ECHO ^ ^ ^ ^ %%t
ECHO ^ ^ configuration(s):
for %%c in (!build_configuration_ALL!) do @ECHO ^ ^ ^ ^ %%c
ECHO ^ ^ platform(s):
for %%p in (!build_platform_ALL!) do @ECHO ^ ^ ^ ^ %%p
ECHO.


for %%t in (!build_target_ALL!) do (
	for %%c in (!build_configuration_ALL!) do (
REM		for %%p in (!build_platform_ALL!) do (

			SET arg_t=%%t
			SET arg_c=%%c
REM			SET arg_p=%%p

			SET arg_p=

			CALL %~dpnx0 !arg_t! !arg_c! !arg_p!
			IF ERRORLEVEL 1 (

				EXIT /B 1
			)
REM		)
	)
)

EXIT /B 0
GOTO :EOF




REM ################
REM build_specific()

:build_specific

SET raw_target=%~1
SET raw_configuration=%~2
SET raw_platform=%~3

IF /I {!raw_target!} == {build} (
	SET raw_target=Build
)
IF /I {!raw_target!} == {clean} (
	SET raw_target=Clean
)
IF /I {!raw_target!} == {rebuild} (
	SET raw_target=Rebuild
)

IF /I {!raw_configuration!} == {debug} (
	SET raw_configuration=Debug
)
IF /I {!raw_configuration!} == {release} (
	SET raw_configuration=Release
)

SET build_target=/t:"!raw_target!"
SET build_configuration=/p:Configuration="!raw_configuration!"
IF {!raw_platform!} == {} (
SET build_platform=/p:Platform="!build_platform_DEFAULT:"=!"
) ELSE (
SET build_platform=/p:Platform="%~3"
)

ECHO Building ...
!build_msbuild! !build_nologo! !build_solution! !build_target! !build_configuration! !build_platform!

IF ERRORLEVEL 1 (

	ECHO Build failed

	EXIT /B 1
) ELSE (

	IF /I {!raw_target!} == {Clean} (

		ECHO Cleaned
	) ELSE (

		ECHO Build Succeeded
	)
)

GOTO :EOF

REM ############################ end of file ############################# #

