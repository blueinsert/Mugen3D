using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace Mugen3D
{
    public class Tokenizer
    {

        public List<Token> GetTokens(TextAsset textAssert)
        {
            string content = textAssert.text;
            return GetTokens(content.ToCharArray());
        }

        public List<Token> GetTokens(string fileName)
        {
            string content = "";
            try{
            StreamReader reader = new StreamReader(fileName, Encoding.UTF8);
            content = reader.ReadToEnd();
            reader.Close();
            }
            catch (IOException e)
            {
                Debug.Log(e.ToString());
            }
            return GetTokens(content.ToCharArray());
        }

        private List<Token> GetTokens(char[] charStream)
        {
            charStream = RemoveComments(charStream);
            charStream = RemoveRepeatSpaceNewline(charStream);
            return ParseToTokens(charStream);
        }

        private char[] RemoveComments(char[] charStream)
        {
            List<char> newCharArray = new List<char>();
            int length = charStream.Length;
            int pos = 0;
            while (pos < length)
            {
                char c = charStream[pos++];
                if (c == '/')
                {
                    c = charStream[pos++];
                    if (c == '/')
                    {
                        while (pos < length)
                        {
                            c = charStream[pos++];
                            if (c == '\n')
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        newCharArray.Add(charStream[pos - 2]);
                        newCharArray.Add(charStream[pos - 1]);
                    }
                }
                else
                {
                    newCharArray.Add(c);
                }
            }
           return newCharArray.ToArray();
        }

        private char[] RemoveRepeatSpaceNewline(char[] charStream)
        {
             List<char> newCharArray = new List<char>();
            int length = charStream.Length;
            int pos = 0;
            while (pos < length)
            {
                char c = charStream[pos++];
                if (c == ' ')
                {
                    if (!(newCharArray.Count > 0 && newCharArray[newCharArray.Count - 1] == ' '))
                    {
                        newCharArray.Add(c);
                    }
                }
                else if (c == '\n')
                {
                    if (!(newCharArray.Count > 0 && newCharArray[newCharArray.Count - 1] == '\n'))
                    {
                        newCharArray.Add(c);
                    }
                }
                else
                {
                    newCharArray.Add(c);
                }
            }
            return newCharArray.ToArray();
        }


        private string ParseVarName(char[] charStream, ref int pos)
        {
            int length = charStream.Length;
            StringBuilder mBuffer = new StringBuilder();
            while (pos < length)
            {
                char c = charStream[pos++];
                if ('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z' || '0' <= c && c <= '9' || c == '.')
                {
                    mBuffer.Append(c);
                }
                else
                {
                    pos--;
                    break;
                }
            }
            return mBuffer.ToString();
        }

        private float ParseNum(char[] charStream, ref int pos)
        {
            int length = charStream.Length;
            float num = 0;
            float numDecimal = 0;
            while (pos < length)
            {
                char c = charStream[pos++];
                if ('0' <= c && c <= '9')
                {
                    num = num * 10 + (c - '0');
                }
                else if (c == '.')
                {
                    float denominator = 0.1f;
                    while (pos < length)
                    {
                        c = charStream[pos++];
                        if ('0' <= c && c <= '9')
                        {
                            numDecimal += (c - '0') * denominator;
                            denominator *= 0.1F;
                        }
                        else
                        {
                            pos--;
                            break;
                        }
                    }

                }
                else
                {
                    pos--;
                    break;
                }
            }
            float result = num + numDecimal;
            return result;
        }

        private List<Token> ParseToTokens(char[] charStream)
        {
            List<Token> tokens = new List<Token>();
            int length = charStream.Length;
            int pos = 0;

            while (pos < length)
            {
                char c = charStream[pos++];
                if ('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z')
                {//a var begin
                    pos--;
                    string tokenStr = ParseVarName(charStream, ref pos);
                    tokens.Add(new Token(tokenStr, TokenType.VarName));  
                }
                else if ('0' <= c && c <= '9')
                {
                    pos--;
                    float num = ParseNum(charStream, ref pos);
                    tokens.Add(new Token(num.ToString(), TokenType.Num));
                }
                else if (c == '"')
                {
                    StringBuilder mBuffer = new StringBuilder();
                    while (pos < length)
                    {
                        c = charStream[pos++];
                        if (c != '"')
                        {
                            mBuffer.Append(c);
                        }
                        else
                        {
                            break;
                        }
                    }
                    tokens.Add(new Token(mBuffer.ToString(), TokenType.Str));
                }
                else if (c == '-')
                {
                    if (charStream[pos] == ' ' && charStream[pos - 2] == ' ')
                    {
                        tokens.Add(new Token("-", TokenType.Op_Sub));
                    }
                    else if ('0' <= charStream[pos] && charStream[pos] <= '9')
                    {
                        float num = -ParseNum(charStream, ref pos);
                        tokens.Add(new Token(num.ToString(), TokenType.Num));
                    }
                    else
                    {
                        tokens.Add(new Token("-", TokenType.Op_Neg));
                    }
                }
                else
                {
                    switch (c)
                    {
                        case '+':
                            tokens.Add(new Token("+", TokenType.Op));
                            break;
                        case '*':
                            tokens.Add(new Token("*", TokenType.Op));
                            break;
                        case '/':
                            tokens.Add(new Token("/", TokenType.Op));
                            break;
                        case '=':
                            c = charStream[pos++];
                            if (c == '=')
                            {
                                Token t = new Token("==", TokenType.Op);
                                tokens.Add(t);
                            }
                            else
                            {
                                pos--;
                                Token t = new Token("=", TokenType.Op);
                                tokens.Add(t);
                            }
                            break;
                        case '>':
                            c = charStream[pos++];
                            if (c == '=')
                            {
                                tokens.Add(new Token(">=", TokenType.Op));
                            }
                            else
                            {
                                pos--;
                                tokens.Add(new Token(">", TokenType.Op));
                            }
                            break;
                        case '<':
                            c = charStream[pos++];
                            if (c == '=')
                            {
                                tokens.Add(new Token("<=", TokenType.Op));
                            }
                            else
                            {
                                pos--;
                                tokens.Add(new Token("<", TokenType.Op));
                            }
                            break;
                        case '!':
                            c = charStream[pos++];
                            if (c == '=')
                                tokens.Add(new Token("!=", TokenType.Op));
                            else
                            {
                                tokens.Add(new Token("!", TokenType.Op));
                                pos--;
                            }
                            break;
                        case '&':
                            c = charStream[pos++];
                            if (c == '&')
                            {
                                tokens.Add(new Token("&&", TokenType.Op));
                            }
                            else
                            {
                                pos--;
                            }
                            break;
                        case '|':
                            c = charStream[pos++];
                            if (c == '|')
                            {
                                tokens.Add(new Token("||", TokenType.Op));
                            }
                            else
                            {
                                pos--;
                            }
                            break;
                        case '(':
                            tokens.Add(new Token("(", TokenType.Op));
                            break;
                        case ')':
                            tokens.Add(new Token(")", TokenType.Op));
                            break;
                        case '[':
                            tokens.Add(new Token("[", TokenType.Op));
                            break;
                        case ']':
                            tokens.Add(new Token("]", TokenType.Op));
                            break;
                        case ',':
                            tokens.Add(new Token(",", TokenType.Op));
                            break;
                        case '\n':  
                            tokens.Add(new Token("\n", TokenType.NewLine));
                            break;
                        case ':':
                            tokens.Add(new Token(":", TokenType.Op));
                            break;
                        case '$':
                            tokens.Add(new Token("$", TokenType.Op));
                            break;
                        case '~':
                            tokens.Add(new Token("~", TokenType.Op));
                            break;
                        default:
                            break;
                    }
                }
            }
            return tokens;
        }
    
    }//class

}//namespace
