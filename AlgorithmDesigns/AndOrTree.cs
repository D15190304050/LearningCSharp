using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    public class AndOrTree
    {
        // CSDN link: https://blog.csdn.net/dala_da/article/details/78586845.

        private const string Or = "or";
        private const string And = "and";
        private const string Unknown = "unknown";
        private const string True = "true";
        private const string False = "false";

        private AndOrTreeNode root;
        private AndOrTreeNode[] treeNodes;

        public AndOrTree()
        {
        }

        public AndOrTree(string filePath)
        {
            string[] treeNodeText = System.IO.File.ReadAllLines(filePath);
            int nodeCount = treeNodeText.Length;
            treeNodes = new AndOrTreeNode[nodeCount];

            for (int i = 0; i < nodeCount; i++)
            {
                string[] line = treeNodeText[i].Split(' ');
                AndOrTreeNodeType nodeType;
                if (line[1] == Or)
                    nodeType = AndOrTreeNodeType.OrNode;
                else if (line[1] == And)
                    nodeType = AndOrTreeNodeType.AndNode;
                else
                    throw new FormatException("Invalid data format of `AndOrTreeNodeType` in file " + filePath + ", line " + i);

                bool? value;
                if (line[2] == True)
                    value = true;
                else if (line[2] == False)
                    value = false;
                else if (line[2] == Unknown)
                    value = null;
                else
                    throw new FormatException("Invalid data format of `value` in file " + filePath + ", line " + i);

                AndOrTreeNode node = new AndOrTreeNode(int.Parse(line[0]), nodeType, value);
                treeNodes[i] = node;

                for (int j = 3; j < line.Length; j++)
                {
                    int child = int.Parse(line[j]);
                    if (child >= i)
                        throw new FormatException("Invalid data format of `parent` in file " + filePath + ", line " + i);

                    treeNodes[child].Parent = node;
                    node.AddNode(treeNodes[child]);
                }
            }

            foreach (AndOrTreeNode node in treeNodes)
            {
                if (node.Parent == null)
                    root = node;
            }
        }

        public AndOrTree DepthFirstSearch()
        {


            return null;
        }

        private void DepthFirstSearch(AndOrTreeNode node, LinkedList<AndOrTreeNode> path, bool solved)
        {
            if (solved)
                return;

            foreach (AndOrTreeNode child in node.GetChildren())
            {
                path.AddLast(child);
                
            }
        }

        public AndOrTree DepthFirstSearch(long maxDepth)
        {
            // If this tree has only 1 node (the root).
            // If the root is true, return the tree itself.
            // Otherwise return null, meaning: no proper solution.
            if (root.IsLeafNode)
                return root.Solvable == true ? this : null;

            // A boolean array that records whether a node is marked before.
            bool[] marked = new bool[treeNodes.Length];

            // According to DFS algorithm from Graph Theory, queue is unnecessary here.
            //Queue<AndOrTreeNode> nodes = new Queue<AndOrTreeNode>();
            //nodes.Enqueue(root);

            DepthFirstSearch(maxDepth, 0, root, marked);

            return null;
        }

        public void DepthFirstSearch(long maxDepth, int currentDepth, AndOrTreeNode node, bool[] marked)
        {
            marked[node.Id] = true;

            if (currentDepth >= maxDepth)
                return;

            // If this is not a leaf node, keep searching.
            // Otherwise, if this is a leaf node and solvable, than try to trace back.
            // Otherwise, this is a leaf node and unsolvable.
            if (!node.IsLeafNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                    DepthFirstSearch(maxDepth, currentDepth + 1, child, marked);
            }
            else if (node.Solvable == true)
            {
                // How to return the trace back result?
            }
        }

        public AndOrTree BreadthFirstSearch()
        {
            // If this tree has only 1 node (the root).
            // If the root is true, return the tree itself.
            // Otherwise return null, meaning: no proper solution.
            if (root.IsLeafNode)
                return root.Solvable == true ? this : null;

            Queue<AndOrTreeNode> nodes = new Queue<AndOrTreeNode>();
            nodes.Enqueue(root);

            // A boolean array that records whether a node is marked before.
            bool[] marked = new bool[treeNodes.Length];

            while (nodes.Count != 0)
            {
                AndOrTreeNode node = nodes.Dequeue();
                if (node.Solvable == true)
                {

                }
                else
                {
                    foreach (AndOrTreeNode n in node.GetChildren())
                        nodes.Enqueue(n);
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="solutionNodes"></param>
        /// <param name="marked"></param>
        /// <returns></returns>
        private AndOrTree TraceUp(AndOrTreeNode node, LinkedList<AndOrTreeNode> solutionNodes, bool[] marked)
        {
            // Build solution tree if trace back to root successfully.
            if (node == root)
            {
                solutionNodes.AddLast(node);
                return BuildSolutionTree(solutionNodes);
            }

            // Trace back to the parent node of this node if this is an or node since there is child node of this node is solvable.
            if (node.NodeType == AndOrTreeNodeType.OrNode)
            {
                solutionNodes.AddLast(node);
                return TraceUp(node.Parent, solutionNodes, marked);
            }

            // Trace down each child node of this and node.
            // Trace up only when each child of this and node is solvable.
            // Return null, which represents that this and node is not solvable, if any child node of this and node is not solvable.
            if (node.NodeType == AndOrTreeNodeType.AndNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                {
                    if (!TraceDown(child, solutionNodes, marked))
                        return null;
                }

                solutionNodes.AddLast(node);
                return TraceUp(node.Parent, solutionNodes, marked);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="solutionNodes"></param>
        /// <param name="marked"></param>
        /// <returns></returns>
        private bool TraceDown(AndOrTreeNode node, LinkedList<AndOrTreeNode> solutionNodes, bool[] marked)
        {
            // If this node is not marked, then do nothing.
            // If process this node here, then the BFS and DFS sequence will be broken.
            if (!marked[node.Id])
                return false;

            // Return true if this node is accessed before.
            // This `if` statement might be unnecessary.
            if (solutionNodes.Contains(node))
                return true;

            // Leaf nodes should be processed specially.
            if (node.IsLeafNode)
            {
                // If this node is a leaf node and it is a solvable node, than add this node to the solution nodes and return true.
                // Otherwise, this node is not solvable, return false.
                if (node.Solvable == true)
                {
                    solutionNodes.AddLast(node);
                    return true;
                }
                else
                    return false;
            }

            // If any child node of an or node is solvable, add this or node to solution nodes.
            // Otherwise, this or node is unsolvable, no need to keep searching, return false.
            if (node.NodeType == AndOrTreeNodeType.OrNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                {
                    if (TraceDown(child, solutionNodes, marked))
                    {
                        solutionNodes.AddLast(node);
                        return true;
                    }
                }


                return false;
            }

            // If all child nodes of an and node are solvable, add this and node to solution nodes.
            // Otherwise, this and node is unsolvable, no need to keep searching, return false.
            if (node.NodeType == AndOrTreeNodeType.AndNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                {
                    if (!TraceDown(child, solutionNodes, marked))
                        return false;
                }

                solutionNodes.AddLast(node);
                return true;
            }

            return false;
        }

        private AndOrTree BuildSolutionTree(LinkedList<AndOrTreeNode> solutionNodes)
        {
            return null;
        }

        public override string ToString()
        {
            StringBuilder treeStructure = new StringBuilder();

            foreach (AndOrTreeNode node in treeNodes)
            {
                string solvable;
                if (node.Solvable == true)
                    solvable = "True";
                else if (node.Solvable == false)
                    solvable = "False";
                else
                    solvable = "Unknown";
                treeStructure.Append(node.Id + " (" + node.NodeType + "), " + solvable + ": ");

                foreach (AndOrTreeNode child in node.GetChildren())
                    treeStructure.Append(child.Id + " ");

                treeStructure.Append(Environment.NewLine);
            }

            treeStructure.Append("Root: " + root.Id);
            treeStructure.Append(Environment.NewLine);

            return treeStructure.ToString();
        }
    }
}
