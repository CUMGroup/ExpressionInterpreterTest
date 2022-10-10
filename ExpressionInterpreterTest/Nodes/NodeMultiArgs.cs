using ExpressionInterpreterTest.Interpreting;
using ExpressionInterpreterTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal class NodeMultiArgs : Node {

        public List<Node> Children { get; set; }
        public TokenType OperationType { get; private set; }
        private Func<double, double, double> op;

        public NodeMultiArgs(List<Node> children, TokenType opType, Func<double, double, double> op) {
            this.Children = children;
            this.OperationType = opType;
            this.op = op;
            if(OperationType == TokenType.SUB) {
                for (int i = 1; i < this.Children.Count; ++i) {
                    this.Children[i] = new NodeMultiArgs(new List<Node> { new NodeNum(-1), Children[i] }, TokenType.MULT, (a, b) => a * b);
                }
                OperationType = TokenType.ADD;
            }else if(OperationType == TokenType.DIV) {

            }
        }

        public override Node Eval(IContext ctx) {
            // eval all Children
            Children = Children.Select(e => e.Eval(ctx)).ToList();

            // Flatten same operators
            var childWithSameOp = Children.Where(e => e is NodeMultiArgs && ((NodeMultiArgs)e).OperationType == this.OperationType).ToList();
            if (childWithSameOp.Any()) {
                Children = Children.Except(childWithSameOp).ToList();
                Children.AddRange(childWithSameOp.SelectMany(e => ((NodeMultiArgs)e).Children));
            }

            // calc up numbers
            var nums = Children.Where(e => e is NodeNum).ToList();
            if (nums.Any()) {
                Children = Children.Except(nums).ToList();
                Children.Add(new NodeNum(nums.Select(e => ((NodeNum)e).GetValue()).Aggregate(op)));
            }

            if (OperationType == TokenType.ADD || OperationType == TokenType.SUB) {
                Dictionary<Node, double> calcedVars = new();
                // calc up vars
                var varNodesOrig = Children.Where(e => e is NodeVar || e is NodeFunction);
                foreach (Node v in varNodesOrig) {
                    if (calcedVars.ContainsKey(v))
                        calcedVars[v] += op(calcedVars[v], 1);
                    else
                        calcedVars.Add(v, op(0,1));
                }
            }else if (OperationType == TokenType.MULT)

        }

        public override string ToString() {
            throw new NotImplementedException();
        }

        // override object.Equals
        public override bool Equals(object obj) {
            
        }

        // override object.GetHashCode
        public override int GetHashCode() {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }
    }
}
