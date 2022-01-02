// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Microsoft.Extensions.CommandLineUtils;

    internal interface ICommand
    {
        string Name();
        string Description();
        void AddOptions(CommandLineApplication app);
        int Execute();
    }
}
