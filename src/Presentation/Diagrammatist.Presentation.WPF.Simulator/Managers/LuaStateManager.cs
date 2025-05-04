using NLua;
using System.Reflection;

namespace Diagrammatist.Presentation.WPF.Simulator.Managers
{
    /// <summary>
    /// A class that defines lua state manager.
    /// </summary>
    public sealed class LuaStateManager
    {
        private Lua _lua;
        private readonly Stack<Dictionary<string, object>> _snapshots = [];
        private bool _isIsolated;

        /// <summary>
        /// Initializes new lua state manager.
        /// </summary>
        public LuaStateManager()
        {
            _lua = new Lua();
        }

        /// <summary>
        /// Initializes isolated environment in the same lua state.
        /// </summary>
        public LuaStateManager(LuaStateManager stateManager)
        {
            _lua = stateManager._lua;
            _isIsolated = true;
        }

        /// <summary>
        /// Initializes lua.
        /// </summary>
        public void Initialize()
            => _lua ??= new Lua();

        /// <summary>
        /// Registers base functions.
        /// </summary>
        /// <param name="target">A class target where function handlers located are.</param>
        /// <param name="methods">Method names</param>
        public void RegisterFunctions(object target, params string[] methods)
        {
            if (methods.Length < 0 || methods.Length < 3)
                return;

            _lua.RegisterFunction("print", target, GetMethod(target, methods[0]));
            _lua.RegisterFunction("read", target, GetMethod(target, methods[1]));
            _lua.RegisterFunction("loop", target, GetMethod(target, methods[2]));
        }

        /// <summary>
        /// Register custom function.
        /// </summary>
        /// <param name="target">A class target where function handler located are.</param>
        /// <param name="name">A function name.</param>
        /// <param name="method">A method name.</param>
        public void RegisterFunction(object target, string name, string method)
        {
            _lua.RegisterFunction(name, target, GetMethod(target, method));
        }

        /// <summary>
        /// Resets current lua.
        /// </summary>
        public void Reset()
        {
            _lua?.Close();
            _lua = new Lua();
        }

        /// <summary>
        /// Executes script and saves snapshots.
        /// </summary>
        /// <param name="command"></param>
        public object[] ExecuteWithSnapshot(string command)
        {
            if (!_isIsolated)
            {
                SaveSnapshot();
            }
            return _lua.DoString(command);
        }

        /// <summary>
        /// Executes file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public object[] ExecuteFile(string filePath)
        {
            return _lua.DoFile(filePath);
        }

        /// <summary>
        /// Executes script.
        /// </summary>
        /// <param name="command"></param>
        public object[] Execute(string command)
        {
            return _lua.DoString(command);
        }

        /// <summary>
        /// Reverts changes back.
        /// </summary>
        public void Undo()
        {
            RestoreSnapshot();
        }

        /// <summary>
        /// Gets lua object's value.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <returns><see cref="Nullable"/> <see cref="object"/></returns>
        public object? GetValue(string key)
        {
            return _lua?[key];
        }

        /// <summary>
        /// Sets lua object's value.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <param name="value">A value.</param>
        public void SetValue(string key, object? value)
        {
            _lua[key] = value;
        }

        private void SaveSnapshot()
        {
            var globals = _lua.GetTable("_G");  
            var snapshot = new Dictionary<string, object>();

            foreach (var key in globals.Keys)
            {
                var keyStr = key?.ToString();  
                object value;

                try
                {
                    value = globals[key];  
                }
                catch (NLua.Exceptions.LuaException)
                {
                    continue;  
                }

                if (keyStr != null)
                {
                    snapshot[keyStr] = value;  
                }
            }

            _snapshots.Push(snapshot);  
        }

        private void RestoreSnapshot()
        {
            if (_snapshots.Count == 0)
                return;

            var snapshot = _snapshots.Pop();  
            var globals = _lua.GetTable("_G");  

            foreach (var key in globals.Keys)
            {
                globals[key] = null;  
            }

            foreach (var kv in snapshot)
            {
                globals[kv.Key] = kv.Value;  
            }
        }

        private static MethodInfo? GetMethod(object engine, string methodName)
        {
            return engine.GetType().GetMethod(methodName);
        }
    }
}
