using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace Mugen3D
{
    public class Tokenizer
    {
        private char[] mCharStream;
        private List<Token> mTokenArray = new List<Token>();

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

        public List<Token> GetTokens(char[] charStream)
        {
            mCharStream = charStream;
            RemoveComments();
            ParseToTokens();
            return mTokenArray;
        }

        private void RemoveComments()
        {
            List<char> newCharArray = new List<char>();
            int pos = 0;
            bool isInEndOfFile = false;
            while (!isInEndOfFile)
            {
                char c = mCharStream[pos++];
                if (pos >= mCharStream.Length)
                    isInEndOfFile = true;
                if (c == '/')
                {
                    c = mCharStream[pos++];
                    if (pos >= mCharStream.Length)
                        isInEndOfFile = true;
                    if (c == '/')
                    {
                        while (!isInEndOfFile)
                        {
                            c = mCharStream[pos++];
                            if (pos >= mCharStream.Length)
                                isInEndOfFile = true;
                            if (c == '\n')
                            {
                                break;
                            }
                        }
                    }
                    else if (c == '*')
                    {
                        while (!isInEndOfFile)
                        {
                            c = mCharStream[pos++];
                            if (pos >= mCharStream.Length)
                                isInEndOfFile = true;
                            if (c == '*')
                            {
                                c = mCharStream[pos++];
                                if (pos >= mCharStream.Length)
                                    isInEndOfFile = true;
                                if (c == '/')
                                {
                                    break;
                                }
                                else
                                {
                                    pos--;
                                }
                            }
                        }
                    }
                    else
                    {
                        newCharArray.Add(mCharStream[pos - 2]);
                        newCharArray.Add(mCharStream[pos - 1]);
                    }
                }
                else
                {
                    newCharArray.Add(c);
                }
            }
            mCharStream = newCharArray.ToArray();
        }

        private void ParseToTokens()
        {
            mTokenArray.Clear();
            bool isInEndOfFile = false;
            int pos = 0;
            while (pos < mCharStream.Length)
            {
                char c = mCharStream[pos++];
                if ('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z')
                {//a var begin
                    StringBuilder mBuffer = new StringBuilder();
                    mBuffer.Append(c);
                    while (pos < mCharStream.Length)
                    {
                        c = mCharStream[pos++];
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
                    string tokenStr = mBuffer.ToString();
                    if (TokenConfig.ReservedWords.Exists((s) => { return s == tokenStr; }))
                    {
                        Token t = new Token();
                        t.value = tokenStr;
                        t.type = TokenType.ReservedWord;
                        mTokenArray.Add(t);
                    }
                    else if (TokenConfig.GetOpCode(tokenStr) != OpCode.None)
                    {
                        Token t = new Token();
                        t.value = tokenStr;
                        t.type = TokenType.Op;
                        mTokenArray.Add(t);
                    }
                    else
                    {
                        Token t = new Token();
                        t.value = tokenStr;
                        t.type = TokenType.Other;
                        mTokenArray.Add(t);
                    }

                }
                else if ('0' <= c && c <= '9')
                {
                    float num = 0;
                    float numDecimal = 0;
                    num = c - '0';
                    while (pos < mCharStream.Length)
                    {
                        c = mCharStream[pos++];
                        if ('0' <= c && c <= '9')
                        {
                            num = num * 10 + (c - '0');
                        }
                        else if (c == '.')
                        {
                            float denominator = 0.1f;
                            while (pos < mCharStream.Length)
                            {
                                c = mCharStream[pos++];
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
                    Token t = new Token();
                    t.value = (num + numDecimal).ToString();
                    t.type = TokenType.Num;
                    mTokenArray.Add(t);
                }
                else if (c == '"')
                {
                    StringBuilder mBuffer = new StringBuilder();
                    while (pos < mCharStream.Length)
                    {
                        c = mCharStream[pos++];
                        if (c != '"')
                        {
                            mBuffer.Append(c);
                        }
                        else
                        {
                            break;
                        }
                    }
                    Token t = new Token();
                    t.value = mBuffer.ToString();
                    t.type = TokenType.Str;
                    mTokenArray.Add(t);
                }
                else
                {
                    switch (c)
                    {
                        case '+':
                            mTokenArray.Add(new Token("+", TokenType.Op));
                            break;
                        case '-':
                            mTokenArray.Add(new Token("-", TokenType.Op));
                            break;
                        case '*':
                            mTokenArray.Add(new Token("*", TokenType.Op));
                            break;
                        case '/':
                            mTokenArray.Add(new Token("/", TokenType.Op));
                            break;
                        case '=':
                            c = mCharStream[pos++];
                            if (c == '=')
                            {
                                Token t = new Token("==", TokenType.Op);
                                mTokenArray.Add(t);
                            }
                            else
                            {
                                pos--;
                                Token t = new Token("=", TokenType.Other);
                                mTokenArray.Add(t);
                            }
                            break;
                        case '>':
                            c = mCharStream[pos++];
                            if (c == '=')
                            {
                                mTokenArray.Add(new Token(">=", TokenType.Op));
                            }
                            else
                            {
                                pos--;
                                mTokenArray.Add(new Token(">", TokenType.Op));
                            }
                            break;
                        case '<':
                            c = mCharStream[pos++];
                            if (c == '=')
                            {
                                mTokenArray.Add(new Token("<=", TokenType.Op));
                            }
                            else
                            {
                                pos--;
                                mTokenArray.Add(new Token("<", TokenType.Op));
                            }
                            break;
                        case '!':
                            mTokenArray.Add(new Token("!", TokenType.Op));
                            break;
                        case '&':
                            c = mCharStream[pos++];
                            if (c == '&')
                            {
                                mTokenArray.Add(new Token("&&", TokenType.Op));
                            }
                            else
                            {
                                pos--;
                            }
                            break;
                        case '|':
                            c = mCharStream[pos++];
                            if (c == '|')
                            {
                                mTokenArray.Add(new Token("||", TokenType.Op));
                            }
                            else
                            {
                                pos--;
                            }
                            break;
                        case '(':
                            mTokenArray.Add(new Token("(", TokenType.Op));
                            break;
                        case ')':
                            mTokenArray.Add(new Token(")", TokenType.Op));
                            break;
                        case '[':
                            mTokenArray.Add(new Token("[", TokenType.Other));
                            break;
                        case ']':
                            mTokenArray.Add(new Token("]", TokenType.Other));
                            break;
                        case ',':
                            mTokenArray.Add(new Token(",", TokenType.Other));
                            break;
                        case '\n':
                            if(mTokenArray[mTokenArray.Count-1].value!="\n")
                                mTokenArray.Add(new Token("\n", TokenType.NewLine));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    
    }

}
