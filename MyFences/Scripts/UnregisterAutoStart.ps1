$taskName = "MyFences"

if (Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue) {
    Unregister-ScheduledTask -TaskName $taskName -Confirm:$false
    Write-Output "Task '$taskName' was found and removed."
} else {
    Write-Output "Task '$taskName' does not exist."
}