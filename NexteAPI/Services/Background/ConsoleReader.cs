using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NexteAPI.CommandHandlers;
using NexteAPI.Commands;
using NexteAPI.CommandsHandler;
using NexteAPI.Interfaces;

namespace NexteAPI.BackgroundServices
{
    public class ConsoleReader : BackgroundService
    {
        private readonly ILogger<ConsoleReader> _logger;
        private readonly IMediator _mediator;
        public static EventWaitHandle ConsoleBlocker => new EventWaitHandle(true, EventResetMode.AutoReset);

        public ConsoleReader(ILogger<ConsoleReader> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ConsoleBlocker.WaitOne();

                    var rawInput = await GetInputAsync(stoppingToken);

                    if (string.IsNullOrWhiteSpace(rawInput))
                        continue;

                    var argv = rawInput.Split(' ');

                    var data = Parser.Default.ParseArguments<CreateProfileOptions,SyncAllOptions, SyncProfilesOptions, SyncUpdatesOptions, GravitOptions>(argv);

                    await data.WithParsedAsync(HandleCommands);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Critical exception occurred");
                }
            }
        }

        async Task HandleCommands(object opts_obj)
        {
            switch (opts_obj)
            {
                case CreateProfileOptions:
                    {
                        var opts = (CreateProfileOptions)opts_obj;
                        await _mediator.Send(new CreateProfileCommand()
                        {
                            Name = opts.nameclient,
                            Version = opts.versionclient,
                        });
                    }
                    break;
                case SyncAllOptions:
                    {
                        var opts = (SyncAllOptions)opts_obj;
                        await _mediator.Send(new SyncAllCommand());
                    }
                    break;
                //case DownloadAssetsOptions:
                //    {
                //        var opts = (DownloadAssetsOptions)opts_obj;
                //        await _mediator.Send(new DownloadAssetCommand()
                //        {
                //            Version = opts.versionclient,
                //            IsMojang = opts.mojang
                //        });
                //    }
                //    break;
                //case DownloadClientOptions:
                //    {
                //        var opts = (DownloadClientOptions)opts_obj;
                //        await _mediator.Send(new DownloadClientCommand()
                //        {
                //            Version = opts.versionclient,
                //            Name = opts.nameclient,
                //            IsMojang = opts.mojang
                //        });
                //    }
                //    break;
                case SyncProfilesOptions:
                    {
                        var opts = (SyncProfilesOptions)opts_obj;
                        await _mediator.Send(new SyncProfilesCommand());
                    }
                    break;
                case SyncUpdatesOptions:
                    {
                        var opts = (SyncUpdatesOptions)opts_obj;
                        await _mediator.Send(new SyncUpdatesCommand());
                    }
                    break;
                case GravitOptions:
                    {
                        var opts = (GravitOptions)opts_obj;
                        await _mediator.Send(new GravitCommand());
                    }
                    break;
            }

            //async (DownloadClientOptions opts) =>
            //{
            //    var response = await _mediator.Send(new DownloadClientCommand()
            //    {
            //        Version = opts.versionclient,
            //        Name = opts.nameclient,
            //        IsMojang = opts.mojang
            //    });

            //    return;
            //}
            ////(UpdateJreOptions opts) =>
            ////{
            ////    //var response = await _mediator.Send(new Ping());
            ////    return 0;
            ////},
            ////(DownloadAssetsOptions opts) =>
            ////{
            ////    var response = _mediator.Send(new DownloadAssetCommand()
            ////    {
            ////        Version = opts.versionclient,
            ////        IsMojang = opts.mojang
            ////    });
            ////return 0;
            ////},
            ////(SyncProfilesOptions opts) =>
            ////{
            ////    //var response = await _mediator.Send(new Ping());
            ////    return 0;
            ////},
            ////(SyncUpdatesOptions opts) =>
            ////{
            ////    //var response = await _mediator.Send(new Ping());
            ////    return 0;
            ////}
            //);

            // TODO: реализовать прокидывание команды в исполнитель
        }


        static Task<string> GetInputAsync(CancellationToken stoppingToken)
        {
            return Task.Run(Console.ReadLine, stoppingToken);
        }
    }
}
