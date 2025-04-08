namespace Diagrammatist.Presentation.WPF.Simulator.Interfaces
{
    /// <summary>
    /// An interface that provides I/O operations.
    /// </summary>
    public interface ISimulationIO
    {
        /// <summary>
        /// Gets input.
        /// </summary>
        /// <param name="variableNames"></param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> of: (variable name) : (value)</returns>
        Dictionary<string, string>? GetInput(List<string> variableNames);
        /// <summary>
        /// Shows output.
        /// </summary>
        /// <param name="output"></param>
        void ShowOutput(string output);
    }
}
