param (
  [Parameter(Mandatory = $true)][string]$AppObjectId,
  [Parameter(Mandatory = $true)][string]$GithubRepo,
  [Parameter(Mandatory = $true)][string][ValidateSet("branch", "environment")]$SubjectType,
  [Parameter(Mandatory = $true)][string]$SubjectValue,
  [Parameter(Mandatory = $true)][string]$CredentialName
)

if ($SubjectType -eq "branch") {
    $subject = "repo:$($GithubRepo):ref:refs/heads/$($SubjectValue)"
} elseif ($SubjectType -eq "environment") {
    $subject = "repo:$($GithubRepo):environment:$($SubjectValue)"
} else {
    Write-Error "SubjectType must be either 'branch' or 'environment'"
    exit 1
}

$tempFile = New-TemporaryFile

@"
{
  "name": "$CredentialName",
  "issuer": "https://token.actions.githubusercontent.com",
  "subject": "$subject",
  "audiences": ["api://AzureADTokenExchange"]
}
"@ | Out-File -Encoding utf8 -FilePath $tempFile.FullName

az ad app federated-credential create `
  --id $AppObjectId `
  --parameters "@$($tempFile.FullName)"

Remove-Item $tempFile.FullName -Force