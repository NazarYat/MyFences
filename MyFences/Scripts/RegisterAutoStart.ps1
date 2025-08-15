param (
    [Parameter(Mandatory = $true)]
    [string]$InstallFolder
)

$exePath = Join-Path $InstallFolder "MyFences.exe"

$action = New-ScheduledTaskAction -Execute $exePath
$trigger = New-ScheduledTaskTrigger -AtLogOn

Register-ScheduledTask -Action $action -Trigger $trigger -TaskName 'MyFences' -Description 'Starts MyFences at login' -User $env:USERNAME -Force

Write-Host "'My fences' successfully added to the scheduled tasks. Press any key to continue..."
Read-Host