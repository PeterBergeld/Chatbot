using Discord;
using Discord.WebSocket;

class Program
{
    private DiscordSocketClient _client = null!;

    static async Task Main()
    {
        Program program = new Program();
        await program.StartBot();
    }

    public async Task StartBot()
    {
        var config = new DiscordSocketConfig
        {
            GatewayIntents =
                GatewayIntents.AllUnprivileged |
                GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);

        _client.Log += Log;
        _client.MessageReceived += MessageReceived;
        _client.Ready += Ready;

        string? token =
            Environment.GetEnvironmentVariable("DISCORD_TOKEN");

        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("ERROR: DISCORD_TOKEN was not found.");
            return;
        }

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        Console.WriteLine("Bot is online!");

        await Task.Delay(-1);
    }

    private Task Ready()
    {
        Console.WriteLine("===== BOT IS READY =====");

        Console.WriteLine(
            $"Connected to {_client.Guilds.Count} server(s)"
        );

        foreach (var guild in _client.Guilds)
        {
            Console.WriteLine($"Server: {guild.Name}");
        }

        return Task.CompletedTask;
    }

    private async Task MessageReceived(SocketMessage message)
    {
        Console.WriteLine("===== MESSAGE EVENT FIRED =====");
        Console.WriteLine($"Author: {message.Author.Username}");
        Console.WriteLine($"Message: {message.Content}");

        // Ignore messages from bots
        if (message.Author.IsBot)
            return;

        string text = message.Content.ToLower();

        // Check what the person wrote
        if (text.Contains("jag är sjuk") ||
            text.Contains("jag ar sjuk") ||
            text.Contains("kommer inte in idag") ||
            text.Contains("brb") ||
            text.Contains("Tillbaka om 10"));
        {
            await message.Channel.SendMessageAsync(
                "Yes, förstår ❤️ Hoppas det blir bättre!"
            );
        }
    }

    private Task Log(LogMessage message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}