using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Interpreting {
    internal class ContextLibrary : IContext {
        public double CallFunction(string funcName, double[] args) {
            // Test for params keyword method
            var paramTypes = new Type[1] { typeof(double[]) };
            var func = this.GetType().GetMethod(funcName,paramTypes);
            if (func != null && func.GetParameters().Last().GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0) {
                var argObjs = new object[] { args };
                return (double)func.Invoke(this, argObjs);

            // Other methods
            } else {
                funcName = funcName.ToLower();
                var types = new Type[args.Length];
                for (int i = 0; i < args.Length; ++i) {
                    types[i] = args[i].GetType();
                }
                func = this.GetType().GetMethod(funcName, types) ;
                if (func == null)
                    throw new InvalidDataException($"Unknown function: {funcName}");

                var argObjs = args.Select(x => (object)x).ToArray();
                return (double)func.Invoke(this, argObjs);
            }

        }

        public double GetVariable(string varName) {
            varName = varName.ToLower();

            var prop = this.GetType().GetProperty(varName);
            if (prop != null) {
                return (double)prop.GetValue(this);
            }

            if (varDict.ContainsKey(varName)) {
                return varDict[varName];
            }

            throw new InvalidDataException($"Unknown variable: '{varName}'");
        }

        public double SetVariable(string varName, double val) {
            varName = varName.ToLower();

            var prop = this.GetType().GetProperty(varName);
            if (prop != null) {
                throw new InvalidDataException($"Variable '{varName}' is a constant and can't be changed!");
            }

            if (varDict.ContainsKey(varName)) {
                varDict[varName] = val;
            } else {
                varDict.Add(varName, val);
            }
            return varDict[varName];
        }

        private Dictionary<string, double> varDict = new();
    }
}
