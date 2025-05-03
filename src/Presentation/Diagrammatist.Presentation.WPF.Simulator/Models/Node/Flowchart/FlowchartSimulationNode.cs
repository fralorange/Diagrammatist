using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Validators.Flowchart;
using System.ComponentModel.DataAnnotations;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart
{
    /// <summary>
    /// A class that derived from <see cref="SimulationNode"/>. Defines flowchart simulation node.
    /// </summary>
    public class FlowchartSimulationNode : SimulationNode
    {
        private string _luaScript = string.Empty;

        /// <inheritdoc/>
        [CustomValidation(typeof(FlowchartSimulationNode), nameof(ValidateLuaScript))]
        public override string LuaScript
        {
            get => _luaScript;
            set
            {
                SetProperty(ref _luaScript, value, true);
            }
        }

        /// <summary>
        /// A method that validates the Lua script.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ValidationResult? ValidateLuaScript(string name, ValidationContext context)
        {
            var instance = (FlowchartSimulationNode)context.ObjectInstance;

            if (instance.Figure is FlowchartFigureModel flowchartFigure)
            {
                if (!FlowchartScriptValidator.Validate(flowchartFigure.Subtype, instance.LuaScript, out string errorMessage))
                {
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
