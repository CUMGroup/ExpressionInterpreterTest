using ExpressionInterpreterTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest {
    internal class Lexer {
        private string input;

        private List<Token> tokens;
        public List<Token> Tokens {
            get {
                if (tokens == null || tokens.Count <= 0) Lex();
                return tokens;
            }
        }

        public int CurrentIndex { get; set; } = 0;
        public Token CurrentToken {
            get {
                return Tokens[CurrentIndex];
            }
        }
        public Token NextToken() {
            if (CurrentIndex + 1 < Tokens.Count) ++CurrentIndex;
            return CurrentToken;
        }

        public Lexer(string input) {
            this.input = input;
        }

        /// <summary>
        /// Adds one specific Tokentype to the tokens
        /// </summary>
        /// <param name="type">Type to add</param>
        /// <param name="lookahead">currently looked at character</param>
        /// <param name="startPos">start pos of the character</param>
        /// <param name="currentPos">reference to the current position in the input</param>
        private void AddOneCharToken(TokenType type, char lookahead, int startPos, ref int currentPos) {
            ++currentPos;
            tokens.Add(new Token(type, lookahead.ToString(), startPos));
        }

        /// <summary>
        /// Adds a Token for a one or two character long tokentype
        /// </summary>
        /// <param name="charToTokenDict">Mapping from the second character of the token to the specific Tokentype. Character '_' is the default: The current character gets interpreted as a one character Token</param>
        /// <param name="lookahead">currently looked at character</param>
        /// <param name="startPos">start pos of the character</param>
        /// <param name="currentPos">reference to the current position in the input</param>
        private void AddTwoCharToken(Dictionary<char, TokenType> charToTokenDict, char lookahead, int startPos, ref int currentPos) {
            // following character is not a possible follow up -> we have a one character token;; Dict has to contain default case '_'
            if (!charToTokenDict.ContainsKey(input[++currentPos]) && charToTokenDict.ContainsKey('_')) {
                tokens.Add(new Token(charToTokenDict['_'], lookahead.ToString(), startPos));

                // Following character is defined -> two character token
            } else if (charToTokenDict.ContainsKey(input[currentPos])) {
                tokens.Add(new Token(charToTokenDict[input[currentPos]], lookahead.ToString() + input[currentPos].ToString(), startPos));
                ++currentPos;

                // Error: default character '_' is not defined in the dictionary
                // occurs if there is a token which only consists of a two character sequence and has no one character counterpart
                // if the user does not enter the specific second character this error should occur
                // TODO: Maybe better error handling? Or recovery?
            } else {
                throw new Exception($"Unexpected character sequence {lookahead.ToString() + input[currentPos].ToString()} at {currentPos}");
            }
        }

        public List<Token> Lex() {
            tokens = new List<Token>();
            int currentPos = 0;

            while (currentPos < input.Length) {
                int startPos = currentPos;
                char lookahead = input[currentPos];
                if (IsWhitespace(lookahead)) {
                    ++currentPos;  // skip whitespaces
                    continue;
                }
                // Test for operators and other symbols
                switch (lookahead) {
                    case '+':
                        AddOneCharToken(TokenType.ADD, lookahead, startPos, ref currentPos);
                        continue;
                    case '-':
                        AddOneCharToken(TokenType.SUB, lookahead, startPos, ref currentPos);
                        continue;
                    case '*':
                        AddOneCharToken(TokenType.MULT, lookahead, startPos, ref currentPos);
                        continue;
                    case '/':
                        AddOneCharToken(TokenType.DIV, lookahead, startPos, ref currentPos);
                        continue;
                    case '^':
                        AddOneCharToken(TokenType.POWER, lookahead, startPos, ref currentPos);
                        continue;
                    case '=':
                        AddOneCharToken(TokenType.ASSIGN, lookahead, startPos, ref currentPos);
                        continue;
                    case '(':
                        AddOneCharToken(TokenType.OPEN_PARENS, lookahead, startPos, ref currentPos);
                        continue;
                    case ')':
                        AddOneCharToken(TokenType.CLOSE_PARENS, lookahead, startPos, ref currentPos);
                        continue;
                    case ',':
                        AddOneCharToken(TokenType.COMMA, lookahead, startPos, ref currentPos);
                        continue;
                }

                // Test for letters
                if (IsLetter(lookahead)) {
                    string text = "";
                    // Add all following letters or digits to the identifier
                    while (currentPos < input.Length && IsLetterOrDigit(input[currentPos])) {
                        text += input[currentPos];
                        ++currentPos;
                    }
                    // is it an identifier or a specific literal?
                    TokenType type = TokenType.IDENTIFIER;
                    tokens.Add(new Token(type, text, startPos));

                    // Test for numbers
                } else if (IsDigit(lookahead) || lookahead == '.') {
                    string text = "";
                    bool hasExp = false;
                    bool hasDecimal = false;
                    // loop through following symbols
                    while (currentPos < input.Length
                        && (IsDigit(input[currentPos])  // digits 
                        || (Char.ToLower(input[currentPos]) == 'e' && !hasExp) //exponent symbol
                        || (input[currentPos] == '-' && hasExp) // or negative symbol for a negative exponent 
                        || (input[currentPos] == '.' && !hasDecimal && !hasExp))) { //Decimal point

                        // only allow negative symbol after the exponent symbol
                        if (input[currentPos] == '-' && Char.ToLower(input[currentPos - 1]) != 'e')
                            break;
                        // set exponent flag
                        if (Char.ToLower(input[currentPos]) == 'e') {
                            hasExp = true;
                            if (text.EndsWith('.')) text += '0';
                        }
                        if (input[currentPos] == '.')
                            hasDecimal = true;

                        text += input[currentPos];
                        ++currentPos;
                    }
                    // if the number ends with the exponent symbol -> remove it; does not belong to the number
                    if (text.EndsWith("e")) {
                        text = text.Substring(0, text.Length - 1);
                        --currentPos;
                    }
                    if (text.EndsWith(".")) text += '0';
                    tokens.Add(new Token(TokenType.NUM, text, startPos));

                    // All other characters should throw an exception
                } else {
                    throw new Exception($"Unknown character {lookahead} at position {currentPos}");
                }
            }

            // Add the end of file marker token to the end
            tokens.Add(new Token(TokenType.EOF, "<EOF>", currentPos));
            return tokens;
        }


        #region Helpers
        private bool IsWhitespace(char inp) {
            return Char.IsWhiteSpace(inp);
        }

        private bool IsDigit(char inp) {
            return Char.IsDigit(inp);
        }

        private bool IsLetter(char inp) {
            return Char.IsLetter(inp);
        }

        private bool IsLetterOrDigit(char inp) {
            return Char.IsLetterOrDigit(inp);
        }
        #endregion
    }
}
