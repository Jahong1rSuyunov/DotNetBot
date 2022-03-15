﻿using DemoDotnetBot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemoDotnetBot.Servess
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly ILogger<ConfigureWebhook> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly BotConfiguration _botConfig;

        public ConfigureWebhook(ILogger<ConfigureWebhook> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scopt = _serviceProvider.CreateScope();

            var botClient = scopt.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            var webhookAdress = $@"{_botConfig.HostAdress}/bot/{_botConfig.Token}";

            _logger.LogInformation("Setting webhook");

            await botClient.SendTextMessageAsync(
               chatId: 5264488091,
               text: "Webhook o'rnatilmoqda");

            await botClient.SetWebhookAsync(
                url: webhookAdress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scopt = _serviceProvider.CreateScope();

            var botClient = scopt.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            _logger.LogInformation("webhook removing");

            await botClient.SendTextMessageAsync(
                chatId: 5264488091,
                text: "Bot uyqichi");

        }
    }
}
