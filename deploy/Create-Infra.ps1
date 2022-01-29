Param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('dv', 'pr')]
    [string]
    $Env,

    [Parameter(Mandatory=$true)]
    [string]
    $SqlAdminLogin,

    [Parameter(Mandatory=$true)]
    [string]
    $SqlAdminPswd
)

$location = "West Europe"

$rg = New-AzResourceGroup -Name "rg-tg-bot-$Env" -Location "$location"

# Create Azure SQL database
$sqlCredentials = $(New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $SqlAdminLogin, $(ConvertTo-SecureString -String $SqlAdminPswd -AsPlainText -Force))

$server = New-AzSqlServer -ResourceGroupName $rg.ResourceGroupName `
    -ServerName "sqlserv-tg-bot-$Env" `
    -Location "$location" `
    -SqlAdministratorCredentials $sqlCredentials

New-AzSqlServerFirewallRule -AllowAllAzureIPs `
    -ResourceGroupName $rg.ResourceGroupName `
    -ServerName $server.ServerName

New-AzSqlDatabase -DatabaseName "db-tg-bot-$Env" `
    -ResourceGroupName $rg.ResourceGroupName `
    -ServerName $server.ServerName `
    -Edition "Basic"

# Create Azure function
$stAccount = New-AzStorageAccount -ResourceGroupName $rg.ResourceGroupName `
    -Name "stacctgbotfunc$Env" `
    -SkuName "Standard_LRS" `
    -Location "$location"

New-AzFunctionApp -Name "func-habr-tg-bot-$Env" `
    -ResourceGroupName $rg.ResourceGroupName `
    -StorageAccountName $stAccount.StorageAccountName `
    -Runtime "dotnet" `
    -Location "$location" `
    -FunctionsVersion 4
