using ExpressionInterpreterTest.Interpreting;
using ExpressionInterpreterTest.Nodes;
using ExpressionInterpreterTest.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Parsing {
    internal class Parser {
        public static Node Parse(string s) {
            return new Parser(new Lexer(s)).ParseExpression();
        }

        public static double Eval(string s, IContext ctx) {
            return Parse(s).Eval(ctx);
        }

        private Lexer lexer;

        public Parser(Lexer lexer) {
            this.lexer = lexer;
        }

        public Node ParseExpression() {
            Node expr = ParseAddition();

            if (lexer.CurrentToken.Type != TokenType.EOF) {
                throw new Exception("Unexpected characters at the end of the expression");
            }

            return expr;
        }

        /// <summary>
        /// Generic Parser Method for the different operators in the order of operations
        /// </summary>
        /// <param name="nextOperator">Next operator group method in the order of operations</param>
        /// <param name="tokenToOperator">Mapping of the operator tokens in the group to specific functions</param>
        /// <returns></returns>
        private Node ParseOperator(Func<Node> nextOperator, Func<TokenType, Func<double, double, double>> tokenToOperator) {
            Node lhs = nextOperator();

            while (true) {
                Func<double, double, double> op = tokenToOperator(lexer.CurrentToken.Type);

                if (op == null)
                    return lhs;

                lexer.NextToken();

                Node rhs = nextOperator();
                lhs = new NodeBinary(lhs, rhs, op,lexer.CurrentToken.Type);
            }
        }

        private Node ParseAddition() {

            return ParseOperator(ParseMultiplication, (t) => {
                return t switch {
                    TokenType.ADD => (a, b) => a + b,
                    TokenType.SUB => (a, b) => a - b,
                    _ => null
                };
            });
        }

        private Node ParseMultiplication() {

            return ParseOperator(ParsePower, (t) => {
                return t switch {
                    TokenType.MULT => (a, b) => a * b,
                    TokenType.DIV => (a, b) => a / b,
                    _ => null
                };
            });
        }

        private Node ParsePower() {

            return ParseOperator(ParseUnary, (t) => {
                return t switch {
                    TokenType.POWER => (a, b) => Math.Pow(a, b),
                    _ => null
                };
            });
        }

        private Node ParseUnary() {

            switch (lexer.CurrentToken.Type) {
                case TokenType.ADD:  // noop -> skip
                    lexer.NextToken();
                    return ParseUnary();
                case TokenType.SUB: {
                        lexer.NextToken();
                        Node rhs = ParseUnary();
                        return new NodeUnary(rhs, (a) => -a, lexer.CurrentToken.Type);
                    }
            }

            return ParseHighest();
        }

        private Node ParseHighest() {

            switch (lexer.CurrentToken.Type) {
                case TokenType.NUM: {
                        Node node = new NodeNum(double.Parse(lexer.CurrentToken.Text, CultureInfo.InvariantCulture));
                        lexer.NextToken();
                        return node;
                    }
                case TokenType.OPEN_PARENS: {
                        lexer.NextToken(); // skip '('
                        Node node = ParseAddition(); // new top level parsing
                        if (lexer.CurrentToken.Type != TokenType.CLOSE_PARENS)
                            throw new Exception("Missing closing parenthesis!");
                        lexer.NextToken();
                        return node;
                    }
                case TokenType.IDENTIFIER: {
                        string name = lexer.CurrentToken.Text;
                        lexer.NextToken();

                        if (lexer.CurrentToken.Type != TokenType.OPEN_PARENS) {
                            return new NodeVar(name);
                        }

                        lexer.NextToken();

                        var args = new List<Node>();
                        while (true) {
                            args.Add(ParseAddition());

                            if (lexer.CurrentToken.Type == TokenType.COMMA) {
                                lexer.NextToken();
                                continue;
                            }

                            break; // all args done
                        }

                        if (lexer.CurrentToken.Type != TokenType.CLOSE_PARENS)
                            throw new Exception("Missing closing parenthesis!");
                        lexer.NextToken();

                        return new NodeFunction(args.ToArray(), name);
                    }
            }

            throw new Exception($"Unexpected Token: {lexer.CurrentToken.Text} at position {lexer.CurrentToken.StartPos}");
        }

    }
}
