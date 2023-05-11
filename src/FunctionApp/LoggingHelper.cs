using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp;

/// <summary>
/// Helper class for emitting log messages.
/// </summary>
public static class LoggingHelper
{
    /// <summary>
    /// Log an informational message.
    /// </summary>
    /// <param name="logClient">The specific logger to use. Valid clients: <see cref="ILogger" /> and <see cref="TelemetryClient" />.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="correlationId">The correlation ID to associate to the log.</param>
    /// <param name="functionContext">The context from the executing function.</param>
    /// <param name="args">An object array that contains zero or more objects to format into the <see cref="message" />.</param>
    public static void LogInformation(object logClient, string message, string correlationId, FunctionContext functionContext, params object[] args)
    {
        switch (logClient is TelemetryClient)
        {
            case true:
                LogInformation((logClient as TelemetryClient)!, message, correlationId, functionContext, args);
                break;

            default:
                LogInformation((logClient as ILogger)!, message, correlationId, args);
                break;
        }
    }

    /// <summary>
    /// Log an informational message.
    /// </summary>
    /// <param name="logger">The specific logger to use. Valid clients: <see cref="ILogger" /> and <see cref="TelemetryClient" />.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="correlationId">The correlation ID to associate to the log.</param>
    /// <param name="args">An object array that contains zero or more objects to format into the <see cref="message" />.</param>
    private static void LogInformation(ILogger logger, string message, string correlationId, params object[] args)
    {
        object[] modifiedArgs = new object[args.Length + 1];
        for (int i = 0; i < args.Length; i++)
        {
            modifiedArgs[i] = args[i];
        }
        modifiedArgs[^1] = correlationId;
        int lastArgIndex = modifiedArgs.Length - 1;

        StringBuilder modifiedMessageBuilder = new(message);
        modifiedMessageBuilder.Append(" [CorrelationId: {");
        modifiedMessageBuilder.Append(lastArgIndex);
        modifiedMessageBuilder.Append("}]");

        logger.LogInformation(modifiedMessageBuilder.ToString(), modifiedArgs);
    }

    /// <summary>
    /// Log an informational message.
    /// </summary>
    /// <param name="telemetryClient">The specific logger to use. Valid clients: <see cref="ILogger" /> and <see cref="TelemetryClient" />.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="correlationId">The correlation ID to associate to the log.</param>
    /// <param name="functionContext">The context from the executing function.</param>
    /// <param name="args">An object array that contains zero or more objects to format into the <see cref="message" />.</param>
    private static void LogInformation(TelemetryClient telemetryClient, string message, string correlationId, FunctionContext functionContext, params object[] args)
    {
        string formattedMessage = string.Format(message, args);

        telemetryClient.TrackTrace(
            message: formattedMessage,
            severityLevel: SeverityLevel.Information,
            properties: new Dictionary<string, string>()
            {
                {
                    "CorrelationId", correlationId
                },
                {
                    "InvocationId", functionContext.InvocationId
                },
                {
                    "Operation", functionContext.FunctionDefinition.Name
                }
            }
        );
    }

    /// <summary>
    /// Log an warning message.
    /// </summary>
    /// <param name="logClient">The specific logger to use. Valid clients: <see cref="ILogger" /> and <see cref="TelemetryClient" />.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="correlationId">The correlation ID to associate to the log.</param>
    /// <param name="functionContext">The context from the executing function.</param>
    /// <param name="args">An object array that contains zero or more objects to format into the <see cref="message" />.</param>
    public static void LogWarning(object logClient, string message, string correlationId, FunctionContext functionContext, params object[] args)
    {
        switch (logClient is TelemetryClient)
        {
            case true:
                LogWarning((logClient as TelemetryClient)!, message, correlationId, functionContext, args);
                break;

            default:
                LogWarning((logClient as ILogger)!, message, correlationId, args);
                break;
        }
    }

    /// <summary>
    /// Log an informational message.
    /// </summary>
    /// <param name="logger">The specific logger to use. Valid clients: <see cref="ILogger" /> and <see cref="TelemetryClient" />.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="correlationId">The correlation ID to associate to the log.</param>
    /// <param name="args">An object array that contains zero or more objects to format into the <see cref="message" />.</param>
    private static void LogWarning(ILogger logger, string message, string correlationId, params object[] args)
    {
        object[] modifiedArgs = new object[args.Length + 1];
        for (int i = 0; i < args.Length; i++)
        {
            modifiedArgs[i] = args[i];
        }
        modifiedArgs[^1] = correlationId;

        int lastArgIndex = modifiedArgs.Length - 1;

        StringBuilder modifiedMessageBuilder = new(message);
        modifiedMessageBuilder.Append(" [CorrelationId: {");
        modifiedMessageBuilder.Append(lastArgIndex);
        modifiedMessageBuilder.Append("}]");

        logger.LogWarning(modifiedMessageBuilder.ToString(), modifiedArgs);
    }

    /// <summary>
    /// Log an informational message.
    /// </summary>
    /// <param name="telemetryClient">The specific logger to use. Valid clients: <see cref="ILogger" /> and <see cref="TelemetryClient" />.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="correlationId">The correlation ID to associate to the log.</param>
    /// <param name="functionContext">The context from the executing function.</param>
    /// <param name="args">An object array that contains zero or more objects to format into the <see cref="message" />.</param>
    private static void LogWarning(TelemetryClient telemetryClient, string message, string correlationId, FunctionContext functionContext, params object[] args)
    {
        string formattedMessage = string.Format(message, args);

        telemetryClient.TrackTrace(
            message: formattedMessage,
            severityLevel: SeverityLevel.Warning,
            properties: new Dictionary<string, string>()
            {
                {
                    "CorrelationId", correlationId
                },
                {
                    "InvocationId", functionContext.InvocationId
                },
                {
                    "Operation", functionContext.FunctionDefinition.Name
                }
            }
        );
    }
}