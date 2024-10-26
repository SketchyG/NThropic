using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Newtonsoft.Json;
using ClaudeApi;
using ClaudeApi.Agents;
using ClaudeApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
            new Option<string>(
                "--input",
                description: "The input file path"),
            new Option<string>(
                "--output",
                description: "The output file path")
        };

        rootCommand.Description = "Code Analyzer CommandLine Application";

        rootCommand.Handler = CommandHandler.Create<string, string>(async (input, output) =>
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Input and output file paths are required.");
                return 1;
            }

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var claudeClient = serviceProvider.GetRequiredService<ClaudeClient>();
            var chatOrchestrator = serviceProvider.GetRequiredService<ChatOrchestrator>();

            var inputFileContent = await File.ReadAllTextAsync(input);
            var result = await ProcessFileAsync(inputFileContent, claudeClient, chatOrchestrator);

            await File.WriteAllTextAsync(output, JsonConvert.SerializeObject(result, Formatting.Indented));

            Console.WriteLine("Processing completed successfully.");
            return 0;
        });

        return await rootCommand.InvokeAsync(args);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole());
        services.AddHttpClient<IClaudeApiService, ClaudeApiService>();
        services.AddSingleton<ISandboxFileManager, SandboxFileManager>();
        services.AddSingleton<ClaudeClient>();
        services.AddSingleton<ChatOrchestrator>();
        services.AddTransient<IPromptService, PromptService>();
        services.AddClaudApi();
        services.AddNThropicAgents();
    }

    private static async Task<object> ProcessFileAsync(string inputFileContent, ClaudeClient claudeClient, ChatOrchestrator chatOrchestrator)
    {
        // Implement the logic to process the input file content using ClaudeClient and ChatOrchestrator
        // This is a placeholder implementation
        var result = new
        {
            Summary = "This is a summary of the input file content.",
            Details = inputFileContent
        };

        return await Task.FromResult(result);
    }
}
