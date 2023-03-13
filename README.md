# Azure AD Password Expiration Notification Service

> ⚠️**Note:**
> 
> This project is currently in development and **is not ready** for production use. A lot of things can change and will break.

Send email notifications to users when their password is about to expire. It is a complete rewrite of my previous project [`PasswordExpiration-Email`](https://github.com/Smalls1652/PasswordExpiration-Email).

## 🪄 How it works

There are three main components to this project:

* **Azure Functions App**
  * The main component for getting users with expiring passwords and sending emails out.
* **Azure CosmosDB**
  * Stores config data for the service.
* **Azure Storage Account Queues**
  * Message queues for the Azure Function app to use for initiating user searches and sending emails.

It's heavily utilizing message queues to batch out user search jobs and sending emails for performance and efficiency.

## 🧑‍💻 Building/Testing

* [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local)

## 🤝 License

This project is licensed under the [MIT License](./LICENSE).
