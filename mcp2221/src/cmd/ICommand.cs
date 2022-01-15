// <copyright company="Louis Henry Nayegon">
// Copyright (c) Louis Henry Nayegon All rights reserved.
// Licensed under the MIT License.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace mcp2221_cli.cmd
{
    using Microsoft.Extensions.CommandLineUtils;

    /// <summary>
    /// Interface to a command
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// Get the name of the command
        /// </summary>
        /// <returns>The name of the command</returns>
        string Name();

        /// <summary>
        /// Get the descritption of the command
        /// </summary>
        /// <returns>The descritption of the command</returns>
        string Description();

        /// <summary>
        /// Add the command options to the app
        /// </summary>
        /// <param name="app">The app to add the options to</param>
        void AddOptions(CommandLineApplication app);

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <returns>Zero if there were no errors</returns>
        int Execute();
    }
}
