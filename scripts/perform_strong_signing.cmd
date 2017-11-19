@ECHO OFF

REM This file invokes the best sn.exe possible on a given assembly (%1)

SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

IF ERRORLEVEL 1 (

	ECHO This script requires extensions and delayed expansion

	EXIT /B 1
)

IF {%2} == {} (

	ECHO USAGE: %~dpn0^(%~x0^) ^<target-path^> ^<key-path^>

	EXIT /B 1
)

SET TargetPath_=%1
SET TargetPath="%TargetPath_:"=%"
SET KeyPath_=%2
SET KeyPath="%KeyPath_:"=%"

IF NOT DEFINED SS_SN_32 (

	IF NOT DEFINED SS_SN_64 (

		ECHO Neither SS_SN_32 nor SS_SN_64 environment variables are defined

		EXIT /B 1
	)
)

IF DEFINED SS_SN_32 (

	SET SsSn32="%SS_SN_32:"=%"

	ECHO Executing !SsSn32! to strongly sign assembly !TargetPath! with !KeyPath! ...

	!SsSn32! -R !TargetPath! !KeyPath!
) ELSE (

	SET SsSn64="%SS_SN_64:"=%"

	ECHO Executing !SsSn64! to strongly sign assembly !TargetPath! with !KeyPath! ...

	!SsSn64! -R !TargetPath! !KeyPath!
)

REM ############################ end of file ############################# #

