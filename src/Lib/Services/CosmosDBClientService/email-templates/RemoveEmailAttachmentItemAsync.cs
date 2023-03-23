﻿using System.Net;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    public async Task RemoveEmailAttachmentItemAsync(string id)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );


        try
        {
            await container.DeleteItemAsync<EmailTemplateAttachmentItem>(
                id: id,
                partitionKey: new("email-template-attachment-item")
            );
        }
        catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine("Attachment item not found.");
        }
    }
}