// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd.mcp4728
{
    using Microsoft.Extensions.CommandLineUtils;
    using Smdn.Devices.MCP2221;
    using mcp2221_cli.validator;

    /// <summary>
    /// Command to get a list of I2C device addresses available
    /// </summary>
    /// <see cref="Cmd"/>
    internal class CmdMcp4728 : Cmd
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CmdMcp4728() : base("mcp4728", "mcp4728 I2C device")
        {
            this.address = new("-a|--address", CommandOptionType.SingleValue)
            { Description = "Address of the I2C device" };

        }

        /// <summary>
        /// Add command options.  <see cref="Cmd.AddOptions(CommandLineApplication)"/>
        /// </summary>
        /// <param name="app">The app to add the options to</param>
        public override void AddOptions(CommandLineApplication app)
        {
            app.Options.Add(this.address);
            List<ICommand> commands = new()
            {
                new CmdMcp4728ChannelRead(),
                new CmdMcp4728ChannelWrite(),
                new CmdMcp4728Reset(),
                new CmdMcp4728SoftwareUpdate(),
                new CmdMcp4728WakeUp(),
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
        }

        /// <summary>
        /// Execute the command. <see cref="Cmd.Execute"/>
        /// </summary>
        /// <returns>Zero if no error occured</returns>
        public override int Execute()
        {
            int ret = 0;
            try
            {
                var device = MCP2221.Open();

                if (Validators.IsValidInt(this.address))
                {
                    I2CAddress i2CAddress = new(Validators.ToInt(this.address));

                    var (writeAddressSet, readAddressSet) = device.I2C.ScanBus(i2CAddress, i2CAddress);

                    if (writeAddressSet.Count != 0)
                    {
                        Console.WriteLine("Writeable");
                    }
                    if (writeAddressSet.Count != 0)
                    {
                        Console.WriteLine("Readable");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid address");
                    ret = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ret = 1;
            }

            return ret;
        }

        private readonly CommandOption address;
    }
}
