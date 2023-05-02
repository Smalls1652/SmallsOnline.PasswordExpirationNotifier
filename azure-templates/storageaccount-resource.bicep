param storageAccountName string

param location string = resourceGroup().location

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location

  kind: 'StorageV2'

  sku: {
    name: 'Standard_LRS'
  }

  properties: {
    accessTier: 'Hot'
  }
}

resource storageAccountQueue 'Microsoft.Storage/storageAccounts/queueServices@2022-09-01' = {
  parent: storageAccount
  name: 'default'
}

resource storageAccountEmailQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-09-01' = {
  parent: storageAccountQueue
  name: 'email-queue'
}

resource storageAccountUserSearchQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-09-01' = {
  parent: storageAccountQueue
  name: 'user-search-queue'
}

output storageAccountResourceId string = storageAccount.id
