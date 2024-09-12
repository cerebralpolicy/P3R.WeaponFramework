# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/P3R.WeaponFramework.Search/*" -Force -Recurse
dotnet publish "./P3R.WeaponFramework.Search.csproj" -c Release -o "$env:RELOADEDIIMODS/P3R.WeaponFramework.Search" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location