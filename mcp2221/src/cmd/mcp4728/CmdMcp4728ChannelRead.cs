// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd.mcp4728
{
    using Microsoft.Extensions.CommandLineUtils;
    using mcp2221_cli.validator;

    /// <summary>
    /// Read a channel command.
    /// </summary>
    /// <see cref="Cmd"/>
    internal class CmdMcp4728ChannelRead : Cmd
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CmdMcp4728ChannelRead() : base("channelread", "read channel data")
        {
            this.address = new("-a|--address", CommandOptionType.SingleValue)
            { Description = "Address of the I2C device" };
            this.channel = new("-c|--channel", CommandOptionType.SingleValue)
            { Description = "The ADC channel. One of A,B,C or D" };
        }

        /// <summary>
        /// Add command options.  <see cref="Cmd.AddOptions(CommandLineApplication)"/>
        /// </summary>
        /// <param name="app">The app to add the options to</param>
        public override void AddOptions(CommandLineApplication app)
        {
            app.Options.Add(this.address);
            app.Options.Add(this.channel);
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
                if (!Validators.IsValidInt(this.address))
                {
                    Console.WriteLine("Invalid address");
                    ret = 1;
                }
                if (!Validators.IsValidChannel(this.channel))
                {
                    Console.WriteLine("Invalid ADC channel. Should be A,B,C or D");
                    ret = 1;
                }

                if ( ret == 0 )
                {
                    Mcp4728 mcp4728 = new(Validators.ToInt(this.address));
                    var data = mcp4728.GetChannel(Validators.ToChannel(this.channel));
                    Console.WriteLine($"{data.Value},{data.VRef},{data.Gain},{data.PowerState}");
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
        private readonly CommandOption channel;
    }
}
