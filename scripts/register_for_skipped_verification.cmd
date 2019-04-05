@ECHO OFF

REM This file attempts to register an assembly (%1) for verification
REM skipping, for a given configuration (%2)

SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

IF ERRORLEVEL 1 (

	ECHO This script requires extensions and delayed expansion

	EXIT /B 1
)

IF {%1} == {} (

	ECHO USAGE: %~dpn0^(%~x0^) ^<target-path^> ^[ ^<configuration-name^> ^]

	EXIT /B 1
)

IF NOT {%2} == {} (

	SET Configuration_=%2
	SET Configuration="%Configuration_:"=%"

	IF /I NOT {%2} == {"Release"} (

		ECHO Skipping verification skipping for configuration '%2' ...

		EXIT /B 0
	)
)

SET TargetPath_=%1
SET TargetPath="%TargetPath_:"=%"

IF NOT DEFINED SS_SN_32 (

	IF NOT DEFINED SS_SN_64 (

		ECHO Neither SS_SN_32 nor SS_SN_64 environment variables are defined

		EXIT /B 1
	)
)

IF DEFINED SS_SN_32 (

	SET SsSn32="%SS_SN_32:"=%"

	ECHO Registering %TargetPath% for 32-bit verification skipping with !SsSn32! ...

	!SsSn32! -q -Vr %TargetPath%
)

IF DEFINED SS_SN_64 (

	SET SsSn64="%SS_SN_64:"=%"

	ECHO Registering %TargetPath% for 64-bit verification skipping with !SsSn64! ...

	!SsSn64! -q -Vr %TargetPath%
)

REM ############################ end of file ############################# #

