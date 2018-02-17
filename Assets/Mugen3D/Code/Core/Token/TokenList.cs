using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Mugen3D
{
    public class TokenList
    {
        private List<Token> mTokens = new List<Token>();
        private Expression mExpression;
        private string mStrValue = "";

        public List<Token> tokens
        {
            get
            {
                return mTokens;
            }
        }

        public Expression asExpression {
            get
            {
                if (mExpression == null)
                {
                    mExpression = new Expression(mTokens, false);
                    //Log.Info("脱靶");
                }
                return mExpression;
            }
        }

        public string asStr
        {
            get
            {
                if (mStrValue == "")
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < mTokens.Count; i++)
                    {
                        sb.Append(mTokens[i].value);
                    }
                    mStrValue = sb.ToString();
                    //Log.Info("脱靶");
                }
                return mStrValue;
            } 
        }

        public void AddToken(Token t)
        {
            mTokens.Add(t);
        }

       
    }
}
