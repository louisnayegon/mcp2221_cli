// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli
{
    using Microsoft.Extensions.CommandLineUtils;
    using mcp2221_cli.cmd;
    using mcp2221_cli.cmd.mcp4728;

    class Program
    {
        public static void Main(string[] args)
        {
            CommandLineApplication app = new(throwOnUnexpectedArg: false);
            List<ICommand> commands = new()
            {
                new CmdList(),
                new CmdInfo(),
                new CmdMcp4728()
            };

            foreach (var cmd in commands)
            {
                app.Command(cmd.Name(),
                    (command) =>
                    {
                        command.Description = cmd.Description();
                        cmd.AddOptions(command);
                        command.HelpOption("-? | -h | --help");
                        command.OnExecute(() => cmd.Execute());
                    }
                );
            }

            app.HelpOption("-? | -h | --help");
            app.Execute(args);
        }
    }
}
