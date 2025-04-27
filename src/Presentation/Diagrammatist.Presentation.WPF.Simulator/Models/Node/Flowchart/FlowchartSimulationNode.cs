using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Validators.Flowchart;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart
{
    /// <summary>
    /// A class that derived from <see cref="SimulationNode"/>. Defines flowchart simulation node.
    /// </summary>
    public class FlowchartSimulationNode : SimulationNode
    {
        private string _luaScript = string.Empty;

        /// <inheritdoc/>
        public override string LuaScript
        {
            get => _luaScript;
            set
            {
                //if (Figure is FlowchartFigureModel flowchartFigure
                //    && !FlowchartScriptValidator.Validate(flowchartFigure.Subtype, value, out var error))
                //{
                //    throw new InvalidOperationException($"LuaScript error: {error}");
                //}

                _luaScript = value;
            }
        }
    }
}
