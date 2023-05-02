param cosmosDbName string

param location string = resourceGroup().location

resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2022-11-15' = {
  name: cosmosDbName
  location: location

  kind: 'GlobalDocumentDB'

  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]

    capabilities: [
      {
        name: 'EnableServerless'
      }
      {
        name: 'EnableTable'
      }
    ]
  }
}

resource cosmosDbAccountTable 'Microsoft.DocumentDB/databaseAccounts/tables@2022-11-15' = {
  parent: cosmosDbAccount
  name: 'configs'

  properties: {
    resource: {
      id: 'configs'
    }
  }
}

output cosmosDbAccountResourceId string = cosmosDbAccount.id
