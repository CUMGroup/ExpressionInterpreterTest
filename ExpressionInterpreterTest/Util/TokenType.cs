using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Util {
    internal enum TokenType {
        EOF,

        ADD,
        SUB,
        MULT,
        DIV,
        POWER,

        OPEN_PARENS,
        CLOSE_PARENS,
        COMMA,
        ASSIGN,

        IDENTIFIER,

        NUM
    }
}
