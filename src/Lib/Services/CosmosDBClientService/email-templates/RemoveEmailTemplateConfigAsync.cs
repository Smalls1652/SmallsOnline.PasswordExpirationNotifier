using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    public async Task RemoveEmailTemplateConfigAsync(string id)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        EmailTemplateConfig emailTemplateConfig = await GetEmailTemplateConfigAsync(id);

        if (emailTemplateConfig.IncludedAttachmentIds is not null)
        {
            foreach (var attachmentItem in emailTemplateConfig.IncludedAttachmentIds)
            {
                await RemoveEmailAttachmentItemAsync(attachmentItem);
            }
        }

        await container.DeleteItemAsync<EmailTemplateConfig>(
            id: id,
            partitionKey: new("email-template-config")
        );
    }
}