// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Microsoft.Extensions.CommandLineUtils;

    internal abstract class Cmd : ICommand
    {
        public Cmd(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public string Name() { return name; }

        public string Description() { return description; }

        public virtual void AddOptions(CommandLineApplication app)
        {
        }

        public abstract int Execute();


        private readonly string name;
        private readonly string description;
    }
}
