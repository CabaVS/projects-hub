param (
  [Parameter(Mandatory = $true)][string]$resourceGroupName,
  [Parameter(Mandatory = $true)][string]$location,
  [Parameter(Mandatory = $true)][string]$sqlAdminsGroupName,
  [Parameter(Mandatory = $true)][string]$storageAccountName,
  [Parameter(Mandatory = $true)][string]$githubAppName
)

$terraformStateContainerName = 'terraform-state'
$appConfigsContainerName = 'app-configs'

$tenantId = az account show --query tenantId -o tsv
$subId = az account show --query id -o tsv

# Create Resource Group
Write-Host "Creating a Resource Group $resourceGroupName..."
az group create `
  --name $resourceGroupName `
  --location $location

if ($LASTEXITCODE -ne 0) {
  Write-Host "Failed to create Resource Group." -ForegroundColor Red
  exit 1
}
else {
  Write-Host "Resource Group is created."
}

# Create Storage Account
Write-Host "Creating Storage Account $storageAccountName..."
az storage account create `
  --name $storageAccountName `
  --resource-group $resourceGroupName `
  --location $location `
  --sku Standard_LRS `
  --kind StorageV2 `
  --enable-hierarchical-namespace false `
  --allow-blob-public-access false `
  --min-tls-version TLS1_2

if ($LASTEXITCODE -ne 0) {
  Write-Host "Failed to create Storage Account." -ForegroundColor Red
  exit 1
}
else {
  Write-Host "Storage Account created."
}

# Enable blob versioning
Write-Host "Enabling versioning..."
az storage account blob-service-properties update `
  --account-name $storageAccountName `
  --enable-versioning true

# Enable soft delete
Write-Host "Enabling soft delete for blobs..."
az storage blob service-properties delete-policy update `
  --account-name $storageAccountName `
  --enable true `
  --days-retained 7

# Create required containers
Write-Host "Creating required containers..."
$accountKey = az storage account keys list `
  --account-name $storageAccountName `
  --resource-group $resourceGroupName `
  --query '[0].value' -o tsv

az storage container create `
  --name $terraformStateContainerName `
  --account-name $storageAccountName `
  --account-key $accountKey `
  --public-access off

az storage container create `
  --name $appConfigsContainerName `
  --account-name $storageAccountName `
  --account-key $accountKey `
  --public-access off

# Create SP for GitHub OIDC login
Write-Host "Creating Azure AD App for GitHub OIDC..."
$app = az ad app create --display-name $githubAppName | ConvertFrom-Json
$appId = $app.appId
$appObjectId = $app.id

az ad sp create --id $appId | Out-Null
Write-Host "App registration created: App ID = $appId"

# Assign Contributor role
Write-Host "Assigning Contributor role on $resourceGroupName..."
az role assignment create `
  --assignee $appId `
  --role "Contributor" `
  --scope "/subscriptions/$subId/resourceGroups/$resourceGroupName"

# Assign User Access Administrator role
Write-Host "Assigning User Access Administrator role on $resourceGroupName..."
az role assignment create `
  --assignee $appId `
  --role "User Access Administrator" `
  --scope "/subscriptions/$subId/resourceGroups/$resourceGroupName"

# Assign Storage Blob Data Contributor
Write-Host "Granting storage access to Terraform backend..."
az role assignment create `
  --assignee $appId `
  --role "Storage Blob Data Contributor" `
  --scope "/subscriptions/$subId/resourceGroups/$resourceGroupName/providers/Microsoft.Storage/storageAccounts/$storageAccountName/blobServices/default/containers/$terraformStateContainerName"

Write-Host "OIDC setup complete!"
Write-Host "AZURE_CLIENT_ID        = $appId"
Write-Host "AZURE_TENANT_ID        = $tenantId"
Write-Host "AZURE_SUBSCRIPTION_ID  = $subId"