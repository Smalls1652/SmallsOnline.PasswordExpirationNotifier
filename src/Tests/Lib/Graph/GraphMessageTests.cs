using System.Text.Json;
using System.Text.Json.Serialization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Tests.Graph;

[TestClass]
public class GraphMessageTests
{
    /// <summary>
    /// Tests that the <see cref="Message"/> type is serialized to a JSON string correctly by ignoring null properties.
    /// </summary>
    [TestMethod]
    public void Test_JsonSerialization_ExcludesNullProperties()
    {
        // Define the expected JSON string.
        string jsonExpected = """{"subject":"Hello world!","body":{"content":"This is a test message!","contentType":"HTML"},"toRecipients":[{"emailAddress":{"address":"jwinger@greendalecc.edu","name":"Jeff Winger"}}],"hasAttachments":false}""";

        // Create a new Message object.
        Message message = new(
            subject: "Hello world!",
            body: new MessageBody(
                contentType: "HTML",
                content: "This is a test message!"
            ),
            toRecipient: new[]
            {
                new Recipient(
                    name: "Jeff Winger",
                    emailAddress: "jwinger@greendalecc.edu"
                )
            },
            attachments: null
        );

        // Serialize the Message object to a JSON string.
        string jsonResult = JsonSerializer.Serialize(
            value: message,
            jsonTypeInfo: GraphJsonContext.Default.Message
        );

        // Assert that the expected JSON string matches the result.
        Assert.IsTrue(
            condition: jsonExpected == jsonResult,
            message: "The serialized JSON string does not match the expected JSON string.\n\nExpected:\n{0}\n\nResult:\n{1}",
            jsonExpected, jsonResult
        );
    }
}