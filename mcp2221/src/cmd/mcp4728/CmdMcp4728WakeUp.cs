// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd.mcp4728
{
    using Microsoft.Extensions.CommandLineUtils;
    using mcp2221_cli.validator;

    internal class CmdMcp4728WakeUp : Cmd
    {
        public CmdMcp4728WakeUp() : base("wakeup", "wakeup will reset the Power-Down bits")
        {
            this.address = new("-a|--address", CommandOptionType.SingleValue)
            { Description = "Address of the I2C device" };
        }

        public override void AddOptions(CommandLineApplication app)
        {
            app.Options.Add(this.address);
        }

        public override int Execute()
        {
            int ret = 0;
            try
            {
                if (Validators.IsValidInt(this.address))
                {
                    Mcp4728 mcp4728 = new(Validators.ToInt(this.address));
                    mcp4728.WakeUp();
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
