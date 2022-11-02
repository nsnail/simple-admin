using SimpleAdmin.Infrastructure.Extensions;

Serve.Run(RunOptions.Default.WithArgs(args)
                    .ConfigureBuilder(builder => builder.UseSerilogDefault(config => config.Init())));