@echo off
REM ============================================================================
REM Smart SDK - Script de Build
REM Compila o projeto SmartSdk para Windows
REM ============================================================================

echo.
echo ========================================
echo    Smart SDK - Script de Build
echo ========================================
echo.

REM Verifica se o dotnet esta instalado
where dotnet >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERRO] dotnet nao encontrado. Instale o .NET SDK 8.0
    pause
    exit /b 1
)

echo [INFO] Limpando build anterior...
dotnet clean SmartSdk.csproj --verbosity quiet

echo [INFO] Compilando projeto...
dotnet build SmartSdk.csproj -c Release --verbosity minimal

if %errorlevel% neq 0 (
    echo.
    echo [ERRO] Falha na compilacao!
    pause
    exit /b 1
)

echo.
echo ========================================
echo    Build concluido com sucesso!
echo ========================================
echo.
echo Executavel: bin\Release\net8.0-windows\SmartSdk.exe
echo.

pause