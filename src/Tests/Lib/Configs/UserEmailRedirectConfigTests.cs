using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Tests;

[TestClass]
public class UserEmailRedirectConfigTests
{
    [TestMethod]
    public void Constructor_IsSuccessful()
    {
        Faker faker = new("en");

        UserEmailRedirectConfig userEmailRedirectConfig = new()
        {
            Id = Guid.NewGuid().ToString(),
            PartitionKey = "user-redirect-config",
            UserPrincipalName = faker.Internet.Email(),
            RedirectUserPrincipalName = faker.Internet.Email()
        };

        Assert.IsNotNull(userEmailRedirectConfig);
    }
}