$params = @{
  Name = "NTBSendService"
  BinaryPathName = '"d:\_pjt\_net\NTB\SenderService\SenderService\bin\Release\netcoreapp3.1\publish\SenderService.exe"'
  DisplayName = "NTBSendService"
  StartupType = "Manual"
  Description = "Message delivery system"
}
New-Service @params