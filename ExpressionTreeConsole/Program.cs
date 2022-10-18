using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpts321;

namespace ExpressionTreeConsole
{
    public class Program
    {
       public static void Main (string[] args)
        {
            // Creats a new expression tree object
            ExpressionTree eTree = new ExpressionTree();

            // Initalizes expression string
            string exp = "";

            // A loop for menu interface
            while (true)
            {
                // Prints menu opions
                Console.WriteLine("Current expression = " + exp);
                Console.WriteLine("1 = Enter a new expression:");
                Console.WriteLine("2 = Set a variable value");
                Console.WriteLine("3 = Evaluate Tree");
                Console.WriteLine("4 = Quit");

                // Takes user input
                string input = Console.ReadLine();

                // A switch based of menu options
                switch (input)
                {
                    // Takes user input for a new expression
                    case "1":
                        Console.Write("Enter a new expression: ");
                        exp = Console.ReadLine();
                        eTree = new ExpressionTree(exp);
                        break;
                
                    case "2":
                        Console.Write("Enter a name for variable: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter a value for variable: ");
                        string valueStr = Console.ReadLine();
                        double value = Convert.ToDouble(valueStr);
                        eTree.SetVar(name, value);
                        break;

                    case "3":
                        Console.WriteLine("Evaluation of Tree = " + eTree.Eval());
                        break;

                    case "4":
                        Console.WriteLine("Exiting Program");
                        System.Environment.Exit(0);
                        break;


                    default:
                        break;
                }
            }
        }
    }
}
