using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalDataStructuresAndAlgorithm.Graphs.DirectedGraph;
using System.IO;

namespace PersonalDataStructuresAndAlgorithm
{
    public class Program
    {
        static int Main(string[] args)
        {
            //Graphs.UnitTest.FordFulkersonUnitTest();
            //Graphs.UnitTest.ArbitrageUnitTest();
            string graphFile = @"F:\DinoStark\Temp\graph.txt";
            //string[] text = File.ReadAllLines(graphFile);
            //for (int i = 0; i < text.Length; i++)
            //{
            //    string[] line = text[i].Split(' ');
            //    char left = line[0][0];
            //    char right = line[1][0];

            //    text[i] = ((int)(left - 'A') + " " + (int)(right - 'A'));
            //}
            //File.WriteAllLines(graphFile, text);
            Digraph G = new Digraph(graphFile);
            G = G.Reverse();
            DepthFirstOrder order = new DepthFirstOrder(G);
            Console.WriteLine(order.ReversePostOrder != null);
            foreach (int i in order.ReversePostOrder)
                Console.Write((char)('A' + i) + " ");
            Console.WriteLine();

            // Keep the console open during debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}