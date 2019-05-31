using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    using Collections;
    using Graphs.DirectedGraph;

    /// <summary>
    /// The NFA class provides a data type for creating a non-determinisitic finite-state automachine
    /// from a regular expression and testing a given string is matched by that regular expression.
    /// </summary>
    public class NFA
    {
        /// <summary>
        /// Digraph of epsilon transitions.
        /// </summary>
        Digraph graph;

        /// <summary>
        /// Regular expression.
        /// </summary>
        private string regex;

        /// <summary>
        /// Number of characetrs in regular expression.
        /// </summary>
        private int count;

        /// <summary>
        /// Initializes the NFA from the specified regular expression.
        /// </summary>
        /// <param name="pattern">The regular expression.</param>
        public NFA(string pattern)
        {
            regex = pattern;
            count = pattern.Length;
            Stack<int> operators = new Stack<int>();
            graph = new Digraph(count + 1);

            for (int i = 0; i < count; i++)
            {
                char current = regex[i];

                int leftParentheses = i;
                if ((current == '(') || (current == '|'))
                    operators.Push(i);
                else if (current == ')')
                {
                    int or = operators.Pop();

                    // 2-way or operator.
                    if (regex[or] == '|')
                    {
                        leftParentheses = operators.Pop();
                        graph.AddEdge(leftParentheses, or + 1);
                        graph.AddEdge(or, i);
                    }
                    else if (regex[or] == '(')
                        leftParentheses = or;
                }

                // Closure operator (use 1-character lookahead).
                if ((i < count - 1) && (regex[i + 1] == '*'))
                {
                    graph.AddEdge(leftParentheses, i + 1);
                    graph.AddEdge(i + 1, leftParentheses);
                }

                if ((current == '(') || (current == '*') || (current == ')'))
                    graph.AddEdge(i, i + 1);
            }

            if (operators.Size != 0)
                throw new ArgumentException("Invalid regular expression.");
        }

        /// <summary>
        /// Returns true if the text is matched by the regular expression, false otherwise.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>True if the text is matched by the regular expression, false otherwise.</returns>
        public bool Recognizes(string text)
        {
            DirectedDFS dfs = new DirectedDFS(graph, 0);
            LinkedList<int> pc = new LinkedList<int>();
            for (int v = 0; v < graph.V; v++)
            {
                if (dfs.HasPathTo(v))
                    pc.AddFirst(v);
            }

            // Compute possible NFA states for text[i + 1].
            for (int i = 0; i < text.Length; i++)
            {
                char current = text[i];
                if ((current == '*') || (current == '|') || (current == '(') || (current == ')'))
                    throw new ArgumentException("Text contains the meta-character " + current);

                LinkedList<int> match = new LinkedList<int>();
                foreach (int v in pc)
                {
                    if (v == count)
                        continue;

                    if ((regex[v] == text[i]) || (regex[v] == '.'))
                        match.AddFirst(v + 1);
                }

                dfs = new DirectedDFS(graph, match);
                pc = new LinkedList<int>();
                for (int v = 0; v < graph.V; v++)
                {
                    if (dfs.HasPathTo(v))
                        pc.AddFirst(v);
                }

                // Optimization if no states reachable.
                if (pc.Size == 0)
                    return false;
            }

            // Check for accept state.
            foreach (int v in pc)
            {
                if (v == count)
                    return true;
            }

            return false;
        }
    }
}