using Diagrammatist.Domain.Figures;
using Diagrammatist.Domain.Simulator;
using Diagrammatist.Domain.Simulator.Flowchart;
using Diagrammatist.Presentation.WPF.Core.Mappers.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Simulator.Models.Context;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;

namespace Diagrammatist.Presentation.WPF.Simulator.Mappers
{
    /// <summary>
    /// Simulation mapper extensions.
    /// </summary>
    public static class SimulationMapperExtensions
    {
        /// <summary>
        /// Maps <see cref="SimulationContextData"/> to <see cref="SimulationContext"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="figures"></param>
        /// <returns></returns>
        public static SimulationContext ToModel(this SimulationContextData context, ICollection<FigureModel> figures)
        {
            return new SimulationContext
            {
                Nodes = context.Nodes.Select(node => node.ToModel(figures)),
                Connections = context.Connections.Select(con => con.ToModel(figures)),
            };
        }

        /// <summary>
        /// Maps <see cref="SimulationContext"/> to <see cref="SimulationContextData"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="figures"></param>
        /// <returns></returns>
        public static SimulationContextData ToDomain(this SimulationContext context, ICollection<Figure> figures)
        {
            return new SimulationContextData
            {
                Nodes = context.Nodes.Select(node => node.ToDomain()),
                Connections = context.Connections.Select(con => con.ToDomain(figures)),
            };
        }

        /// <summary>
        /// Maps <see cref="SimulationNodeData"/> to <see cref="SimulationNode"/>.
        /// </summary>
        public static SimulationNode ToModel(this SimulationNodeData data, ICollection<FigureModel> figures)
        {
            var figure = figures.FirstOrDefault(f => f.Id == data.FigureId)
                         ?? throw new InvalidOperationException($"Figure with id {data.FigureId} not found");

            return data switch
            {
                FlowchartSimulationNodeData flowchart =>
                    new FlowchartSimulationNode
                    {
                        Figure = figure,
                        LuaScript = flowchart.LuaScript
                    },
                _ => throw new NotSupportedException($"Simulation node type {data.GetType().Name} is not supported.")
            };
        }

        /// <summary>
        /// Maps <see cref="SimulationNode"/> to <see cref="SimulationNodeData"/>.
        /// </summary>
        public static SimulationNodeData ToDomain(this SimulationNode model)
        {
            return model switch
            {
                FlowchartSimulationNode flowchart =>
                    new FlowchartSimulationNodeData
                    {
                        FigureId = flowchart.Figure.Id,
                        LuaScript = flowchart.LuaScript
                    },
                _ => throw new NotSupportedException($"Simulation node type {model.GetType().Name} is not supported.")
            };
        }
    }
}
