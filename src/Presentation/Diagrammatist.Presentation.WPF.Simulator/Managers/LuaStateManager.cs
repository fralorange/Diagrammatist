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

        /// <summary>
        /// Initializes new lua state manager.
        /// </summary>
        public LuaStateManager()
        {
            _lua = new Lua();
        }

        /// <summary>
        /// Initializes lua.
        /// </summary>
        public void Initialize()
            => _lua ??= new Lua();

        /// <summary>
        /// Registers functions
        /// </summary>
        /// <param name="engine">A class target where function handlers located are.</param>
        /// <param name="methods">Method names</param>
        public void RegisterFunctions(object engine, params string[] methods)
        {
            if (methods.Length < 0 || methods.Length < 3)
                return;

            _lua.RegisterFunction("print", engine, GetMethod(engine, methods[0]));
            _lua.RegisterFunction("read", engine, GetMethod(engine, methods[1]));
            _lua.RegisterFunction("loop", engine, GetMethod(engine, methods[2]));
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
            SaveSnapshot();
            return _lua.DoString(command);
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
            if (_lua != null)
            {
                _lua[key] = value;
            }
        }


        private void SaveSnapshot()
        {
            var globals = _lua.GetTable("_G");
            var snapshot = new Dictionary<string, object>();

            foreach (var key in globals)
            {
                var keyStr = key.ToString();
                var value = _lua[keyStr];

                if (keyStr is not null)
                {
                    snapshot[keyStr] = value;
                }
            }
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
                _lua[kv.Key] = kv.Value;
            }
        }

        private static MethodInfo? GetMethod(object engine, string methodName)
        {
            return engine.GetType().GetMethod(methodName);
        }
    }
}
