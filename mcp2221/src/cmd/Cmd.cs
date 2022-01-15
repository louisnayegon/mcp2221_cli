// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Microsoft.Extensions.CommandLineUtils;

    /// <summary>
    /// Generic command
    /// </summary>
    /// <see cref="ICommand"/>
    internal abstract class Cmd : ICommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The short name of the command. Used by the --help switch</param>
        /// <param name="description">The long descrition of the command. Used by the --help switch</param>
        public Cmd(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        /// <summary>
        /// Get the name of the command
        /// </summary>
        /// <see cref="ICommand.Name"/>
        /// <returns>The name of the command</returns>
        public string Name() { return name; }

        /// <summary>
        /// Get the descritption of the command
        /// </summary>
        /// <see cref="ICommand.Description"/>
        /// <returns>The descritption of the command</returns>
        public string Description() { return description; }

        /// <summary>
        /// Add the command options to the app
        /// </summary>
        /// <see cref="ICommand.AddOptions(CommandLineApplication)"/>
        /// <param name="app">The app to add the options to</param>
        public virtual void AddOptions(CommandLineApplication app)
        {
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <see cref="ICommand.Execute"/>
        /// <returns>Zero if there were no errors</returns>
        public abstract int Execute();

        private readonly string name;
        private readonly string description;
    }
}
