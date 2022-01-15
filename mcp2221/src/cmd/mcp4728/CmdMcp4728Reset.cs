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
    /// Reset command
    /// </summary>
    /// <see cref="Cmd"/>
    internal class CmdMcp4728Reset : Cmd
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CmdMcp4728Reset() : base("reset", "reset similar to Power-On-Reset")
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
                if (Validators.IsValidInt(this.address))
                {
                    Mcp4728 mcp4728 = new(Validators.ToInt(this.address));
                    mcp4728.Reset();
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
