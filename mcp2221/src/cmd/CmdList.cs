// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Microsoft.Extensions.CommandLineUtils;
    using Smdn.Devices.MCP2221;

    internal class CmdList : Cmd
    {
        public CmdList() : base("list", "List addresses of devices")
        {
            this.read = new("-r|--read", CommandOptionType.NoValue)
            { Description = "Devices that are readable" };
            this.write = new("-w|--write", CommandOptionType.NoValue)
            { Description = "Devices that are writable" };
        }

        public override void AddOptions(CommandLineApplication app)
        {
            app.Options.Add(this.read);
            app.Options.Add(this.write);
        }

        public override int Execute()
        {
            int ret = 0;
            try
            {
                var device = MCP2221.Open();
                device.I2C.BusSpeed = I2CBusSpeed.Default;

                I2CAddress addressRangeMin = I2CAddress.DeviceMinValue;
                I2CAddress addressRangeMax = I2CAddress.DeviceMaxValue;

                var (writeAddressSet, readAddressSet) = device.I2C.ScanBus(addressRangeMin, addressRangeMax);

                var printWrite = this.write.HasValue() || (!this.read.HasValue() && !this.write.HasValue());
                var printRead = this.read.HasValue();

                if (printWrite)
                {
                    foreach (I2CAddress item in writeAddressSet)
                    {
                        Console.WriteLine($"0x{item}");
                    }
                }
                if (printRead)
                {
                    foreach (I2CAddress item in readAddressSet)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ret = 1;
            }

            return ret;
        }

        private readonly CommandOption read;
        private readonly CommandOption write;
    }
}
