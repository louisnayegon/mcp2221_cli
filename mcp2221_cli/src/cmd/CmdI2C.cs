// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Smdn.Devices.MCP2221;

    internal class CmdI2C : Cmd
    {
        public CmdI2C() : base("i2c", "I2C commands")
        {
        }

        public override int Execute()
        {
            int ret = 0;
            try
            {
                var device = MCP2221.Open();

                Console.WriteLine($"HID Device:{device.I2C.GetType()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ret = 1;
            }

            return ret;
        }
    }
}
