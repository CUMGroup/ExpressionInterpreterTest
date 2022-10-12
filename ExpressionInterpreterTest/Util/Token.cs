using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Util {
    internal class Token {
        public TokenType Type { get; set; }
        public string Text { get; set; }
        public int StartPos { get; set; }

        public Token(TokenType type, string text, int startPos) {
            this.Type = type;
            this.Text = text;
            this.StartPos = startPos;
        }
    }
}
