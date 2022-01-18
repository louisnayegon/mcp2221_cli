// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Microsoft.Extensions.CommandLineUtils;
    using Smdn.Devices.MCP2221;
    using System.Device.Gpio;

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
            app.Options.Add(this.value);
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
                    var pinValue = bool.Parse(this.value.Value());

                    try
                    {
                        SetValue(gpio, pinValue);
                    }
                    catch (Exception)
                    {
                        gpio.ConfigureAsGPIO();
                        SetValue(gpio, pinValue);
                    }
                }
                else
                {
                    foreach (var gpio in gpios.Values )
                    {
                        PinValue pinValue;
                        try
                        {
                            pinValue = GetValue(gpio);
                        }
                        catch (Exception)
                        {
                            gpio.ConfigureAsGPIO();
                            pinValue = GetValue(gpio);
                        }
                        Console.WriteLine($"{pinValue.Equals(PinValue.High)}");
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

        /// <summary>
        /// Get the pin value
        /// </summary>
        /// <param name="gpio">The gpio</param>
        /// <returns>The value</returns>
        private static PinValue GetValue(MCP2221.GPFunctionality gpio)
        {
            return gpio.GetValue();
        }

        /// <summary>
        /// Get the pin value
        /// </summary>
        /// <param name="gpio">The gpio</param>
        /// <param name="value">The value to set it to</param>
        /// <returns></returns>
        private static void SetValue(MCP2221.GPFunctionality gpio, bool value)
        {
            gpio.SetValue(value);
        }

        private readonly CommandOption write;
        private readonly CommandOption id;
        private readonly CommandOption value;
    }
}
