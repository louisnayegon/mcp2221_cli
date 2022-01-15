// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd.mcp4728
{
    using Smdn.Devices.MCP2221;
    using System.Collections.Generic;

    /// <summary>
    /// Mcp4728 command interface
    /// </summary>
    internal class Mcp4728
    {
        /// <summary>
        /// Mcp4728 channel data
        /// </summary>
        public class Channel
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public Channel(int index)
            {
                this.Index = index;
                this.Value = 0;
                this.VRef = 0;
                this.Gain = 0;
                this.PowerState = 0;
            }

            public int Index { get; }
            public uint Value { get; set; }
            public uint VRef { get; set; }
            public uint Gain { get; set; }
            public uint PowerState { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Mcp4728(int address)
        {
            this.address = new(address);
            this.mcp2221 = MCP2221.Open();
            this.channels = new();
            for (int i = 0; i < MCP4728_CHANNELS; i++)
            {
                this.channels.Add(new Channel(i));
            }
            this.ReadChannels();
        }

        public void Reset()
        {
            var toWrite = new byte[] {
                MCP4728_CMD_GENERAL_CALL,
                MCP4728_GENERAL_CALL_RESET_COMMAND };
            this.SendCommand(toWrite);
        }

        public void WakeUp()
        {
            var toWrite = new byte[] {
                MCP4728_CMD_GENERAL_CALL,
                MCP4728_GENERAL_CALL_WAKEUP_COMMAND };
            this.SendCommand(toWrite);
        }

        public void SoftUpdate()
        {
            var toWrite = new byte[] {
                MCP4728_CMD_GENERAL_CALL,
                MCP4728_GENERAL_CALL_SOFTWARE_UPDATE_COMMAND };
            this.SendCommand(toWrite);
        }

        public Channel GetChannel(uint index)
        {
            return this.channels[(int)index];
        }

        public void UpdateChannel(uint index, Channel channel)
        {
            var toWrite = new byte[] {
                (byte)(MCP4728_CMD_WRITE_DAC | (channel.Index << 1)),
                (byte)((channel.Value >> 8) | (channel.VRef << 7) | (channel.PowerState << 5) | (channel.Gain << 4)),
                (byte)(channel.Value & 0xff)
            };
            this.SendCommand(toWrite);
        }

        private void SendCommand(byte[] toWrite)
        {
            this.mcp2221.I2C.Write(this.address, toWrite);
        }

        private void ReadChannels()
        {
            byte[] buffer = new byte[24];
            bool ok = false;
            for (int i = 0; (i < 4) && !ok; ++i)
            {
                try
                {
                    this.mcp2221.I2C.Read(this.address, buffer);
                    ok = true;
                }
                catch (Exception)
                {
                    ++i;
                }
            }

            if (ok)
            {
                int offset = 0;
                foreach (var channel in this.channels)
                {
                    var header = buffer[offset++];
                    var high_byte = buffer[offset++];
                    var low_byte = buffer[offset++];
                    _ = buffer[offset++];
                    _ = buffer[offset++];
                    _ = buffer[offset++];

                    channel.Value = (uint)((high_byte & 0x0F) << 8 | low_byte);
                    channel.VRef = (uint)((high_byte & 0x80) >> 7);
                    channel.Gain = (uint)((high_byte & 0x10) >> 4);
                    channel.PowerState = (uint)((high_byte & 0x60) >> 5);
                }
            }
            else
            {
                Console.WriteLine("ReadChannels failed");
                throw new Exception("ReadChannels failed");
            }
        }

        private readonly I2CAddress address;
        private readonly MCP2221 mcp2221;
        private readonly List<Channel> channels;
        private static readonly int MCP4728_CHANNELS = 4;
        private static readonly byte MCP4728_CMD_GENERAL_CALL = 0x00;
        private static readonly byte MCP4728_CMD_WRITE_DAC = 0x50;
        private static readonly byte MCP4728_GENERAL_CALL_RESET_COMMAND = 0x06;
        private static readonly byte MCP4728_GENERAL_CALL_WAKEUP_COMMAND = 0x09;
        private static readonly byte MCP4728_GENERAL_CALL_SOFTWARE_UPDATE_COMMAND = 0x08;
        public readonly static uint MIN_ADC_VALUE = 0x0;
        public readonly static uint MAX_ADC_VALUE = 0xfff;
        public readonly static uint MIN_VREF_VALUE = 0x0;
        public readonly static uint MAX_VREF_VALUE = 0x1;
        public readonly static uint MIN_GAIN_VALUE = 0x0;
        public readonly static uint MAX_GAIN_VALUE = 0x1;
        public readonly static uint MIN_POWER_STATE_VALUE = 0x0;
        public readonly static uint MAX_POWER_STATE_VALUE = 0x3;
    }
}
