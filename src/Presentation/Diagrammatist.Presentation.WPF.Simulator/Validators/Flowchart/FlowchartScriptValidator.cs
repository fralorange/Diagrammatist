using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using NLua;
using NLua.Exceptions;
using System.Text.RegularExpressions;

namespace Diagrammatist.Presentation.WPF.Simulator.Validators.Flowchart
{
    /// <summary>
    /// A static class that is responsible for flowchart script validation.
    /// </summary>
    public static partial class FlowchartScriptValidator
    {
        /// <summary>
        /// A method that validates the flowchart script.
        /// </summary>
        /// <param name="subtype"></param>
        /// <param name="script"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool Validate(FlowchartSubtypeModel subtype, string script, out string errorMessage)
        {
            errorMessage = string.Empty;

            switch (subtype)
            {
                case FlowchartSubtypeModel.StartEnd:
                    if (!string.IsNullOrWhiteSpace(script))
                        return Fail("The 'StartEnd' block must not contain LuaScript.", out errorMessage);
                    break;

                case FlowchartSubtypeModel.Process:
                    if (!TryCompileLua(script, out errorMessage))
                        return false;
                    break;

                case FlowchartSubtypeModel.InputOutput:
                    if (!IORegex().IsMatch(script))
                        return Fail("The 'InputOutput' block may only contain calls to print(...) or read(...).", out errorMessage);

                    if (!TryCompileLua_WithIO(script, out errorMessage))
                        return false;
                    break;

                case FlowchartSubtypeModel.Decision:
                    if (!DecisionRegex().IsMatch(script))
                        return Fail("The 'Decision' block must contain a return statement.", out errorMessage);
                    break;

                case FlowchartSubtypeModel.Connector:
                    if (!string.IsNullOrWhiteSpace(script))
                        return Fail("The 'Connector' block must not contain LuaScript.", out errorMessage);
                    break;

                case FlowchartSubtypeModel.Preparation:
                    if (!LoopRegex().IsMatch(script))
                        return Fail("The 'Preparation' block must contain at least one loop(...) call.", out errorMessage);

                    if (!TryCompileLua(script, out errorMessage))
                        return false;
                    break;

                case FlowchartSubtypeModel.PredefinedProcess:
                    if (!TryCompileLua(script, out errorMessage))
                        return false;
                    break;

                default:
                    return Fail("Unknown block type.", out errorMessage);
            }

            return true;
        }

        /// <summary>
        /// A method that compiles the Lua code.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private static bool TryCompileLua(string code, out string errorMessage)
        {
            errorMessage = string.Empty;
            using var lua = new Lua();
            lua.RegisterFunction("loop",
                                 null,
                                 typeof(FlowchartScriptValidator).GetMethod(nameof(LoopStub)));

            try
            {
                lua.LoadString(code, "__validate__");
                return true;
            }
            catch (LuaScriptException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// A method that compiles the Lua code with IO functions.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private static bool TryCompileLua_WithIO(string code, out string errorMessage)
        {
            errorMessage = string.Empty;
            using var lua = new Lua();
            lua.RegisterFunction("print",
                                 null,
                                 typeof(FlowchartScriptValidator).GetMethod(nameof(PrintStub)));
            lua.RegisterFunction("read",
                                 null,
                                 typeof(FlowchartScriptValidator).GetMethod(nameof(ReadStub)));

            try
            {
                lua.LoadString(code, "__validate_io__");
                return true;
            }
            catch (LuaScriptException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// A method that compiles and runs the decision code.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private static bool TryCompileAndRunDecision(string code, out string errorMessage)
        {
            errorMessage = string.Empty;
            using var lua = new Lua();
            lua.RegisterFunction("loop",
                                 null,
                                 typeof(FlowchartScriptValidator).GetMethod(nameof(LoopStub)));
            lua.RegisterFunction("print",
                                 null,
                                 typeof(FlowchartScriptValidator).GetMethod(nameof(PrintStub)));
            lua.RegisterFunction("read",
                                 null,
                                 typeof(FlowchartScriptValidator).GetMethod(nameof(ReadStub)));

            try
            {
                string wrapped = $"function __decider__()\n{code}\nend";
                lua.DoString(wrapped);

                var func = lua.GetFunction("__decider__");
                var result = func.Call();

                if (result == null || result.Length == 0 || !(result[0] is bool))
                    return Fail("Decision must return a boolean value.", out errorMessage);

                return true;
            }
            catch (LuaScriptException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// A stub function for loop.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool LoopStub(params object[] args) => true;
        /// <summary>
        /// A stub function for print.
        /// </summary>
        /// <param name="args"></param>
        public static void PrintStub(params object[] args) {  }
        /// <summary>
        /// A stub function for read.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object? ReadStub(params object[] args) => null;

        /// <summary>
        /// A helper method that returns false and sets the error message.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private static bool Fail(string msg, out string errorMessage)
        {
            errorMessage = msg;
            return false;
        }

        /// <summary>
        /// A regex that matches print(...) or read(...) statements.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"\b(print|read)\s*\(")]
        private static partial Regex IORegex();
        /// <summary>
        /// A regex that matches loop(...) statements.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"\bloop\s*\(")]
        private static partial Regex LoopRegex();
        [GeneratedRegex(@"\breturn\s+.+")]
        private static partial Regex DecisionRegex();
    }
}
