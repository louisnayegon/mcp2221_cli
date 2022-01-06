// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.validator
{
    using Microsoft.Extensions.CommandLineUtils;

    internal static class Validators
    {
        static public bool IsValidInt(CommandOption option, int min=int.MinValue, int max=int.MaxValue, bool optional = false)
        {
            bool ret = optional;
            if (option.HasValue())
            {
                if ( int.TryParse(option.Value(), out _) )
                {
                    var _ = int.Parse(option.Value());
                    ret = (_ >= min) && (_ <= max);
                }
            }
            return ret;
        }

        static public bool IsValidUint(CommandOption option, uint min=uint.MinValue, uint max=uint.MaxValue, bool optional=false)
        {
            bool ret = optional;
            if (option.HasValue())
            {
                if ( uint.TryParse(option.Value(), out _) )
                {
                    var _ = uint.Parse(option.Value());
                    ret = (_ >= min) && (_ <= max);
                }
            }
            return ret;
        }

        static public bool IsValidChannel(CommandOption option)
        {
            bool ret = false;
            if (option.HasValue())
            {
                ret = Channels.ContainsKey(option.Value());
            }
            return ret;
        }

        static public int ToInt(CommandOption option)
        {
            return int.Parse(option.Value());
        }

        static public uint ToUint(CommandOption option)
        {
            return uint.Parse(option.Value());
        }

        static public uint ToChannel(CommandOption option)
        {
            return Channels[option.Value()];
        }

        private static readonly Dictionary<string, uint> Channels = new() {
            { "A", 0 },
            { "B", 1 },
            { "C", 2 },
            { "D", 3 },
        };
    }
}
