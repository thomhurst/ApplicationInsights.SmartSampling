@echo off
set /p version="Enter Version Number to Build With: "

@echo on
dotnet pack ".\TomLonghurst.ApplicationInsights.SmartSampling\TomLonghurst.ApplicationInsights.SmartSampling.csproj"  --configuration Release /p:Version=%version%

pause