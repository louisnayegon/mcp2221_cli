// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Microsoft.Extensions.CommandLineUtils;
    using Smdn.Devices.MCP2221;

    /// <summary>
    /// GPIO actions
    /// </summary>
    internal class CmdGpio : Cmd
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <see cref="Cmd"/>
        public CmdGpio() : base("gpio", "Get/set GPIO")
        {
            this.write = new("-w|--write", CommandOptionType.NoValue)
            { Description = "Write gpio" };
            this.id = new("-i|--id", CommandOptionType.SingleValue)
            { Description = "ID of gpio to write" };
            this.value = new("-v|--value", CommandOptionType.SingleValue)
            { Description = "When writing the value to set" };
        }

        /// <summary>
        /// Add command options.  <see cref="Cmd.AddOptions(CommandLineApplication)"/>
        /// </summary>
        /// <param name="app">The app to add the options to</param>
        public override void AddOptions(CommandLineApplication app)
        {
            app.Options.Add(this.write);
            app.Options.Add(this.id);
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
                device.I2C.BusSpeed = I2CBusSpeed.Default;

                I2CAddress addressRangeMin = I2CAddress.DeviceMinValue;
                I2CAddress addressRangeMax = I2CAddress.DeviceMaxValue;

                Dictionary<int, MCP2221.GPFunctionality> gpios = new (){
                    [0] = device.GP0,
                    [1] = device.GP1,
                    [2] = device.GP2,
                    [3] = device.GP3
                };
                if (this.write.HasValue())
                {
                    var gpio = gpios[int.Parse(this.id.Value())];
                    var pinValue = gpio.GetValue();

                    pinValue = bool.Parse(this.value.Value());
                    gpio.SetValue(pinValue);
                }
                else
                {
                    Console.WriteLine("{device.GP0.GetValue}{device.GP1.GetValue}{device.GP2.GetValue}{device.GP3.GetValue}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ret = 1;
            }

            return ret;
        }

        private readonly CommandOption write;
        private readonly CommandOption id;
        private readonly CommandOption value;
    }
}
