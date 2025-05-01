using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using System.Text.RegularExpressions;

namespace Diagrammatist.Presentation.WPF.Simulator.Validators.Flowchart
{
    /// <summary>
    /// A static class that is responsible for flowchart script validation.
    /// </summary>
    public static class FlowchartScriptValidator
    {
        /// <summary>
        /// Validates lua script judging by flowchart subtype.
        /// </summary>
        /// <param name="subtype">Flowchart subtype.</param>
        /// <param name="script">Current lua script.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns><see cref="bool"/> result and <see cref="string"/> error message.</returns>
        public static bool Validate(FlowchartSubtypeModel subtype, string script, out string errorMessage)
        {
            errorMessage = string.Empty;

            switch (subtype)
            {
                case FlowchartSubtypeModel.StartEnd:
                    if (!string.IsNullOrWhiteSpace(script))
                    {
                        errorMessage = "The 'StartEnd' block must not contain LuaScript.";
                        return false;
                    }
                    break;

                case FlowchartSubtypeModel.Process:
                    if (string.IsNullOrWhiteSpace(script))
                    {
                        errorMessage = "The 'Process' block must contain the code.";
                        return false;
                    }
                    break;

                case FlowchartSubtypeModel.InputOutput:
                    if (!string.IsNullOrWhiteSpace(script))
                    {
                        errorMessage = "The 'InputOutput' block must not contain LuaScript.";
                        return false;
                    }
                    break;

                case FlowchartSubtypeModel.Decision:
                    if (!Regex.IsMatch(script, @"return\s+(true|false)\b"))
                    {
                        errorMessage = "The 'Decision' block must return a boolean value (return true/false).";
                        return false;
                    }
                    break;

                case FlowchartSubtypeModel.Connector:
                    if (!string.IsNullOrWhiteSpace(script))
                    {
                        errorMessage = "The 'Connector' block must not contain LuaScript.";
                        return false;
                    }
                    break;

                case FlowchartSubtypeModel.Preparation:
                    if (!Regex.IsMatch(script, @"\b(for|while|repeat)\b"))
                    {
                        errorMessage = "The 'Preparation' block must contain a loop (for, while, repeat).";
                        return false;
                    }
                    break;

                case FlowchartSubtypeModel.PredefinedProcess:
                    if (string.IsNullOrWhiteSpace(script))
                    {
                        errorMessage = "The 'PredefinedProcess' block must contain the code.";
                        return false;
                    }
                    break;

                default:
                    errorMessage = "Unknown block type.";
                    return false;
            }

            return true;
        }
    }
}
