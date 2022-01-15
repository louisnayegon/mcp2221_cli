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
    /// Write Channel data
    /// </summary>
    /// <see cref="Cmd"/>
    internal class CmdMcp4728ChannelWrite : Cmd
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CmdMcp4728ChannelWrite() : base("channelwrite", "write channel data")
        {
            this.address = new("-a|--address", CommandOptionType.SingleValue)
            { Description = "Address of the I2C device" };
            this.channel = new("-c|--channel", CommandOptionType.SingleValue)
            { Description = "The ADC channel. One of A,B,C or D" };
            this.value = new("-v|--value", CommandOptionType.SingleValue)
            { Description = "Value to set the ADC to" };
            this.vref = new("-r|--vref", CommandOptionType.SingleValue)
            { Description = "Value to set the VRef to" };
            this.gain = new("-g|--gain", CommandOptionType.SingleValue)
            { Description = "Value to set the Gain to" };
            this.powerState = new("-p|--powerstate", CommandOptionType.SingleValue)
            { Description = "Value to set the Power State to" };
        }

        /// <summary>
        /// Add command options.  <see cref="Cmd.AddOptions(CommandLineApplication)"/>
        /// </summary>
        /// <param name="app">The app to add the options to</param>
        public override void AddOptions(CommandLineApplication app)
        {
            app.Options.Add(this.address);
            app.Options.Add(this.channel);
            app.Options.Add(this.value);
            app.Options.Add(this.vref);
            app.Options.Add(this.gain);
            app.Options.Add(this.powerState);
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
                if (!Validators.IsValidUint(this.value, Mcp4728.MIN_ADC_VALUE, Mcp4728.MAX_ADC_VALUE, true))
                {
                    Console.WriteLine($"Invalid ADC value. Should be in range {Mcp4728.MIN_ADC_VALUE} or {Mcp4728.MAX_ADC_VALUE}");
                    ret = 1;
                }
                if (!Validators.IsValidUint(this.vref, Mcp4728.MIN_VREF_VALUE, Mcp4728.MAX_VREF_VALUE, true))
                {
                    Console.WriteLine($"Invalid VRef value. Should be in range {Mcp4728.MIN_VREF_VALUE} or {Mcp4728.MAX_VREF_VALUE}");
                    ret = 1;
                }
                if (!Validators.IsValidUint(this.gain, Mcp4728.MIN_GAIN_VALUE, Mcp4728.MAX_GAIN_VALUE, true))
                {
                    Console.WriteLine($"Invalid Gain value. Should be in range {Mcp4728.MIN_GAIN_VALUE} or {Mcp4728.MAX_GAIN_VALUE}");
                    ret = 1;
                }
                if (!Validators.IsValidUint(this.powerState, Mcp4728.MIN_POWER_STATE_VALUE, Mcp4728.MAX_POWER_STATE_VALUE, true))
                {
                    Console.WriteLine($"Invalid Power State value. Should be in range {Mcp4728.MIN_POWER_STATE_VALUE} or {Mcp4728.MAX_POWER_STATE_VALUE}");
                    ret = 1;
                }

                if ( ret == 0 )
                {
                    Mcp4728 mcp4728 = new(Validators.ToInt(this.address));
                    var ch = mcp4728.GetChannel(Validators.ToChannel(this.channel));
                    if (this.value.HasValue() )
                        ch.Value = Validators.ToUint(this.value);
                    if (this.vref.HasValue())
                        ch.VRef = Validators.ToUint(this.vref);
                    if (this.gain.HasValue())
                        ch.Gain = Validators.ToUint(this.gain);
                    if (this.powerState.HasValue())
                        ch.PowerState = Validators.ToUint(this.powerState);
                    mcp4728.UpdateChannel(Validators.ToChannel(this.channel), ch);
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
        private readonly CommandOption value;
        private readonly CommandOption vref;
        private readonly CommandOption gain;
        private readonly CommandOption powerState;
    }
}
