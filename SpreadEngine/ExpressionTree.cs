using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpts321
{
    public class ExpressionTree
    {
        private abstract class Node
        {
            public abstract double Eval();
        }


        private class ConstNode : Node
        {
            private double val;

            public ConstNode(double value)
            {
                val = value;
            }

            public override double Eval()
            {
                return val;
            }
        }

        private class OpNode : Node
        {
            private char nodeOp;
            private Node nodeLeft;
            private Node nodeRight;

            public OpNode(char op, Node left, Node right)
            {
                nodeOp = op;
                nodeLeft = left;
                nodeRight = right;
            }

            public override double Eval()
            {
                double left = nodeLeft.Eval();
                double right = nodeRight.Eval();

                switch (nodeOp)
                {
                    case '+': return left + right;

                    case '-': return left - right;

                    case '*': return left * right;

                    case '/': return left / right;
                }
                return 0;
            }
        }


        private class VarNode : Node
        {
            private string varName;

            private Dictionary<string, double> search;

            public VarNode(string varNode)
            {
                varName = varNode;
            }

            public override double Eval()
            {
                search = varDict;
                if (!search.ContainsKey(varName)) return double.NaN;
                return search[varName];
            }
        }

        private Node root;

        // a member dictionary
        private static Dictionary<string, double> varDict = new Dictionary<string, double>();

  
        public void SetVar(string varName, double varValue)
        {
            varDict[varName] = varValue;
        }

        // constructor
        public ExpressionTree()
        {
        }

        public ExpressionTree(string expression)
        {
            root = Compile(expression);
        }

   
        private static Node BuildSimple(string term)
        {
            double num;
            if (double.TryParse(term, out num))
            {
                return new ConstNode(num);
            }
            return new VarNode(term);
        }


        private static Node Compile(string exp)  
        {
    

            exp = exp.Replace(" ", "");


            if (exp == "") return null;
            if (exp[0] == '(')
            {
                int counter = 1;
                for (int i = 1; i < exp.Length; i++)
                {
                    if (exp[i] == ')')
                    {
                        counter--;
                        if (counter == 0)
                        {
                            if (i == exp.Length - 1)
                            {
                                return Compile(exp.Substring(1, exp.Length - 2));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (exp[i] == '(')
                    {
                        counter++;
                    }
                }
            }

      
            int index = GetLowOpIndex(exp);
            if (index != -1)
            {
                return new OpNode
                    (
                    exp[index],
                    Compile(exp.Substring(0, index)),
                    Compile(exp.Substring(index + 1))
                    );
            }
            return BuildSimple(exp);
        }

        public double Eval()
        {
            if (root != null) { return root.Eval(); }
            else
                return double.NaN;
        }

        private static int GetLowOpIndex(string exp)
        {
            int parenCounter = 0;
            int index = -1;
            for (int i = exp.Length - 1; i >= 0; i--)
            {
                switch (exp[i])
                {
                    case ')':
                        parenCounter--;
                        break;
                    case '(':
                        parenCounter++;
                        break;
                    case '+':
                    case '-':
                        if (parenCounter == 0) return i;
                        break;
                    case '*':
                    case '/':
                        if (parenCounter == 0 && index == -1) index = i;
                        break;
                }
            }
            return index;
        }
        public string[] GetAllVariables()
        {
            return varDict.Keys.ToArray();
        }
    }
}
