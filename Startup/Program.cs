using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Startup
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //MySqlConnectionTest.ConnectionTest();

            double[,] parameters = new double[6, 3];

            parameters[0, 0] = 0;
            parameters[0, 1] = 0;
            parameters[0, 2] = 0;

            parameters[1, 0] = 0;
            parameters[1, 1] = 0;
            parameters[1, 2] = 1;

            parameters[2, 0] = 0;
            parameters[2, 1] = 4;
            parameters[2, 2] = 5;

            parameters[3, 0] = 1;
            parameters[3, 1] = 2;
            parameters[3, 2] = 1;

            parameters[4, 0] = 1;
            parameters[4, 1] = 2;
            parameters[4, 2] = 3;

            parameters[5, 0] = 1;
            parameters[5, 1] = 3;
            parameters[5, 2] = 2;

            QuadraticEquation qe;
            for (int i = 0; i < parameters.GetLength(0); i++)
            {
                qe = new QuadraticEquation(parameters[i, 0], parameters[i, 1], parameters[i, 2]);
                if (!qe.IsSolvable)
                    Console.WriteLine("({0}) * x^2 + ({1}) * x + ({2}) = 0 has no solution\n", parameters[i, 0], parameters[i, 1], parameters[i, 2]);
                else
                {
                    Console.WriteLine("Solution for {0} = 0 is", qe);
                    Console.WriteLine("Root1 = {0}", qe.Root);
                }
            }

            //TaskDemos.InstantiateTasks();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}