// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Smdn.Devices.MCP2221;

    /// <summary>
    /// Get the device information
    /// </summary>
    /// <see cref="Cmd"/>
    internal class CmdInfo : Cmd
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CmdInfo() : base("info", "MCP2221 Info")
        {
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

                Console.WriteLine($"HID Device                :{device.HidDevice}");
                Console.WriteLine($"Hardware Revision         :{device.HardwareRevision}");
                Console.WriteLine($"Firmware Revision         :{device.FirmwareRevision}");
                Console.WriteLine($"Manufacturer Descriptor   :{device.ManufacturerDescriptor}");
                Console.WriteLine($"Product Descriptor        :{device.ProductDescriptor}");
                Console.WriteLine($"Serial Number Descriptor  :{device.SerialNumberDescriptor}");
                Console.WriteLine($"Chip Factory Serial Number:{device.ChipFactorySerialNumber}");
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
