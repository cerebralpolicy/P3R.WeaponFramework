# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/P3R.WeaponFramework/*" -Force -Recurse
dotnet publish "./P3R.WeaponFramework.csproj" -c Release -o "$env:RELOADEDIIMODS/P3R.WeaponFramework" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location