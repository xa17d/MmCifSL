using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MmCifSL
{
    public class MmCifTokenizer
    {
        public MmCifTokenizer(string s)
        {
            this.s = s;
            this.status = TokenizeStatus.Idle;
            this.previousChar = ' ';
            this.i = 0;
        }

        private string s;
        private int i;
        private TokenizeStatus status;
        private char previousChar;

        StringBuilder currentToken = new StringBuilder(75); // 75 is maximum data name length

        public bool nextToken(out string outputToken, out TokenType tokenType)
        {
            while (i < s.Length)
            {
                char c = s[i];
                i++; // increment here, because if this method returns inside switch, i++ wouldn't be called anymore

                switch (status)
                {
                    case TokenizeStatus.Idle:

                        if (isWhiteSpace(c))
                        {
                            // do nothing
                            // stay in Idle status
                        }
                        else if (c == '\'')
                        {
                            status = TokenizeStatus.InSingleQuotation;
                        }
                        else if (c == '"')
                        {
                            status = TokenizeStatus.InDoubleQuotation;
                        }
                        else if (c == '#')
                        {
                            status = TokenizeStatus.InComment;
                        }
                        else if (isEol(previousChar) && c == ';')
                        {
                            status = TokenizeStatus.InSemiColonTextField;
                        }
                        else
                        {
                            status = TokenizeStatus.InGenericToken;
                            currentToken = new StringBuilder(); // currentToken.Clear();
                            currentToken.Append(c);
                        }
                        break;
                    case TokenizeStatus.InComment:

                        if (isEol(c))
                        {
                            status = TokenizeStatus.Idle;
                        }

                        break;
                    case TokenizeStatus.InGenericToken:

                        if (isWhiteSpace(c))
                        {
                            // end of token reached
                            return finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                        }
                        else
                        {
                            currentToken.Append(c);
                        }

                        break;
                    case TokenizeStatus.InSingleQuotation:

                        if (c == '\'')
                        {
                            status = TokenizeStatus.AfterSingleQuotation;
                        }
                        else
                        {
                            currentToken.Append(c);
                        }

                        break;
                    case TokenizeStatus.InDoubleQuotation:

                        if (c == '"')
                        {
                            status = TokenizeStatus.AfterDoubleQuotation;
                        }
                        else
                        {
                            currentToken.Append(c);
                        }

                        break;

                    case TokenizeStatus.AfterSingleQuotation:

                        if (isWhiteSpace(c))
                        {
                            // End of token reached
                            return finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                        }
                        else if (c == '\'')
                        {
                            currentToken.Append(c); // append, but stay in this mode in case of 'hello''
                        }
                        else
                        {
                            // not the end of the quotation
                            // for example 'a dog's life' 
                            // see http://www.iucr.org/resources/cif/spec/version1.1/cifsyntax Point 15.

                            currentToken.Append('\'');
                            currentToken.Append(c);
                        }

                        break;

                    case TokenizeStatus.AfterDoubleQuotation:

                        if (isWhiteSpace(c))
                        {
                            // End of token reached
                            return finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                        }
                        else if (c == '"')
                        {
                            currentToken.Append(c); // append, but stay in this mode in case of "hello""
                        }
                        else
                        {
                            // not the end of the quotation
                            // for example 'a dog's life' 
                            // see http://www.iucr.org/resources/cif/spec/version1.1/cifsyntax Point 15.

                            currentToken.Append('\"');
                            currentToken.Append(c);
                        }

                        break;

                    case TokenizeStatus.InSemiColonTextField:

                        if (isEol(previousChar) && c == ';')
                        {
                            currentToken.Remove(currentToken.Length - 1, 1); // remove last (which is the EOL)

                            // make sure to remove \n\r or \r\n but not \n\n or \r\r
                            if (currentToken.Length > 0)
                            {
                                char prePreviousChar = currentToken[currentToken.Length - 1];
                                if (prePreviousChar != previousChar)
                                {
                                    currentToken.Remove(currentToken.Length - 1, 1);
                                }
                            }

                            return finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                        }
                        else
                        {
                            currentToken.Append(c);
                        }

                        break;
                    default:
                        throw new MmCifParseException("internal error: unknown TokenizeStatus: "+status.ToString());
                }

                previousChar = c;
            }

            // Handle last token

            switch (status)
            {
                case TokenizeStatus.Idle:
                    finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                    return false;
                case TokenizeStatus.InComment:
                    finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                    return false;
                case TokenizeStatus.InGenericToken:
                    finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                    return false;
                case TokenizeStatus.InSingleQuotation:
                    throw new MmCifParseException("no ending single quotation mark (') found on end of file");
                case TokenizeStatus.InDoubleQuotation:
                    throw new MmCifParseException("no ending double quotation mark (\") found on end of file");
                case TokenizeStatus.InSemiColonTextField:
                    finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                    return false;
                case TokenizeStatus.AfterSingleQuotation:
                    finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                    return false;
                case TokenizeStatus.AfterDoubleQuotation:
                    finishToken(out outputToken, out tokenType, TokenizeStatus.Idle);
                    return false;
                default:
                    throw new MmCifParseException("internal error: unknown TokenizeStatus: " + status.ToString());
            }
        }

        private bool finishToken(out string outputToken, out TokenType tokenType, TokenizeStatus nextStatus)
        {
            outputToken = currentToken.ToString();
            currentToken = new StringBuilder(); // currentToken.Clear();

            if (
                status == TokenizeStatus.AfterSingleQuotation || 
                status == TokenizeStatus.AfterDoubleQuotation || 
                status == TokenizeStatus.InSingleQuotation || 
                status == TokenizeStatus.InDoubleQuotation || 
                status == TokenizeStatus.InSemiColonTextField)
            { tokenType = TokenType.Quoted; }
            else
            { tokenType = TokenType.NonQuoted; }

            status = nextStatus;

            return true;
        }

        private static bool isWhiteSpace(char c)
        {
            return (c == ' ' || c == '\t' || isEol(c));
        }

        private static bool isEol(char c)
        {
            return (c == '\n' || c == '\r');
        }

        private enum TokenizeStatus
        {
            Idle,
            InComment,
            InGenericToken,
            InSingleQuotation,
            InDoubleQuotation,
            InSemiColonTextField,
            AfterSingleQuotation,
            AfterDoubleQuotation
        }

        public enum TokenType
        {
            Quoted,
            NonQuoted
        }
    }
}
